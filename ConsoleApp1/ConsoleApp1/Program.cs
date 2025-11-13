using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp1.Program;

namespace ConsoleApp1
{
    internal class Program
    {
        public class Client
        {
            public Parts BrokenPart { get; set; }
            public string ClientName { get; set; }
            public decimal RepairCost { get; set; }

            public Client(string name, Parts part, decimal workCost = 500)
            {
                BrokenPart = part;
                ClientName = name;
                Work(workCost);

            }

            public void Work(decimal workCost)
            {
                if (BrokenPart.Price < 0)
                {
                    Console.WriteLine("Ошибка: Цена детали не может быть отриц.");
                    return;
                }

                
                RepairCost = workCost + BrokenPart.Price;

            }
        }

        public class Game
        {
            public decimal workCost = 500; 
            public int penalty = 700;
            public Player player;
            private List<Parts> allParts;
            private List<PartsInStock> partsInStock;

            public Game()
            {
                partsInStock = new List<PartsInStock>();
                allParts = new List<Parts>();
                LoadPartsDB();
            }

            public void LoadPartsDB()
            {
                allParts = Core.Context.Parts.ToList();
            }

            public void GameData(int playerID)
            {
                player = Core.Context.Player.FirstOrDefault(p => p.PlayerID == playerID);
                if (player == null)
                {
                    Console.WriteLine("Игрок с таким ID не найден");
                    Player newPlayer = new Player { MyBalance = 10000 };
                    Core.Context.Player.Add(newPlayer);
                    Core.Context.SaveChanges();
                    player = Core.Context.Player.FirstOrDefault(p => p.PlayerID == newPlayer.PlayerID);

                    if (player == null)
                    {
                        throw new InvalidOperationException("Не удалось загрузить созданного игрока");
                    }

                    Console.WriteLine("Новый игрок создан!");
                }

                partsInStock = Core.Context.PartsInStock.Where(p => p.PlayerID == player.PlayerID).ToList();
            }

            public Client GenerateClient()
            {
                Random random = new Random();
                int randomIndex = random.Next(0, allParts.Count);
                Parts randomPart = allParts[randomIndex];

                List<string> names = new List<string>();
                names.Add("Максим Олегович");
                names.Add("Ковальски");
                names.Add("Герой в воде");
                names.Add("Дёмкина");
                names.Add("Владимир Горланов");

                string clientName = names[random.Next(names.Count)];

                return new Client(clientName, randomPart, workCost);
            }

            public bool HasPart(Parts part)
            {
                PartsInStock repairPart = partsInStock.FirstOrDefault(p => p.PartID == part.PartID && p.PlayerID == player.PlayerID);

                if (repairPart != null && repairPart.Quantity > 0)
                {
                    return true; // деталь есть на складке
                }
                return false; // детали нет в принципе или закончилась
            }

            public void Repair(Client client)
            {
                PartsInStock repairPart = partsInStock.FirstOrDefault(p => p.PartID == client.BrokenPart.PartID && p.PlayerID == player.PlayerID);

                if (repairPart != null && repairPart.Quantity > 0)
                {
                    repairPart.Quantity -= 1;
                    player.MyBalance += client.RepairCost;

                    Core.Context.SaveChanges();
                    partsInStock = Core.Context.PartsInStock.Where(p => p.PlayerID == player.PlayerID).ToList();

                    Console.WriteLine($"Ремонт выполнен! Использовано: {client.BrokenPart.PartName}");
                }
                else
                {
                    Console.WriteLine("Ремонт невозможен, т.к. отсутствует деталь");
                    RefuseClient();
                }
            }

            public void RefuseClient()
            {
                player.MyBalance -= penalty;
                Core.Context.SaveChanges();
            }

            public void OrderParts(Parts part, int quantity)
            {
                if (player == null)
                {
                    Console.WriteLine("Ошибка: Данные игрока не загружены. Невозможно выполнить покупку.");
                    return;
                }

                var updatePlayer = Core.Context.Player.FirstOrDefault(p => p.PlayerID == player.PlayerID);
                if (updatePlayer == null)
                {
                    Console.WriteLine($"Ошибка: игрок не найден в БД ;(");
                    return; 
                }
                player = updatePlayer;

                if (quantity <= 0)
                {
                    Console.WriteLine("Кол-во покупаемых деталей должно быть положительным!");
                    return;
                }

                decimal totalCost = part.Price * quantity;

                if (player.MyBalance < totalCost)
                {
                    Console.WriteLine("Недостаточно средств для покупки");
                    return;
                }

                PartsInStock repairPart = Core.Context.PartsInStock.FirstOrDefault(p => p.PartID == part.PartID && p.PlayerID == player.PlayerID);

                if (repairPart != null)
                {
                    repairPart.Quantity += quantity;
                }
                else
                {
                    repairPart = new PartsInStock
                    {
                        PartID = part.PartID,
                        PlayerID = player.PlayerID,
                        Quantity = quantity
                    };

                    Core.Context.PartsInStock.Add(repairPart);
                }

                player.MyBalance -= totalCost;

                try
                {
                    Core.Context.SaveChanges();
                    Console.WriteLine($"Заказ на {part.PartName} {quantity} шт. оформлен. Стоимость: {totalCost}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при сохранении изменений в базе данных: {ex.Message}");
                }

                Core.Context.SaveChanges();
                Console.WriteLine($"Заказ на {part.PartName} {quantity} шт. оформлен. Стоимость: {totalCost}");

            }

            public void GameStatus()
            {
                Console.WriteLine($"\nНынешний баланс: {player.MyBalance}");
                Console.WriteLine("Название детали | Кол-во | Цена (за 1 шт.)");

                var details = Core.Context.PartsInStock
                .Where(p => p.PlayerID == player.PlayerID && p.Quantity > 0)
                .Join(
                    Core.Context.Parts,
                    repairPart => repairPart.PartID,
                    part => part.PartID,
                    (stockItem, part) => new { PartName = part.PartName, Quantity = stockItem.Quantity, Price = part.Price, PartID = part.PartID } 
                )
                .ToList();

                if (details.Any())
                {
                    foreach (var item in details)
                    {
                        Console.WriteLine($"{item.PartName} | {item.Quantity} | {item.Price}");
                    }
                }
                else
                {
                    Console.WriteLine("На складе пока нет деталей");
                }

            }
        }

        static void Main(string[] args)
        {
            var playerReset = Core.Context.Player.FirstOrDefault(p => p.PlayerID == 1);
            if (playerReset != null)
            {
                decimal balance = 10000;
                if (playerReset.MyBalance != balance)
                {
                    playerReset.MyBalance = balance;
                    Core.Context.SaveChanges(); 
                    Console.WriteLine($"Баланс игрока сброшен до {balance}.");
                }
            }

            Console.WriteLine("Добро пожаловать в автосервис!");

            Game game = new Game();
            game.GameData(1);

            bool gameRun = true;
            while (gameRun)
            {
                game.GameStatus();

                Client client = game.GenerateClient();
                if (client == null) continue;
                Console.WriteLine($"\nПриехал(-a) {client.ClientName}");
                Console.WriteLine($"Cломалось: {client.BrokenPart.PartName}");
                Console.WriteLine($"Стоимость ремонта: {client.RepairCost}");

                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Починить");
                Console.WriteLine("2. Отказать");
                Console.WriteLine("3. Закупить детали");
                Console.WriteLine("4. Выйти");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (game.HasPart(client.BrokenPart))
                        {
                            game.Repair(client);
                        }
                        else
                        {
                            Console.WriteLine("У Вас нет нужной детали на складе!");
                            game.RefuseClient();
                            Console.WriteLine($"\nШтраф за отказ клиенту {game.penalty}");
                        }
                        break;
                    case "2":
                        game.RefuseClient();
                        Console.WriteLine($"\nШтраф за отказ клиенту {game.penalty}");
                        break;
                    case "3":
                        var allParts = Core.Context.Parts.ToList();
                        for (int i = 0; i < allParts.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {allParts[i].PartName} - Цена: {allParts[i].Price}");
                        }
                        Console.Write("Выберите номер детали для покупки: ");
                        if (int.TryParse(Console.ReadLine(), out int partIndex) && partIndex > 0 && partIndex <= allParts.Count)
                        {
                            Console.WriteLine("Введите кол-во: ");
                            if (int.TryParse(Console.ReadLine(), out int q))
                            {
                                game.OrderParts(allParts[partIndex - 1], q);
                            }
                            else
                            {
                                Console.WriteLine("Неверное кол-во");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неверный выбор детали");
                        }
                        break;
                    case "4":
                        gameRun = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }

                if (game.player.MyBalance < 0)
                {
                    Console.WriteLine("Вы - банкрот!");
                    gameRun = false;
                }
            }

            Console.WriteLine("Доремонтировали..."); // игра окончена
            Console.ReadKey();

        }

    }

}
