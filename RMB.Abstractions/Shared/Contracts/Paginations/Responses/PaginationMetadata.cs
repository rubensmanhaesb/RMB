namespace RMB.Abstractions.Shared.Contracts.Paginations.Responses
{
    public class PaginationMetadata
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

}
