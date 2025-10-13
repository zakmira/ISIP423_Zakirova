using System;
using System.Collections.Generic;

namespace TextRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    // Типы врагов
    public enum EnemyType
    {
        Goblin,
        Skeleton,
        Mage
    }

    // Типы боссов
    public enum BossType
    {
        VVG,
        Kovalsky,
        ArchmageCPP,
        PestovCS
    }

    // Игрок
    public class Player
    {
        public int MaxHP { get; set; } = 100;
        public int CurrentHP { get; set; } = 100;
        public Weapon EquippedWeapon { get; set; }
        public Armor EquippedArmor { get; set; }
        public bool IsFrozen { get; set; } = false;

        public Player()
        {
            // Стартовая экипировка
            EquippedWeapon = new Weapon("Ржавый меч", 10);
            EquippedArmor = new Armor("Потертые доспехи", 5);
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

    public abstract class Enemy
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; } // Защита

        public abstract int CalculateDamage(Player player, Random random);
        public abstract void ApplySpecialEffect(Player player, Random random);

        public bool IsAlive()
        {
            return HP > 0;
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
        }
    }

    // Обычные враги
    public class Goblin : Enemy
    {
        private double critChance = 0.2; // 20% шанс крита

        public Goblin()
        {
            Name = "Гоблин";
            HP = 30;
            Attack = 15;
            Defense = 5;
        }

        public override int CalculateDamage(Player player, Random random)
        {
            bool isCrit = random.NextDouble() < critChance;
            return isCrit ? Attack * 2 : Attack;
        }

        public override void ApplySpecialEffect(Player player, Random random) { }
    }

    public class Skeleton : Enemy
    {
        public Skeleton()
        {
            Name = "Скелет";
            HP = 25;
            Attack = 12;
            Defense = 8;
        }

        public override int CalculateDamage(Player player, Random random)
        {
            return Attack; // Игнорирует защиту
        }

        public override void ApplySpecialEffect(Player player, Random random) { }
    }

    public class Mage : Enemy
    {
        private double freezeChance = 0.25; // 25% шанс заморозки

        public Mage()
        {
            Name = "Маг";
            HP = 20;
            Attack = 18;
            Defense = 3;
        }

        public override int CalculateDamage(Player player, Random random)
        {
            return Attack;
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