// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.AspNetCore.Builder;

namespace Softeq.NetKit.Auth.Web.ExceptionHandling
{
    public static class ExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}