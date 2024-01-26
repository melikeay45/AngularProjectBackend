using AngularProject.CORE.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularProject.API.Models
{
    public class ApiResult<T> : Result
    {
        public ApiResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }

        public ApiResult(T data, bool success) : base(success)
        {
            Data = data;
        }


        public T Data { get; }
    }

    public class Result
    {
        public bool Success { get; }
        public string Message { get; }

        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public Result(bool success)
        {
            Success = success;
            Message = string.Empty;
        }
    }

}