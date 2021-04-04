using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.ViewModels.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/tags";
        public TagRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Tag>> Get()
        {
            var response = await _httpService.Get2<List<Tag>>($"{url}/all");

            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }

        public async Task<PaginatedResponse<List<Tag>>> Get(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<Tag>>(url, paginationSearchDTO);
        }
        
        public async Task<Tag> Get(long id)
        {
            var response = await _httpService.Get2<Tag>($"{url}/{id}");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }

        public async Task<long> Create(AddTagVMRequest entity)
        {
            var response = await _httpService.Post2<AddTagVMRequest, long>(url, entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }

        public async Task Edit(EditTagVMRequest entity)
        {
            var response = await _httpService.Put<EditTagVMRequest>(url, entity);
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
