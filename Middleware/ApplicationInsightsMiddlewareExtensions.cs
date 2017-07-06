using Microsoft.AspNetCore.Builder;

namespace TodoApi.Middleware
{
    public static class ApplicationInsightsMiddlewareExtensions
    {
        public static IApplicationBuilder UseApplicationInsightsExceptions(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApplicationInsightsMiddleware>();
        }
    }
}