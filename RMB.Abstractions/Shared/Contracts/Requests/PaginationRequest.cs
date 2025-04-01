namespace RMB.Abstractions.Shared.Contracts.Requests
{
    public class PaginationRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
