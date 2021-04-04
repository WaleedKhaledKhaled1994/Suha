using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<PaginatedResponse<List<Order>>> GetUserOrders(IndexOrderVMRequest paginationDTO);
        Task<PaginatedResponse<List<Order>>> GetStoreOrders(IndexStoreOrderVMRequest paginationDTO);
        Task<Order> Get(long id);
        Task<long> Create(AddOrderVMRequest entity);
        Task<List<long>> CreateAll(List<AddOrderVMRequest> entities);
        Task Edit(EditOrderVMRequest entity);
        Task EditRate(OrderRateVMRequest entity);
        Task<Order> GetOrderDetailsForUser(long id);
        Task<Order> GetOrderDetailsForStore(long id);
    }
}
