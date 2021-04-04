using AD.Shared.DTOs;
using AD.Shared.DTOs.Product;
using AD.Shared.Entities;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> Get();
        Task<PaginatedResponse<List<Product>>> Get(PaginationSearchDTO paginationSearchDTO);
        Task<PaginatedResponse<List<ProductDetailsDTO>>> GetProductsByStore(PaginationSearchDTO paginationDTO, long storeId);
        Task<PaginatedResponse<List<ProductDetailsDTO>>> GetProductsByCategory(PaginationSearchDTO paginationDTO, long categoryId);
        Task<PaginatedResponse<List<ProductUserDTO>>> GetIndexProductUser(PaginationSearchDTO paginationDTO);
        Task<ProductDetailsDTO> Get(long id);
        Task<ProductDetailsUserDTO> GetProductForUser(long id);
        Task<long> Create(AddProductVMRequest entity);
        Task<long> Create2(AddProductVMRequest entity);

        Task Edit(EditProductVMRequest entity);
        Task Delete(long id);
        Task<PaginatedResponse<List<Product>>> GetFreezedProducts(PaginationSearchDTO paginationSearchDTO);
        Task Freeze_UnFreeze(FreezeVMRequest entity);
    }
}
