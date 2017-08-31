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
        private readonly IUserNameProjection _userNameProjection;

        public JournalController(IUserNameProjection userNameProjection, IJournalEntriesQuery journalEntriesQuery,
            IAddJournalEntryCommand addCommand)
        {
            _userNameProjection = userNameProjection;
            _journalEntriesQuery = journalEntriesQuery;
            _addCommand = addCommand;
        }

        public HttpResponseMessage Get()
        {
            var userName = _userNameProjection.GetUserName(Request);
            if (userName == null)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "No user name was supplied.");

            var entries = _journalEntriesQuery.GetJournalEntries(userName);

            return Request.CreateResponse(HttpStatusCode.OK, new JournalModel
            {
                Entries = entries.ToArray()
            });
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            var userName = _userNameProjection.GetUserName(Request);
            if (userName == null)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "No user name was supplied.");

            _addCommand.AddJournalEntry(journalEntry, userName);

            return Request.CreateResponse();
        }
    }
}