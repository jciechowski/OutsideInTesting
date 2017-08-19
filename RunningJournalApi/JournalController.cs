using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RunningJournalApi
{
    public class JournalController : ApiController
    {
        private static readonly List<JournalEntryModel> entries = new List<JournalEntryModel>();

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new JournalModel {Entries = entries.ToArray()});
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            entries.Add(journalEntry);
            return Request.CreateResponse();
        }
    }
}