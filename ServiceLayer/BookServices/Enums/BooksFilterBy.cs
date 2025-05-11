using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BookServices.Enums;

public enum BooksFilterBy
{
    [Display(Name = "All")]
    NoFilter = 0,

    [Display(Name = "By Votes...")]
    ByVotes,

    [Display(Name = "By Categories...")]
    ByTags,

    [Display(Name = "By Year Published...")]
    ByPublicationYear,
}
