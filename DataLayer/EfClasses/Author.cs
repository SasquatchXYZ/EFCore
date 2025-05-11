namespace DataLayer.EfClasses;

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; }

    // Relationships
    public ICollection<BookAuthor> BooksLink { get; set; }
}
