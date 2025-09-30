using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement
{

    public class Book
    {
        private static int _nextId = 1;

        // Доступные жанры
        public static readonly string[] Genres = {
            "Художественная литература",
            "Научная фантастика",
            "Детектив",
            "Фэнтези",
            "Романтика",
            "Научная литература",
            "Биография",
            "История",
            "Философия",
            "Поэзия"
        };

        public int Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        public Book(string title, string author, string genre, int year, decimal price)
        {
            Id = _nextId++;
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Price = price;
        }

        public static bool IsValidGenre(string genre)
        {
            return Array.Exists(Genres, g => g.Equals(genre, StringComparison.OrdinalIgnoreCase));
        }

        public override string ToString()
        {
            return $"ID: {Id} | \"{Title}\" - {Author} ({Genre}, {Year} г., {Price:C})";
        }

    }

    public class Library
    {
        private List<Book> books;

        public Library()
        {
            books = new List<Book>();
        }

        // Добавление книги в библиотеку
        public Book AddBook(string title, string author, string genre, int year, decimal price)
        {
            if (!Book.IsValidGenre(genre))
            {
                throw new ArgumentException($"Недопустимый жанр: {genre}");
            }

            if (year < 0 || year > DateTime.Now.Year)
            {
                throw new ArgumentException($"Недопустимый год издания: {year}");
            }

            if (price < 0)
            {
                throw new ArgumentException($"Цена не может быть отрицательной: {price}");
            }

            var book = new Book(title, author, genre, year, price);
            books.Add(book);
            return book;
        }

        // Удаление книги по ID
        public Book RemoveBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                throw new ArgumentException($"Книга с ID {id} не найдена");
            }

            books.Remove(book);
            return book;
        }

        // Поиск книги по:
        // названию
        public List<Book> FindByTitle(string title)
        {
            return books.Where(book =>
                book.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();
        }

        // автору
        public List<Book> FindByAuthor(string author)
        {
            return books.Where(book =>
                book.Author.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();
        }

        // жанру
        public List<Book> FindByGenre(string genre)
        {
            return books.Where(book =>
                book.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        // Сортировка по названию
        public List<Book> SortByTitle()
        {
            return books.OrderBy(book => book.Title).ToList();
        }

        // по году издания
        public List<Book> SortByYear()
        {
            return books.OrderBy(book => book.Year).ToList();
        }

        // Самая дорогая книга
        public Book GetMostExpensiveBook()
        {
            return books.Count == 0 ? null : books.OrderByDescending(book => book.Price).First();
        }

        // Самая дешёвая книга
        public Book GetCheapestBook()
        {
            return books.Count == 0 ? null : books.OrderBy(book => book.Price).First();
        }

        // Группировка по авторам
        public Dictionary<string, List<Book>> GroupByAuthor()
        {
            return books.GroupBy(book => book.Author)
                       .ToDictionary(group => group.Key, group => group.ToList());
        }

        // Возвращает все книги
        public List<Book> GetAllBooks()
        {
            return new List<Book>(books);
        }

        // Возвращает кол-во книг
        public int GetBookCount()
        {
            return books.Count;
        }
    }

    public class ConsoleInterface
    {
        private Library library;

        public ConsoleInterface()
        {
            library = new Library();
        }

        //Главное меню
        private void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Система учета книг в библиотеке");
            Console.WriteLine("Выберите действие: ");
            Console.WriteLine("1. Добавить книгу");
            Console.WriteLine("2. Удалить книгу по ID");
            Console.WriteLine("3. Найти книги");
            Console.WriteLine("4. Сортировать книги");
            Console.WriteLine("5. Показать самую дорогую/дешёвую книгу");
            Console.WriteLine("6. Группировка книг по авторам");
            Console.WriteLine("7. Показать все книги");
            Console.WriteLine("0. Выход");
            Console.WriteLine($"\nВсего книг в библиотеке: {library.GetBookCount()}");
        }

        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Добро пожаловать в систему учёта книг!");
            Console.ResetColor();

            while (true)
            {
                ShowMainMenu();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nВыберите действие (0-7): ");
                Console.ResetColor();

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddBook();
                            break;
                        case "2":
                            RemoveBook();
                            break;
                        case "3":
                            FindBooks();
                            break;
                        case "4":
                            SortBooks();
                            break;
                        case "5":
                            ShowExtremeBooks();
                            break;
                        case "6":
                            GroupBooksByAuthor();
                            break;
                        case "7":
                            ShowAllBooks();
                            break;
                        case "0":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("До свидания!");
                            Console.ResetColor();
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            Console.ResetColor();
                            WaitForEnter();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    Console.ResetColor();
                    WaitForEnter();
                }
            }
        }
        // Новая книга
        private void AddBook()
        {
            Console.WriteLine("\nДобавление новой книги");
            Console.ResetColor();

            Console.Write("Введите название книги: ");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Название книги не может быть пустым");
            }

            Console.Write("Введите автора книги: ");
            string author = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("Автор книги не может быть пустым");
            }

            Console.WriteLine("\nДоступные жанры:");
            for (int i = 0; i < Book.Genres.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Book.Genres[i]}");
            }

            Console.Write("Выберите жанр (номер): ");
            if (!int.TryParse(Console.ReadLine(), out int genreIndex) ||
                genreIndex < 1 || genreIndex > Book.Genres.Length)
            {
                throw new ArgumentException("Неверный номер жанра");
            }
            string genre = Book.Genres[genreIndex - 1];

            Console.Write("Введите год издания: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                throw new ArgumentException("Год издания должен быть числом");
            }

            Console.Write("Введите цену (руб.): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                throw new ArgumentException("Цена должна быть числом");
            }

            var book = library.AddBook(title, author, genre, year, price);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nКнига успешно добавлена!");
            Console.ResetColor();
            Console.WriteLine(book.GetFullInfo());

            WaitForEnter();
        }

        // Делит книги по ID
        private void RemoveBook()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nУдаление книги");
            Console.ResetColor();

            if (library.GetBookCount() == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("В библиотеке нет книг для удаления.");
                Console.ResetColor();
                WaitForEnter();
                return;
            }

            Console.WriteLine("\nДоступные книги:");
            foreach (var book in library.GetAllBooks())
            {
                Console.WriteLine($"{book.Id}: {book.Title} - {book.Author}");
            }

            Console.Write("\nВведите ID книги для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                throw new ArgumentException("ID должен быть числом");
            }

            var removedBook = library.RemoveBook(id);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nКнига успешно удалена");
            Console.ResetColor();
            Console.WriteLine(removedBook.ToString());

            WaitForEnter();
        }

        // Поиск книг
        private void FindBooks()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nПоиск книг");
            Console.ResetColor();
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По автору");
            Console.WriteLine("3. По жанру");

            Console.Write("Выберите тип поиска: ");
            string choice = Console.ReadLine();
            List<Book> results = new List<Book>();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите название (или часть названия): ");
                    string title = Console.ReadLine();
                    results = library.FindByTitle(title);
                    break;
                case "2":
                    Console.Write("Введите автора (или часть имени): ");
                    string author = Console.ReadLine();
                    results = library.FindByAuthor(author);
                    break;
                case "3":
                    Console.WriteLine("\nДоступные жанры:");
                    for (int i = 0; i < Book.Genres.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Book.Genres[i]}");
                    }
                    Console.Write("Выберите жанр (номер): ");
                    if (!int.TryParse(Console.ReadLine(), out int genreIndex) ||
                        genreIndex < 1 || genreIndex > Book.Genres.Length)
                    {
                        throw new ArgumentException("Неверный номер жанра");
                    }
                    results = library.FindByGenre(Book.Genres[genreIndex - 1]);
                    break;
                default:
                    throw new ArgumentException("Неверный выбор");
            }

            DisplayBooks(results, "Результаты поиска");
            WaitForEnter();
        }

        // Сортировка книг
        private void SortBooks()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nСортировка книг");
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По году издания");

            Console.Write("Выберите тип сортировки: ");
            string choice = Console.ReadLine();
            List<Book> sortedBooks = new List<Book>();

            switch (choice)
            {
                case "1":
                    sortedBooks = library.SortByTitle();
                    DisplayBooks(sortedBooks, "Книги, отсортированные по названию");
                    break;
                case "2":
                    sortedBooks = library.SortByYear();
                    DisplayBooks(sortedBooks, "Книги, отсортированные по году издания");
                    break;
                default:
                    throw new ArgumentException("Неверный выбор");
            }

            WaitForEnter();
        }

        // Самая дорогая и дешевая книги
        private void ShowExtremeBooks()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nСамая дорогая и дешевая книги");
            Console.ResetColor();

            if (library.GetBookCount() == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("В библиотеке нет книг.");
                Console.ResetColor();
                WaitForEnter();
                return;
            }

            var mostExpensive = library.GetMostExpensiveBook();
            var cheapest = library.GetCheapestBook();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nСамая дорогая книга:");
            Console.ResetColor();
            Console.WriteLine(mostExpensive.GetFullInfo());

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nСамая дешевая книга:");
            Console.ResetColor();
            Console.WriteLine(cheapest.GetFullInfo());

            WaitForEnter();
        }

        // Группировка по авторам
        private void GroupBooksByAuthor()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nГруппировка по авторам");
            Console.ResetColor();

            if (library.GetBookCount() == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("В библиотеке нет книг.");
                Console.ResetColor();
                WaitForEnter();
                return;
            }

            var groups = library.GroupByAuthor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nСтатистика авторов:");
            Console.ResetColor();

            foreach (var group in groups)
            {
                string author = group.Key;
                var books = group.Value;
                string bookWord = books.Count == 1 ? "книга" :
                                 books.Count < 5 ? "книги" : "книг";

                Console.WriteLine($"\n {author} ({books.Count} {bookWord}):");
                foreach (var book in books)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"{book.Title} ({book.Year} г., {book.Price:C})");
                    Console.ResetColor();
                }
            }

            WaitForEnter();
        }

        // Все книги
        private void ShowAllBooks()
        {
            var books = library.GetAllBooks();
            DisplayBooks(books, "Все книги в библиотеке");
            WaitForEnter();
        }

        // Список книг
        private void DisplayBooks(List<Book> books, string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n{title.ToUpper()}");
            Console.ResetColor();

            if (books.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Книги не найдены.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Найдено книг: {books.Count}");
            Console.ResetColor();

            foreach (var book in books)
            {
                Console.WriteLine(book.GetFullInfo());
            }
        }

        // Ожидает нажатия Enter
        private void WaitForEnter()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nНажмите Enter для продолжения...");
            Console.ResetColor();
            Console.ReadLine();
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.InputEncoding = System.Text.Encoding.UTF8;

                var app = new ConsoleInterface();
                app.Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}