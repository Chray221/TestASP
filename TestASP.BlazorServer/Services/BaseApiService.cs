using System.ComponentModel;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestASP.BlazorServer.Models;
using TestASP.Common.Extensions;
using TestASP.Common.Helpers;
using TestASP.Model;

namespace TestASP.BlazorServer.Services
{
    public class BaseApiService
    {
        public IHttpClientFactory _httpClient { get; set; }
        public ILogger _logger { get; }
        public ConfigurationManager _configuration { get; }
        public string ApiRootUrl { get; }

        public BaseApiService(IHttpClientFactory httpClient, ILogger logger, ConfigurationManager configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            ApiRootUrl = configuration["Urls:ApiRootUrl"];
        }

        public async Task<ApiResult<TResponse>> SendAsync<TRequest,TResponse>(ApiRequest<TRequest> apiRequest)
            where TRequest : class
            where TResponse: class
        {
            _logger.LogMessage($"API Request-[{apiRequest.Method}]: {ApiRootUrl}/{apiRequest.Url}");
            var client = _httpClient.CreateClient("MagicAPI");
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("Accept", "application/json");
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
                    MultipartContent multipart = new MultipartContent();
                    object? item;
                    string propertyName = "";
                    foreach(var property in apiRequest.Data.GetType().GetProperties())
                    {
                        AddContent(multipart, apiRequest.Data, property);
                        //propertyName = property.Name;
                        //item = property.GetValue(apiRequest.Data);
                        //if (IsListType(property.GetType()))
                        //{

                        //}
                        //else if(item is IBrowserFile browserFile)
                        //{
                        //    multipart.Add(CreateStreamContent(browserFile.OpenReadStream(),
                        //                                      propertyName,
                        //                                      item.ToString() ?? ""));
                        //}
                        //else if (item != null)
                        //{
                        //    multipart.Add(CreateStringContent(propertyName, item.ToString() ?? ""));
                        //}
                    }
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

                var responseContent = await response.Content.ReadAsStringAsync();

                ApiResult<TResponse> dataResponse = JsonConvert.DeserializeObject<ApiResult<TResponse>>(responseContent);

                _logger.LogMessage($"API Response-[{apiRequest.Method}]: {apiRequest.Url} : {responseContent}");
                return dataResponse;
            }
            catch (Exception e)
            {
                //var exceptionResponse = JsonConvert.DeserializeObject<T>(responseContent);
                _logger.LogException(e);
                return ApiResult<TResponse>.InternalServerError();
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
                                                  propertyName,
                                                  item.ToString() ?? ""));
            }
            if (item != null)
            {
                multipart.Add(CreateStringContent(propertyName, item.ToString() ?? ""));
            }
        }

        private string? GetPropertyValue<T>(T data, PropertyInfo property)
        {
            Type propType = property.GetType();
            if (propType.IsValueType)
            {
                return property.GetValue(data)?.ToString();
            }
            else
            {
                switch (propType.Name)
                {
                    case nameof(System.String):
                        return property.GetValue(data)?.ToString();
                    default:
                        if (propType.IsEnum)
                        {
                            return ""+(int)property.GetValue(data);
                        }
                        else if (Nullable.GetUnderlyingType(propType) is Type valueType)
                        {
                            return GetPropertyApiSchema(valueType);
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
        public static StringContent CreateStringContent(string fieldPathName, string value, bool isSecure = false)
        {
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
        public static StreamContent CreateStreamContent(Stream imageStream, string filepath, string fieldPathName)
        {
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

