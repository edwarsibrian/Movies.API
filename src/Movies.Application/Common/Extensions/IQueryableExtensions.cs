using Movies.Application.DTOs;

namespace Movies.Application.Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO pagination)
        {
            return queryable
                .Skip((pagination.PageNumber - 1) * pagination.RecordsByPage)
                .Take(pagination.RecordsByPage);
        }
    }
}
