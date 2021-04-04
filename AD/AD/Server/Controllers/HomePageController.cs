using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs.Product;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using AD.Shared.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomePageController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Store> _storeRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<StoreCountries> _storeCountriesRepository;
        private readonly IRepository<StoreCities> _storeCitiesRepository;
        public HomePageController(IRepository<Category> categoryRepository, IRepository<Product> productRepository, IRepository<Tag> tagRepository, IRepository<StoreCities> storeCitiesRepository, IRepository<Store> storeRepository, IRepository<StoreCountries> storeCountriesRepository, IRepository<Order> orderRepository, IRepository<Currency> currencyRepository)
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
            _storeCountriesRepository = storeCountriesRepository;
            _storeCitiesRepository = storeCitiesRepository;
            _storeRepository = storeRepository;
            _orderRepository = orderRepository;
            _currencyRepository = currencyRepository;
        }

        /// <summary>
        /// Notice: By default, it will return 10 Tags, 10 Categories, 10 Products as Maximum
        /// </summary>
        [HttpPost("Home")]
        public async Task<ActionResult> GetHome(HomePageVMRequest input)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            try
            {
                HomePageVMResponse response = new();

                response.Tags = _tagRepository.TableNoTracking.Where(x => x.Status == Status.Online).OrderByDescending(x => x.Id).Take(10).ToList();
                response.Categories = _categoryRepository.TableNoTracking.Where(x => x.Status == Status.Online).OrderByDescending(x => x.Id).Take(10).ToList();
                var query = _productRepository.TableNoTracking.Where(x => x.Status == Status.Online).AsQueryable();

                //Filter
                if (!String.IsNullOrWhiteSpace(input.Product))
                    query = query.Where(x => x.Name.ToLower().Contains(input.Product.ToLower()));

                if (!String.IsNullOrWhiteSpace(input.Brand))
                    query = query.Where(x => x.Brand.ToLower().Contains(input.Brand.ToLower()));

                if (input.StoreId != 0)
                    query = query.Where(x => x.StoreId == input.StoreId);

                if (input.CategoryId != 0)
                    query = query.Where(x => x.CategoryId == input.CategoryId);

                if (input.ProductRelease != null)
                    query = query.Where(x => x.CreatedOnUtc.Year == input.ProductRelease.Value.Year &&
                    x.CreatedOnUtc.Month == input.ProductRelease.Value.Month);

                if (input.CountryId != 0)
                {
                    var storesCountry = _storeCountriesRepository.TableNoTracking.Where(x => x.CountryId == input.CountryId).Select(x => x.StoreId);
                    var quer = query.Where(x => storesCountry.Contains(x.StoreId));
                }

                if (input.CityId != 0)
                {
                    var storesCity = _storeCitiesRepository.TableNoTracking.Where(x => x.CityId == input.CityId).Select(x => x.StoreId);
                    var quer = query.Where(x => storesCity.Contains(x.StoreId));
                }

                if (input.StoreRate > 0)
                {
                    List<ProductWithStoreDTO> list = new();
                    foreach (var item in query)
                    {
                        var store = await _storeRepository.GetByIdAsync(item.StoreId);
                        list.Add(new ProductWithStoreDTO() { Store = store, Product = item });
                    }
                    query = list.Where(x => x.Store.AverageRate >= input.StoreRate).Select(x => x.Product).AsQueryable();
                }

                //Sort
                List<Product> sortedProducts = new();
                switch (input.SortType)
                {
                    case SortType.ProductRelease:
                        if (input.OrderByDate == OrderByDate.Desc)
                            query = query.OrderByDescending(x => x.Id);
                        else
                            query = query.OrderBy(x => x.Id);

                        sortedProducts = query.ToList();
                        break;

                    case SortType.BestSeller:
                        var orders = _orderRepository.TableNoTracking.Include(x => x.Product)
                            .GroupBy(x => x.ProductId)
                            .Select(x => new ProductWithNumberOfSalesDTO() { NumberOfSales = x.Sum(x => x.Quantity), ProductId = x.Key })
                            .OrderByDescending(x => x.NumberOfSales);

                        var productsIds = orders.Select(x => x.ProductId).ToList();
                        sortedProducts = query.ToList();
                        //Sort products list according to List of Products_Ids
                        sortedProducts = productsIds.Join(
                                sortedProducts,
                                i => i,
                                d => d.Id,
                                (i, d) => d).ToList();
                        break;
                    case SortType.HieghPrice:
                        List<ProductWithBasePriceDTO> productsDTOs = new();
                        foreach (var item in query)
                        {
                            ProductWithBasePriceDTO productDTO = new();
                            productDTO.Product = item;
                            productDTO.Price = await GetProductPriceInBase(item);
                            productsDTOs.Add(productDTO);
                        }
                        productsDTOs = productsDTOs.OrderByDescending(x => x.Price).ToList();
                        sortedProducts = productsDTOs.Select(x => x.Product).ToList();
                        break;
                    case SortType.LowPrice:
                        productsDTOs = new();
                        foreach (var item in query)
                        {
                            ProductWithBasePriceDTO productDTO = new();
                            productDTO.Product = item;
                            productDTO.Price = await GetProductPriceInBase(item);
                            productsDTOs.Add(productDTO);
                        }
                        productsDTOs = productsDTOs.OrderBy(x => x.Price).ToList();
                        sortedProducts = productsDTOs.Select(x => x.Product).ToList();
                        break;
                }
                response.Products = sortedProducts.Take(10).ToList();

                var result = ApiResponse.Create(HttpStatusCode.OK, response);

                #region End the watch  
                watch.Stop();
                result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
                #endregion

                return Ok(result);
            }
            catch (Exception ex)
            {
                var result = ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
                return Ok(result);
            }
        }

        #region Helper
        private async Task<decimal> GetProductPriceInBase(Product product)
        {
            var ProductCurrencyRate = (await _currencyRepository.TableNoTracking.Where(x=>x.Id == product.CurrencyId).SingleOrDefaultAsync()).Rate;
            decimal priceInBase = decimal.Divide(product.Price, Convert.ToDecimal(ProductCurrencyRate));
            return priceInBase;
        }
        #endregion

        //[HttpPost("Test")]
        //public ActionResult Test()
        //{
        //    #region Start the watch   
        //    var watch = new Stopwatch();
        //    watch.Start();
        //    #endregion

        //    try
        //    {
        //        List<ProductWithNumberOfSalesDTO> list = _productRepository.TableNoTracking
        //            .Select(x => new ProductWithNumberOfSalesDTO
        //            {
        //                Name = x.Name,
        //                Brand = x.Brand
        //            }).ToList();

        //        var result = ApiResponse.Create(HttpStatusCode.OK, list);

        //        #region End the watch  
        //        watch.Stop();
        //        result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
        //        #endregion

        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {
        //        var result = ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
        //        return Ok(result);
        //    }
        //}
    }
}
