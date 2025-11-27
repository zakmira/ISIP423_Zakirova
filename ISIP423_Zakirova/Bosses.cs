using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class VVG : Goblin
    {
        public VVG()
        {
            Name = "ВВГ (раса Гоблин)";
            HP = (int)(30 * 2.0);
            Attack = (int)(15 * 1.5);
            Defense = (int)(5 * 1.2);
        }
        public override int CalculateDamage(Player player, Random random)
        {
            bool isCrit = random.NextDouble() < (0.2 + 0.1); // 20% (обычный) + 10% (бонус босса)
            return isCrit ? Attack * 2 : Attack;
        }
    }

    public class Kovalsky : Skeleton
    {
        public Kovalsky()
        {
            Name = "Ковальский (раса Скелет)";
            HP = (int)(25 * 2.5);
            Attack = (int)(12 * 1.3);
            Defense = (int)(8 * 1.4);
        }
    }

    public class ArchmageCPP : Mage
    {
        public ArchmageCPP()
        {
            Name = "Архимаг C++ (раса Маг)";
            HP = (int)(20 * 1.8);
            Attack = (int)(18 * 1.6);
            Defense = (int)(3 * 1.1);
        }
        public override void ApplySpecialEffect(Player player, Random random)
        {
            if (random.NextDouble() < (0.25 + 0.10)) // 25% (обычный) + 10% (бонус босса)
            {
                player.IsFrozen = true;
            }
        }
    }

    public class PestovCS : Skeleton
    {
        private double freezeChance = 0.25 + 0.15; // 25% (обычный маг) + 15% (бонус босса)
        public PestovCS()
        {
            Name = "Пестов С-- (раса Скелет)";
            HP = (int)(25 * 1.3);
            Attack = (int)(12 * 1.8);
            Defense = (int)(8 * 0.6);
        }
        public override int CalculateDamage(Player player, Random random)
        {
            return Attack; // игнор защиты
        }
        public override void ApplySpecialEffect(Player player, Random random)
        {
            if (random.NextDouble() < freezeChance)
            {
                player.IsFrozen = true;
            }
        }
    }
}