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