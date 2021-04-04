using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IMessageServices
    {
        Task<ApiResponse> Get(long orderId, string userName);
        Task<ApiResponse> Post(AddMessageVMRequest input, string userName);
    }
}
