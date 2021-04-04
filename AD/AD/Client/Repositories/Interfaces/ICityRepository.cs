using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.ViewModels.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface ICityRepository
    {
        Task<List<City>> Get();
        Task<PaginatedResponse<List<City>>> Get(PaginationSearchDTO paginationSearchDTO);
        Task<City> Get(long id);
        Task<long> Create(AddCityVMRequest entity);
        Task Edit(EditCityVMRequest entity);
        Task Delete(long id);
    }
}
