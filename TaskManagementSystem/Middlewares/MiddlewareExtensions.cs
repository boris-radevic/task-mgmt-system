namespace TaskManagementSystem.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpExceptionMiddleware>();
        }
    }
}
