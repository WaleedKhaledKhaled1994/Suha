using AD.Shared.DTOs;
using AD.Shared.DTOs.Auth;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IUserCServices
    {
        Task<ApiResponse> GetUserCBySearch(string searchText);
        Task<ApiResponse> GetCurrentAPI(string userName);
        Task<UserC> GetCurrent(string userName);
        Task<ApiResponse> Get();
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> Post(UserC input);
        Task<ApiResponse> Put(UserC input);
        Task<ApiResponse> Delete(long id);
        Task<ApiResponse> Freeze_UnFreeze(FreezeVMRequest entity);
    }
}
