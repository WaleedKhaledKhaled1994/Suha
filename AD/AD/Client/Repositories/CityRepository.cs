using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.ViewModels.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/cities";
        public CityRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<City>> Get()
        {
            var response = await _httpService.Get2<List<City>>($"{url}/all");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<PaginatedResponse<List<City>>> Get(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<City>>(url, paginationSearchDTO);
        }

        public async Task<City> Get(long id)
        {
            var response = await _httpService.Get2<City>($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<long> Create(AddCityVMRequest entity)
        {
            var response = await _httpService.Post2<AddCityVMRequest, long>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task Edit(EditCityVMRequest entity)
        {
            var response = await _httpService.Put<EditCityVMRequest>(url, entity);
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
