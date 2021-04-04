using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IAuditServices
    {
        Task<List<Audit>> Get();
        Task<Audit> Get(long id);
        Task<long> Post(Audit input);
        Task Put(Audit input);
        Task Delete(long id);
    }
}
