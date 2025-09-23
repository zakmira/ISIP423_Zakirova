using System;
using System.Collections.Generic;
using System.Text;

class TextStatistics
{
    public string Text { get; set; }
    public int WordCount { get; set; }
    public string ShortestWord { get; set; }
    public int SentenceCount { get; set; }
    public int ABcount { get; set; } //гласные и согласные
    public string LongestWord { get; set; }
    public Dictionary<char, int> LetterFreq { get; set; } = new Dictionary<char, int>(); //почитать про char!
                                                          //Через равно прописываем, чтобы каждый раз не объявлять  
    public override string ToString() //справка о состоянии программы
    {
        return ($"Символов: {Text}, Слов: {WordCount}, Предложений: {SentenceCount}");
    }

}

class Program
{
    private static List<TextStatistics> allStatistics = new List<TextStatistics>(); //создаем "архив" под видом переменной, чтобы в нее записывать изменения 

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        RunProgram();
    }

    static void RunProgram()
    {
        bool ContinueWorking = true;

        while (ContinueWorking)
        {
            Console.Clear();
            ShowMainMenu();

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AnalyzeNewText();
                    break;
                case "2":
                    ShowHistory();
                    break;
                case "3":
                    ContinueWorking = false;
                    break;
                default:
                    Console.WriteLine("Неверный выбор");
                    Console.ReadKey(); //чтобы приложение сразу не закрылось
                    break;

            }
        }

    }

    static void ShowMainMenu()
    {
        Console.WriteLine("Анализ текста");
        Console.WriteLine("1 - Проанализировать новый текст");
        Console.WriteLine("2 - Просмотр истории анализов");
        Console.WriteLine("3 - Выход");

    }
}