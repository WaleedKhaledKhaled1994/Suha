using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/orders2";
        public OrderRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<PaginatedResponse<List<Order>>> GetUserOrders(IndexOrderVMRequest paginationDTO)
        {
            var responseHTTP = await _httpService.Post2<IndexOrderVMRequest, List<Order>>($"{url}/GetUserOrders", paginationDTO);
            var totalAmountPages = int.Parse(responseHTTP.HttpResponseMessage.Headers.GetValues("totalAmountPages").FirstOrDefault());
            var totalAmount = int.Parse(responseHTTP.HttpResponseMessage.Headers.GetValues("totalAmount").FirstOrDefault());
            var paginatedResponse = new PaginatedResponse<List<Order>>()
            {
                Response = responseHTTP.Response,
                TotalAmountPages = totalAmountPages,
                TotalAmount = totalAmount
            };
            return paginatedResponse;
        }

        public async Task<PaginatedResponse<List<Order>>> GetStoreOrders(IndexStoreOrderVMRequest paginationDTO)
        {
            var responseHTTP = await _httpService.Post2<IndexStoreOrderVMRequest, List<Order>>($"{url}/GetStoreOrders", paginationDTO);
            var totalAmountPages = int.Parse(responseHTTP.HttpResponseMessage.Headers.GetValues("totalAmountPages").FirstOrDefault());
            var totalAmount = int.Parse(responseHTTP.HttpResponseMessage.Headers.GetValues("totalAmount").FirstOrDefault());
            var paginatedResponse = new PaginatedResponse<List<Order>>()
            {
                Response = responseHTTP.Response,
                TotalAmountPages = totalAmountPages,
                TotalAmount = totalAmount
            };
            return paginatedResponse;
        }

        public async Task<Order> Get(long id)
        {
            var response = await _httpService.Get2<Order>($"{url}/{id}");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }

        public async Task<Order> GetOrderDetailsForUser(long id)
        {
            var response = await _httpService.Get2<Order>($"{url}/GetOrderDetailsForUser/{id}");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }
      
        public async Task<Order> GetOrderDetailsForStore(long id)
        {
            var response = await _httpService.Get2<Order>($"{url}/GetOrderDetailsForStore/{id}");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }

        public async Task<long> Create(AddOrderVMRequest entity)
        {
            var response = await _httpService.Post2<AddOrderVMRequest, long>(url, entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }
        
        public async Task<List<long>> CreateAll (List<AddOrderVMRequest> entities)
        {
            var response = await _httpService.Post2<List<AddOrderVMRequest>, List<long>>($"{url}/CreateAll", entities);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }

        public async Task Edit(EditOrderVMRequest entity)
        {
            var response = await _httpService.Put<EditOrderVMRequest>(url, entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
        }

        public async Task EditRate(OrderRateVMRequest entity)
        {
            var response = await _httpService.Put<OrderRateVMRequest>($"{url}/PutRate", entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
        }
    }
}
