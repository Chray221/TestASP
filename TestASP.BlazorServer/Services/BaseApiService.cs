using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestASP.BlazorServer.Models;
using TestASP.Common.Extensions;
using TestASP.Data.Enums;

namespace TestASP.BlazorServer.Services
{
    public class BaseApiService
    {
        public readonly IHttpClientFactory _httpClient;
        public readonly ILogger _logger;
        public readonly ConfigurationManager _configuration;
        public readonly string ApiRootUrl;
        public readonly ProtectedLocalStorage _localStorage;

        public BaseApiService(
            IHttpClientFactory httpClient,
            ILogger logger,
            ConfigurationManager configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            ApiRootUrl = configuration["Urls:ApiRootUrl"]!;
        }

        public BaseApiService(
            IHttpClientFactory httpClient,
            ILogger logger,
            ConfigurationManager configuration,
            ProtectedLocalStorage localStorage)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            ApiRootUrl = configuration["Urls:ApiRootUrl"]!;
            _localStorage = localStorage;
        }

        public Task<ApiResult<TResponse>> SendAsync<TResponse>(ApiRequest<object> apiRequest)
            where TResponse: class
        {
            return SendAsync<object, TResponse>(apiRequest);
        }

        public async Task<ApiResult<TResponse>> SendAsync<TRequest,TResponse>(ApiRequest<TRequest> apiRequest)
            where TRequest : class
            where TResponse: class
        {
            _logger.LogMessage($"API Request-[{apiRequest.Method}]: {ApiRootUrl}/{apiRequest.Url}");
            var client = _httpClient.CreateClient("TestASP.API");
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("Accept", "application/json");

            if (_localStorage != null)
            {
                var token = await _localStorage.GetAsync<string>("JWTToken");
                if (token.Success)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                }
            }

            string url = apiRequest.Url;
            if(!url.StartsWith(ApiRootUrl))
            {
                url = $"{ApiRootUrl}/{apiRequest.Url}";
            }
            request.RequestUri = new Uri(url);

            if(apiRequest.Data != null)
            {
                if (apiRequest.IsMultipart)
                {
                    MultipartFormDataContent multipart = new MultipartFormDataContent();   
                    foreach(var property in apiRequest.Data.GetType().GetProperties())
                    {
                        AddContent(multipart, apiRequest.Data, property);
                    }
                    request.Content = multipart;
                }
                else
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
            }
            request.Method = apiRequest.Method;
            try
            {

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return ApiResult.Unauthorized<TResponse>();
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                ApiResult<TResponse> dataResponse = JsonConvert.DeserializeObject<ApiResult<TResponse>>(responseContent);

                _logger.LogMessage($"API Response-[{apiRequest.Method}:{response.StatusCode}]: {apiRequest.Url} : {responseContent}");
                return dataResponse;
            }
            catch (Exception e)
            {
                //var exceptionResponse = JsonConvert.DeserializeObject<T>(responseContent);
                _logger.LogException(e);
                return ApiResult.InternalServerError<TResponse>();
            }
            //var APIResponse = JsonConvert.DeserializeObject<T>(responseContent);
            //return ApiResult<TResponse>.InternalServerError();
        }

        public void AddContent<T>(MultipartContent multipart,T data, PropertyInfo property, string basePropertyName = "")
        {
            string propertyName = basePropertyName + property.Name;
            object? item = property.GetValue(data);
            if (IsListType(property.GetType()))
            {
                var properties = property.GetType().GetProperties();
                if (properties.FirstOrDefault(prop => prop.Name == "Item") is PropertyInfo itemInfo &&
                    itemInfo.PropertyType != null)
                {
                    if (!propertyName.Contains('.'))
                    {
                        propertyName = "";
                    }
                    var listItemSchema = GetPropertyValue(data, itemInfo);
                    //if (listItemSchema == null)
                    //{
                    //    foreach (var itemSchema in GetApiSchema($"{propertyName}[0]", itemInfo.PropertyType))
                    //    {
                    //        newProps.Add(itemSchema);
                    //    }
                    //}
                    //else
                    //{
                    //    if (listItemSchema != null)
                    //    {
                    //        newProps.Add(new ApiParameterProperty($"{propertyName}[0]", listItemSchema, false));
                    //    }
                    //}

                }
            }
            else if (item is IBrowserFile browserFile)
            {
                multipart.Add(CreateStreamContent(browserFile.OpenReadStream(),
                                                  browserFile.Name,
                                                  propertyName));
            }
            if (item != null)
            {
                multipart.Add(CreateStringContent(propertyName, item.ToString() ?? ""));
            }
        }

        private string? GetPropertyValue<T>(T data, PropertyInfo property)
        {
            Type propType = property.GetType();
            if (propType.IsValueType || propType.Name == nameof(System.String))
            {
                return property.GetValue(data)?.ToString();
            }
            else
            {
                switch (propType.Name)
                {
                    default:
                        if (propType.IsEnum)
                        {
                            return ""+(int)property.GetValue(data);
                        }
                        else if (Nullable.GetUnderlyingType(propType) is Type valueType)
                        {
                            //return GetPropertyApiSchema(valueType);
                        }
                        return null;
                }
            }
        }

        /// <summary>
        /// <br>use for multipart</br>
        /// <br>Example Object:</br>
        /// <br>User.Name="Sample"</br>
        /// <br>User.Address.City="CityData"</br>
        /// </summary>
        /// <param name="fieldPathName">  <br> e.g "Name" </br> or <br> "Address.City" </br> </param>
        /// <param name="value"> e.g "Sample"</param>
        /// <returns></returns>
        private StringContent CreateStringContent(string fieldPathName, string value, bool isSecure = false)
        {
            _logger.LogMessage($"MULTIPART ITEM: {{{fieldPathName}, {value}}}");
            var dataContent = new StringContent(value, Encoding.UTF8, isSecure ? "text/plain" : "application/json");
            dataContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"" + fieldPathName + "\"",
            };
            return dataContent;
        }

        /// <summary>
        /// <br>use for multipart</br>
        /// <br>Example Object:</br>
        /// <br>User.Name</br>
        /// <br>User.Address.City</br>
        /// </summary>
        /// <param name="imageStream"></param>
        /// <param name="filepath"></param>
        /// <param name="fieldPathName">  <br> e.g "Name" </br> or <br> "Address.City" </br> </param>
        /// <returns></returns>
        private StreamContent CreateStreamContent(Stream imageStream, string filepath, string fieldPathName)
        {
            _logger.LogMessage($"MULTIPART ITEM: {{{fieldPathName}, binary}}");
            if (imageStream != null)
            {
                var pictureContent = new StreamContent(imageStream);
                pictureContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = "\"" + Path.GetFileNameWithoutExtension(filepath) + "" + Path.GetExtension(filepath) + "\"",
                    Name = "\"" + fieldPathName + "\""
                };
                return pictureContent;
            }
            return null;
        }

        private static bool IsListType(Type propertyType)
        {
            return (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>)) ||
                   propertyType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}

