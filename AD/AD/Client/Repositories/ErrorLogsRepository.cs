using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class ErrorLogsRepository : IErrorLogsRepository
    {

        private readonly IHttpService _httpService;
        private readonly string url = "api/errorLogs";
        public ErrorLogsRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<WebApiLogs>> Get()
        {
            var response = await _httpService.Get<List<WebApiLogs>>($"{url}/all");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<PaginatedResponse<List<WebApiLogs>>> Get(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<WebApiLogs>>(url, paginationSearchDTO);
        }

        public async Task<WebApiLogs> Get(long id)
        {
            var response = await _httpService.Get<WebApiLogs>($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        //public async Task<long> Create(WebApiLogs entity)
        //{
        //    var response = await _httpService.Post<WebApiLogs, long>(url, entity);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }
        //    return response.Response;
        //}

        //public async Task Edit(WebApiLogs entity)
        //{
        //    var response = await _httpService.Put<WebApiLogs>(url, entity);
        //    if (!response.Success)
        //    {
        //        throw new ApplicationException(await response.GetBody());
        //    }
        //}

        public async Task Delete(long id)
        {
            var response = await _httpService.Delete($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
