using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG;

namespace TextRPG
{
    public class Player
    {
        public int MaxHP { get; set; } = 100;
        public int CurrentHP { get; set; } = 100;
        public Weapon EquippedWeapon { get; set; }
        public Armor EquippedArmor { get; set; }
        public bool IsFrozen { get; set; } = false;
        public bool IsDefending { get; set; } = false;
        public int BlockAmount { get; set; } = 0;

        public Player()
        {
            EquippedWeapon = new Weapon("Ржавый меч", 10);
            EquippedArmor = new Armor("Потертые доспехи", 5);
        }

        public void ApplyDamage(int rawDamage, Random random)
        {
            int damageToTake = rawDamage;
            if (IsDefending)
            {
                if (random.NextDouble() < 0.4)
                {
                    Console.WriteLine("Вы уклонились!");
                    return;
                }
                damageToTake -= BlockAmount;
                if (damageToTake < 0) damageToTake = 0;
                Console.WriteLine($"Вы блокируете {BlockAmount} урона. Получено: {damageToTake}.");
            }
            TakeDamage(damageToTake);
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0) CurrentHP = 0;
        }

        public void Heal()
        {
            CurrentHP = MaxHP;
        }

        public bool IsAlive()
        {
            return CurrentHP > 0;
        }
    }
}