using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AD.Server.Models;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs.Employee;
using AD.Shared.DTOs.Store;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class StoreUserServices : IStoreUserServices
    {
        private readonly IRepository<StoreUsers> _entityRepository;
        private readonly IRepository<UserC> _userCRepository;

        private readonly IUserCServices _userCServices;

        public StoreUserServices(IRepository<StoreUsers> entityRepository, IUserCServices userCServices, IRepository<UserC> userCRepository)
        {
            _entityRepository = entityRepository;
            _userCServices = userCServices;
            _userCRepository = userCRepository;
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

        public async Task<ApiResponse> GetUsersForAddEmployees(long storeId)
        {
            try
            {
                var users = await _userCRepository.TableNoTracking.ToListAsync();
                var storeUsersIds = await _entityRepository.TableNoTracking.Where(x => x.StoreId == storeId)
                    .Select(x => x.UserCId).ToListAsync();

                foreach (var item in storeUsersIds)
                {
                    var user = users.Where(x => x.Id == item).SingleOrDefault();
                    users.Remove(user);
                }

                return ApiResponse.Create(HttpStatusCode.OK, users);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> GetStoreUserByStore(long storeId, string userName)
        {
            try
            {
                var userId = (await _userCServices.GetCurrent(userName)).Id;
                var storeUser = await _entityRepository.TableNoTracking.Where(x => x.UserCId == userId && x.StoreId == storeId).SingleOrDefaultAsync();
                return ApiResponse.Create(HttpStatusCode.OK, storeUser);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> GetCurrentStoreUsersByStore(long storeId)
        {
            try
            {
                List<EmployeeDTO> employeesDTO = new();
                var entities = await _entityRepository.TableNoTracking
                    .Where(x => x.StoreId == storeId && x.Status == StoreUserStatus.Accepted).ToListAsync();
                foreach (var item in entities)
                {
                    EmployeeDTO employee = new();

                    List<int> roleDeserialize = JsonConvert.DeserializeObject<List<int>>(item.Role);

                    if (roleDeserialize.Contains(0))
                        employee.Admin = true;
                    if (roleDeserialize.Contains(1))
                        employee.ManageProducts = true;
                    if (roleDeserialize.Contains(2))
                        employee.ManageOrders = true;
                    if (roleDeserialize.Contains(3))
                        employee.ManageComments = true;

                    employee.StoreUser = item;

                    employeesDTO.Add(employee);
                }
                return ApiResponse.Create(HttpStatusCode.OK, employeesDTO);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> GetStoreUser(long storeUserId)
        {
            try
            {
                var entity = await _entityRepository.TableNoTracking
                    .Where(x => x.Id == storeUserId)
                    .Include(x => x.User).ThenInclude(x => x.City).ThenInclude(x => x.Country)
                    .SingleOrDefaultAsync();
                EmployeeDTO employee = new();
                employee.StoreUser = entity;

                List<int> roleDeserialize = JsonConvert.DeserializeObject<List<int>>(entity.Role);

                if (roleDeserialize.Contains(0))
                    employee.Admin = true;
                if (roleDeserialize.Contains(1))
                    employee.ManageProducts = true;
                if (roleDeserialize.Contains(2))
                    employee.ManageOrders = true;
                if (roleDeserialize.Contains(3))
                    employee.ManageComments = true;

                return ApiResponse.Create(HttpStatusCode.OK, employee);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Post(EmployeeDTO entity)
        {
            try
            {
                List<int> roles = new();
                if (entity.Admin == true)
                    roles.Add(0);
                if (entity.ManageProducts == true)
                    roles.Add(1);
                if (entity.ManageOrders == true)
                    roles.Add(2);
                if (entity.ManageComments == true)
                    roles.Add(3);
                entity.StoreUser.Role = JsonConvert.SerializeObject(roles);
                var newEntity = await _entityRepository.InsertAsync(entity.StoreUser);

                return ApiResponse.Create(HttpStatusCode.OK, newEntity.Id);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Put(EmployeeDTO entity)
        {
            try
            {
                List<int> roles = new();

                var entityDB = await _entityRepository.GetByIdAsync(entity.StoreUser.Id);
                if (entity.Admin == true)
                    roles.Add(0);
                if (entity.ManageProducts == true)
                    roles.Add(1);
                if (entity.ManageOrders == true)
                    roles.Add(2);
                if (entity.ManageComments == true)
                    roles.Add(3);
                entityDB.Role = JsonConvert.SerializeObject(roles);
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

        public async Task<ApiResponse> Accept_Reject(StoreUsers entity)
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
    }
}
