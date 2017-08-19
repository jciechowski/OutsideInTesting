using System.Net.Http;
using System.Web.Http;

namespace RunningJournalApi
{
    public class JournalController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse();
        }
    }
}