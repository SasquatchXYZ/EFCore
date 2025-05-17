using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.AdminServices;

public class ChangePubDateDto
{
    public int BookId { get; set; }
    public string Title { get; set; }
    [DataType(DataType.Date)] public DateTime PublishedOn { get; set; }
}
