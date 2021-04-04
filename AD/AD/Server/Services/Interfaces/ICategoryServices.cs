using AD.Shared.DTOs;
using AD.Shared.DTOs.Category;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface ICategoryServices
    {
        Task<ApiResponse> Get();
        Task<ApiResponse> GetCategoriesWithoutProducts();
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> GetDetailsForUser(long id, string userName);
        Task<List<Category>> GetUpperCategories(Category category);
        Task<List<Category>> GetChildCategories(Category category);
        Task<ApiResponse> Post(AddCategoryVMRequest input);
        Task<ApiResponse> Put(EditCategoryVMRequest input);
        Task<ApiResponse> Delete(long id);
        Task<ApiResponse> Freeze_UnFreeze(FreezeVMRequest entity);
        Task<bool> IsCategoryHasProducts(long categortId);
    }
}
