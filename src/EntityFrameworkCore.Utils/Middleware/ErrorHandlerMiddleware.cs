using Microsoft.AspNetCore.Http;
using System.Net;

namespace Utils.EF.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
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

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //Log.Error(exception, "Unhandled exception");

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                error = new
                {
                    message = "An unexpected error occurred.",
                    detailed = exception.Message
                }
            });

            await context.Response.WriteAsync(result);
        }
    }
}
