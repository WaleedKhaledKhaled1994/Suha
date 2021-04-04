using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/notifications";
        public NotificationRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Notification>> Get()
        {
            var response = await _httpService.Get2<List<Notification>>($"{url}/all");

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<PaginatedResponse<List<Notification>>> Get(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<Notification>>(url, paginationSearchDTO);
        }

        public async Task<Notification> Get(long id)
        {
            var response = await _httpService.Get2<Notification>($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<long> GetNewCount()
        {
            var response = await _httpService.Get2<int>($"{url}/GetNewCount");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<List<long>> Create(AddNotificationVMRequest entity)
        {
            var response = await _httpService.Post2<AddNotificationVMRequest, List<long>>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task Edit(List<long> listNotificationToUpdateRead)
        {
            var response = await _httpService.Put<List<long>>(url, listNotificationToUpdateRead);
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
