using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;

namespace TestASP.API.Extensions
{
    public static class MessageHelper
    {
        public static IActionResult Ok(string message)
        {
            return new OkObjectResult(ResultBase.Success(message));
        }

        public static IActionResult Ok<T>(T data, string message)
        {
            return new OkObjectResult(ResultBase.Success(data, message));
        }

        public static IActionResult BadRequest(string message)
        {
            return (ObjectResult)ResultBase.Error(message, StatusCodes.Status400BadRequest);
        }

        public static IActionResult BadRequest(ModelError error)
        {
            return (ObjectResult)ResultBase.Error(error);
        }

        public static IActionResult NotFound(string message)
        {
            return (ObjectResult)ResultBase.Error(message,StatusCodes.Status404NotFound);
        }

        public static IActionResult InternalServerError(string message)
        {
            return new ObjectResult(ResultBase.Error(message, StatusCodes.Status500InternalServerError))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }

    public class ResultBase
    {
        public int StatusCode { get; }
        public ResultBase() { }

        public ResultBase(int statusCode)
        {
            StatusCode = statusCode;
        }

        public static ResultBase Success(string message) =>
            new SuccessResult(message);

        public static ResultBase Success<T>(T data, string message) =>
            new DataResult<T>(data, message);

        public static ResultBase Error(string error, int statusCode) =>
            new ErrorResult(error, statusCode);

        public static ResultBase Error(ModelError error) =>
            new ModelErrorResult(error);

        public static implicit operator ObjectResult(ResultBase err)
        {
            return new ObjectResult(err) { StatusCode = err.StatusCode };
        }

        public static explicit operator ResultBase(ObjectResult objectResult)
        {
            if(objectResult.Value is ErrorResult err)
            {
                return err;
            }

            return new ErrorResult("", objectResult.StatusCode ?? StatusCodes.Status400BadRequest);
        }

        //public static implicit operator IActionResult(ResultBase err)
        //{
        //    return new ObjectResult(err) { StatusCode = err.StatusCode };
        //}

        //public static explicit operator ResultBase(IActionResult actionResult)
        //{
        //    if (actionResult is ObjectResult objectResult)
        //    {
        //        if (objectResult.Value is ErrorResult err)
        //        {
        //            return err;
        //        }

        //        return new ErrorResult("", objectResult.StatusCode ?? StatusCodes.Status400BadRequest);
        //    }
        //    return new ErrorResult("", StatusCodes.Status500InternalServerError);
        //}
    }

	public class SuccessResult : ResultBase
    {
		public string Message { get; }

        public SuccessResult() { }
        public SuccessResult(string message): base(StatusCodes.Status200OK)
		{
            Message = message;
        }
	}

    public class ErrorResult<T> : ResultBase
    {
        public T Error { get; }

        public ErrorResult() { }
        public ErrorResult(T error, int statusCode) : base(statusCode)
        {
            Error = error;
        }
    }

    public class ErrorResult : ErrorResult<string>
    {
        public ErrorResult() { }
        public ErrorResult(string message, int statusCode) : base(message, statusCode)
        {
        }
    }

    public class DataResult<T> : SuccessResult
    {
        public T Data { get; set; }

        public DataResult() { }
        public DataResult(T data, string message) : base(message)
        {
            Data = data;
        }
    }

    public class CustomMessageResult : ResultBase
    {
        public CustomMessage CustomMessage { get; set; }

        public CustomMessageResult(CustomMessage customMessage)
        {
            CustomMessage = customMessage;
        }
    }

    public class ModelErrorResult : ErrorResult<ImmutableDictionary<string, string[]>>
    {
        public ModelErrorResult() { }
        public ModelErrorResult(ModelError error) : base(error.ToDictionary().ToImmutableDictionary(), StatusCodes.Status400BadRequest)
        {

        }

        public ModelErrorResult(Dictionary<string, string[]> error) : base(error.ToImmutableDictionary(), StatusCodes.Status400BadRequest)
        {

        }
    }

    public class ModelError : Dictionary<string, List<string>>
    {
        //public ModelError():base() { }

        public void Add(string field, string message)
        {
            if (ContainsKey(field))
            {
                if (!this[field].Contains(message))
                {
                    this[field].Add(message);
                }
            }
            else
            {
                Add(field, new List<string>() { message });
            }
        }

        public void AddRange(string field, List<string> messages)
        {
            messages.ForEach(m => Add(field, m));
        }

        /// <summary>
        /// Remove Specific error message in a field
        /// </summary>
        /// <param name="field"></param>
        /// <param name="message"></param>
        public void Remove(string field, string message)
        {
            if (ContainsKey(field) && this[field].Contains(message))
            {
                this[field].Remove(message);
            }
        }

        public Dictionary<string, string[]> ToDictionary()
        {
            return this.Select(err => new KeyValuePair<string, string[]>(err.Key, err.Value.ToArray()))
                       .ToDictionary(err => err.Key, err => err.Value);
        }

    }

    public class CustomMessage
    {
        public CustomMessage(string title, string message, string okayButton, string cancelButton, int statusCode)
        {
            Title = title;
            Message = message;
            OkayButton = okayButton;
            CancelButton = cancelButton;
            StatusCode = statusCode;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public string OkayButton { get; set; }
        public string CancelButton { get; set; }
        public int StatusCode { get; set; }
    }
}

