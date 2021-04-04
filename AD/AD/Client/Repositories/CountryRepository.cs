using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.ViewModels.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/countries";
        public CountryRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<List<Country>> Get()
        {
            var response = await _httpService.Get2<List<Country>>($"{url}/all");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task<PaginatedResponse<List<Country>>> Get(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<Country>>(url, paginationSearchDTO);
        }
        public async Task<Country> Get(long id)
        {
            var response = await _httpService.Get2<Country>($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task<long> Create(AddCountryVMRequest entity)
        {
            var response = await _httpService.Post2<AddCountryVMRequest, long>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task Edit(EditCountryVMRequest entity)
        {
            var response = await _httpService.Put<EditCountryVMRequest>(url, entity);
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
