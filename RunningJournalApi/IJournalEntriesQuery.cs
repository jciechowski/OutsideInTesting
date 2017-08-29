using System.Collections.Generic;

namespace RunningJournalApi
{
    public interface IJournalEntriesQuery
    {
        IEnumerable<JournalEntryModel> GetJournalEntries(string userName);
    }
}