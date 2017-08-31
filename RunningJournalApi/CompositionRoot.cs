using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Simple.Data;

namespace RunningJournalApi
{
    public class CompositionRoot : IHttpControllerActivator
    {
        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var db = CreateDb();
            return new JournalController(
                new SimpleWebTokenUserNameProjection(),
                new JournalEntriesQuery(db),
                new AddJournalEntryCommand(db));
        }

        private static dynamic CreateDb()
        {
            var connStr =
                ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            return Database.OpenConnection(connStr);
        }
    }
}