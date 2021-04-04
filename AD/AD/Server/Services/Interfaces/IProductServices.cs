using AD.Shared.DTOs.Product;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IProductServices
    {
        Task<ApiResponse> Get();
        Task<ApiResponse> Get(long id);
        Task<ApiResponse> GetProductForUser(long id, Currency UserCurrency);
        Task<ApiResponse> Post(AddProductVMRequest input);
        Task<ApiResponse> Put(EditProductVMRequest input);
        Task<ApiResponse> Delete(long id);
        Task<ApiResponse> Freeze_UnFreeze(FreezeVMRequest entity);
        Task<List<Product>> GetProductsByStoreIdALL(long storeId);
        Task<List<Product>> GetProductsByCategoryIdALL(long categoryId);
    }
}
