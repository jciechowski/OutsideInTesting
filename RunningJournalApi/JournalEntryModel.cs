using System;

namespace RunningJournalApi
{
    public class JournalEntryModel
    {
        public DateTimeOffset Time { get; set; }
        public int Distance { get; set; }
        public TimeSpan Duration { get; set; }
    }
}