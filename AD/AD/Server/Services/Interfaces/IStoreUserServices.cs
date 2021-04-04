using AD.Shared.DTOs.Employee;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.MTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IStoreUserServices
    {
        Task<ApiResponse> Get();
        Task<ApiResponse> GetStoreUserByStore(long storeId, string userName);
        Task<ApiResponse> GetCurrentStoreUsersByStore(long storeId);
        Task<ApiResponse> GetStoreUser(long id);
        Task<ApiResponse> Post(EmployeeDTO input);
        Task<ApiResponse> Put(EmployeeDTO input);
        Task<ApiResponse> Delete(long id);
        Task<ApiResponse> Accept_Reject(StoreUsers entity);
        Task<ApiResponse> GetUsersForAddEmployees(long storeId);
    }
}
