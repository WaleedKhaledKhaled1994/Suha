using AD.Shared.DTOs;
using AD.Shared.DTOs.Employee;
using AD.Shared.DTOs.Store;
using AD.Shared.Entities;
using AD.Shared.Entities.MTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IStoreUserRepository
    {
        Task<List<StoreUsers>> Get();
        Task<PaginatedResponse<List<EmployeeDTO>>> GetStoreEmployeesByStore(PaginationSearchDTO paginationDTO, long storeId);
        Task<StoreUsers> GetStoreUserByStore(long storeId);
        Task<EmployeeDTO> GetStoreUser(long storeUserId);
        Task<long> Add(EmployeeDTO entity);
        Task Edit(EmployeeDTO entity);
        Task Delete(long id);
        Task Accept_Reject(StoreUsers entity);
        Task<List<UserC>> GetUsersForAddEmployees(long storeId);
        Task<List<EmployeeDTO>> GetCurrentStoreUsersByStore(long storeId);
    }
}
