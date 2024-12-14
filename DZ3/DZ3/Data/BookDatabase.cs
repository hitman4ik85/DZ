using DZ3.Models;

namespace DZ3.Data;

public class BookDatabase : IDatabase<Book>
{
    private readonly List<Book> _books;

    public BookDatabase()
    {
        _books = new List<Book>();
    }

    public void Add(Book item)
    {
        _books.Add(item);
    }

    public IEnumerable<Book> Get()
    {
        return _books;
    }

    public void Remove(Book book)
    {
        _books.RemoveAll(x => x.Id == book.Id);
    }

    public void Update(Book oldBook, Book newBook)
    {
        int index = _books.FindIndex(x => x.Id == oldBook.Id);
        if (index >= 0)
        {
            _books[index] = newBook;
        }
    }
}
