
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VotingSystem.API.ExceptionMiddleware
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = exception switch
            {
                KeyNotFoundException => new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found",
                    Detail = exception.Message
                },
                InvalidOperationException => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Bad Request",
                    Detail = exception.Message
                },
                UnauthorizedAccessException => new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Forbidden",
                    Detail = exception.Message
                },
                _ => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred."
                }
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}


//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using Microsoft.AspNetCore.Diagnostics;
//using System.ComponentModel.DataAnnotations;
//using VotingSystem.API.Exceptions;

//namespace VotingSystem.API.ExceptionMiddleware
//{
//    internal sealed class GlobalExceptionHandler: IExceptionHandler
//    {
//        private readonly ILogger<GlobalExceptionHandler> _logger;
//        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
//        {
//            _logger = logger;
//        }
//        public async ValueTask<bool> TryHandleAsync(
//          HttpContext httpContext,
//          Exception exception,
//          CancellationToken cancellationToken
//            )
//        {
//            _logger.LogError(exception, "Exception occured :{Message}", exception.Message);
//            var statusCode = exception switch
//            {
//                NotFoundException => (int)HttpStatusCode.NotFound,
//                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
//                ValidationException => (int)HttpStatusCode.BadRequest,
//                _ => (int)HttpStatusCode.InternalServerError
//            };

//            var problemDetails = new ProblemDetails
//            {
//                Status = statusCode,
//                Title = GetTitleForException(exception),
//                Detail = exception.Message,
//            };

//            httpContext.Response.StatusCode = statusCode;
//            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
//            return true;
//        }

//        private static string GetTitleForException(Exception exception)
//        {
//            return exception switch
//            {
//                NotFoundException => "Resource not found",
//                UnauthorizedException => "Unauthorized access",
//                ValidationException => "Validation error",
//                _ => "Server error"
//            };

//        }
//    }
//}
