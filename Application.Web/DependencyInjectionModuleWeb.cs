using Application.Domain.Interfaces;
using Application.Infrastructure.Repository;
using Application.Service;

namespace Application.Web
{
    public static class DependencyInjectionModuleWeb
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<AuthenticationService>();
            services.AddScoped<UserService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<AccountService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
