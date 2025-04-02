namespace RMB.Abstractions.Shared.Contracts.Paginations.Responses
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public PaginationMetadata Pagination { get; set; } = new();
    }

}
