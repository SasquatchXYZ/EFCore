namespace DataLayer.EfClasses;

public class PriceOffer
{
    public int PriceOfferId { get; set; }
    public decimal NewPrice { get; set; }
    public string PromotionalText { get; set; }

    // Relationships
    public int BookId { get; set; }
}
