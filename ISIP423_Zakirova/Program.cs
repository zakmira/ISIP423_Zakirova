using System;
class Program
{
    static void Main()
    {
        int count;
        while (!int.TryParse(Console.ReadLine(), out count) || count < 2 || count > 40) ;
        {
            Console.WriteLine("Ошибка! Можно вводить от 2 до 40 операций");
        }

        string[] names = new string[count];
        double[] amounts = new double[count];

        Console.WriteLine("Введите операции в формате: Название услуги или товара; Кол-во средств. Необходимы траты в рублях!");

        for (int i = 0; i < count; i++)
        {
            while (true)
            {
                Console.Write($"{i + 1}. ");
                string input = Console.ReadLine();
                string[] parts = input.Split(';');

                if (parts.Length == 2)
                {
                    string name = parts[0].Trim();
                    string value = parts[1].Trim();

                    if (!string.IsNullOrEmpty(name) && double.TryParse(value, out double amount) && amount > 0)
                    {
                        names[i] = name;
                        amounts[i] = amount;
                        break;

                    }
                }

                Console.WriteLine("Ошибка формата! Используйте: Название; Сумма (например: Кофе; 150)");
            }
        }

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика");
            Console.WriteLine("3. Сортировка по цене");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");

            Console.Write("Выберите пункт меню: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowData(names, amounts);
                    break;
                case "2":
                    ShowStatistics(amounts);
                    break;
                case "3":
                    BubbleSort(names, amounts);
                    Console.WriteLine("Данные отсортированы по возрастанию цены!");
                    break;
                case "4":
                    ConvertCurrency(amounts);
                    break;
                case "5":
                    SearchByName(names, amounts);
                    break;
                case "0":
                    exit = true;
                    Console.WriteLine("До свидания!");
                    break;
                default:
                    Console.WriteLine("Неверный выбор! Попробуйте снова.");
                    break;
            }
        }
    }

    static void ShowData(string[] names, double[] amounts)
    {
        Console.WriteLine("\n=== ВАШИ РАСХОДЫ ===");
        double total = 0;
        for (int i = 0; i < names.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {names[i]} - {amounts[i]:C2}");
            total += amounts[i];
        }
        Console.WriteLine($"Итого: {total:C2}");
    }

    static void ShowStatistics(double[] amounts)
    {
        if (amounts.Length == 0) return;

        double sum = 0;
        double min = amounts[0];
        double max = amounts[0];

        foreach (double amount in amounts)
        {
            sum += amount;
            if (amount < min) min = amount;
            if (amount > max) max = amount;
        }

        double average = sum / amounts.Length;

        Console.WriteLine("\n=== СТАТИСТИКА ===");
        Console.WriteLine($"Сумма: {sum:C2}");
        Console.WriteLine($"Среднее: {average:C2}");
        Console.WriteLine($"Минимальная трата: {min:C2}");
        Console.WriteLine($"Максимальная трата: {max:C2}");
        Console.WriteLine($"Количество операций: {amounts.Length}");
    }

    static void BubbleSort(string[] names, double[] amounts)
    {
        for (int i = 0; i < amounts.Length - 1; i++)
        {
            for (int j = 0; j < amounts.Length - i - 1; j++)
            {
                if (amounts[j] > amounts[j + 1])
                {
                    // Меняем местами суммы
                    double tempAmount = amounts[j];
                    amounts[j] = amounts[j + 1];
                    amounts[j + 1] = tempAmount;

                    // Меняем местами названия
                    string tempName = names[j];
                    names[j] = names[j + 1];
                    names[j + 1] = tempName;
                }
            }
        }
    }

    static void ConvertCurrency(double[] amounts)
    {
        Console.WriteLine("\n=== КОНВЕРТАЦИЯ ВАЛЮТЫ ===");
        Console.WriteLine("1. Доллар США (USD)");
        Console.WriteLine("2. Евро (EUR)");
        Console.WriteLine("3. Тенге (KZT)");
        Console.WriteLine("4. Другая валюта (ввести курс)");
        
        Console.Write("Выберите валюту: ");
        string currencyChoice = Console.ReadLine();
        
        double exchangeRate = 0;
        string currencySymbol = "";
        
        switch (currencyChoice)
        {
            case "1":
                exchangeRate = 90.0; // Пример курса USD
                currencySymbol = "$";
                break;
            case "2":
                exchangeRate = 98.0; // Пример курса EUR
                currencySymbol = "€";
                break;
            case "3":
                exchangeRate = 0.2; // Пример курса KZT
                currencySymbol = "₸";
                break;
            case "4":
                Console.Write("Введите курс конвертации (1 RUB = X): ");
                while (!double.TryParse(Console.ReadLine(), out exchangeRate) || exchangeRate <= 0)
                {
                    Console.Write("Ошибка! Введите положительное число: ");
                }
                currencySymbol = "ед.";
                break;
            default:
                Console.WriteLine("Неверный выбор!");
                return;
        }
        
        Console.WriteLine($"\nКурс: 1 RUB = {exchangeRate} {currencySymbol}");
        Console.WriteLine("Суммы в выбранной валюте:");
        
        for (int i = 0; i < amounts.Length; i++)
        {
            double convertedAmount = amounts[i] * exchangeRate;
            Console.WriteLine($"{i + 1}. {convertedAmount:F2} {currencySymbol}");
        }
    }

}