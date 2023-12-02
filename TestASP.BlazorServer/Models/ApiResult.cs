using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using TestASP.Model;

namespace TestASP.BlazorServer.Models
{
	public class ApiResult<T>: ResultBase
	{
		public bool IsSuccess
		{
			get
			{
				return StatusCode == StatusCodes.Status200OK && (string.IsNullOrEmpty(Error) || Errors == null || !Errors.Any());
			}
		}
		public string Message { get; set; }
		public string Error { get; set; }
		public Dictionary<string, string[]> Errors { get; set; }
        public T Data { get; set; }
		public ApiResult() : base() { }
        public ApiResult(int statusCode) : base(statusCode)
        {
        }

		public static ApiResult<T> InternalServerError(string message = "Something went wrong in parsing api response.")
		{
			return new ApiResult<T>(StatusCodes.Status500InternalServerError)
			{
				Error = message
			};
		}
	}
}

