using System;
using TestASP.Web.IServices;
using TestASP.Web.Models;
using TestASP.Model;

namespace TestASP.Web.Services
{
    public class CRUDBaseApiService<T> : BaseApiService, ICRUDBaseApiService<T>
        where T: BaseDto
    {
        public string BaseApiURL { get; set; }
        public CRUDBaseApiService(
            IHttpClientFactory httpClient,
            ILogger<CRUDBaseApiService<T>> logger,
            ConfigurationManager configuration)
            : base(httpClient, logger, configuration)
        {
        }

        public virtual Task<ApiResult<T>> CreateAsync(T newData)
        {
            return SendAsync<T, T>(ApiRequest.PostRequest(BaseApiURL, newData));
        }

        public virtual Task<ApiResult<T>> DeleteAsync(int id)
        {
            return SendAsync<T, T>(ApiRequest.DeleteRequest<T>(BaseApiURL+ $"?id={id}"));
        }

        public virtual Task<ApiResult<List<T>>> GetAllAsync()
        {
            return SendAsync<object, List<T>>(ApiRequest.GetRequest(BaseApiURL));
        }

        public virtual Task<ApiResult<T>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<ApiResult<T>> UpdateAsync(T updateData)
        {
            throw new NotImplementedException();
        }
    }
}

