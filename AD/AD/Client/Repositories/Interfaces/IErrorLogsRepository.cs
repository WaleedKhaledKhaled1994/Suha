using AD.Shared.DTOs;
using AD.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IErrorLogsRepository
    {
        Task<List<WebApiLogs>> Get();
        Task<PaginatedResponse<List<WebApiLogs>>> Get(PaginationSearchDTO paginationSearchDTO);
        Task<WebApiLogs> Get(long id);
        //Task<long> Create(WebApiLogs entity);
        //Task Edit(WebApiLogs entity);
        Task Delete(long id);
    }
}
