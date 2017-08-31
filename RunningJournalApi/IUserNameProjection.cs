using System.Net.Http;

namespace RunningJournalApi
{
    public interface IUserNameProjection
    {
        string GetUserName(HttpRequestMessage request);
    }
}