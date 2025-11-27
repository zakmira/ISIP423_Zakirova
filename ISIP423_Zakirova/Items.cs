using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Weapon
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public Weapon(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }
    }

    public class Armor
    {
        public string Name { get; set; }
        public int Defense { get; set; }
        public Armor(string name, int defense)
        {
            Name = name;
            Defense = defense;
        }
    }

    public class HealthPotion
    {
        public void Use(Player player)
        {
            player.Heal();
        }
    }
}