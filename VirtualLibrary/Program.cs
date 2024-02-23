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
        const int ParameterYes = 1;
        const int ParameterNo = 2;

        private string _title;
        private string _author;
        private string _genre;
        private string _userInput;
        private Library _library;
        private List<Book> _myBooks;
        private List<Book> _searchingBooks;
        private Librarian _libraryan;

        public CommandHandler(Library library)
        {
            _library = library;
            _myBooks = new List<Book>();
            _searchingBooks = new List<Book>();
            _libraryan = new Librarian((ICollection<Book>)library.ReadOnlyBooksCollection);
        }

        public void ShowAllBooks()
        {
            PrintBookInfofmation((ICollection<Book>)_library.ReadOnlyBooksCollection);
        }

        public void ShowMyBooks()
        {
            if (_myBooks.Count != 0)
                PrintBookInfofmation(_myBooks);
            else
                Console.WriteLine("Вы пока не брали книг попробуйте взять");
        }

        public void AddNewBook()
        {
            Console.Write("Ведите название книги. \n");

            _title = Console.ReadLine();

            Console.Write("Ведите автора книги. \n");

            _author = Console.ReadLine();

            Console.Write("Ведите жанр книги. \n");

            _genre = Console.ReadLine();

            Console.Write("Ведите год выхода книги. \n");

            int.TryParse(Console.ReadLine(), out int year);

            if (year <= DateTime.Now.Year && year != 0)
            {
                _library.AddBook(new Book(_title, _author, _genre, year));

                Console.WriteLine($"Книга {_title} добавлена!\n");
            }
            else
            {
                Console.WriteLine("Книга не была добавлена вы ввели не корректный год!\n");
            }
        }

        public void TakeBook()
        {
            Console.Write("Введите название книги которую хотите взять: ");

            _title = Console.ReadLine();

            _searchingBooks = _libraryan.SearchByTitle(_title);

            if (_searchingBooks.Count >= 1)
            {
                foreach (Book book in _searchingBooks)
                {
                    _library.TakeBook(book);
                    _myBooks.Add(book);
                }

                Console.WriteLine("Вы успеешно взялди книгу.\n");
            }
            else
            {
                Console.WriteLine("К сожалению такой книги нет в библиотеке.\n");
            }

            _searchingBooks.Clear();
        }

        public void GiveBook()
        {
            Console.Write("Введите название книги которую хотите сдать: ");

            _title = Console.ReadLine();

            _searchingBooks = _myBooks.Where(book => book.Title.Contains(_title)).ToList();

            if (_searchingBooks.Count >= 1)
            {
                foreach (Book book in _searchingBooks)
                {
                    _library.AddBook(book);
                    _myBooks.Remove(book);
                }

                Console.WriteLine("Вы успеешно сдали книгу.\n");
            }
            else
            {
                Console.WriteLine("У вас нет такой книги.\n");
            }

            _searchingBooks.Clear();
        }

        public void DropBook()
        {
            Console.Write("Введите название книги которую хотите выкинуть: ");

            _title = Console.ReadLine();

            _searchingBooks = _myBooks.Where(book => book.Title.Contains(_title)).ToList();

            if (_searchingBooks.Count >= 1)
            {
                foreach (Book book in _searchingBooks)
                    _myBooks.Remove(book);

                Console.WriteLine("Вы безвозвратно выкинули книгу.\n");
            }
            else
            {
                Console.WriteLine("У вас нет такой книги, возьмите в библиотеке или создайте.\n");
            }

            _searchingBooks.Clear();
        }

        public void HandleByTitle()
        {
            Console.Write("Введите название книги которую хотите найти: ");

            _title = Console.ReadLine();

            _searchingBooks = _libraryan.SearchByTitle(_title);

            ConteinedBooks("В библиотеке нет данной книги.\n");
        }

        public void HandleByAuthor()
        {
            Console.Write("Введите автора книг которые хотите найти: ");

            _author = Console.ReadLine();

            _searchingBooks = _libraryan.SearchByAuthor(_author);

            ConteinedBooks("В библиотеке нет книг данного автора.\n");
        }

        public void HandleByGenre()
        {
            Console.Write("Введите жанр книг которые хотите найти: ");

            _genre = Console.ReadLine();

            _searchingBooks = _libraryan.SearchByGenre(_genre);

            ConteinedBooks("В библиотеке нет книг данного жанра.\n");
        }

        public void HandleByYear()
        {
            Console.Write("Введите год книг которые хотите найти: ");

            _userInput = Console.ReadLine();

            if (int.TryParse(_userInput, out int year))
            {
                _searchingBooks = _libraryan.SearchByYear(year);

                ConteinedBooks("В библиотеке нет книг данного года.\n");
            }
            else
            {
                Console.WriteLine("Неверный ввод!");
            }
        }

        public void ChoosingBook()
        {
            if (_searchingBooks.Count >= 1)
            {
                ShowBooks(_searchingBooks);

                Console.WriteLine($"Хотите ли вы взять книгу? {ParameterYes} - да, {ParameterNo} - нет.");

                _userInput = Console.ReadLine();

                int.TryParse(_userInput, out int number);

                if (number == ParameterYes)
                {                   
                    if (_searchingBooks.Count > 1)
                    {
                        Console.WriteLine($"Если хотите взять любую из этих книг введите ее название.\n");

                        _userInput = Console.ReadLine();
                        _searchingBooks = _libraryan.SearchByTitle(_userInput);                       
                    }              

                    if (_searchingBooks.Count == 1)
                    {
                        GetBook();    
                            
                        Console.WriteLine("Поздравляю, вы взяли книгу!");
                    }                    
                }
                else if (number == ParameterNo)
                {
                    Console.WriteLine("Для продолжения нажмите любую кнопку.");
                }
            }

            _searchingBooks.Clear();
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

        private void GetBook()
        {
            foreach (Book book in _searchingBooks)
            {
                _library.TakeBook(book);
                _myBooks.Add(book);
            }
        }

        private void ShowBooks(ICollection<Book> books)
        {
            foreach (var book in books)
                Console.WriteLine($"{book.Title}\n{book.Author}\n{book.Genre}\n{book.Year}\n\n");
        }

        private void ConteinedBooks(string messenge)
        {
            if (_searchingBooks.Count < 1)
                Console.WriteLine(messenge);
        }

        private void PrintBookInfofmation(ICollection<Book> books)
        {
            foreach (var book in books)
                Console.WriteLine($"{book.Title}\n{book.Author}\n{book.Genre}\n{book.Year}\n\n");
        }
    }

    public class InputHandler
    {
        const int ParameterShowAllBooks = 1;
        const int ParameterShowMyBooks = 2;
        const int ParameterAddNewBook = 3;
        const int ParameterTakeBook = 4;
        const int ParameterGiveBook = 5;
        const int ParameterDropBook = 6;
        const int ParameterSerchBookByTitle = 7;
        const int ParameterSerchBookByAuthor = 8;
        const int ParameterSerchBookByGenre = 9;
        const int ParameterSerchBookBYear = 10;
        const int ParameterExit = 11;

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

            _commandHandler.ChoosingBook();
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

        public IReadOnlyCollection<Book> ReadOnlyBooksCollection => _books;
       
    }
}
