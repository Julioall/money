using Application.Domain.Interfaces;
using Application.Domain.Models;
using Application.Domain.Validators;
using Application.Infrastructure.Repository;
using Application.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Embedded;
using Raven.TestDriver;

namespace Application.Test.Setup
{
    public class BaseTest : RavenTestDriver
    {
        public IServiceCollection _services;
        public ServiceProvider _serviceProvider;
        public IDocumentStore _store;
        public IDocumentSession _session;

        private static bool IsAlreadyConfigured { get; set; }

        public BaseTest()
        {
            InitializeDataBase();
            _services = InicializeServices();

            _services.AddScoped<IServiceRavenDB>((provider) => new ServiceRavenDB
            {
                Store = _store,
                Session = _session
            });

            _serviceProvider = _services.BuildServiceProvider();
        }

        private static ServiceCollection InicializeServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<UserService, UserService>();
            serviceCollection.AddScoped<TransactionService, TransactionService>();
            serviceCollection.AddScoped<IValidator<User>, UserValidator>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<ITransactionRepository, TransactionRepository>();

            return serviceCollection;
        }

        private void InitializeDataBase()
        {
            if (!IsAlreadyConfigured)
            {
                ConfigureServer(new TestServerOptions
                {
                    Licensing = new ServerOptions.LicensingOptions
                    { ThrowOnInvalidOrMissingLicense = false }
                });
                IsAlreadyConfigured = true;
            }

            _store = GetDocumentStore(null, Guid.NewGuid().ToString());
            _session = _store.OpenSession();
        }

        protected override void PreInitialize(IDocumentStore documentStore)
        {
            base.PreInitialize(documentStore);
            documentStore.Conventions.IdentityPartsSeparator = '-';
        }
    }
}
