using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using AD.Shared.ViewModels.Order;
using Microsoft.EntityFrameworkCore;
using AD.Shared.ViewModels.Notification;

namespace AD.Server.Services
{
    public class OrderServices : IOrderSrevices
    {
        private readonly IRepository<Order> _entityRepository;
        private readonly IRepository<Store> _storeRepository;
        private readonly IUserCServices _userCServices;
        private readonly INotificationServices _notificationSrevices;

        public OrderServices(IRepository<Order> entityRepository, IUserCServices userCServices, INotificationServices notificationSrevices, IRepository<Store> storeRepository)
        {
            _entityRepository = entityRepository;
            _userCServices = userCServices;
            _notificationSrevices = notificationSrevices;
            _storeRepository = storeRepository;
        }

        public async Task<ApiResponse> Get(long id)
        {
            try
            {
                var entity = await _entityRepository.TableNoTracking.Where(x => x.Id == id)
                    .Include(x => x.Product).ThenInclude(x => x.Store).SingleOrDefaultAsync();
                return ApiResponse.Create(HttpStatusCode.OK, entity);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> GetOrderDetailsForUser(long id)
        {
            try
            {
                var entity = await _entityRepository.TableNoTracking
                .Where(x => x.Id == id)
                .Include(x => x.Product).ThenInclude(x => x.Store)
                .Include(x => x.StorePaymentMethod).ThenInclude(x => x.PaymentMethod)
                .SingleOrDefaultAsync();
                return ApiResponse.Create(HttpStatusCode.OK, entity);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> GetOrderDetailsForStore(long id)
        {
            try
            {
                var entity = await _entityRepository.TableNoTracking
                .Where(x => x.Id == id)
                .Include(x => x.Product)
                .Include(x => x.UserC).ThenInclude(x => x.City).ThenInclude(x => x.Country)
                .Include(x => x.StorePaymentMethod).ThenInclude(x => x.PaymentMethod)
                .SingleOrDefaultAsync();
                return ApiResponse.Create(HttpStatusCode.OK, entity);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Post(AddOrderVMRequest entity, string userName)
        {
            try
            {
                Order order = new()
                {
                    UserCId = (await _userCServices.GetCurrent(userName)).Id,
                    ProductId = entity.ProductId,
                    OrderDate = DateTime.UtcNow,
                    Quantity = entity.Quantity,
                    Total = entity.Total,
                    StorePaymentMethodId = entity.StorePaymentMethodId
                };
                var newEntity = await _entityRepository.InsertAsync(order);

                return ApiResponse.Create(HttpStatusCode.OK, newEntity.Id);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> PostAll(List<AddOrderVMRequest> entities, string userName)
        {
            try
            {
                List<long> response = new();
                foreach (var entity in entities)
                {
                    Order order = new()
                    {
                        UserCId = (await _userCServices.GetCurrent(userName)).Id,
                        ProductId = entity.ProductId,
                        OrderDate = DateTime.UtcNow,
                        Quantity = entity.Quantity,
                        Total = entity.Total,
                        StorePaymentMethodId = entity.StorePaymentMethodId
                    };
                    response.Add((await _entityRepository.InsertAsync(order)).Id);
                }
                return ApiResponse.Create(HttpStatusCode.OK, response);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> Put(EditOrderVMRequest entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);
                if (entity.ShippingDate != 0)
                    entityDB.ShippingDate = DateTime.UtcNow.AddDays(entity.ShippingDate);

                entityDB.Status = entity.Status;

                await _notificationSrevices.Post(entity.AddNotificationVM);

                await _entityRepository.UpdateAsync(entityDB);

                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> PutRate(OrderRateVMRequest entity, string userName)
        {
            try
            {
                var userId = (await _userCServices.GetCurrent(userName)).Id;
                var entityDB = await _entityRepository.Table
                    .Where(x => x.UserCId == userId && x.Id == entity.OrderId)
                    .Include(x => x.Product).SingleOrDefaultAsync();
                if (entityDB != null)
                {
                    entityDB.Rate = entity.Rate;
                    await _entityRepository.UpdateAsync(entityDB);

                    var store = await _storeRepository.GetByIdAsync(entityDB.Product.StoreId);
                    store.AverageRate = await CalculateAverage(store.Id);
                }
                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        #region Helper
        private async Task<double> CalculateAverage(long storeId)
        {
            var list = await _entityRepository.TableNoTracking
                .Include(x => x.Product).ThenInclude(x => x.Store).Where(x => x.Product.StoreId == storeId).ToListAsync();
            int count = 0;
            int sum = 0;
            foreach (var item in list)
            {
                if (item.Rate != 0)
                {
                    count++;
                    sum += item.Rate;
                }
            }
            double avg = count != 0 ? sum / count : 0;

            return avg;
        }
        #endregion
    }
}
