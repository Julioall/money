using Application.Domain.Interface;
using Application.Domain.Model;
using Application.Domain.Validator;
using Application.Infrastructure.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Service
{
    public static class DependencyInjectionModuleService
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<IValidator<User>, UserValidator>();
        }
    }
}
