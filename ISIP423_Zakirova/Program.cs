using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreInventory
{
    public enum Category
    {
        Electronics,
        Clothing,
        Food,
        Books,
        Sports
    }

    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InStock { get; set; }
        public Category Category { get; set; }

        public Product(string code, string name, decimal price, int quantity, Category category)
        {
            Code = code;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
            InStock = quantity > 0;
        }

        public void UpdateStockStatus()
        {
            InStock = Quantity > 0;
        }

        public override string ToString()
        {
            return $"Код: {Code}, Название: {Name}, Цена: {Price:C}, " +
                   $"Количество: {Quantity}, В наличии: {(InStock ? "Да" : "Нет")}, " +
                   $"Категория: {Category}";
        }
    }

    class Program
    {
        private static List<Product> products = new List<Product>();
        private static int nextId = 1;

        static void Main(string[] args)
        {
            InitializeTestData();
            ShowMenu();
        }

        static void InitializeTestData()
        {
            AddProduct("Ноутбук", 50000, 10, Category.Electronics);
            AddProduct("Футболка", 1500, 25, Category.Clothing);
            AddProduct("Хлеб", 50, 100, Category.Food);
            AddProduct("Роман", 350, 15, Category.Books);
            AddProduct("Мяч", 1200, 8, Category.Sports);
            
            Console.WriteLine("Добавлены тестовые товары:");
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();
        }

        static void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("=== МЕНЮ УЧЁТА ТОВАРОВ ===");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Удалить товар");
                Console.WriteLine("3. Заказать поставку товара");
                Console.WriteLine("4. Продать товар");
                Console.WriteLine("5. Поиск товаров");
                Console.WriteLine("6. Показать все товары");
                Console.WriteLine("7. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProductMenu();
                        break;
                    case "2":
                        RemoveProductMenu();
                        break;
                    case "3":
                        OrderSupplyMenu();
                        break;
                    case "4":
                        SellProductMenu();
                        break;
                    case "5":
                        SearchProductsMenu();
                        break;
                    case "6":
                        ShowAllProducts();
                        break;
                    case "7":
                        Console.WriteLine("Выход из программы...");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void AddProductMenu()
        {
            try
            {
                Console.WriteLine("\n=== ДОБАВЛЕНИЕ ТОВАРА ===");
                
                Console.Write("Введите название товара: ");
                string name = Console.ReadLine().Trim();
                
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Ошибка: название не может быть пустым.");
                    return;
                }

                Console.Write("Введите цену товара: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
                {
                    Console.WriteLine("Ошибка: цена должна быть положительным числом.");
                    return;
                }

                Console.Write("Введите количество товара: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
                {
                    Console.WriteLine("Ошибка: количество не может быть отрицательным.");
                    return;
                }

                Console.WriteLine("Доступные категории:");
                foreach (Category category in Enum.GetValues(typeof(Category)))
                {
                    Console.WriteLine($"{(int)category}. {category}");
                }

                Console.Write("Выберите категорию (номер): ");
                if (!int.TryParse(Console.ReadLine(), out int categoryNumber) || 
                    !Enum.IsDefined(typeof(Category), categoryNumber))
                {
                    Console.WriteLine("Ошибка: неверный выбор категории.");
                    return;
                }

                Category categoryChoice = (Category)categoryNumber;
                AddProduct(name, price, quantity, categoryChoice);
                Console.WriteLine("Товар успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении товара: {ex.Message}");
            }
        }

        static void AddProduct(string name, decimal price, int quantity, Category category)
        {
            string code = $"1{nextId.ToString().PadLeft(4, '0')}";
            var product = new Product(code, name, price, quantity, category);
            products.Add(product);
            nextId++;
        }

        static void RemoveProductMenu()
        {
            Console.WriteLine("\n=== УДАЛЕНИЕ ТОВАРА ===");
            Console.Write("Введите код товара для удаления: ");
            string code = Console.ReadLine().Trim();

            var product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }

            products.Remove(product);
            Console.WriteLine($"Товар '{product.Name}' успешно удалён.");
        }

        static void OrderSupplyMenu()
        {
            Console.WriteLine("\n=== ЗАКАЗ ПОСТАВКИ ===");
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine().Trim();

            var product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }

            Console.Write("Введите количество для поставки: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Ошибка: количество должно быть положительным числом.");
                return;
            }

            product.Quantity += quantity;
            product.UpdateStockStatus();
            Console.WriteLine($"Поставка успешно оформлена. Новое количество: {product.Quantity}");
        }

        static void SellProductMenu()
        {
            Console.WriteLine("\n=== ПРОДАЖА ТОВАРА ===");
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine().Trim();

            var product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }

            if (!product.InStock)
            {
                Console.WriteLine("Товара нет в наличии.");
                return;
            }

            Console.Write("Введите количество для продажи: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Ошибка: количество должно быть положительным числом.");
                return;
            }

            if (product.Quantity < quantity)
            {
                Console.WriteLine($"Недостаточно товара. В наличии: {product.Quantity}");
                return;
            }

            product.Quantity -= quantity;
            product.UpdateStockStatus();
            decimal total = product.Price * quantity;
            Console.WriteLine($"Продажа успешно оформлена. Сумма: {total:C}");
            Console.WriteLine($"Остаток: {product.Quantity}");
        }

        static void SearchProductsMenu()
        {
            Console.WriteLine("\n=== ПОИСК ТОВАРОВ ===");
            Console.WriteLine("1. По коду");
            Console.WriteLine("2. По названию");
            Console.WriteLine("3. По категории");
            Console.Write("Выберите тип поиска: ");

            string choice = Console.ReadLine();
            List<Product> results = new List<Product>();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите код: ");
                    string code = Console.ReadLine().Trim();
                    results = products.Where(p => p.Code.Contains(code)).ToList();
                    break;
                case "2":
                    Console.Write("Введите название: ");
                    string name = Console.ReadLine().Trim();
                    results = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "3":
                    Console.WriteLine("Доступные категории:");
                    foreach (Category category in Enum.GetValues(typeof(Category)))
                    {
                        Console.WriteLine($"{(int)category}. {category}");
                    }
                    Console.Write("Выберите категорию (номер): ");
                    if (int.TryParse(Console.ReadLine(), out int categoryNumber) && 
                        Enum.IsDefined(typeof(Category), categoryNumber))
                    {
                        Category categoryChoice = (Category)categoryNumber;
                        results = products.Where(p => p.Category == categoryChoice).ToList();
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор категории.");
                        return;
                    }
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    return;
            }

            if (results.Count == 0)
            {
                Console.WriteLine("Товары не найдены.");
            }
            else
            {
                Console.WriteLine($"Найдено товаров: {results.Count}");
                foreach (var product in results)
                {
                    Console.WriteLine(product);
                }
            }
        }

        static void ShowAllProducts()
        {
            Console.WriteLine("\n=== ВСЕ ТОВАРЫ ===");
            if (products.Count == 0)
            {
                Console.WriteLine("Товаров нет.");
                return;
            }

            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
        }
    }
}
