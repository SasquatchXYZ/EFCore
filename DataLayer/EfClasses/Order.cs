namespace DataLayer.EfClasses;

public class Order
{
    public int OrderId { get; set; }
    public DateTime DateOrderedUtc { get; set; }
    public Guid CustomerId { get; set; }

    // Relationships
    public ICollection<LineItem> LineItems { get; set; }

    // Extra Columns not used by EF
    public string OrderNumber => $"SO{OrderId:D6}";

    public Order()
    {
        DateOrderedUtc = DateTime.UtcNow;
    }
}
