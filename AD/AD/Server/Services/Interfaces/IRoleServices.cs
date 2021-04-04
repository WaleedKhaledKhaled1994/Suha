using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IRoleServices
    {
        Task<ApiResponse> GetRolesForStore(long storeId, string userName);
    }
}
