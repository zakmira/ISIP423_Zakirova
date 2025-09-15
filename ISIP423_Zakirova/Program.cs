using System;
class Program
{
    static void Main()
    {
        int count;
        while (!int.TryParse(Console.ReadLine(), out count) || count < 2 || count > 40);
        {
            Console.WriteLine("Ошибка! Можно вводить от 2 до 40 операций");
        }
    
        string[] oper = new string[count];
        double[] amounts = new double[count];

        Console.WriteLine("Введите операции в формате: Название услуги или товара; Кол-во средств. Необходимы траты в рублях!");

        for (int i = 0; i < count; i++)
        {
            while (true)
            {
                Console.Write($"{i + 1}. ");
                string input = Console.ReadLine();
                string[] parts = input.Split(';');

                if (parts.Length == 2 )
                {
                    string name = parts[0].Trim();
                    string value = parts[1].Trim();

                    if (!string.IsNullOrEmpty(name) && double.TryParse(value, out double amount) && amount > 0)
                    {
                        oper[i] = name;
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

        }
}