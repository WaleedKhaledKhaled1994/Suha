using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.Entities.MTM;
using AD.Shared.ViewModels.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IPaymentMethodRepository
    {
        Task<List<PaymentMethod>> Get();
        Task<PaginatedResponse<List<PaymentMethod>>> Get(PaginationSearchDTO paginationSearchDTO);
        Task<PaymentMethod> Get(long id);
        Task<List<StorePaymentMethods>> GetPaymentMethodsByStoreId(long storeId);
        Task<long> Create(AddPaymentMethodVMRequest entity);
        Task Edit(EditPaymentMethodVMRequest entity);
        Task Delete(long id);
    }
}
