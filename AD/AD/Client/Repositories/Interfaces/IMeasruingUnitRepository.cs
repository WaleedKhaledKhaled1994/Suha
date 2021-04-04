using AD.Client.Helpers;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.MeasruingUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IMeasruingUnitRepository
    {
        Task<List<MeasruingUnit>> Get();
        Task<PaginatedResponse<List<MeasruingUnit>>> Get(PaginationSearchDTO paginationSearchDTO);
        Task<MeasruingUnit> Get(long id);
        Task<long> Create(AddMeasruingUnitVMRequest entity);
        Task Edit(EditMeasruingUnitVMRequest entity);
        Task Delete(long id);
    }
}
