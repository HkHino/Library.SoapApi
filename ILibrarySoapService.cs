using Library.SoapApi.Faults;
using Library.SoapApi.Models;
using System.ServiceModel;

[ServiceContract]
public interface ILibrarySoapService
{
    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    int CreateBook(string title, int authorId, int publishingCompanyId, int publishingYear);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Book GetBookById(int id);
}

public class LibrarySoapService : ILibrarySoapService
{
    private static List<Book> _books = new();
    private static int _nextId = 1;

    public int CreateBook(string title, int authorId, int publishingCompanyId, int publishingYear)
    {
        var book = new Book
        {
            Id = _nextId++,
            Title = title,
            AuthorId = authorId,
            PublishingCompanyId = publishingCompanyId,
            PublishingYear = publishingYear
        };

        _books.Add(book);

        return book.Id;
    }

    public Book GetBookById(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);

        return book; // we will fix error handling later
    }
}