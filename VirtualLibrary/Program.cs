using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VirtualLibrary
{
    internal class Program
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
        const int ParameterYes = 1;
        const int ParameterNo = 2;

        static void Main()
        {           
            string userInput;
            string title;
            string author;
            string genre;
            bool isActiv = true;

            Library library = new Library();
            List<Book> myBooks = new List<Book>();
            List<Book> searchingBooks = new List<Book>();

            Book book1 = new Book("Игра Эндера", "Кард Орсон Скотт", "Фантастика", 1985);
            Book book2 = new Book("Тень Эндера", "Кард Орсон Скотт", "Фантастика", 1999);
            Book book3 = new Book("Седьмое Солнце: игры с вниманием", "Рина Ра", "Паранормальное", 2021);
            Book book4 = new Book("Арсанты: Дети богов", "Фарутин Антон", "Фантастика", 2020);
            Book book5 = new Book("Арсанты 2. Линии судьбы", "Фарутин Антон", "Фантастика", 2021);
            Book book6 = new Book("Я - Легенда", "Матесон Ричард", "Постапокалиптика", 1954);

            library.AddBook(book1);
            library.AddBook(book2);
            library.AddBook(book3);
            library.AddBook(book4);
            library.AddBook(book5);
            library.AddBook(book6);

            SerchBook libraryBooks = new SerchBook((ICollection<Book>)library.ReadOnlyBooksCollection);

            while (isActiv)
            {
                ShowMenu();

                userInput = Console.ReadLine();

                int.TryParse(userInput, out int parameter);

                if (parameter != 0)
                {
                    if (parameter == ParameterShowAllBooks)
                    {
                        ShowBooks((ICollection<Book>)library.ReadOnlyBooksCollection);
                    }

                    if (parameter == ParameterShowMyBooks)
                    {
                        if (myBooks.Count != 0)
                        {
                            ShowBooks(myBooks);
                        }
                        else
                        {
                            Console.WriteLine("Вы пока не брали книг попробуйте взять");
                        }
                    }

                    if (parameter == ParameterAddNewBook)
                    {
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
                            library.AddBook(new Book(title, author, genre, year));

                            Console.ForegroundColor = ConsoleColor.Green;

                            Console.WriteLine($"Книга {title} добавлена!\n");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;

                            Console.WriteLine("Книга не была добавлена вы ввели не корректный год!\n");
                        }

                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    if (parameter == ParameterTakeBook)
                    {
                        Console.Write("Введите название книги которую хотите взять: ");

                        title = Console.ReadLine();

                        searchingBooks = libraryBooks.SearchByTitle(title);

                        if (searchingBooks.Count >= 1)
                        {
                            foreach (Book book in searchingBooks)
                            {
                                library.TakeBook(book);
                                myBooks.Add(book);
                            }

                            Console.WriteLine("Вы успеешно взялди книгу.\n");
                        }
                        else
                        {
                            Console.WriteLine("К сожалению такой книги нет в библиотеке.\n");
                        }

                        searchingBooks.Clear();
                    }
                    
                    if (parameter == ParameterGiveBook)
                    {
                        Console.Write("Введите название книги которую хотите сдать: ");

                        title = Console.ReadLine();

                        searchingBooks = myBooks.Where(book => book.Title.Contains(title)).ToList();

                        if (searchingBooks.Count >= 1)
                        {
                            foreach (Book book in searchingBooks)
                            {
                                library.AddBook(book);
                                myBooks.Remove(book);
                            }

                            Console.WriteLine("Вы успеешно сдали книгу.\n");
                        }
                        else
                        {
                            Console.WriteLine("У вас нет такой книги.\n");
                        }

                        searchingBooks.Clear();
                    }
                    
                    if (parameter == ParameterDropBook)
                    {
                        Console.Write("Введите название книги которую хотите выкинуть: ");

                        title = Console.ReadLine();

                        searchingBooks = myBooks.Where(book => book.Title.Contains(title)).ToList();

                        if (searchingBooks.Count >= 1)
                        {
                            foreach (Book book in searchingBooks)                           
                                myBooks.Remove(book);                           

                            Console.WriteLine("Вы безвозвратно выкинули книгу.\n");
                        }
                        else
                        {
                            Console.WriteLine("У вас нет такой книги, возьмите в библиотеке или создайте.\n");
                        }

                        searchingBooks.Clear();
                    }

                    if (parameter == ParameterSerchBookByTitle)
                    {
                        Console.Write("Введите название книги которую хотите найти: ");

                        title = Console.ReadLine();

                        searchingBooks = libraryBooks.SearchByTitle(title);

                        ConteinedBooks(searchingBooks, "В библиотеке нет данной книги.\n");
                    }

                    if (parameter == ParameterSerchBookByAuthor)
                    {
                        Console.Write("Введите автора книг которые хотите найти: ");

                        author = Console.ReadLine();

                        searchingBooks = libraryBooks.SearchByAuthor(author);

                        ConteinedBooks(searchingBooks, "В библиотеке нет книг данного автора.\n");
                    }

                    if (parameter == ParameterSerchBookByGenre)
                    {
                        Console.Write("Введите жанр книг которые хотите найти: ");

                        genre = Console.ReadLine();

                        searchingBooks = libraryBooks.SearchByGenre(genre);

                        ConteinedBooks(searchingBooks, "В библиотеке нет книг данного жанра.\n");
                    }

                    if (parameter == ParameterSerchBookBYear)
                    {
                        Console.Write("Введите год книг которые хотите найти: ");

                        userInput = Console.ReadLine();

                        if (int.TryParse(userInput, out int year))
                        {
                            searchingBooks = libraryBooks.SearchByYear(year);

                            ConteinedBooks(searchingBooks, "В библиотеке нет книг данного года.\n");
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                    }

                    ChoosingBook(searchingBooks, userInput, myBooks, library, libraryBooks);

                    if (parameter == ParameterExit)
                    {
                        isActiv = false;
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка ввода!");
                }

                searchingBooks.Clear();

                Console.ReadKey();

                Console.Clear();
            }
        }

        static private void ShowMenu()
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

        static private void ChoosingBook(List<Book> searchingBooks, string userInput, List<Book> myBooks, Library library, SerchBook libraryBooks)
        {
            if (searchingBooks.Count >= 1)
            {
                ShowBooks(searchingBooks);

                Console.WriteLine($"Хотите ли вы взять книгу? {ParameterYes} - да, {ParameterNo} - нет.");

                userInput = Console.ReadLine();

                int.TryParse(userInput, out int number);

                if (number == ParameterYes)
                {
                    if (searchingBooks.Count == 1)
                    {
                        GetBook(searchingBooks, myBooks, library);
                    }
                    else if (searchingBooks.Count > 1)
                    {
                        Console.WriteLine($"Если хотите взять любую из этих книг введите ее название.\n");

                        userInput = Console.ReadLine();

                        searchingBooks = libraryBooks.SearchByTitle(userInput);

                        if (searchingBooks.Count >= 1)
                        {
                            GetBook(searchingBooks, myBooks, library);
                        }
                    }

                    if (searchingBooks.Count == 1)
                    {
                        Console.WriteLine("Поздравляю, вы взяли книгу!");
                    }
                }
                else if (number == ParameterNo)
                {
                    Console.WriteLine("Для продолжения нажмите любую кнопку.");
                }
                else if (number == 0 || number > ParameterNo)
                {
                    Console.WriteLine("Вы ввели невернрое значение!");
                }
            }
        }

        static private void GetBook(List<Book> searchingBooks, List<Book> myBooks, Library library)
        {
            foreach (Book book in searchingBooks)
            {
                library.TakeBook(book);
                myBooks.Add(book);
            }
        }

        static private void ShowBooks(ICollection<Book> books)
        {
            foreach (var book in books)
                Console.WriteLine($"{book.Title}\n{book.Author}\n{book.Genre}\n{book.Year}\n\n");
        }

        static private void ConteinedBooks(List<Book> searchingBooks, string messenge)
        {
            if (searchingBooks.Count < 1)
                Console.WriteLine(messenge);
        }
    }

    public class SerchBook
    {
        private ICollection<Book> _books;

        public SerchBook(ICollection<Book> books)
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
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Genre { get; private set; }
        public int Year { get; private set; }

        public Book(string title, string author, string genre, int year)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
        }
    }

    public class Library
    {
        const string ExceptionMessenge = "В библиотеке нет книг";

        private List<Book> _books;

        public Library()
        {
            _books = new List<Book>();
        }

        public void AddBook(Book book) => _books.Add(book);

        public void TakeBook(Book book) => _books.Remove(book);

        public IReadOnlyCollection<Book> ReadOnlyBooksCollection => _books.Count >= 1 ? _books : throw new Exception(ExceptionMessenge);
    }
}
