using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AD.Server.Data;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs.Store;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class StoreServices : IStoreServices
    {
        private readonly ApplicationDbContext _context;

        private readonly IRepository<Store> _entityRepository;
        private readonly IRepository<StoreUsers> _storeUserRepository;

        private readonly IUserCServices _userCServices;
        private readonly IProductServices _productServices;
        private readonly IFileStorageService _fileService;
        private const string path = "stores";
        public StoreServices(IRepository<Store> entityRepository, IFileStorageService fileService, ApplicationDbContext context, IUserCServices userCServices, IRepository<StoreUsers> storeUserRepository, IProductServices productServices)
        {
            _entityRepository = entityRepository;
            _fileService = fileService;
            _context = context;
            _userCServices = userCServices;
            _storeUserRepository = storeUserRepository;
            _productServices = productServices;
        }

        public async Task<ApiResponse> Get()
        {
            try
            {
                var query = _entityRepository.TableNoTracking
                    .Where(x => x.Status == Status.Online)
                    .Include(x => x.Currency)
                    .Include(x => x.City).ThenInclude(x => x.Country)
                    .Include(x => x.StoreTags).ThenInclude(x => x.Tag)
                    .Include(x => x.StoreCountries).ThenInclude(x => x.Country)
                    .Include(x => x.StoreCities).ThenInclude(x => x.City)
                    .Include(x => x.StoreUsers).ThenInclude(x => x.User)
                    .Include(x => x.StorePaymentMethods).ThenInclude(x => x.PaymentMethod);

                var entities = await query.OrderByDescending(x => x.Id).ToListAsync();
                return ApiResponse.Create(HttpStatusCode.OK, entities);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        //public async Task<ApiResponse> GetMyStoresAll(string userName)
        //{
        //    try
        //    {
        //        var user = await _userCServices.GetCurrent(userName);
        //        var userId = user != null ? user.Id : 0;
                
        //        var entities = await _storeUserRepository.TableNoTracking
        //            .Where(x => x.UserCId == userId && x.Store.Status == Status.Online)
        //            .Include(x=>x.Store)
        //            .OrderByDescending(x => x.Id).ToListAsync();
        //        List<Store> stores = new List<Store>();
        //        foreach (var item in entities)
        //        {
        //            Store store = item.Store;
        //            stores.Add(store);
        //        }
        //        return ApiResponse.Create(HttpStatusCode.OK, stores);
        //    }
        //    catch (Exception)
        //    {
        //        return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
        //    }
        //}

        public async Task<ApiResponse> Get(long id)
        {
            try
            {
                var entity = await _entityRepository.TableNoTracking
                    .Include(x => x.UserC)
                    .Include(x => x.Currency)
                    .Include(x => x.City).ThenInclude(x => x.Country)
                    .Include(x => x.StoreTags).ThenInclude(x => x.Tag)
                    .Include(x => x.StoreCountries).ThenInclude(x => x.Country)
                    .Include(x => x.StoreCities).ThenInclude(x => x.City)
                    .Include(x => x.StoreUsers).ThenInclude(x => x.User)
                    .Include(x => x.StorePaymentMethods).ThenInclude(x => x.PaymentMethod)
                    .SingleOrDefaultAsync(x => x.Id == id);
                return ApiResponse.Create(HttpStatusCode.OK, entity);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Post(AddStoreVMRequest entity, string userName)
        {
            try
            {
                var user = await _userCServices.GetCurrent(userName);
                long max = 0;
                var query = await _entityRepository.TableNoTracking.FirstOrDefaultAsync();
                if (query != null)
                    max = await _entityRepository.TableNoTracking.MaxAsync(x => x.Id);
                Store store = new()
                {
                    Name = entity.Name,
                    Address = entity.Address,
                    CityId = entity.CityId,
                    CurrencyId = entity.CurrencyId,
                    Image = entity.Image,
                    Code = $"Store_{max + 1}",
                    userCId = user.Id
                };
                if (!string.IsNullOrWhiteSpace(entity.Image))
                {
                    var entityImage = Convert.FromBase64String(entity.Image);
                    store.Image = await _fileService.SaveFile(entityImage, "jpg", path, store.Code);
                }
                var newEntity = await _entityRepository.InsertAsync(store);
                var storeId = newEntity.Id;
                foreach (var item in entity.Tags)
                {
                    StoreTags x = new()
                    {
                        StoreId = storeId,
                        TagId = item
                    };
                    await _context.StoreTags.AddAsync(x);
                }
                foreach (var item in entity.Countries)
                {
                    StoreCountries x = new()
                    {
                        StoreId = storeId,
                        CountryId = item
                    };
                    await _context.StoreCountries.AddAsync(x);
                }
                foreach (var item in entity.Cities)
                {
                    StoreCities x = new()
                    {
                        StoreId = storeId,
                        CityId = item
                    };
                    await _context.StoreCities.AddAsync(x);
                }
                foreach (var item in entity.Users)
                {
                    StoreUsers x = new()
                    {
                        StoreId = storeId,
                        UserCId = item
                    };
                    await _context.StoreUsers.AddAsync(x);
                }
                foreach (var item in entity.PaymentMethods)
                {
                    StorePaymentMethods x = new()
                    {
                        StoreId = storeId,
                        PaymentMethodId = item.PaymentMethodId,
                        Details = item.Details
                    };
                    await _context.StorePaymentMethods.AddAsync(x);
                }
                await _context.SaveChangesAsync();
                StoreUsers storeUser = new();
                storeUser.StoreId = newEntity.Id;
                storeUser.UserCId = user.Id;
                storeUser.Role = "[0,1,2,3]";
                storeUser.Status = StoreUserStatus.Accepted;
                _storeUserRepository.Insert(storeUser);
                return ApiResponse.Create(HttpStatusCode.OK, storeId);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Put(EditStoreVMRequest entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);

                if (!string.IsNullOrWhiteSpace(entity.Image))
                {
                    if (entityDB.Image != null)
                        await _fileService.DeleteFile(entityDB.Image, path);

                    var entityImage = Convert.FromBase64String(entity.Image);
                    entityDB.Image = await _fileService.EditFile(entityImage, "jpg", path, entityDB.Image, entityDB.Code);
                }
                entityDB.Name = entity.Name;
                entityDB.Address = entity.Address;
                entityDB.CityId = entity.CityId;
                entityDB.CurrencyId = entity.CurrencyId;
                entityDB.Status = entity.Status;

                //for tabeles linked to this table we delete then Add again
                await _context.Database.ExecuteSqlInterpolatedAsync(
                $"delete from StoreTags where StoreId = {entity.Id}; delete from StoreCountries where StoreId = {entity.Id}; delete from StoreCities where StoreId = {entity.Id}; delete from StorePaymentMethods where StoreId = {entity.Id};");

                foreach (var item in entity.Tags)
                {
                    StoreTags x = new()
                    {
                        StoreId = entity.Id,
                        TagId = item
                    };
                    await _context.StoreTags.AddAsync(x);
                }
                foreach (var item in entity.Countries)
                {
                    StoreCountries x = new()
                    {
                        StoreId = entity.Id,
                        CountryId = item
                    };
                    await _context.StoreCountries.AddAsync(x);
                }
                foreach (var item in entity.Cities)
                {
                    StoreCities x = new()
                    {
                        StoreId = entity.Id,
                        CityId = item
                    };
                    await _context.StoreCities.AddAsync(x);
                }
                foreach (var item in entity.PaymentMethods)
                {
                    StorePaymentMethods x = new()
                    {
                        StoreId = entity.Id,
                        PaymentMethodId = item.PaymentMethodId,
                        Details = item.Details
                    };
                    await _context.StorePaymentMethods.AddAsync(x);
                }

                await _context.SaveChangesAsync();
                await _entityRepository.UpdateAsync(entityDB);

                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Freeze_UnFreeze(FreezeVMRequest entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);
                entityDB.Status = entity.Status;
                await _entityRepository.UpdateAsync(entityDB);
                var products =await _productServices.GetProductsByStoreIdALL(entity.Id);
                foreach (var item in products)
                {
                    FreezeVMRequest vm = new()
                    {
                        Id = item.Id,
                        Status = entity.Status
                    };
                    await _productServices.Freeze_UnFreeze(vm);
                }
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

                if (!string.IsNullOrWhiteSpace(entity.Image))
                    await _fileService.DeleteFile(entity.Image, path);

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