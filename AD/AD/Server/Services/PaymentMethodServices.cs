using Microsoft.EntityFrameworkCore;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.MTM;
using AD.Shared.ViewModels.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class PaymentMethodServices : IPaymentMethodServices
    {
        private readonly IRepository<PaymentMethod> _entityRepository;
        private readonly IRepository<StorePaymentMethods> _storePaymentMethodsRepository;
        public PaymentMethodServices(IRepository<PaymentMethod> entityRepository, IRepository<StorePaymentMethods> storePaymentMethodsRepository)
        {
            _entityRepository = entityRepository;
            _storePaymentMethodsRepository = storePaymentMethodsRepository;
        }
        public async Task<ApiResponse> Get()
        {
            try
            {
                var query = _entityRepository.TableNoTracking;
                var entities = await query.OrderByDescending(x => x.Id).ToListAsync();
                return ApiResponse.Create(HttpStatusCode.OK, entities);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> Get(long id)
        {
            try
            {
                var entity = await _entityRepository.GetByIdAsync(id);
                return ApiResponse.Create(HttpStatusCode.OK, entity);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> GetPaymentMethodsByStoreId(long storeId)
        {
            try
            {
                var entities = await _storePaymentMethodsRepository.TableNoTracking
                    .Where(x => x.StoreId == storeId)
                    .Include(x => x.PaymentMethod)
                    .ToListAsync();
                return ApiResponse.Create(HttpStatusCode.OK, entities);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> Post(AddPaymentMethodVMRequest entity)
        {
            try
            {
                long max = 0;
                var query = await _entityRepository.TableNoTracking.FirstOrDefaultAsync();
                if (query != null)
                    max = await _entityRepository.TableNoTracking.MaxAsync(x => x.Id);
                PaymentMethod paymentMethod = new()
                {
                    Name = entity.Name,
                    Code = $"PaymentMethod_{max + 1}"
                };
                var newEntity = await _entityRepository.InsertAsync(paymentMethod);
                return ApiResponse.Create(HttpStatusCode.OK, newEntity.Id);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> Put(EditPaymentMethodVMRequest entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);
                entityDB.Name = entity.Name;
                await _entityRepository.SaveChangesAsync();
                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> Delete(long id)
        {
            try
            {
                var entity = await _entityRepository.GetByIdAsync(id);
                await _entityRepository.DeleteAsync(entity);

                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
    }
}
