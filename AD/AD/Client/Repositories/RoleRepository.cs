using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class RoleRepository :IRoleRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/roles";
        public RoleRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<List<EmployeeRole>> GetRolesForStore(long storeId)
        {
            var response = await _httpService.Get2<List<EmployeeRole>>($"{url}/{storeId}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
    }
}
