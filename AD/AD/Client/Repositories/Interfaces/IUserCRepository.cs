using AD.Shared.DTOs;
using AD.Shared.DTOs.Auth;
using AD.Shared.Entities;
using AD.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IUserCRepository
    {
        Task<List<UserC>> GetUserCBySearch(string searchText);
        Task<UserC> GetCurrent();
        Task<List<UserC>> Get();
        Task<PaginatedResponse<List<UserC>>> Get(PaginationSearchDTO paginationDTO);
        Task<UserC> Get(long id);
        Task<long> Create(UserC entity);
        Task Edit(UserC entity);
        Task Delete(long id);
        Task<PaginatedResponse<List<UserC>>> GetFreezedUsers(PaginationSearchDTO paginationSearchDTO);
        Task Freeze_UnFreeze(FreezeVMRequest entity);
    }
}
