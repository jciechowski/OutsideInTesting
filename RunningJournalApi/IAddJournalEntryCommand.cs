namespace RunningJournalApi
{
    public interface IAddJournalEntryCommand
    {
        void AddJournalEntry(JournalEntryModel journalEntry, string userName);
    }
}