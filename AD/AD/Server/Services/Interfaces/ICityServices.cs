using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface ICityServices
    {
        Task<ApiResponse> Get();
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> Post(AddCityVMRequest input);
        Task<ApiResponse> Put(EditCityVMRequest input);
        Task<ApiResponse> Delete(long id);
    }
}
