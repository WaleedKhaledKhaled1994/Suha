using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface INotificationServices
    {
        Task<ApiResponse> Get(string userName);
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> GetNewCount(string userName);
        Task<ApiResponse> Post(AddNotificationVMRequest input);
        Task<ApiResponse> Put(List<long> listNotificationToUpdateRead);
        Task<ApiResponse> Delete(long id);
    }
}
