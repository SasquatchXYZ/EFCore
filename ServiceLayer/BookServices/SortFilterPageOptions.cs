using ServiceLayer.BookServices.Enums;

namespace ServiceLayer.BookServices;

public class SortFilterPageOptions
{
    public const int DefaultPageSize = 10;

    private int _pageNum = 1;
    private int _pageSize = DefaultPageSize;

    public int[] PageSizes = [5, DefaultPageSize, 20, 50, 100, 500, 1000];
    public OrderByOptions OrderByOptions { get; set; }
    public BooksFilterBy FilterBy { get; set; }
    public string FilterValue { get; set; }

    public int PageNum
    {
        get => _pageNum;
        set => _pageNum = value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value;
    }

    public int NumPages { get; private set; }
    public string PrevCheckState { get; set; }

    public void SetupRestOfDto<T>(IQueryable<T> query)
    {
        NumPages = (int) Math.Ceiling(
            (double) query.Count() / PageSize);

        PageNum = Math.Min(
            Math.Max(1, PageNum), NumPages);

        var newCheckState = GenerateCheckState();
        if (PrevCheckState != newCheckState)
            PageNum = 1;

        PrevCheckState = newCheckState;
    }

    private string GenerateCheckState() =>
        $"{(int) FilterBy},{FilterValue},{PageSize},{NumPages}";
}
