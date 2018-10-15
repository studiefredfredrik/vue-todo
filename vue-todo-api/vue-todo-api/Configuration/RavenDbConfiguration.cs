using System;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace VueTodoApi.Configuration
{
    public static class RavenDbConfiguration
    {
        public static IDocumentStore Configure(RavenDbSettings settings)
        {
            var documentStore = new DocumentStore
            {
                Urls = settings.Urls,
                Database = settings.DatabaseName,
            };

            documentStore.Initialize();
            
            try {
                documentStore.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(documentStore.Database)));
            } catch(Exception) {
                // database exists
            }

            return documentStore;
        }
    }

    public class RavenDbSettings
    {
        public string[] Urls { get; set; }
        public string DatabaseName { get; set; }
    }
}
