using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Simple.Data;

namespace RunningJournalApi
{
    public class JournalController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var connStr = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connStr);

            var entries = db.JournalEntry.FindAll(db.JournalEntry.User.Username == "foo").ToArray<JournalEntryModel>();
            return Request.CreateResponse(HttpStatusCode.OK, new JournalModel
            {
                Entries = entries
            });
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            var connStr = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connStr);

            var userId = db.User.Insert(Username: "foo").UserId;
            db.JournalEntry.Insert(
                UserId: userId,
                Time: journalEntry.Time,
                Distance: journalEntry.Distance,
                Duration: journalEntry.Duration);
            return Request.CreateResponse();
        }
    }
}