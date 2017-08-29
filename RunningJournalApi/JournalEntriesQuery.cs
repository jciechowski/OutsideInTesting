using System.Collections.Generic;

namespace RunningJournalApi
{
    public class JournalEntriesQuery : IJournalEntriesQuery
    {
        public IEnumerable<JournalEntryModel> GetJournalEntries(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}