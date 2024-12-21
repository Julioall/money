using Application.Domain.Interfaces;
using Application.Domain.Models;
using Application.Domain.Validators;
using Application.Infrastructure.Repository;
using Application.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Test.Setup
{
    public static class DependencyInjectionModule
    {
        public static void BindService(IServiceCollection services)
        {
            //var embeddedServer = EmbeddedServer.Instance;
            //embeddedServer.StartServer(new ServerOptions
            //{
            //    DataDirectory = "C:\\RavenData",
            //    ServerUrl = "http://127.0.0.1:8080"
            //});

            //var documentStore = embeddedServer.GetDocumentStore("EmbeddedDatabase");
            //services.AddSingleton(documentStore);
        }
    }
}
