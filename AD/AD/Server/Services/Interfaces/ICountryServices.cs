using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface ICountryServices
    {
        Task<ApiResponse> Get();
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> Post(AddCountryVMRequest input);
        Task<ApiResponse> Put(EditCountryVMRequest input);
        Task<ApiResponse> Delete(long id);
    }
}
