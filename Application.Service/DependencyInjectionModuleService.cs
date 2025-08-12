using Application.Domain.Interfaces;
using Application.Domain.Models;
using Application.Domain.Validators;
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
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<IValidator<User>, UserValidator>();
        }
    }
}
