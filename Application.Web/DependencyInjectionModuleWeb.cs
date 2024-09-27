using Application.Domain.Interface;
using Application.Infrastructure.Repository;
using Application.Service.Service;

namespace Application.Api
{
    public static class DependencyInjectionModuleWeb
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<UserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<UserService, UserService>();
            services.AddScoped<TokenService, TokenService>();
        }
    }
}
