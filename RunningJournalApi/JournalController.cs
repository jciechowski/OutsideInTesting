using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RunningJournalApi
{
    public class JournalController : ApiController
    {
        private readonly IAddJournalEntryCommand _addCommand;
        private readonly IJournalEntriesQuery _journalEntriesQuery;

        public JournalController(IJournalEntriesQuery journalEntriesQuery, IAddJournalEntryCommand addCommand)
        {
            _journalEntriesQuery = journalEntriesQuery;
            _addCommand = addCommand;
        }

        public HttpResponseMessage Get()
        {
            var userName = GetUserName();
            var entries = _journalEntriesQuery.GetJournalEntries(userName);

            return Request.CreateResponse(HttpStatusCode.OK, new JournalModel
            {
                Entries = entries.ToArray()
            });
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            var userName = GetUserName();

            _addCommand.AddJournalEntry(journalEntry, userName);

            return Request.CreateResponse();
        }

        private string GetUserName()
        {
            SimpleWebToken.TryParse(Request.Headers.Authorization.Parameter, out var swt);
            var userName = swt.Single(c => c.Type == "userName").Value;
            return userName;
        }
    }
}