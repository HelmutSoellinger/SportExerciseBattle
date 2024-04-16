using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportExerciseBattle.Models
{
    public class Tournament
    {
        public bool IsRunning { get; set; }
        public DateTime StartTime { get; set; }
        public string LeadingUser { get; set; } = "";
    }
}
