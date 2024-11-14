using Rewards.Business.Exceptions;
using System.Net;

namespace Rewards.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An unexpected error occurred. Please try again later.",
                Details = ex.Message
            };

            switch (ex)
            {
                case NotFoundException notFoundEx:
                    response = new
                    {
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = notFoundEx.Message,
                        Details = "The requested resource was not found."
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case NotValidException notValidEx:
                    response = new
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = notValidEx.Message,
                        Details = "Validation failed. Please check your input."
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case InvalidFileFormatException invalidFileEx:
                    response = new
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = invalidFileEx.Message,
                        Details = "The uploaded file format is not supported."
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case BadRequestException badRequestEx:
                    response = new
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = badRequestEx.Message,
                        Details = "Please ensure that all required fields are provided and properly formatted.."
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;


                default:
                    break;
            }

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
