using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface ITagServices
    {
        Task<ApiResponse> Get();
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> Post(AddTagVMRequest input);
        Task<ApiResponse> Put(EditTagVMRequest input);
        Task<ApiResponse> Delete(long id);
    }
}
