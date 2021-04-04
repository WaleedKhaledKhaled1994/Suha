using Microsoft.EntityFrameworkCore;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class NotificationServices : INotificationServices
    {
        private readonly IRepository<Notification> _entityRepository;
        private readonly IUserCServices _userCServices;

        public NotificationServices(IRepository<Notification> entityRepository, IUserCServices userCServices)
        {
            _entityRepository = entityRepository;
            _userCServices = userCServices;
        }
        public async Task<ApiResponse> Get(string userName)
        {
            try
            {
                long userId = (await _userCServices.GetCurrent(userName)).Id;
                var query = _entityRepository.TableNoTracking.Where(x => x.UserCId == userId);
                var entities = await query.OrderByDescending(x => x.NotificationTime).ToListAsync();
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
        public async Task<ApiResponse> GetNewCount(string userName)
        {
            try
            {
                long userId = (await _userCServices.GetCurrent(userName)).Id;
                long number = await _entityRepository.CountAsync(x => x.UserCId == userId && x.IsRead == false);
                return ApiResponse.Create(HttpStatusCode.OK, number);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> Post(AddNotificationVMRequest entity)
        {
            try
            {
                List<long> usersToSendNotifications = new();
                foreach (var item in entity.UserCIds)
                {
                    Notification notification = new()
                    {
                        Title = entity.Title,
                        Body = entity.Body,
                        Type = entity.Type,
                        UserCId = item,
                        Link = entity.Link
                    };

                    var newEntity = await _entityRepository.InsertAsync(notification);
                    usersToSendNotifications.Add(newEntity.Id);
                }
                return ApiResponse.Create(HttpStatusCode.OK, usersToSendNotifications);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
        public async Task<ApiResponse> Put(List<long> listNotificationToUpdateRead)
        {
            try
            {
                foreach (var item in listNotificationToUpdateRead)
                {
                    var entityDB = await _entityRepository.GetByIdAsync(item);
                    entityDB.IsRead = true;
                    await _entityRepository.UpdateAsync(entityDB);
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