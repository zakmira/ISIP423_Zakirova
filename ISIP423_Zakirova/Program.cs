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
        Console.Write("Выберите действие: ");
    }

    static void AnalyzeNewText()
    {
        string text = GetTextFromUser();

        if (text.Length >= 100)
        {
            TextStatistics stats = AnalyzeText(text);
            allStatistics.Add(stats);
            DisplayStatistics(stats);
            Console.WriteLine("Возврат меню по клавише");
            Console.ReadKey();
        }
    }

    static string GetTextFromUser()
    {
        Console.WriteLine("Введите текст");
        StringBuilder textBuilder = new StringBuilder();
        int totalLength = 0;

        while (totalLength < 100)
        {
            Console.Write($"[{totalLength}/100+] > ");
            string inputLine = Console.ReadLine() ?? "";

            if (string.IsNullOrEmpty(inputLine))
            {
                if (totalLength >= 100)
                {
                    break;
                }

                else if (totalLength > 0)
                {
                    Console.WriteLine($"Введено только {totalLength} символов. Нужно еще {100 - totalLength}");
                    Console.WriteLine("Ввод..");
                    continue;
                }
            }

            if (!string.IsNullOrEmpty(inputLine))
            {
                textBuilder.AppendLine(inputLine);
                totalLength += inputLine.Length;
            }

            if (totalLength >= 100)
            {
                break;
            }
        }

        string finalText = textBuilder.ToString().Trim();
        Console.WriteLine($"Текст принят. Введено {finalText.Length} символов");
        return finalText;
    }

    static TextStatistics AnalyzeText(string text) //приступаем к самому анализу
    {
        var stats = new TextStatistics { Text = text }; //присваиваем нашему первоначальной переменной Техt значение text (int)
        string[] words = SplitIntoWords(text);
        stats.WordCount = words.Length;

        if (words.Length > 0)
        {
            FindShAndLongWord(words, stats);
        }

        stats.SentenceCount = CountSentences(text);

        CountAB(text, stats);

        CountFrequency(text, stats);

        return stats; //тут короче прописываем чд программе
    }

    static string[] SplitIntoWords(string text)
    {
        List<string> words = new List<string>();
        StringBuilder currentWord = new StringBuilder();
        char[] separators = { ' ', '\t', '\n', '\r', ',', '.', '!', '?', ';', ':', '-', '(', ')', '[', ']', '\"', '\'' };

        foreach (char c in text)
        {
            bool IsSeparator = false;
            for (int i = 0; i < separators.Length; i++)
            {
                if (separators[i] == c)
                {
                    IsSeparator = true;
                    break;
                }

            }

            if (IsSeparator)
            {
                if (currentWord.Length > 0)
                {
                    words.Add(currentWord.ToString());
                    currentWord.Clear();
                }
            }

            else
            {
                currentWord.Append(c);
            }
        }

        if (currentWord.Length > 0)
        {
            words.Add(currentWord.ToString());
            string[] wordsArray = new string[words.Count];
            for (int i = 0; i < words.Count; i++)
            {
                wordsArray[i] = words[i];
            }

            return wordsArray; //добавка последнего слова
        }

        static void FindShAndLongWord(string[] words, TextStatistics stats)
        {
            if (words.Length == 0) return;

            stats.ShortestWord = words[0];
            stats.LongestWord = words[0];

            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length < stats.ShortestWord.Length)
                {
                    stats.ShortestWord = words[i];
                }

                if (words.Length > stats.LongestWord.Length)
                {
                    stats.LongestWord = words[i];
                }
            }
        }
    }
}