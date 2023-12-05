using System;
using System.Net;
using Microsoft.AspNetCore.CookiePolicy;

namespace TestASP.BlazorServer.Models
{
    public static class ApiRequest
    {
        public static ApiRequest<T> GetRequest<T>(string url, T data) where T : class
        {
            return new ApiRequest<T>(HttpMethod.Get, url, data);
        }

        public static ApiRequest<object> GetRequest(string url)
        {
            return new ApiRequest<object>(HttpMethod.Get, url);
        }

        public static ApiRequest<T> GetRequest<T>(string url) where T : class
        {
            return new ApiRequest<T>(HttpMethod.Get, url);
        }

        public static ApiRequest<T> PostRequest<T>(string url, T data, bool isMultipart = false) where T : class
        {
            return new ApiRequest<T>(HttpMethod.Post, url, data, isMultipart);
        }

        public static ApiRequest<T> PutRequest<T>(string url, T data, bool isMultipart = false) where T : class
        {
            return new ApiRequest<T>(HttpMethod.Put, url, data, isMultipart);
        }

        public static ApiRequest<object> DeleteRequest(string url)
        {
            return new ApiRequest<object>(HttpMethod.Delete, url);
        }

        public static ApiRequest<T> DeleteRequest<T>(string url) where T : class
        {
            return new ApiRequest<T>(HttpMethod.Delete, url);
        }
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

