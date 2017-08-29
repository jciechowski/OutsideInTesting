using System.Collections.Generic;

namespace RunningJournalApi
{
    public class JournalEntriesQuery : IJournalEntriesQuery
    {
        private readonly dynamic _db;

        public JournalEntriesQuery(dynamic db)
        {
            _db = db;
        }

        public IEnumerable<JournalEntryModel> GetJournalEntries(string userName)
        {
            return _db.JournalEntry.FindAll(_db.JournalEntry.User.Username == userName)
                .ToArray<JournalEntryModel>();
        }
    }
}