using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IOT_API.Filters;

public class HttpResponseException : Exception
{
    public HttpResponseException(HttpStatusCode statusCode, object? value = null) =>
        (StatusCode, Value) = ((int)statusCode, value);

    public int StatusCode { get; }

    public object? Value { get; }
}

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is HttpResponseException httpResponseException)
        {
            if (httpResponseException.Value is null)
            {
                context.Result = new ObjectResult(new
                {
                    title = GetErrorString(httpResponseException.StatusCode),
                    status = httpResponseException.StatusCode
                })
                {
                    StatusCode = httpResponseException.StatusCode
                };
            }
            else
            {
                context.Result = new ObjectResult(httpResponseException.Value)
                {
                    StatusCode = httpResponseException.StatusCode
                };
            }

            context.ExceptionHandled = true;
        }
    }

    private static string GetErrorString(int statusCode)
    {
        if (Enum.IsDefined(typeof(HttpStatusCode), statusCode))
        {
            HttpStatusCode httpStatusCode = (HttpStatusCode)statusCode;
            return httpStatusCode.ToString();
        }
        else
        {
            return "Unknown";
        }
    }
}
