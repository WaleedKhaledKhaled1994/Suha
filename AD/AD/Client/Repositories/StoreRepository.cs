using AD.Client.Helpers;
using AD.Client.Pages.Stores;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Store;
using AD.Shared.Entities;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/stores";
        public StoreRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Store>> Get()
        {
            var response = await _httpService.Get2<List<Store>>($"{url}/all");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }

        public async Task<PaginatedResponse<List<Store>>> GetAllStoresAdmin(PaginationSearchDTO paginationDTO)
        {
            return await _httpService.GetHelper2<List<Store>>($"{url}/GetAllStoresAdmin", paginationDTO);
        }

        public async Task<PaginatedResponse<List<Store>>> GetAllFreezedStoresAdmin(PaginationSearchDTO paginationDTO)
        {
            return await _httpService.GetHelper2<List<Store>>($"{url}/GetAllFreezedStoresAdmin", paginationDTO);
        }

        public async Task<PaginatedResponse<List<IndexStoresUser>>> GetAllStoresUser(PaginationSearchDTO paginationDTO)
        {
            return await _httpService.GetHelper2<List<IndexStoresUser>>($"{url}/GetAllStoresUser", paginationDTO);
        }
      
        public async Task<PaginatedResponse<List<AD.Shared.DTOs.Store.IndexMyStores>>> GetMyStores(PaginationSearchDTO paginationDTO)
        {
            return await _httpService.GetHelper2<List<AD.Shared.DTOs.Store.IndexMyStores>>($"{url}/GetMyStores", paginationDTO);
        }

        //public async Task<List<Store>> GetMyStoresAll()
        //{
        //    var response = await _httpService.Get2<List<Store>>($"{url}/GetMyStoresAll");
        //    if (!response.Success)
        //        throw new ApplicationException(await response.GetBody());
        //    return response.Response;
        //}

        public async Task<PaginatedResponse<List<IndexRequestAdminStores>>> GetRequestAdmin(PaginationSearchDTO paginationDTO)
        {
            return await _httpService.GetHelper2<List<IndexRequestAdminStores>>($"{url}/GetRequestAdmin", paginationDTO);
        }

        public async Task<Store> Get(long id)
        {
            var response = await _httpService.Get2<Store>($"{url}/{id}");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }

        public async Task<long> Create(AddStoreVMRequest entity)
        {
            var response = await _httpService.Post2<AddStoreVMRequest, long>(url, entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }

        public async Task Edit(EditStoreVMRequest entity)
        {
            var response = await _httpService.Put<EditStoreVMRequest>(url, entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
        }

        public async Task Freeze_UnFreeze(FreezeVMRequest entity)
        {
            var response = await _httpService.Put<FreezeVMRequest>($"{url}/Freeze_UnFreeze", entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
        }

        public async Task Delete(long id)
        {
            var response = await _httpService.Delete($"{url}/{id}");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
        }
    }
}
