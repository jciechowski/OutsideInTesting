using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Simple.Data;

namespace RunningJournalApi
{
    public class JournalController : ApiController
    {
        private readonly IJournalEntriesQuery _journalEntriesQuery;

        public JournalController(IJournalEntriesQuery journalEntriesQuery)
        {
            _journalEntriesQuery = journalEntriesQuery;
        }

        public HttpResponseMessage Get()
        {
            var userName = GetUserName();
            var connStr = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connStr);

            var entries = db.JournalEntry.FindAll(db.JournalEntry.User.Username == userName)
                .ToArray<JournalEntryModel>();
            return Request.CreateResponse(HttpStatusCode.OK, new JournalModel
            {
                Entries = entries
            });
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            var userName = GetUserName();
            var connStr = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connStr);

            var userId = db.User.Insert(Username: userName).UserId;
            db.JournalEntry.Insert(
                UserId: userId,
                Time: journalEntry.Time,
                Distance: journalEntry.Distance,
                Duration: journalEntry.Duration);
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