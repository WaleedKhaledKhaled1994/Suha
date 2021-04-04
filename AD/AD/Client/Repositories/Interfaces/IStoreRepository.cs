using AD.Shared.DTOs;
using AD.Shared.DTOs.Store;
using AD.Shared.Entities;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IStoreRepository
    {
        Task<List<Store>> Get();
        Task<PaginatedResponse<List<Store>>> GetAllStoresAdmin(PaginationSearchDTO paginationDTO);
        Task<PaginatedResponse<List<IndexStoresUser>>> GetAllStoresUser(PaginationSearchDTO paginationDTO);
        Task<PaginatedResponse<List<IndexMyStores>>> GetMyStores(PaginationSearchDTO paginationDTO);
        //Task<List<Store>> GetMyStoresAll();
        Task<PaginatedResponse<List<IndexRequestAdminStores>>> GetRequestAdmin(PaginationSearchDTO paginationDTO);
        Task<Store> Get(long id);
        Task<long> Create(AddStoreVMRequest entity);
        Task Edit(EditStoreVMRequest entity);
        Task Delete(long id);
        Task Freeze_UnFreeze(FreezeVMRequest entity);
        Task<PaginatedResponse<List<Store>>> GetAllFreezedStoresAdmin(PaginationSearchDTO paginationDTO);
    }
}
