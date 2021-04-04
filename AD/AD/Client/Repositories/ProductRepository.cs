using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Product;
using AD.Shared.Entities;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/products";
        public ProductRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<List<Product>> Get()
        {
            var response = await _httpService.Get2<List<Product>>($"{url}/all");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }
        public async Task<PaginatedResponse<List<Product>>> Get(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<Product>>(url, paginationSearchDTO);
        }
        public async Task<PaginatedResponse<List<Product>>> GetFreezedProducts(PaginationSearchDTO paginationSearchDTO)
        {
            return await _httpService.GetHelper2<List<Product>>($"{url}/GetFreezedProducts", paginationSearchDTO);
        }
        public async Task<PaginatedResponse<List<ProductDetailsDTO>>> GetProductsByStore(PaginationSearchDTO paginationDTO, long storeId)
        {
            return await _httpService.GetHelper2<List<ProductDetailsDTO>>($"{url}/GetProductsByStore/{storeId}", paginationDTO);
        }
        public async Task<PaginatedResponse<List<ProductDetailsDTO>>> GetProductsByCategory(PaginationSearchDTO paginationDTO, long categoryId)
        {
            return await _httpService.GetHelper2<List<ProductDetailsDTO>>($"{url}/GetProductsByCategory/{categoryId}", paginationDTO);
        }

        public async Task<PaginatedResponse<List<ProductUserDTO>>> GetIndexProductUser(PaginationSearchDTO paginationDTO)
        {
            return await _httpService.GetHelper2<List<ProductUserDTO>>($"{url}/GetIndexProductUser", paginationDTO);
        }

        public async Task<ProductDetailsDTO> Get(long id)
        {
            var response = await _httpService.Get2<ProductDetailsDTO>($"{url}/{id}");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }

        public async Task<ProductDetailsUserDTO> GetProductForUser(long id)
        {
            var response = await _httpService.Get2<ProductDetailsUserDTO>($"{url}/GetProductForUser/{id}");
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }

        public async Task<long> Create(AddProductVMRequest entity)
        {
            var response = await _httpService.Post2<AddProductVMRequest, long>(url, entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }
        public async Task<long> Create2(AddProductVMRequest entity)
        {
            var response = await _httpService.Post2<AddProductVMRequest, long>(url, entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
            return response.Response;
        }
        public async Task Edit(EditProductVMRequest entity)
        {
            var response = await _httpService.Put<EditProductVMRequest>(url, entity);
            if (!response.Success)
                throw new ApplicationException(await response.GetBody());
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
                throw new ApplicationException(await response.GetBody());
        }
    }
}
