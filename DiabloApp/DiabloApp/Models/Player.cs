using System;
using System.Collections.Generic;
using System.Text;

namespace DiabloApp.Models
{
    public class Player
    {
        public string Name { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
        public ICollection<Character> Characters { get; set; }
    }
}
