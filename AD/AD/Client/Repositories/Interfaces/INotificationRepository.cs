using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> Get();
        Task<PaginatedResponse<List<Notification>>> Get(PaginationSearchDTO paginationSearchDTO);
        Task<Notification> Get(long id);
        Task<long> GetNewCount();
        Task<List<long>> Create(AddNotificationVMRequest entity);
        Task Edit(List<long> listNotificationToUpdateRead);
        Task Delete(long id);
    }
}
