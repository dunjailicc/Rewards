using Microsoft.EntityFrameworkCore;

namespace Rewards.Application.Pagination
{
    public class PaginationUtils : IPaginationUtils
    {   
        public PaginationUtils()
        {
        }

        public async Task<PaginatedResult<T>> ApplyPagination<T>(IQueryable<T> query, int? page, int? itemsPerPage)
        {
            const int defaultPage = 1;
            const int defaultItemsPerPage = 10;

            int currentPage = page ?? defaultPage;
            int pageSize = itemsPerPage ?? defaultItemsPerPage;

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = currentPage
            };
        }
    }
}
