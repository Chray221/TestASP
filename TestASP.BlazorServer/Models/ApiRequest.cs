using System;
using System.Net;
using Microsoft.AspNetCore.CookiePolicy;
using TestASP.Common.Utilities;

namespace TestASP.BlazorServer.Models
{
    public static class ApiRequest
    {
        public static ApiRequest<T> GetRequest<T>(string url, T data) where T : class
        {
            return new ApiRequest<T>(HttpMethod.Get, url, data);
        }
        #region GET
        public static ApiRequest<object> GetRequest(ApiEndpoint url, params object[] urlObjects)
        {
            return new ApiRequest<object>(HttpMethod.Get, ApiEndpoint.FromV1Format(url, urlObjects));
        }

        public static ApiRequest<object> GetRequest(ApiVersion version, ApiEndpoint url, params object[] urlObjects)
        {
            return new ApiRequest<object>(HttpMethod.Get, ApiEndpoint.FromFormat(version, url, urlObjects));
        }

        public static ApiRequest<object> GetRequest(string url)
        {
            return new ApiRequest<object>(HttpMethod.Get, url);
        }

        public static ApiRequest<T> GetRequest<T>(string url) where T : class
        {
            return new ApiRequest<T>(HttpMethod.Get, url);
        }
        #endregion

        #region POST
        /// <summary>
        /// POST: v1/api/{url}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="urlObjects"></param>
        /// <returns></returns>
        public static ApiRequest<T> PostRequest<T>(T data, ApiEndpoint url, params object[] urlObjects) where T : class
        {
            return PostRequest(ApiEndpoint.FromV1Format(url, urlObjects), data);
        }

        public static ApiRequest<T> PostRequest<T>(T data, ApiVersion version, ApiEndpoint url, params object[] urlObjects) where T : class
        {
            return PostRequest(ApiEndpoint.FromFormat(version, url, urlObjects), data);
        }

        public static ApiRequest<T> PostRequest<T>(string url, T data, bool isMultipart = false) where T : class
        {
            return new ApiRequest<T>(HttpMethod.Post, url, data, isMultipart);
        }
        #endregion

        #region PUT
        /// <summary>
        /// PUT: v1/api/{url}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="urlObjects"></param>
        /// <returns></returns>
        public static ApiRequest<T> PutRequest<T>(T data, ApiEndpoint url, params object[] urlObjects) where T : class
        {
            return PutRequest(ApiEndpoint.FromV1Format(url, urlObjects), data);
        }

        public static ApiRequest<T> PutRequest<T>(T data, ApiVersion version, ApiEndpoint url, params object[] urlObjects) where T : class
        {
            return PutRequest(ApiEndpoint.FromFormat(version, url, urlObjects), data);
        }

        public static ApiRequest<T> PutRequest<T>(string url, T data, bool isMultipart = false) where T : class
        {
            return new ApiRequest<T>(HttpMethod.Put, url, data, isMultipart);
        }
        #endregion

        #region DELETE

        /// <summary>
        /// PUT: v1/api/{url}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="urlObjects"></param>
        /// <returns></returns>
        public static ApiRequest<object> DeleteRequest(ApiEndpoint url, params object[] urlObjects) 
        {
            return DeleteRequest(ApiEndpoint.FromV1Format(url, urlObjects));
        }

        public static ApiRequest<object> DeleteRequest(ApiVersion version, ApiEndpoint url, params object[] urlObjects)
        {
            return DeleteRequest(ApiEndpoint.FromFormat(version, url, urlObjects));
        }
        public static ApiRequest<object> DeleteRequest(string url)
        {
            return new ApiRequest<object>(HttpMethod.Delete, url);
        }

        public static ApiRequest<T> DeleteRequest<T>(string url) where T : class
        {
            return new ApiRequest<T>(HttpMethod.Delete, url);
        }
        #endregion
    }

	public class ApiRequest<T> where T: class
	{
		public string Url { get; set; }
		public HttpMethod Method { get; set; }
		public T Data { get; set; }
        public bool IsMultipart { get; set; }

        public ApiRequest(HttpMethod method, string url, T data, bool isMultipart = false)
        {
            Url = url;
            Method = method;
            Data = data;
            IsMultipart = isMultipart;
        }

        public ApiRequest(HttpMethod method, string url)
        {
            Url = url;
            Method = method;
            Data = null;
        }
    }
}

