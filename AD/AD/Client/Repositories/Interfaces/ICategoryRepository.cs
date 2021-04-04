using AD.Shared.DTOs;
using AD.Shared.DTOs.Category;
using AD.Shared.Entities;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> Get();
        Task<List<Category>> GetCategoriesWithoutProducts();
        Task<PaginatedResponse<List<CategoryDTO>>> GetAllCategoriesAdmin(PaginationSearchDTO paginationSearchDTO);
        Task<PaginatedResponse<List<CategoryForUser>>> GetAllCategoriesUser(PaginationSearchDTO paginationDTO);
        Task<CategoryDTO> Get(long id);
        Task<CategoryDetailsForUser> GetDetailsForUser(long id);
        Task<long> Create(AddCategoryVMRequest entity);
        Task Edit(EditCategoryVMRequest entity);
        Task Delete(long id);
        Task Freeze_UnFreeze(FreezeVMRequest entity);
        Task<PaginatedResponse<List<CategoryDTO>>> GetFreezedCategories(PaginationSearchDTO paginationSearchDTO);
    }
}
