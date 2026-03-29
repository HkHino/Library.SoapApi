using Library.SoapApi.Faults;
using Library.SoapApi.Models;
using System.ServiceModel;

/// <summary>
/// This interface defines the SOAP service contract.
/// 
/// In SOAP, the interface defines ALL operations available to clients
/// and It is used to generate the WSDL (the API contract)
/// 
/// Think of this as:
/// "What methods does our SOAP API expose?"
/// </summary>
/// 
[ServiceContract(Namespace = "http://library.soapapi.org/")]
public interface ILibrarySoapService
{
    /// <summary>
    /// Creates a new book.
    /// 
    /// OperationContract:
    /// Exposes this method as a SOAP operation
    /// 
    /// FaultContract:
    /// Defines that this method may return a ValidationFault
    /// This becomes part of the WSDL
    /// This repeats for each method and each possible fault, making the API contract very explicit
    /// </summary>
    // BOOK
    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    int CreateBook(string title, int authorId, int publishingCompanyId, int publishingYear);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Book GetBookById(int id);

    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    [FaultContract(typeof(NotFoundFault))]
    void UpdateBook(Book book);

    // AUTHOR
    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    int CreateAuthor(string name, string surname);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    Author GetAuthorById(int id);

    /// <summary>
    /// Returns all authors.
    /// SOAP automatically handles lists (ArrayOfAuthor in WSDL)
    /// </summary>
    [OperationContract]
    List<Author> ListAuthors();

    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    [FaultContract(typeof(NotFoundFault))]
    void UpdateAuthor(Author author);

    // PUBLISHING COMPANY
    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    int CreatePublishingCompany(string name);

    [OperationContract]
    [FaultContract(typeof(NotFoundFault))]
    PublishingCompany GetPublishingCompanyById(int id);

    [OperationContract]
    List<PublishingCompany> ListPublishingCompanies();

    [OperationContract]
    [FaultContract(typeof(ValidationFault))]
    [FaultContract(typeof(NotFoundFault))]
    void UpdatePublishingCompany(PublishingCompany company);
}

public class LibrarySoapService : ILibrarySoapService
{
    /// <summary>
    /// Returns all authors.
    /// SOAP automatically handles lists (ArrayOfAuthor in WSDL)
    /// </summary>
    private static List<Book> _books = new();
    private static int _nextId = 1;
    private static List<Author> _authors = new();
    private static int _nextAuthorId = 1;
    private static List<PublishingCompany> _companies = new();
    private static int _nextCompanyId = 1;

    public int CreateBook(string title, int authorId, int publishingCompanyId, int publishingYear)
{
    if (string.IsNullOrWhiteSpace(title))
    {
        throw new FaultException<ValidationFault>(
            new ValidationFault { Message = "Title is required" }
        );
    }

    if (publishingYear < 1900)
    {
        throw new FaultException<ValidationFault>(
            new ValidationFault { Message = "Publishing year must be >= 1900" }
        );
    }

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

        if (book == null)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = "Book not found" }
            );
        }

        return book;
    }

    public int CreateAuthor(string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = "Name and surname are required" }
            );
        }

        var author = new Author
        {
            Id = _nextAuthorId++,
            Name = name,
            Surname = surname
        };

        _authors.Add(author);

        return author.Id;
    }

    public Author GetAuthorById(int id)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);

        if (author == null)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = "Author not found" }
            );
        }

        return author;
    }

    public List<Author> ListAuthors()
    {
        return _authors;
    }

    public void DeleteAuthor(int id)
    {
        var author = _authors.FirstOrDefault(a => a.Id == id);

        if (author == null)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = "Author not found" }
            );
        }

        // Check if any book uses this author
        var isUsed = _books.Any(b => b.AuthorId == id);

        if (isUsed)
        {
            throw new FaultException<ConflictFault>(
                new ConflictFault { Message = "Cannot delete author: referenced by books" }
            );
        }

        _authors.Remove(author);
    }

    public int CreatePublishingCompany(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = "Name is required" }
            );
        }

        var company = new PublishingCompany
        {
            Id = _nextCompanyId++,
            Name = name
        };

        _companies.Add(company);

        return company.Id;
    }

    public PublishingCompany GetPublishingCompanyById(int id)
    {
        var company = _companies.FirstOrDefault(c => c.Id == id);

        if (company == null)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = "Publishing company not found" }
            );
        }

        return company;
    }

    public List<PublishingCompany> ListPublishingCompanies()
    {
        return _companies;
    }

    public void DeletePublishingCompany(int id)
    {
        var company = _companies.FirstOrDefault(c => c.Id == id);

        if (company == null)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = "Publishing company not found" }
            );
        }

        // Check if any book uses this company
        var isUsed = _books.Any(b => b.PublishingCompanyId == id);

        if (isUsed)
        {
            throw new FaultException<ConflictFault>(
                new ConflictFault { Message = "Cannot delete publishing company: referenced by books" }
            );
        }

        _companies.Remove(company);
    }

    public void UpdateBook(Book book)
    {
        if (book == null || string.IsNullOrWhiteSpace(book.Title))
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = "Invalid book data" }
            );
        }

        if (book.PublishingYear < 1900)
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = "Publishing year must be >= 1900" }
            );
        }

        var existing = _books.FirstOrDefault(b => b.Id == book.Id);

        if (existing == null)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = "Book not found" }
            );
        }

        existing.Title = book.Title;
        existing.AuthorId = book.AuthorId;
        existing.PublishingCompanyId = book.PublishingCompanyId;
        existing.PublishingYear = book.PublishingYear;
    }

    public void UpdateAuthor(Author author)
    {
        if (author == null || string.IsNullOrWhiteSpace(author.Name) || string.IsNullOrWhiteSpace(author.Surname))
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = "Invalid author data" }
            );
        }

        var existing = _authors.FirstOrDefault(a => a.Id == author.Id);

        if (existing == null)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = "Author not found" }
            );
        }

        existing.Name = author.Name;
        existing.Surname = author.Surname;
    }

    public void UpdatePublishingCompany(PublishingCompany company)
    {
        if (company == null || string.IsNullOrWhiteSpace(company.Name))
        {
            throw new FaultException<ValidationFault>(
                new ValidationFault { Message = "Invalid company data" }
            );
        }

        var existing = _companies.FirstOrDefault(c => c.Id == company.Id);

        if (existing == null)
        {
            throw new FaultException<NotFoundFault>(
                new NotFoundFault { Message = "Publishing company not found" }
            );
        }

        existing.Name = company.Name;
    }
}