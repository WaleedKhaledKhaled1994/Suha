using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.MeasruingUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IMeasruingUnitServices
    {
        Task<ApiResponse> Get();
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> Post(AddMeasruingUnitVMRequest input);
        Task<ApiResponse> Put(EditMeasruingUnitVMRequest input);
        Task<ApiResponse> Delete(long id);
    }
}
