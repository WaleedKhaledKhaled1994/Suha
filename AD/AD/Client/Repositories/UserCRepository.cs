using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Auth;
using AD.Shared.Entities;
using AD.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class UserCRepository : IUserCRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/userc";
        public UserCRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<UserC>> GetUserCBySearch(string searchText)
        {
            var response = await _httpService.Get2<List<UserC>>($"{url}/GetUserCBySearch/{searchText}");

            if (!response.Success)
                throw new ApplicationException(await response.GetBody());

            return response.Response;
        }

        public async Task<UserC> GetCurrent()
        {
            var response = await _httpService.Get2<UserC>($"{url}/GetCurrent");

            if (!response.Success)
                throw new ApplicationException(await response.GetBody());

            return response.Response;
        }

        public async Task<List<UserC>> Get()
        {
            var response = await _httpService.Get2<List<UserC>>($"{url}/all");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<PaginatedResponse<List<UserC>>> Get(PaginationSearchDTO paginationDTO)
        {
            return await _httpService.GetHelper2<List<UserC>>(url, paginationDTO);
        }

        public async Task<PaginatedResponse<List<UserC>>> GetFreezedUsers(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<UserC>>($"{url}/GetFreezedUsers", paginationSearchDTO);
        }


        public async Task<UserC> Get(long id)
        {
            var response = await _httpService.Get2<UserC>($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<long> Create(UserC entity)
        {
            var response = await _httpService.Post2<UserC, long>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task Edit(UserC entity)
        {
            var response = await _httpService.Put<UserC>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task Freeze_UnFreeze(FreezeVMRequest entity)
        {
            var response = await _httpService.Put<FreezeVMRequest>($"{url}/Freeze_UnFreeze", entity);
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
