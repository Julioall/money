using Raven.Client.Documents.Session;
using Raven.Client.Documents;
using Application.Domain.Interfaces;

namespace Application.Domain.Models
{
    public class ServiceRavenDB : IServiceRavenDB
    {
        public IDocumentStore Store { get; set; }
        public IDocumentSession Session { get; set; }
        public IAsyncDocumentSession AsyncSession { get; set; }
    }
}
