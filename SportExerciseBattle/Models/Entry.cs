
namespace SportExerciseBattle.Models
{
    public class Entry
    {
        public string Username { get; set; } = "";
        public string EntryName { get; set; } = "";
        public int Count { get; set; } = 0;
        public int DurationInSeconds { get; set; } = 0;
        public DateTime Timestamp { get; set; } = DateTime.Now;

    }
}
