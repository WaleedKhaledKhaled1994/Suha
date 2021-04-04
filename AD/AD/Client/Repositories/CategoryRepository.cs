using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Category;
using AD.Shared.Entities;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/categories";
        public CategoryRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Category>> Get()
        {
            var response = await _httpService.Get2<List<Category>>($"{url}/all");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<List<Category>> GetCategoriesWithoutProducts()
        {
            var response = await _httpService.Get2<List<Category>>($"{url}/GetCategoriesWithoutProducts");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task<PaginatedResponse<List<CategoryDTO>>> GetAllCategoriesAdmin(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<CategoryDTO>>($"{url}/GetAllCategoriesAdmin", paginationSearchDTO);
        }

        public async Task<PaginatedResponse<List<CategoryForUser>>> GetAllCategoriesUser(PaginationSearchDTO paginationDTO)
        {
            return await _httpService.GetHelper2<List<CategoryForUser>>($"{url}/GetAllCategoriesUser", paginationDTO);
        }

        public async Task<PaginatedResponse<List<CategoryDTO>>> GetFreezedCategories(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<CategoryDTO>>($"{url}/GetFreezedCategories", paginationSearchDTO);
        }

        public async Task<CategoryDTO> Get(long id)
        {
            var response = await _httpService.Get2<CategoryDTO>($"{url}/{id}");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());

            return response.Response;
        }

        public async Task<CategoryDetailsForUser> GetDetailsForUser(long id)
        {
            var response = await _httpService.Get2<CategoryDetailsForUser>($"{url}/GetDetailsForUser/{id}");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());

            return response.Response;
        }

        public async Task<long> Create(AddCategoryVMRequest entity)
        {
            var response = await _httpService.Post2<AddCategoryVMRequest, long>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task Edit(EditCategoryVMRequest entity)
        {
            var response = await _httpService.Put<EditCategoryVMRequest>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task Freeze_UnFreeze(FreezeVMRequest entity)
        {
            var response = await _httpService.Put<FreezeVMRequest>($"{url}/Freeze_UnFreeze", entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
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
