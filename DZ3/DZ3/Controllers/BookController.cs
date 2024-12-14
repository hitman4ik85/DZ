using DZ3.Data;
using DZ3.Models;
using Microsoft.AspNetCore.Mvc;

namespace DZ3.Controllers;

public class BookController : Controller
{
    private static readonly IDatabase<Book> _bookDatabase = new BookDatabase();

    static BookController()
    {
        _bookDatabase.Add(new Book
        {
            Id = 1,
            Name = "Book Kate",
            Price = 100,
            Pages = 200,
            Author = new Author { FirstName = "Kate", LastName = "Rjan" }
        });

        _bookDatabase.Add(new Book
        {
            Id = 2,
            Name = "Book Lenni",
            Price = 150,
            Pages = 300,
            Author = new Author { FirstName = "Lenni", LastName = "Kravic" }
        });
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        var books = _bookDatabase.Get();
        return View(books);
    }

    [HttpGet]
    public IActionResult AddBook()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddBook(Book book, string authorFirstName, string authorLastName)
    {
        book.Id = _bookDatabase.Get().Count() + 1;

        book.Author = new Author
        {
            FirstName = authorFirstName,
            LastName = authorLastName
        };

        _bookDatabase.Add(book);

        return RedirectToAction(nameof(GetBooks));
    }

    [HttpGet]
    public IActionResult DeleteBook(int id)
    {
        var book = _bookDatabase.Get().FirstOrDefault(x => x.Id == id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    [HttpPost]
    public IActionResult DeleteBook(Book book)
    {
        _bookDatabase.Remove(book);
        return RedirectToAction(nameof(GetBooks));
    }

    [HttpGet]
    public IActionResult EditBook(int id)
    {
        return View(_bookDatabase.Get().First(x => x.Id == id));
    }

    [HttpPost]
    public IActionResult EditBook(Book book, string authorFirstName, string authorLastName)
    {
        var oldBook = _bookDatabase.Get().First(x => x.Id == book.Id);

        oldBook.Name = book.Name;
        oldBook.Price = book.Price;
        oldBook.Pages = book.Pages;

        oldBook.Author.FirstName = authorFirstName;
        oldBook.Author.LastName = authorLastName;

        return RedirectToAction(nameof(GetBooks));
    }
}
