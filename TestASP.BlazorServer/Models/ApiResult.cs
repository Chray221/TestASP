using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using TestASP.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestASP.BlazorServer.Models
{
	public class ApiResult
    {
		public static ApiResult<T> Success<T>(T data, string message = "Success")
        {
            return new ApiResult<T>(StatusCodes.Status200OK)
            {
                Message = message,
                Data = data
            };
        }

        public static ApiResult<T> InternalServerError<T>(string message = "Something went wrong in parsing api response.")
        {
            return new ApiResult<T>(StatusCodes.Status500InternalServerError)
            {
                Error = message
            };
        }
    }

    public class ApiResult<T>: ResultBase
	{
		public bool IsSuccess
		{
			get
			{
				return StatusCode == StatusCodes.Status200OK &&
					   (string.IsNullOrEmpty(Error) ||
					   Errors == null || !Errors.Any()) &&
					   (Data != null || !string.IsNullOrEmpty(Message));
			}
		}

        public bool IsModelError {
            get
            {
                return StatusCode == StatusCodes.Status400BadRequest &&
                       Errors != null &&
                       Errors.Count > 0;
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
	}
}

