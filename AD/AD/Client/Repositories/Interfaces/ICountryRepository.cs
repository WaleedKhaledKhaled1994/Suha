using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.ViewModels.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<Country>> Get();
        Task<PaginatedResponse<List<Country>>> Get(PaginationSearchDTO paginationSearchDTO);
        Task<Country> Get(long id);
        Task<long> Create(AddCountryVMRequest entity);
        Task Edit(EditCountryVMRequest entity);
        Task Delete(long id);
    }
}
