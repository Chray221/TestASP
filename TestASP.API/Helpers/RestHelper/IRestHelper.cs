//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net.Http;
//using System.Threading.Tasks;
//using TestAPI.Models;

//namespace TestASP.API.Helpers.RestHelper
//{
//    public interface IRestHelper
//    {
//        Task<RestResponse<T>> GetAsync<T>(string apiUrl);
//        Task<RestResponse<T>> PostAsync<T>(string apiUrl, string payload, bool isSecure = false);
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="apiUrl"></param>
//        /// <param name="payload"></param>
//        /// <param name="streams"></param>
//        /// <param name="restMethod">default post</param>
//        /// <param name="isSecured"></param>
//        /// <returns></returns>        
//        Task<RestResponse<T>> MultipartAsync<T>(string apiUrl, string payload, Dictionary<string, Stream> streams, HttpMethod restMethod = null, bool isSecured = false);
//    }
//}
