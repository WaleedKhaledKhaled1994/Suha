using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.MTM;
using AD.Shared.ViewModels.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IPaymentMethodServices 
    {
        Task<ApiResponse> Get();
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> GetPaymentMethodsByStoreId(long storeId);
        Task<ApiResponse> Post(AddPaymentMethodVMRequest input);
        Task<ApiResponse> Put(EditPaymentMethodVMRequest input);
        Task<ApiResponse> Delete(long id);
    }
}
