using System.Net;
using TaskManagementSystem.Exceptions;

namespace TaskManagementSystem.Middlewares
{
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException brex)
            {
                await HandleExceptionAsync(context, brex, HttpStatusCode.BadRequest);
            }
            catch (NotFoundException nfex)
            {
                await HandleExceptionAsync(context, nfex, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);

            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
