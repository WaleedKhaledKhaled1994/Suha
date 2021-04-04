using AD.Shared.Entities;
using AD.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.Order;
using AD.Shared.ViewModels.Notification;

namespace AD.Server.Services.Interfaces
{
    public interface IOrderSrevices
    {
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> GetOrderDetailsForStore(long id);
        Task<ApiResponse> GetOrderDetailsForUser(long id);
        Task<ApiResponse> Post(AddOrderVMRequest input, string userName);
        Task<ApiResponse> PostAll(List<AddOrderVMRequest> input, string userName);
        Task<ApiResponse> Put(EditOrderVMRequest input);
        Task<ApiResponse> PutRate(OrderRateVMRequest entity, string userName);
    }
}
