using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Employee;
using AD.Shared.DTOs.Store;
using AD.Shared.Entities;
using AD.Shared.Entities.MTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class StoreUserRepository : IStoreUserRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/storeuser";
        public StoreUserRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<StoreUsers>> Get()
        {
            var response = await _httpService.Get2<List<StoreUsers>>($"{url}/all");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<List<UserC>> GetUsersForAddEmployees(long storeId)
        {
            var response = await _httpService.Get2<List<UserC>>($"{url}/NotUsers/{storeId}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<PaginatedResponse<List<EmployeeDTO>>> GetStoreEmployeesByStore(PaginationSearchDTO paginationDTO, long storeId)
        {
            return await _httpService.GetHelper2<List<EmployeeDTO>>($"{url}/GetStoreEmployeesByStore/{storeId}", paginationDTO);
        }

        public async Task<List<EmployeeDTO>> GetCurrentStoreUsersByStore(long storeId)
        {
            var response = await _httpService.Get2<List<EmployeeDTO>>($"{url}/GetCurrentStoreUsersByStore/{storeId}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<StoreUsers> GetStoreUserByStore(long storeId)
        {
            var response = await _httpService.Get2<StoreUsers>($"{url}/GetStoreUserByStore/{storeId}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<EmployeeDTO> GetStoreUser(long storeUserId)
        {
            var response = await _httpService.Get2<EmployeeDTO>($"{url}/GetStoreUser/{storeUserId}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<long> Add(EmployeeDTO entity)
        {
            var response = await _httpService.Post2<EmployeeDTO, long>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task Edit(EmployeeDTO entity)
        {
            var response = await _httpService.Put<EmployeeDTO>(url, entity);
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

        public async Task Accept_Reject(StoreUsers entity)
        {
            var response = await _httpService.Put<StoreUsers>($"{url}/Accept_Reject", entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
