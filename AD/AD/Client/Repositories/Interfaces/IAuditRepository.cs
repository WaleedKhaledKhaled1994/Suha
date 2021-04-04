using AD.Shared.DTOs;
using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IAuditRepository
    {
        Task<List<Audit>> Get();
        Task<PaginatedResponse<List<Audit>>> Get(PaginationSearchDTO paginationSearchDTO);
        Task<Audit> Get(long id);
        Task<long> Create(Audit entity);
        Task Edit(Audit entity);
        Task Delete(long id);
    }
}
