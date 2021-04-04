using AD.Shared.DTOs.Store;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IStoreServices
    {
        Task<ApiResponse> Get();
        //Task<ApiResponse> GetMyStoresAll(string userName);
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> Post(AddStoreVMRequest input, string userName);
        Task<ApiResponse> Put(EditStoreVMRequest input);
        Task<ApiResponse> Delete(long id);
        Task<ApiResponse> Freeze_UnFreeze(FreezeVMRequest entity);
    }
}
