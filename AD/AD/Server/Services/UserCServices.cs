using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AD.Server.Models;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Auth;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using AD.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class UserCServices : IUserCServices
    {
        private readonly IRepository<UserC> _entityRepository;
        private readonly IRepository<StoreUsers> _storeUsersRepository;
        //Not Used
        private readonly IRepository<ApplicationUser> _userRepository;

        public UserCServices(IRepository<UserC> entityRepository, IRepository<ApplicationUser> userRepository, IRepository<StoreUsers> storeUsersRepository)
        {
            _entityRepository = entityRepository;
            _userRepository = userRepository;
            _storeUsersRepository = storeUsersRepository;
        }

        public async Task<ApiResponse> GetUserCBySearch(string searchText)
        {
            try
            {
                var result = await _entityRepository.TableNoTracking
                    .Where(x => x.FirstName.ToLower().StartsWith(searchText.ToLower()) ||
                    x.LastName.ToLower().StartsWith(searchText.ToLower()) ||
                    x.UserName.ToLower().StartsWith(searchText.ToLower())).ToListAsync();
                return ApiResponse.Create(HttpStatusCode.OK, result);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> GetCurrentAPI(string userName)
        {
            try
            {
                var result = await _entityRepository.TableNoTracking.SingleOrDefaultAsync(x => x.UserName == userName);
                return ApiResponse.Create(HttpStatusCode.OK, result);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<UserC> GetCurrent(string userName)
        {
            var result = await _entityRepository.TableNoTracking.SingleOrDefaultAsync(x => x.UserName == userName);
            return result;
        }

        public async Task<ApiResponse> Get()
        {
            try
            {
                var query = _entityRepository.TableNoTracking.Where(x => x.Status == Status.Online).Include(n => n.City).ThenInclude(n => n.Country);
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

        public async Task<ApiResponse> Post(UserC entity)
        {
            try
            {
                long max = 0;
                var query = await _entityRepository.TableNoTracking.FirstOrDefaultAsync();
                if (query != null)
                    max = await _entityRepository.TableNoTracking.MaxAsync(x => x.Id);
                entity.Code = $"UserC_{max + 1}";
                var newEntity = await _entityRepository.InsertAsync(entity);
                return ApiResponse.Create(HttpStatusCode.OK, newEntity.Id);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> Put(UserC entity)
        {
            try
            {
                var entityDB = await _entityRepository.GetByIdAsync(entity.Id);
                entityDB.PhoneNumber = entity.PhoneNumber;
                entityDB.UserName = entity.UserName;
                entityDB.FirstName = entity.FirstName;
                entityDB.LastName = entity.LastName;
                entityDB.Email = entity.Email;
                entityDB.BirthYear = entity.BirthYear;
                entityDB.CityId = entity.CityId;
                entityDB.Status = entity.Status;
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
