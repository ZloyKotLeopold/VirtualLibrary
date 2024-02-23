using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VirtualLibrary
{
    internal class Program
    {
        static void Main()
        {
            Library library = new Library();
            CommandHandler commandHandler = new CommandHandler(library);
            InputHandler inputHandler = new InputHandler(commandHandler);

            while (inputHandler.IsActiv)
            {
                inputHandler.ProcessInput();

                Console.ReadKey();

                Console.Clear();
            }
        }
    }

    public class CommandHandler
    {
        private Library _library;
        private Librarian _libraryan;

        public CommandHandler(Library library)
        {
            _library = library;
            _libraryan = new Librarian((ICollection<Book>)library.LibraryBooks);
        }

        public void ShowAllBooks()
        {
            PrintBookInfofmation((ICollection<Book>)_library.LibraryBooks);
        }

        public void AddNewBook()
        {
            string title;
            string author;
            string genre;

            Console.Write("Ведите название книги. \n");

            title = Console.ReadLine();

            Console.Write("Ведите автора книги. \n");

            author = Console.ReadLine();

            Console.Write("Ведите жанр книги. \n");

            genre = Console.ReadLine();

            Console.Write("Ведите год выхода книги. \n");

            int.TryParse(Console.ReadLine(), out int year);

            if (year <= DateTime.Now.Year && year != 0)
            {
                _library.AddBook(new Book(title, author, genre, year));

                Console.WriteLine($"Книга {title} добавлена!\n");
            }
            else
            {
                Console.WriteLine("Книга не была добавлена вы ввели не корректный год!\n");
            }
        }

        public void DropBook()
        {
            string title;

            Console.Write("Введите название книги которую хотите выкинуть: ");

            title = Console.ReadLine();

            foreach (Book book in _library.LibraryBooks.Where(book => book.Title.Contains(title)).ToList())
            {
                _library.RemoveBook(book);

                Console.WriteLine("Вы безвозвратно выкинули книгу.\n");
            }
        }

        public void HandleByTitle()
        {
            string title;

            Console.Write("Введите название книги которую хотите найти: ");

            title = Console.ReadLine();

            ShowBooks((ICollection<Book>)_libraryan.SearchByTitle(title));
        }

        public void HandleByAuthor()
        {
            string author;

            Console.Write("Введите автора книг которые хотите найти: ");

            author = Console.ReadLine();

            ShowBooks((ICollection<Book>)_libraryan.SearchByAuthor(author));
        }

        public void HandleByGenre()
        {
            string genre;

            Console.Write("Введите жанр книг которые хотите найти: ");

            genre = Console.ReadLine();

            ShowBooks((ICollection<Book>)_libraryan.SearchByGenre(genre));
        }

        public void HandleByYear()
        {
            string userInput;

            Console.Write("Введите год книг которые хотите найти: ");

            userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int year))           
                ShowBooks((ICollection<Book>)_libraryan.SearchByYear(year));            
            else            
                Console.WriteLine("Неверный ввод!");            
        }

        public void FillLibrary()
        {
            Book book1 = new Book("Игра Эндера", "Кард Орсон Скотт", "Фантастика", 1985);
            Book book2 = new Book("Тень Эндера", "Кард Орсон Скотт", "Фантастика", 1999);
            Book book3 = new Book("Седьмое Солнце: игры с вниманием", "Рина Ра", "Паранормальное", 2021);
            Book book4 = new Book("Арсанты: Дети богов", "Фарутин Антон", "Фантастика", 2020);
            Book book5 = new Book("Арсанты 2. Линии судьбы", "Фарутин Антон", "Фантастика", 2021);
            Book book6 = new Book("Я - Легенда", "Матесон Ричард", "Постапокалиптика", 1954);

            _library.AddBook(book1);
            _library.AddBook(book2);
            _library.AddBook(book3);
            _library.AddBook(book4);
            _library.AddBook(book5);
            _library.AddBook(book6);
        }
        private void ShowBooks(ICollection<Book> books)
        {
            foreach (var book in books)
                Console.WriteLine($"{book.Title}\n{book.Author}\n{book.Genre}\n{book.Year}\n\n");
        }

        private void PrintBookInfofmation(ICollection<Book> books)
        {
            foreach (var book in books)
                Console.WriteLine($"{book.Title}\n{book.Author}\n{book.Genre}\n{book.Year}\n\n");
        }
    }

    public class InputHandler
    {
        private const int ParameterShowAllBooks = 1;
        private const int ParameterAddNewBook = 2;
        private const int ParameterDropBook = 3;
        private const int ParameterSerchBookByTitle = 4;
        private const int ParameterSerchBookByAuthor = 5;
        private const int ParameterSerchBookByGenre = 6;
        private const int ParameterSerchBookBYear = 7;
        private const int ParameterExit = 8;

        private string _userInput;
        private CommandHandler _commandHandler;

        public InputHandler(CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
            _commandHandler.FillLibrary();
        }

        public bool IsActiv { get; private set; } = true;

        public void ProcessInput()
        {
            ShowMenu();

            _userInput = Console.ReadLine();

            int.TryParse(_userInput, out int parameter);

            switch (parameter)
            {
                case ParameterShowAllBooks:
                    _commandHandler.ShowAllBooks();
                    break;

                case ParameterAddNewBook:
                    _commandHandler.AddNewBook();
                    break;

                case ParameterDropBook:
                    _commandHandler.DropBook();
                    break;

                case ParameterSerchBookByTitle:
                    _commandHandler.HandleByTitle();
                    break;

                case ParameterSerchBookByAuthor:
                    _commandHandler.HandleByAuthor();
                    break;

                case ParameterSerchBookByGenre:
                    _commandHandler.HandleByGenre();
                    break;

                case ParameterSerchBookBYear:
                    _commandHandler.HandleByYear();
                    break;

                case ParameterExit:
                    IsActiv = false;
                    break;

                default:
                    Console.WriteLine("Ошибка ввода.");
                    break;
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine($"Для того чтобы посмотреть список всех доступных книг введите - {ParameterShowAllBooks}");
            Console.WriteLine($"Для того чтобы добавить свою книгу введите - {ParameterAddNewBook}");
            Console.WriteLine($"Для того чтобы выкинуть книгу введите - {ParameterDropBook}");
            Console.WriteLine($"Для поиска книг по названию введите - {ParameterSerchBookByTitle}");
            Console.WriteLine($"Для поиска книг по автору введите - {ParameterSerchBookByAuthor}");
            Console.WriteLine($"Для поиска книг по жанру введите - {ParameterSerchBookByGenre}");
            Console.WriteLine($"Для поиска книг по году введите - {ParameterSerchBookBYear}");
            Console.WriteLine($"Для выхода введите - {ParameterExit}\n\n");
        }
    }

    public class Librarian
    {
        private ICollection<Book> _books;

        public Librarian(ICollection<Book> books)
        {
            _books = books;
        }

        public IReadOnlyCollection<Book> SearchByTitle(string title) => _books.Where(book => book.Title.Contains(title)).ToList();

        public IReadOnlyCollection<Book> SearchByAuthor(string author) => _books.Where(book => book.Author.Contains(author)).ToList();

        public IReadOnlyCollection<Book> SearchByGenre(string genre) => _books.Where(book => book.Genre.Contains(genre)).ToList();

        public IReadOnlyCollection<Book> SearchByYear(int year) => _books.Where(book => book.Year == year).ToList();
    }

    public class Book
    {
        public Book(string title, string author, string genre, int year)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
        }

        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Genre { get; private set; }
        public int Year { get; private set; }
    }

    public class Library
    {
        public Library()
        {
            _books = new List<Book>();
        }

        private List<Book> _books;

        public void AddBook(Book book) => _books.Add(book);

        public void RemoveBook(Book book) => _books.Remove(book);

        public IReadOnlyCollection<Book> LibraryBooks => _books;
    }
}
