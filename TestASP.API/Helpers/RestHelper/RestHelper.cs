//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using TestAPI.Models;

//namespace TestASP.API.Helpers.RestHelper
//{
//    public class RestHelper : IRestHelper
//    {
//        public RestHelper()
//        {

//        }

//        public async Task<RestResponse<T>> GetAsync<T>(string apiUrl)
//        {
//            using (HttpClient client = new HttpClient())
//            {
//                var result = await client.GetAsync(apiUrl);

//                return await GetRespose<T>(result);
//            }
//        }

//        public async Task<RestResponse<T>> PostAsync<T>(string apiUrl,string payload, bool isSecure = false)
//        {
//            using (var client = new HttpClient())
//            {
//                StringContent postPayload = new StringContent(payload, Encoding.UTF8, isSecure ? "text/plain" : "application/json");
//                HttpResponseMessage result = await client.PostAsync(apiUrl, postPayload);

//                return await GetRespose<T>(result);
//            }
//        }
       
//        public async Task<RestResponse<T>> MultipartAsync<T>(string apiUrl, string payload, Dictionary<string, Stream> streams , HttpMethod restMethod = null, bool isSecured = false)
//        {
//            restMethod = restMethod ?? HttpMethod.Post;
//            using (HttpClient client = new HttpClient())
//            {
//                var json = JObject.Parse(payload);
//                var multipartFormData = new MultipartFormDataContent();

//                foreach (var keyValuePair in json)
//                {
//                    if (keyValuePair.Value.HasValues) //user[id] = 1, user[name] = name
//                    {
//                        var innerJson = JObject.Parse(keyValuePair.Value.ToString());
//                        foreach (var innerKeyValuePair in innerJson)
//                        {
//                            var jsonPath = $"{keyValuePair.Key}[{innerKeyValuePair.Key}]";
//                            if (streams != null && streams.ContainsKey(jsonPath))
//                            {
//                                multipartFormData.Add(CreateStreamContent(streams[jsonPath], innerKeyValuePair.Value.ToString(), jsonPath));
//                            }
//                            else if (!isSecured) // except on IsSecured
//                            {
//                                multipartFormData.Add(CreateStringContent(innerKeyValuePair.Value.ToString(), jsonPath));
//                            }
//                        }
//                    }
//                    else if (!isSecured) //user_id = 1, except on IsSecured
//                    {
//                        multipartFormData.Add(CreateStringContent(keyValuePair.Value.ToString(), keyValuePair.Key));
//                    }
//                }

//                //if (isSecured)
//                //{
//                //    multipartFormData.Add(CreateStringContent(payload.EncryptEncode(), "payload"));
//                //}

//                var multipartFormDataString = JsonConvert.SerializeObject(multipartFormData);
//                //App.Log("MultiPartFormData: " + multipartFormDataString);
//                HttpResponseMessage response = restMethod == HttpMethod.Put ? await client.PutAsync(apiUrl, multipartFormData) : await client.PostAsync(apiUrl, multipartFormData);
//                return await GetRespose<T>(response);
//            }
//        }

//        public static StreamContent CreateStreamContent(Stream imageStream, string filepath, string jsonPath)
//        {
//            if (imageStream != null)
//            {
//                var pictureContent = new StreamContent(imageStream);
//                pictureContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
//                {
//                    FileName = "\"" + Path.GetFileNameWithoutExtension(filepath) + "" + Path.GetExtension(filepath) + "\"",
//                    Name = "\"" + jsonPath + "\""
//                };
//                return pictureContent;
//            }
//            return null;
//        }

//        public static StringContent CreateStringContent(string data, string jsonPath, bool isSecured = false)
//        {
//            var dataContent = new StringContent(data, Encoding.UTF8, isSecured ? "text/plain" : "application/json");
//            dataContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
//            {
//                Name = "\"" + jsonPath + "\"",
//            };
//            return dataContent;
//        }

//        static async Task<RestResponse<T>> GetRespose<T>(HttpResponseMessage result)
//        {
//            if (result.IsSuccessStatusCode)
//            {
//                string json = await result.Content.ReadAsStringAsync();
//                return new RestResponse<T>(JsonConvert.DeserializeObject<T>(json));
//            }
//            else //web api sent error response 
//            {
//                string jsonerror = await result.Content.ReadAsStringAsync();
//                if (!string.IsNullOrEmpty(jsonerror) && jsonerror.Contains("custom_error"))
//                {
//                    JObject jobject = JObject.Parse(jsonerror);
//                    if (jobject.ContainsKey("custom_error"))
//                    {
//                        //log response status here..
//                        return new RestResponse<T>(JsonConvert.DeserializeObject<CustomMessage>(jobject["custom_message"].ToString()));
//                    }
//                }
//                return new RestResponse<T>(result.StatusCode.ToString(), result.ReasonPhrase);
//            }
//        }
//    }
//}
