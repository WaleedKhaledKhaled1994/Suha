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
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/currencies";
        public CurrencyRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<List<Currency>> Get()
        {
            var response = await _httpService.Get2<List<Currency>>($"{url}/all");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task<PaginatedResponse<List<Currency>>> Get(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<Currency>>(url, paginationSearchDTO);
        }
        public async Task<Currency> Get(long id)
        {
            var response = await _httpService.Get2<Currency>($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task<long> Create(Currency entity)
        {
            var response = await _httpService.Post2<Currency, long>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task Edit(Currency entity)
        {
            var response = await _httpService.Put<Currency>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
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
