using Application.Domain.Interfaces;
using Application.Infrastructure.Repository;
using Application.Infrastructure.Repository.RavenDB;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Infrastructure
{
    public static class DependencyInjectionModuleInfra
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddRavenDB();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
        }
    }
}
