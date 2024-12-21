using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Application.Domain.Interfaces
{
    public interface IServiceRavenDB
    {
        IDocumentStore Store { get; set; }
        IDocumentSession Session { get; set; }
        IAsyncDocumentSession AsyncSession { get; set; }
    }
}
