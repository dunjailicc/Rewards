namespace Rewards.Application.Pagination
{
    public interface IPaginationUtils
    {
        public Task<PaginatedResult<T>> ApplyPagination<T>(IQueryable<T> query, int? pageNumber, int? itemsPerPage);

    }
}
