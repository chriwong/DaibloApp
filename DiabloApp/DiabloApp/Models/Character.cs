using System;
using System.Collections.Generic;
using System.Text;

namespace DiabloApp.Models
{
    public class Character
    {
        public string Name { get; set; }
        public CharacterClass CharacterClass { get; set; }
        public uint Level { get; set; }
        public string Location { get; set; }
    }

    public enum CharacterClass
    {
        Barbarian = 1,
        Necromancer = 2,
        Mage = 3,
        Rogue = 4,
        Druid = 5
    }
}
