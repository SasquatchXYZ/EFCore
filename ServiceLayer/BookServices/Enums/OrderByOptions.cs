using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BookServices.Enums;

public enum OrderByOptions
{
    [Display(Name = "Sort by...")]
    SimpleOrder = 0,

    [Display(Name = "Votes ↑")]
    ByVotes,

    [Display(Name = "Publication Date ↑")]
    ByPublicationDate,

    [Display(Name = "Price ↓")]
    ByPriceLowestFirst,

    [Display(Name = "Price ↑")]
    ByPriceHighestFirst,
}
