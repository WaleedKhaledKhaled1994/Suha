using AD.Shared.DTOs;
using AD.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IErrorLogsServices
    {
        Task<List<WebApiLogs>> Get();
        Task<WebApiLogs> Get(long id);
        //Task<long> Post(WebApiLogs input);
        //Task Put(WebApiLogs input);
        Task Delete(long id);
    }
}
