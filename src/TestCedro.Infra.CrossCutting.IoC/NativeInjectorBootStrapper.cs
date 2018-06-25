using TestCedro.Domain.Interfaces;
using TestCedro.Domain.Interfaces.Repositories;
using TestCedro.Domain.Interfaces.Services;
using TestCedro.Domain.Services;
using TestCedro.Infra.Data.Context;
using TestCedro.Infra.Data.Repositories;
using TestCedro.Infra.Data.UoW;
//using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TestCedro.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Application

            // Domain
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IUserService, UserService>();
          
            // Infra - Data
            services.AddScoped<IDishRepository, DishRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
         
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<MainContext>();
        }
    }
}