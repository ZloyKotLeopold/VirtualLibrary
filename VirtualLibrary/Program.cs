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
                inputHandler.ProcessingInput();

                Console.ReadKey();

                Console.Clear();
            }
        }
    }

    public class CommandHandler
    {
        private const int ParameterPressYes = 1;
        private const int ParameterPressNo = 2;

        private Library _library;
        private List<Book> _readingBooks;
        private Librarian _libraryan;

        public CommandHandler(Library library)
        {
            _library = library;
            _readingBooks = new List<Book>();
            _libraryan = new Librarian((ICollection<Book>)library.LibraryBooks);
        }

        public void ShowAllBooks()
        {
            PrintBookInfofmation((ICollection<Book>)_library.LibraryBooks);
        }

        public void ShowMyBooks()
        {
            if (_readingBooks.Count != 0)
                PrintBookInfofmation(_readingBooks);
            else
                Console.WriteLine("Вы пока не брали книг попробуйте взять");
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

        public void TakeBook()
        {
            string title;

            Console.Write("Введите название книги которую хотите взять: ");

            title = Console.ReadLine();

            foreach (Book book in _libraryan.SearchByTitle(title))
            {
                _library.TakeBook(book);
                _readingBooks.Add(book);

                Console.WriteLine("Вы успеешно взялди книгу.\n");
            }
        }

        public void GiveBook()
        {
            string title;

            Console.Write("Введите название книги которую хотите сдать: ");

            title = Console.ReadLine();

            foreach (Book book in _readingBooks.Where(book => book.Title.Contains(title)).ToList())
            {
                _library.AddBook(book);
                _readingBooks.Remove(book);

                Console.WriteLine("Вы успеешно сдали книгу.\n");
            }
        }

        public void DropBook()
        {
            string title;

            Console.Write("Введите название книги которую хотите выкинуть: ");

            title = Console.ReadLine();

            foreach (Book book in _readingBooks.Where(book => book.Title.Contains(title)).ToList())
            {
                _readingBooks.Remove(book);

                Console.WriteLine("Вы безвозвратно выкинули книгу.\n");
            }
        }

        public void HandleByTitle()
        {
            string title;

            Console.Write("Введите название книги которую хотите найти: ");

            title = Console.ReadLine();

            ChoosingBook(_libraryan.SearchByTitle(title));
        }

        public void HandleByAuthor()
        {
            string author;

            Console.Write("Введите автора книг которые хотите найти: ");

            author = Console.ReadLine();

            ChoosingBook(_libraryan.SearchByAuthor(author));
        }

        public void HandleByGenre()
        {
            string genre;

            Console.Write("Введите жанр книг которые хотите найти: ");

            genre = Console.ReadLine();

            ChoosingBook(_libraryan.SearchByGenre(genre));
        }

        public void HandleByYear()
        {
            string userInput;

            Console.Write("Введите год книг которые хотите найти: ");

            userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int year))           
                ChoosingBook(_libraryan.SearchByYear(year));            
            else            
                Console.WriteLine("Неверный ввод!");            
        }

        public void FillingLibrary()
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

        private void ChoosingBook(ICollection<Book> books)
        {
            string userInput;

            if (books.Count >= 1)
            {
                ShowBooks(books);

                Console.WriteLine($"Хотите ли вы взять книгу? {ParameterPressYes} - да, {ParameterPressNo} - нет.");

                userInput = Console.ReadLine();

                int.TryParse(userInput, out int number);

                if (number == ParameterPressYes)
                {
                    if (books.Count > 1)
                    {
                        Console.WriteLine($"Если хотите взять любую из этих книг введите ее название.\n");

                        userInput = Console.ReadLine();

                        GetBook(_libraryan.SearchByTitle(userInput));
                    }

                    if (books.Count == 1)
                    {
                        GetBook(books);

                        Console.WriteLine("Поздравляю, вы взяли книгу!");
                    }
                }
                else if (number == ParameterPressNo)
                {
                    Console.WriteLine("Для продолжения нажмите любую кнопку.");
                }
            }
            else
            {
                Console.WriteLine("Ни одной такой книги нет.");
            }
        }

        private void GetBook(ICollection<Book> books)
        {
            foreach (Book book in books)
            {
                _library.TakeBook(book);
                _readingBooks.Add(book);
            }
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
        private const int ParameterShowMyBooks = 2;
        private const int ParameterAddNewBook = 3;
        private const int ParameterTakeBook = 4;
        private const int ParameterGiveBook = 5;
        private const int ParameterDropBook = 6;
        private const int ParameterSerchBookByTitle = 7;
        private const int ParameterSerchBookByAuthor = 8;
        private const int ParameterSerchBookByGenre = 9;
        private const int ParameterSerchBookBYear = 10;
        private const int ParameterExit = 11;

        private string _userInput;
        private CommandHandler _commandHandler;

        public InputHandler(CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
            _commandHandler.FillingLibrary();
        }

        public bool IsActiv { get; private set; } = true;

        public void ProcessingInput()
        {
            ShowMenu();

            _userInput = Console.ReadLine();

            int.TryParse(_userInput, out int parameter);

            switch (parameter)
            {
                case ParameterShowAllBooks:
                    _commandHandler.ShowAllBooks();
                    break;

                case ParameterShowMyBooks:
                    _commandHandler.ShowMyBooks();
                    break;

                case ParameterAddNewBook:
                    _commandHandler.AddNewBook();
                    break;

                case ParameterTakeBook:
                    _commandHandler.TakeBook();
                    break;

                case ParameterGiveBook:
                    _commandHandler.GiveBook();
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
            Console.WriteLine($"Для того чтобы посмотреть список всех моих книг введите - {ParameterShowMyBooks}");
            Console.WriteLine($"Для того чтобы добавить свою книгу введите - {ParameterAddNewBook}");
            Console.WriteLine($"Для того чтобы взять книгу введите - {ParameterTakeBook}");
            Console.WriteLine($"Для того чтобы сдать книгу введите - {ParameterGiveBook}");
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

        public List<Book> SearchByTitle(string title) => _books.Where(book => book.Title.Contains(title)).ToList();

        public List<Book> SearchByAuthor(string author) => _books.Where(book => book.Author.Contains(author)).ToList();

        public List<Book> SearchByGenre(string genre) => _books.Where(book => book.Genre.Contains(genre)).ToList();

        public List<Book> SearchByYear(int year) => _books.Where(book => book.Year == year).ToList();
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

        public void TakeBook(Book book) => _books.Remove(book);

        public IReadOnlyCollection<Book> LibraryBooks => _books;

    }
}
