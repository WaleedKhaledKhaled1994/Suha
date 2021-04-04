using AD.Shared.DTOs;
using AD.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<List<Currency>> Get();
        Task<PaginatedResponse<List<Currency>>> Get(PaginationSearchDTO paginationSearchDTO);
        Task<Currency> Get(long id);
        Task<long> Create(Currency entity);
        Task Edit(Currency entity);
        Task Delete(long id);
    }
}
