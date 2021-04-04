using AD.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Helpers
{
    public static class IHttpServiceExtentionMethods
    {
        public static async Task<T> GetHelper<T>(this IHttpService httpService, string url)
        {
            var response = await httpService.Get<T>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public static async Task<PaginatedResponse<T>> GetHelper<T>(this IHttpService httpService, string url,
            PaginationDTO paginationDTO)
        {
            string newURL = "";
            if (url.Contains("?"))
            {
                newURL = $"{url}&page={paginationDTO.Page}&recordsPerPage={paginationDTO.RecordsPerPage}";
            }
            else
            {
                newURL = $"{url}?page={paginationDTO.Page}&recordsPerPage={paginationDTO.RecordsPerPage}";
            }

            var httpResponse = await httpService.Get<T>(newURL);
            var totalAmountPages = int.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmountPages").FirstOrDefault());
            var totalAmount = long.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmount").FirstOrDefault());
            var paginatedResponse = new PaginatedResponse<T>
            {
                Response = httpResponse.Response,
                TotalAmountPages = totalAmountPages,
                TotalAmount = totalAmount
            };
            return paginatedResponse;
        }

        //Old
        public static async Task<PaginatedResponse<T>> GetHelper<T>(this IHttpService httpService, string url,
            PaginationSearchDTO paginationSearchDTO)
        {
            string newURL = "";
            if (url.Contains("?"))
                newURL = $"{url}&page={paginationSearchDTO.Page}&recordsPerPage={paginationSearchDTO.RecordsPerPage}&searchText={paginationSearchDTO.SearchText}";
            else
                newURL = $"{url}?page={paginationSearchDTO.Page}&recordsPerPage={paginationSearchDTO.RecordsPerPage}&searchText={paginationSearchDTO.SearchText}";

            var httpResponse = await httpService.Get<T>(newURL);
            var totalAmountPages = int.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmountPages").FirstOrDefault());
            var totalAmount = long.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmount").FirstOrDefault());
            var paginatedResponse = new PaginatedResponse<T>
            {
                Response = httpResponse.Response,
                TotalAmountPages = totalAmountPages,
                TotalAmount = totalAmount
            };
            return paginatedResponse;
        }

        //New
        public static async Task<PaginatedResponse<T>> GetHelper2<T>(this IHttpService httpService, string url,
    PaginationSearchDTO paginationSearchDTO)
        {
            string newURL = "";
            if (url.Contains("?"))
                newURL = $"{url}&page={paginationSearchDTO.Page}&recordsPerPage={paginationSearchDTO.RecordsPerPage}&searchText={paginationSearchDTO.SearchText}";
            else
                newURL = $"{url}?page={paginationSearchDTO.Page}&recordsPerPage={paginationSearchDTO.RecordsPerPage}&searchText={paginationSearchDTO.SearchText}";

            var httpResponse = await httpService.GetPagination<T>(newURL);
            var totalAmountPages = int.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmountPages").FirstOrDefault());
            var totalAmount = long.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmount").FirstOrDefault());
            var paginatedResponse = new PaginatedResponse<T>
            {
                Response = httpResponse.Response,
                TotalAmountPages = totalAmountPages,
                TotalAmount = totalAmount
            };
            return paginatedResponse;
        }
    }
}
