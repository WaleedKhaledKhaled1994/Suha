using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/audits";
        public AuditRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Audit>> Get()
        {
            var response = await _httpService.Get<List<Audit>>($"{url}/all");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<PaginatedResponse<List<Audit>>> Get(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<Audit>>(url, paginationSearchDTO);
        }

        public async Task<Audit> Get(long id)
        {
            var response = await _httpService.Get<Audit>($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<long> Create(Audit entity)
        {
            var response = await _httpService.Post<Audit, long>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task Edit(Audit entity)
        {
            var response = await _httpService.Put<Audit>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task Delete(long id)
        {
            var response = await _httpService.Delete($"{url}/{id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
