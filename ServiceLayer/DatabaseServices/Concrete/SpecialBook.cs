using DataLayer.EfClasses;

namespace ServiceLayer.DatabaseServices.Concrete;

public static class SpecialBook
{
    public static Book CreateSpecialBook()
    {
        var book = new Book
        {
            Title = "Quantum Networking",
            Description = "Entangled quantum networking provides faster-than-light data communications",
            PublishedOn = new DateTime(2057, 1, 1),
            Price = 220,
            Tags = new List<Tag>
            {
                new()
                {
                    TagId = "Quantum Entanglement"
                }
            }
        };

        book.AuthorsLink = new List<BookAuthor>
        {
            new()
            {
                Author = new Author
                {
                    Name = "Future Person"
                },
                Book = book
            }
        };

        book.Reviews = new List<Review>
        {
            new()
            {
                VoterName = "Jon P Smith",
                NumStars = 5,
                Comment = "I look forward to reading this book, if I am still alive!"
            },
            new()
            {
                VoterName = "Albert Einstein",
                NumStars = 5,
                Comment = "I will write this book if I was still alive!v"
            }
        };

        book.Promotion = new PriceOffer
        {
            NewPrice = 219,
            PromotionalText = "Save $1 if you order 40 years ahead!"
        };

        return book;
    }
}
