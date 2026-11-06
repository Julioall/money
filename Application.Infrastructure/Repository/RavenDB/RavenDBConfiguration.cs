using Application.Domain.Interfaces;
using Application.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Exceptions;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System.Security.Cryptography.X509Certificates;

namespace Application.Infrastructure.Repository.RavenDB
{
    public static class RavenDBConfiguration
    {
        public static void AddRavenDB(this IServiceCollection services)
        {
            if (string.IsNullOrEmpty(ApplicationConstants.DATABASE_URLS))
            {
                throw new ApplicationException("The RavenDB URL has not been configured correctly. Check the environment variable 'RAVENDBSETTINGS_URLS'.");
            }

            if (string.IsNullOrEmpty(ApplicationConstants.DATABASE_NAME))
            {
                throw new ApplicationException("The name of the RavenDB database has not been configured correctly. Check the environment variable 'RAVENDBSETTINGS_DATABASE_NAME'.");
            }

            var certificate = GetCertificateFromStore();

            var documentStore = new DocumentStore
            {
                Urls = new[] { ApplicationConstants.DATABASE_URLS },
                Database = ApplicationConstants.DATABASE_NAME,
                Certificate = certificate,
            };

            documentStore.Initialize();
            EnsureDatabaseExists(documentStore, ApplicationConstants.DATABASE_NAME);

            services.AddSingleton<IServiceRavenDB>(_ => new ServiceRavenDB
            {
                Store = documentStore,
                Session = documentStore.OpenSession()
            });

            services.AddScoped<IUserRepository, UserRepository>();
        }

        private static X509Certificate2? GetCertificateFromStore()
        {
            if (string.IsNullOrWhiteSpace(ApplicationConstants.CERTIFICATE_SUBJECT))
            {
                return null;
            }

            using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            var certs = store.Certificates.Find(
                X509FindType.FindBySubjectDistinguishedName,
                ApplicationConstants.CERTIFICATE_SUBJECT,
                validOnly: false);

            if (certs.Count == 0)
            {
                throw new InvalidOperationException($"Certificate with subject '{ApplicationConstants.CERTIFICATE_SUBJECT}' not found in the LocalMachine certificate store.");
            }

            var certificate = certs[0];
            if (!certificate.HasPrivateKey)
            {
                throw new InvalidOperationException($"Certificate with subject '{ApplicationConstants.CERTIFICATE_SUBJECT}' does not contain a private key. Ensure the certificate is imported with its private key.");
            }

            return certificate;
        }

        private static void EnsureDatabaseExists(IDocumentStore documentStore, string databaseName)
        {
            try
            {
                var record = new DatabaseRecord(databaseName);
                documentStore.Maintenance.Server.Send(new CreateDatabaseOperation(record));
            }
            catch (ConcurrencyException)
            {
                // Database already exists.
            }
        }
    }
}
