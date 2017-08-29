namespace RunningJournalApi
{
    public class AddJournalEntryCommand : IAddJournalEntryCommand
    {
        private readonly dynamic _db;

        public AddJournalEntryCommand(dynamic db)
        {
            _db = db;
        }

        public void AddJournalEntry(JournalEntryModel journalEntry, string userName)
        {
            var userId = _db.User
                .FindAllByUserName(userName)
                .Select(_db.User.UserId)
                .ToScalarOrDefault<int>();
            if (userId == 0)
                userId = _db.User.Insert(UserName: userName).UserId;

            _db.JournalEntry.Insert(
                UserId: userId,
                Time: journalEntry.Time,
                Distance: journalEntry.Distance,
                Duration: journalEntry.Duration);
        }
    }
}