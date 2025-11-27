using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG;

namespace TextRPG
{
    public class Game
    {
        private Player player;
        private Random random;
        private int turnCount;

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
            Console.WriteLine("Выберите действие");

            while (player.IsAlive())
            {
                turnCount++;
                Console.WriteLine($"\nХод {turnCount}");
                Console.WriteLine($"Здоровье: {player.CurrentHP}/{player.MaxHP}");
                Console.WriteLine($"Оружие: {player.EquippedWeapon.Name} (урон: {player.EquippedWeapon.Damage})");
                Console.WriteLine($"Доспехи: {player.EquippedArmor.Name} (защита: {player.EquippedArmor.Defense})");

                // каждые 10 ходов - босс
                if (turnCount % 10 == 0)
                {
                    Console.WriteLine("\nБосс!!!");
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

                if (turnCount >= 20)
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
            Enemy enemy = MonsterFactory.CreateRandomEnemy(random);
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
            Enemy boss = MonsterFactory.CreateRandomBoss(random);
            Console.WriteLine($"\nПоявляется Босс: {boss.Name} (HP: {boss.HP}, атака: {boss.Attack}, защита: {boss.Defense})");

            while (boss.IsAlive() && player.IsAlive())
            {
                PlayerTurn(boss);
                if (!boss.IsAlive()) break;
                EnemyTurn(boss);
            }

            if (!player.IsAlive()) return;
            Console.WriteLine($"Вы победили {boss.Name}!");
        }

        private void PlayerTurn(Enemy enemy)
        {
            if (player.IsFrozen)
            {
                Console.WriteLine("Вы пропускаете ход из-за заморозки!");
                player.IsFrozen = false;
                return;
            }

            Console.WriteLine("1 - Атака");
            Console.WriteLine("2 - Защита");
            Console.WriteLine("\nВаш ход ");
            int choice = GetPlayerChoice(1, 2);

            if (choice == 1)
            {
                int damage = player.EquippedWeapon.Damage;
                enemy.TakeDamage(damage);
                Console.WriteLine($"Вы нанесли {damage} урона {enemy.Name}!");
            }
            else // choice == 2
            {
                // защита - шанс увернуться 40%
                if (random.NextDouble() < 0.4)
                {
                    Console.WriteLine("Вы уклонились от атаки!");
                    return;
                }
                else
                {
                    // блок урона (70-100% от защиты)
                    double blockPercent = 0.7 + random.NextDouble() * 0.3;
                    int blockedDamage = (int)(player.EquippedArmor.Defense * blockPercent);
                    Console.WriteLine($"Вы блокируете {blockedDamage} урона в следующей атаке!");
                }
            }
        }

        private void EnemyTurn(Enemy enemy)
        {
            Console.WriteLine($"\nХод {enemy.Name}:");
            int damage = enemy.CalculateDamage(player, random);
            enemy.ApplySpecialEffect(player, random);

            if (player.IsDefending)
            {
                int damage1 = enemy.CalculateDamage(player, random);
                player.ApplyDamage(damage, random);
                enemy.ApplySpecialEffect(player, random);
                player.IsDefending = false;
                player.BlockAmount = 0;
            }
            else
            {
                player.TakeDamage(damage);
                enemy.ApplySpecialEffect(player, random);
            }
            Console.WriteLine($"{enemy.Name} наносит вам {damage} урона!");
            if (player.IsFrozen)
            {
                Console.WriteLine("Вас заморозили! Вы пропускаете ход.");
            }
        }

        private void OpenChest()
        {
            Console.WriteLine("\nВам повезло - вы нашли сундук!");
            int itemType = random.Next(3);
            switch (itemType)
            {
                case 0:
                    HealthPotion potion = new HealthPotion();
                    potion.Use(player);
                    Console.WriteLine("Вы нашли лечебное зелье! Здоровье полностью восстановлено :)");
                    break;
                case 1:
                    Weapon newWeapon = possibleWeapons[random.Next(possibleWeapons.Length)];
                    Console.WriteLine($"Вы нашли новое оружие: {newWeapon.Name} (урон: {newWeapon.Damage})");
                    Console.WriteLine($"Ваше текущее оружие: {player.EquippedWeapon.Name} (урон: {player.EquippedWeapon.Damage})");
                    OfferItemSwap(newWeapon);
                    break;
                case 2:
                    Armor newArmor = possibleArmors[random.Next(possibleArmors.Length)];
                    Console.WriteLine($"Вы нашли новые доспехи: {newArmor.Name} (защита: {newArmor.Defense})");
                    Console.WriteLine($"Ваши текущие доспехи: {player.EquippedArmor.Name} (защита: {player.EquippedArmor.Defense})");
                    OfferItemSwap(newArmor);
                    break;
            }
        }

        private void OfferItemSwap<T>(T newItem) where T : class
        {
            Console.WriteLine("Хотите взять новый предмет?");
            Console.WriteLine("1 - Взять новый предмет");
            Console.WriteLine("2 - Оставить старый");
            int choice = GetPlayerChoice(1, 2);
            if (choice == 1)
            {
                if (newItem is Weapon newWeapon)
                {
                    player.EquippedWeapon = newWeapon;
                    Console.WriteLine($"Вы экипировали: {newWeapon.Name}");
                }
                else if (newItem is Armor newArmor)
                {
                    player.EquippedArmor = newArmor;
                    Console.WriteLine($"Вы экипировали: {newArmor.Name}");
                }
            }
            else
            {
                Console.WriteLine("Вы оставили старый предмет");
            }
        }

        private int GetPlayerChoice(int min, int max)
        {
            while (true)
            {
                Console.WriteLine("Ваш выбор: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                Console.WriteLine($"Пожалуйста, введите число от {min} до {max}");
            }
        }
    }
}