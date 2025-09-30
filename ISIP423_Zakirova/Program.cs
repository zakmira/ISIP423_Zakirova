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
    }
}