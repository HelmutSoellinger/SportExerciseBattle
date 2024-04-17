using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportExerciseBattle.Models
{
    public class Tournament
    {
        private static readonly Lazy<Tournament> LazyInstance = new Lazy<Tournament>(() => new Tournament());

        private Tournament() { }
        public static Tournament Instance => LazyInstance.Value;
        public bool IsRunning { get; set; } = false;
        public DateTime StartTime { get; set; }
        public string LeadingUser { get; set; } = "";
        public List<string> Participants { get; set; } = new List<string>();
    }
    //turn into an singleton, unique feature maybe counting wins draws and loses and tournament record for each user
}
