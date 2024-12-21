using Application.Domain.Interfaces;
using Application.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using System.Security.Cryptography.X509Certificates;

namespace Application.Infrastructure.Repository.RavenDB
{
    public static class RavenDBConfiguration
    {
        public static void AddRavenDB(this IServiceCollection services)
        {
            // Uncomment this section if you want to use a certificate.
            // var certificate = GetCertificateFromStore();

            if (string.IsNullOrEmpty(ApplicationConstants.DATABASE_URLS))
            {
                throw new ApplicationException("The RavenDB URL has not been configured correctly. Check the environment variable 'RAVENDBSETTINGS_URLS'.");
            }

            if (string.IsNullOrEmpty(ApplicationConstants.DATABASE_NAME))
            {
                throw new ApplicationException("The name of the RavenDB database has not been configured correctly. Check the environment variable 'RAVENDBSETTINGS_DATABASE_NAME'.");
            }

            // Uncomment this section if you want to use a certificate.
            // if (certificate == null)
            // {
            //     throw new ApplicationException("The RavenDB certificate has not been configured correctly. Ensure the certificate is installed in the LocalMachine certificate store.");
            // }

            var documentStore = new DocumentStore
            {
                Urls = new[] { ApplicationConstants.DATABASE_URLS },
                Database = ApplicationConstants.DATABASE_NAME,
                // Uncomment this section if you want to use a certificate.
                // Certificate = certificate,
            };

            documentStore.Initialize();

            //Replace / for -
            //documentStore.Maintenance.Send(
            //new PutClientConfigurationOperation(
            //    new ClientConfiguration { IdentityPartsSeparator = '-' }
            //));

            services.AddSingleton<IServiceRavenDB>((provider) => new ServiceRavenDB
            {
                Store = documentStore,
                Session = documentStore.OpenSession()
            });

            services.AddScoped<IUserRepository, UserRepository>();
        }

        private static X509Certificate2 GetCertificateFromStore()
        {
            if (ApplicationConstants.CERTIFICATE_SUBJECT == null)
            {
                throw new InvalidOperationException("The environment variable RAVENDBSETTINGS_CERTIFICATE_SUBJECT has not been defined.");
            }

            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, ApplicationConstants.CERTIFICATE_SUBJECT, false);
                if (certs.Count > 0)
                {
                    var certificate = certs.FirstOrDefault();
                    if (!certificate.HasPrivateKey)
                    {
                        throw new Exception($"Certificate with subject '{ApplicationConstants.CERTIFICATE_SUBJECT}' does not have a private key.");
                    }
                    return certificate;
                }
                else
                {
                    throw new Exception($"Certificate with subject '{ApplicationConstants.CERTIFICATE_SUBJECT}' not found in the LocalMachine certificate store.");
                }
            }
        }
    }
}
