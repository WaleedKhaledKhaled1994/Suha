using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.Entities.MTM;
using AD.Shared.ViewModels.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/paymentMethods";
        public PaymentMethodRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<PaymentMethod>> Get()
        {
            var response = await _httpService.Get2<List<PaymentMethod>>($"{url}/all");

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<PaginatedResponse<List<PaymentMethod>>> Get(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<PaymentMethod>>(url, paginationSearchDTO);
        }

        public async Task<PaymentMethod> Get(long id)
        {
            var response = await _httpService.Get2<PaymentMethod>($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<List<StorePaymentMethods>> GetPaymentMethodsByStoreId(long storeId)
        {
            var response = await _httpService.Get2<List<StorePaymentMethods>>($"{url}/GetPaymentMethodsByStoreId/{storeId}");

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<long> Create(AddPaymentMethodVMRequest entity)
        {
            var response = await _httpService.Post2<AddPaymentMethodVMRequest, long>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task Edit(EditPaymentMethodVMRequest entity)
        {
            var response = await _httpService.Put<EditPaymentMethodVMRequest>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task Delete(long id)
        {
            var response = await _httpService.Delete($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}