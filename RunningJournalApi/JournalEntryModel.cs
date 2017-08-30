using System;

namespace RunningJournalApi
{
    public class JournalEntryModel : IEquatable<JournalEntryModel>
    {
        public DateTimeOffset Time { get; set; }
        public int Distance { get; set; }
        public TimeSpan Duration { get; set; }

        public bool Equals(JournalEntryModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Time.Equals(other.Time) && Distance == other.Distance && Duration.Equals(other.Duration);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((JournalEntryModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Time.GetHashCode();
                hashCode = (hashCode * 397) ^ Distance;
                hashCode = (hashCode * 397) ^ Duration.GetHashCode();
                return hashCode;
            }
        }
    }
}