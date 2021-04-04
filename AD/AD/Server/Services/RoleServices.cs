using Newtonsoft.Json;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
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
    public class RoleServices : IRoleServices
    {
        private readonly IRepository<StoreUsers> _entityRepository;
        private readonly IUserCServices _userCServices;


        public RoleServices(IRepository<StoreUsers> entityRepository, IUserCServices userCServices)
        {
            _entityRepository = entityRepository;
            _userCServices = userCServices;
        }
        public async Task<ApiResponse> GetRolesForStore(long storeId, string userName)
        {
            List<EmployeeRole> roles = new();
            try
            {
                var user = await _userCServices.GetCurrent(userName);
                if (user != null)
                {
                    var entity = _entityRepository.TableNoTracking
                             .Where(x => x.StoreId == storeId && x.UserCId == user.Id).SingleOrDefault();
                    if (entity != null)
                        roles = JsonConvert.DeserializeObject<List<EmployeeRole>>(entity.Role);
                }
                return ApiResponse.Create(HttpStatusCode.OK, roles);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
    }
}
