namespace HotelBookingSystem.Application.DTOs.Common;

public class PaginationMetadata
{
    public int PageNumber { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public string? PreviousPageLink { get; set; }
    public string? NextPageLink { get; set; }

    public PaginationMetadata(int pageNumber, int pageSize, int totalCount)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
