using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<EmployeeRole>> GetRolesForStore(long storeId);
    }
}
