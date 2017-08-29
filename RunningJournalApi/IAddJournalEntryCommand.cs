namespace RunningJournalApi
{
    public interface IAddJournalEntryCommand
    {
        void AddJournalEntry(JournalEntryModel journal, string userName);
    }
}