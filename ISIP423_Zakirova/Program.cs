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
        double[] amount = new double[count];

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
                }
            }
            

        }



    }
}