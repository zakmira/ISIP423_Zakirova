using System;
using System.Collections.Generic;
using System.Text;

class TextStatistics
{
    public string Text { get; set; }
    public int WordCount { get; set; }
    public string ShortestWord { get; set; }
    public int SentenceCount { get; set; }
    public int ABCount { get; set; }
    public int Glcount { get; set; }
    public string LongestWord { get; set; }
    public Dictionary<char, int> LetterFrequency { get; set; } = new Dictionary<char, int>();

   

    public override string ToString()
    {
        return $"Символов: {Text.Length} | Слов: {WordCount} | Предложений: {SentenceCount}";
    }
}

class Program
{
    private static List<TextStatistics> allStatistics = new List<TextStatistics>();

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        RunProgram();
    }

    static void RunProgram()
    {
        bool continueWorking = true;

        while (continueWorking)
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
                    continueWorking = false;
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void ShowMainMenu()
    {
 
        Console.WriteLine("1 - Анализ нового текста");
        Console.WriteLine("2 - Просмотр истории анализов");
        Console.WriteLine("3 - Выход");
        Console.Write("Выберите действие: ");
    }

    static void AnalyzeNewText()
    {
        Console.Clear();
        string text = GetTextFromUser();

        if (text.Length >= 100)
        {
            TextStatistics stats = AnalyzeText(text);
            allStatistics.Add(stats);
            DisplayStatistics(stats);

            Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
    }

    static string GetTextFromUser()
    {
        
        Console.WriteLine("Введите текст не менее 100 символов.");
        Console.WriteLine("Вы можете вводить текст в несколько строк.");
        Console.WriteLine("Для завершения ввода нажмите Enter на пустой строке.\n");

        StringBuilder textBuilder = new StringBuilder();
        int totalLength = 0;

        Console.WriteLine("Начинайте ввод текста:");

        while (totalLength < 100)
        {
            Console.Write($"[{totalLength}/100+] > ");
            string inputLine = Console.ReadLine() ?? "";

            if (string.IsNullOrEmpty(inputLine))
            {
                if (totalLength >= 100)
                {
                    break; // Текст достаточно длинный, завершаем ввод
                }
                else if (totalLength > 0)
                {
                    // Пользователь завершил ввод, но текста мало
                    Console.WriteLine($"\nВведено только {totalLength} символов. Нужно еще {100 - totalLength}.");
                    continue;
                }
            }

            if (!string.IsNullOrEmpty(inputLine))
            {
                textBuilder.AppendLine(inputLine);
                totalLength += inputLine.Length;

                if (totalLength >= 100)
                {
                    break; // Достигли нужной длины
                }
            }
        }

        string finalText = textBuilder.ToString().Trim();
        Console.WriteLine($"\nТекст принят. Общее количество символов: {finalText.Length}");
        return finalText;
    }

    static TextStatistics AnalyzeText(string text)
    {
        var stats = new TextStatistics { Text = text };

        // Подсчет слов
        string[] words = SplitIntoWords(text);
        stats.WordCount = words.Length;

        if (words.Length > 0)
        {
            // Самое короткое и самое длинное слово
            FindShortestAndLongestWord(words, stats);
        }

        // Подсчет предложений
        stats.SentenceCount = CountSentences(text);

        // Подсчет гласных и согласных
        CountGl(text, stats);

        // Статистика по буквам
        CalculateLetterFrequency(text, stats);

        return stats;
    }

    static string[] SplitIntoWords(string text)
    {
        List<string> words = new List<string>();
        StringBuilder currentWord = new StringBuilder();
        char[] separators = { ' ', '\t', '\n', '\r', ',', '.', '!', '?', ';', ':', '-', '(', ')', '[', ']', '\"', '\'' };

        foreach (char c in text)
        {
            bool isSeparator = false;
            for (int i = 0; i < separators.Length; i++)
            {
                if (separators[i] == c)
                {
                    isSeparator = true;
                    break;
                }
            }

            if (isSeparator)
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

        // Добавляем последнее слово, если оно есть
        if (currentWord.Length > 0)
        {
            words.Add(currentWord.ToString());
        }

        return words.ToArray();
    }

    static void FindShortestAndLongestWord(string[] words, TextStatistics stats)
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

            if (words[i].Length > stats.LongestWord.Length)
            {
                stats.LongestWord = words[i];
            }
        }
    }

    static int CountSentences(string text)
    {
        int sentenceCount = 0;
        bool inSentence = false;

        foreach (char c in text)
        {
            if (char.IsLetterOrDigit(c))
            {
                if (!inSentence)
                {
                    inSentence = true;
                }
            }
            else if (c == '.' || c == '!' || c == '?')
            {
                if (inSentence)
                {
                    sentenceCount++;
                    inSentence = false;
                }
            }
        }

        // Учитываем последнее предложение, если текст не заканчивается точкой
        if (inSentence)
        {
            sentenceCount++;
        }

        return sentenceCount;
    }

    static void CountGl(string text, TextStatistics stats)
    {
        string vowels = "аеёиоуыэюяaeiouy";
        string consonants = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxz";

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                char lowerC = char.ToLower(c);
                if (vowels.IndexOf(lowerC) >= 0)
                    stats.ABCount++;
                else if (consonants.IndexOf(lowerC) >= 0)
                    stats.Glcount++;
            }
        }
    }

    static void CalculateLetterFrequency(string text, TextStatistics stats)
    {
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                char lowerC = char.ToLower(c);
                if (stats.LetterFrequency.ContainsKey(lowerC))
                    stats.LetterFrequency[lowerC]++;
                else
                    stats.LetterFrequency[lowerC] = 1;
            }
        }

        // Сортируем по убыванию частоты
        stats.LetterFrequency = SortDictionaryByValue(stats.LetterFrequency);
    }

    static Dictionary<char, int> SortDictionaryByValue(Dictionary<char, int> dict)
    {
        List<KeyValuePair<char, int>> list = new List<KeyValuePair<char, int>>();

        // Преобразуем словарь в список
        foreach (var pair in dict)
        {
            list.Add(pair);
        }

        // Сортируем пузырьком по убыванию значения
        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = 0; j < list.Count - i - 1; j++)
            {
                if (list[j].Value < list[j + 1].Value)
                {
                    var temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                }
            }
        }

        // Создаем новый отсортированный словарь
        Dictionary<char, int> sortedDict = new Dictionary<char, int>();
        foreach (var pair in list)
        {
            sortedDict.Add(pair.Key, pair.Value);
        }

        return sortedDict;
    }

    static void DisplayStatistics(TextStatistics stats)
    {
        Console.WriteLine("Результаты анализа");
        Console.WriteLine($"Общее количество символов: {stats.Text.Length}");
        Console.WriteLine($"Количество слов: {stats.WordCount}");
        Console.WriteLine($"Количество предложений: {stats.SentenceCount}");
        Console.WriteLine($"Гласные буквы: {stats.ABCount}");
        Console.WriteLine($"Согласные буквы: {stats.Glcount}");

        if (stats.WordCount > 0)
        {
            Console.WriteLine($"Самое короткое слово: \"{stats.ShortestWord}\" ({stats.ShortestWord.Length} симв.)");
            Console.WriteLine($"Самое длинное слово: \"{stats.LongestWord}\" ({stats.LongestWord.Length} симв.)");
        }

        Console.WriteLine("\nЧастота букв");
        if (stats.LetterFrequency.Count > 0)
        {
            int counter = 0;
            foreach (var pair in stats.LetterFrequency)
            {
                Console.Write($"{char.ToUpper(pair.Key)}: {pair.Value}\t");
                counter++;
                if (counter % 5 == 0) Console.WriteLine();
            }
            if (counter % 5 != 0) Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Буквы не найдены");
        }

    }

    static void ShowHistory()
    {
        Console.Clear();
        Console.WriteLine("История анализа");
        
        if (allStatistics.Count == 0)
        {
            Console.WriteLine("История анализов пуста.");
        }
        else
        {
            for (int i = 0; i < allStatistics.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {allStatistics[i]}");
            }

            Console.WriteLine("\nВведите номер анализа для подробного просмотра");
            Console.WriteLine("или 0 для возврата в меню:");
            

            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice) && choice > 0 && choice <= allStatistics.Count)
            {
                DisplayStatistics(allStatistics[choice - 1]);
                Console.WriteLine("Нажмите любую клавишу для возврата");
                Console.ReadKey();
            }
        }

        if (allStatistics.Count > 0)
        {
            Console.WriteLine("\nНажмите любую клавишу для возврата в меню");
            Console.ReadKey();
        }
    }
}