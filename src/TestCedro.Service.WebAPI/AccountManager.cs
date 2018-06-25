using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TestCedro.Service.WebAPI
{
    public class Account
    {
        public static Guid CurrentUserId => Guid.Parse(ClaimsIdentityRepository.GetByType("id"));
        public static bool IsAuthenticated => ClaimsIdentityRepository.IsAuthenticated();
        public static string CurrentUserName => ClaimsIdentityRepository.GetByType(ClaimTypes.GivenName);
        public static bool IsInRole(string role) => ClaimsIdentityRepository.IsInRole(role);
    }

    public static class HttpContext
    {
        private static IHttpContextAccessor _contextAccessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
    }

    public static class StaticHttpContextExtensions
    {
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContext.Configure(httpContextAccessor);
            return app;
        }
    }
}