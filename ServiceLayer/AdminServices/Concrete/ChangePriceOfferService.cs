using System.ComponentModel.DataAnnotations;
using DataLayer.EfClasses;
using DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.AdminServices.Concrete;

public class ChangePriceOfferService : IChangePriceOfferService
{
    private readonly EfCoreContext _context;

    public ChangePriceOfferService(EfCoreContext context)
    {
        _context = context;
    }

    public Book? OrgBook { get; private set; }

    public PriceOffer GetOriginal(int id)
    {
        OrgBook = _context.Books
            .Include(book => book.Promotion)
            .Single(book => book.BookId == id);

        return OrgBook.Promotion
               ?? new PriceOffer
               {
                   BookId = id,
                   NewPrice = OrgBook.Price
               };
    }

    public ValidationResult? AddRemovePriceOffer(PriceOffer promotion)
    {
        var book = _context.Books
            .Include(book => book.Promotion)
            .Single(book => book.BookId == promotion.BookId);

        if (book.Promotion is not null)
        {
            _context.Remove(book.Promotion);
            _context.SaveChanges();
            return null;
        }

        if (string.IsNullOrEmpty(promotion.PromotionalText))
        {
            return new ValidationResult(
                errorMessage: "This field cannot be null or empty",
                memberNames: [nameof(PriceOffer.PromotionalText)]);
        }

        book.Promotion = promotion;
        _context.SaveChanges();

        return null;
    }
}
