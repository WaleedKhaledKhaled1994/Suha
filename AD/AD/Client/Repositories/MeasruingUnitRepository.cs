using Newtonsoft.Json;
using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.MeasruingUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class MeasruingUnitRepository : IMeasruingUnitRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/measruingunits";
        public MeasruingUnitRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<MeasruingUnit>> Get()
        {
            var response = await _httpService.Get2<List<MeasruingUnit>>($"{url}/all");

            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            
            return response.Response;
        }

        public async Task<PaginatedResponse<List<MeasruingUnit>>> Get(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<MeasruingUnit>>(url, paginationSearchDTO);
        }

        public async Task<MeasruingUnit> Get(long id)
        {
            var response = await _httpService.Get2<MeasruingUnit>($"{url}/{id}");

            if (!response.Success)
                throw new ApplicationException(await response.GetBody());

            return response.Response;
        }

        public async Task<long> Create(AddMeasruingUnitVMRequest entity)
        {
            var response = await _httpService.Post2<AddMeasruingUnitVMRequest, long>(url, entity);

            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            
            return response.Response;
        }

        public async Task Edit(EditMeasruingUnitVMRequest entity)
        {
            var response = await _httpService.Put<EditMeasruingUnitVMRequest>(url, entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
        }

        public async Task Delete(long id)
        {
            var response = await _httpService.Delete($"{url}/{id}");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
        }
    }
}
