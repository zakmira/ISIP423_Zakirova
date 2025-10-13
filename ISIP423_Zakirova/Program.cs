using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

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

    // Боссы
    public class VVG : Goblin
    {
        public VVG()
        {
            Name = "ВВГ (раса Гоблин)";
            HP = (int)(30 * 2.0);
            Attack = (int)(15 * 1.5);
            Defense = (int)(5 * 1.2);
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
    }

    public class PestovCS : Skeleton
    {
        private double freezeChance = 0.4; // 25% + 15%

        public PestovCS()
        {
            Name = "Пестов С-- (раса Скелет)";
            HP = (int)(25 * 1.3);
            Attack = (int)(12 * 1.8);
            Defense = (int)(8 * 0.6);
        }

        public override void ApplySpecialEffect(Player player, Random random)
        {
            if (random.NextDouble() < freezeChance)
            {
                player.IsFrozen = true;
            }
        }
    }

    // Предметы
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

    // Сама игра
    public class Game
    {
        private Player player;
        private Random random;
        private int turnCount;

        // Списки для случайного врага
        private readonly EnemyType[] enemyTypes = { EnemyType.Goblin, EnemyType.Skeleton, EnemyType.Mage };
        private readonly BossType[] bossTypes = { BossType.VVG, BossType.Kovalsky, BossType.ArchmageCPP, BossType.PestovCS };

        // Возможные предметы для сундуков
        private readonly Weapon[] possibleWeapons = {
            new Weapon("Острый меч", 15),
            new Weapon("Топор воина", 20),
            new Weapon("Посох мага", 25),
            new Weapon("Легендарный клинок", 35)
        };

        private readonly Armor[] possibleArmors = {
            new Armor("Кожаные доспехи", 8),
            new Armor("Кольчуга", 12),
            new Armor("Латные доспехи", 18),
            new Armor("Магические доспехи", 25)
        };

        public Game()
        {
            player = new Player();
            random = new Random();
            turnCount = 0;
        }

        public void Start()
        {
            Console.WriteLine("Добро пожаловать в игру!");
            Console.WriteLine("Введите цифры для выбора действий");

            while (player.IsAlive())
            {
                turnCount++;
                Console.WriteLine($"\nХод {turnCount}");
                Console.WriteLine($"Здоровье: {player.CurrentHP}/{player.MaxHP}");
                Console.WriteLine($"Оружие: {player.EquippedWeapon.Name} (урон: {player.EquippedWeapon.Damage})");
                Console.WriteLine($"Доспехи: {player.EquippedArmor.Name} (защита: {player.EquippedArmor.Defense})");

                // Каждые 10 ходов - босс
                if (turnCount % 10 == 0)
                {
                    Console.WriteLine("\nВнимание!!! Босс");
                    FightBoss();
                }
                else
                {
                    // 50/50 шанс на сундук или врага
                    if (random.Next(2) == 0)
                    {
                        FightEnemy();
                    }
                    else
                    {
                        OpenChest();
                    }
                }

                // Проверка на победу (условно - после 30 ходов)
                if (turnCount >= 30)
                {
                    Console.WriteLine("\nВы прошли игру!");
                    break;
                }

                if (!player.IsAlive())
                {
                    Console.WriteLine("\nИгра окончена! Вы погибли :(");
                    break;
                }
            }
        }

        private void FightEnemy()
        {
            Enemy enemy = CreateRandomEnemy();
            Console.WriteLine($"\nПоявляется враг: {enemy.Name} (HP: {enemy.HP}, атака: {enemy.Attack}, защита: {enemy.Defense})");

            while (enemy.IsAlive() && player.IsAlive())
            {
                PlayerTurn(enemy);
                if (!enemy.IsAlive()) break;

                EnemyTurn(enemy);
            }

            if (!player.IsAlive()) return;

            Console.WriteLine($"Вы победили {enemy.Name}!");
        }

        private void FightBoss()
        {
            Enemy boss = CreateRandomBoss();
            Console.WriteLine($"\nПоявляется Босс: {boss.Name} (HP: {boss.HP}, атака: {boss.Attack}, защита: {boss.Defense})");

            while (boss.IsAlive() && player.IsAlive())
            {
                PlayerTurn(boss);
                if (!boss.IsAlive()) break;

                EnemyTurn(boss);
            }

            if (!player.IsAlive()) return;

            Console.WriteLine($"Вы победили Босса {boss.Name}!");
        }

        private void PlayerTurn(Enemy enemy)
        {
            if (player.IsFrozen)
            {
                Console.WriteLine("Вы заморожены и пропускаете ход!");
                player.IsFrozen = false;
                return;
            }

            Console.WriteLine("\nВаш ход:");
            Console.WriteLine("1 - Атаковать");
            Console.WriteLine("2 - Защищаться");

            int choice = GetPlayerChoice(1, 2);

            if (choice == 1)
            {
                int damage = player.EquippedWeapon.Damage;
                enemy.TakeDamage(damage);
                Console.WriteLine($"Вы нанесли {damage} урона {enemy.Name}!");
            }
            else
            {
                // Защита - шанс увернуться 40%
                if (random.NextDouble() < 0.4)
                {
                    Console.WriteLine("Вы успешно уклонились от атаки!");
                    return;
                }
                else
                {
                    // Блокирование урона (70-100% от защиты)
                    double blockPercent = 0.7 + random.NextDouble() * 0.3;
                    int blockedDamage = (int)(player.EquippedArmor.Defense * blockPercent);
                    Console.WriteLine($"Вы блокируете {blockedDamage} урона в следующей атаке!");
                    // Здесь блокирование будет учтено при расчете урона врага
                }
            }

            Console.WriteLine($"{enemy.Name}: HP {Math.Max(0, enemy.HP)}/{enemy.HP + damage}");
        }

        private void EnemyTurn(Enemy enemy)
        {
            Console.WriteLine($"\nХод {enemy.Name}:");

            // Враг всегда атакует
            int damage = enemy.CalculateDamage(player, random);

            // Применяем особые эффекты врага
            enemy.ApplySpecialEffect(player, random);

            player.TakeDamage(damage);
            Console.WriteLine($"{enemy.Name} наносит вам {damage} урона!");

            if (player.IsFrozen)
            {
                Console.WriteLine("Враг заморозил вас! Вы пропустите следующий ход.");
            }
        }
    }
}