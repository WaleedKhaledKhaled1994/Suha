using AD.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page - 1) * paginationDTO.RecordsPerPage)
                .Take(paginationDTO.RecordsPerPage);
        }
        public static List<T> PaginateList<T>(this List<T> list, PaginationDTO paginationDTO)
        {
            return list
                .Skip((paginationDTO.Page - 1) * paginationDTO.RecordsPerPage)
                .Take(paginationDTO.RecordsPerPage)
                .ToList();
        }
        public static List<T> PaginateSearch<T>(this List<T> list, PaginationSearchDTO paginationSearchDTO)
        {
            return list
                .Skip((paginationSearchDTO.Page - 1) * paginationSearchDTO.RecordsPerPage)
                .Take(paginationSearchDTO.RecordsPerPage)
                .ToList();
        }
    }
}
