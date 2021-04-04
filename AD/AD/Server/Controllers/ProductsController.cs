using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using AD.Client.Pages.Products;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Product;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AD.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductServices _entityServices;
        private readonly IRepository<Product> _entityRepository;
        private readonly IRepository<Currency> _currencyRepository;

        public ProductsController(IProductServices entityServices, IRepository<Product> entityRepository, IRepository<Currency> currencyRepository)
        {
            _entityServices = entityServices;
            _entityRepository = entityRepository;
            _currencyRepository = currencyRepository;
        }

        [HttpGet("all")]
        public async Task<ActionResult> Get()
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.Get();

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationSearchDTO paginationSearchDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<Product> list = new();

                if (String.IsNullOrWhiteSpace(paginationSearchDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                    .Where(x => x.Status == Status.Online)
                    .Include(x => x.Category).Include(x => x.Store).OrderByDescending(x => x.Id).ToListAsync();

                else
                    list = await _entityRepository.TableNoTracking
                       .Where(x => x.Name.ToLower().Contains(paginationSearchDTO.SearchText.ToLower()) && x.Status == Status.Online)
                       .Include(x => x.Category)
                       .Include(x => x.Store)
                       .OrderByDescending(x => x.Id)
                       .ToListAsync();

                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationSearchDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationSearchDTO);

                var page = paginationSearchDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationSearchDTO.RecordsPerPage);

                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, list);

                #region End the watch  
                watch.Stop();
                result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
                #endregion

                return Ok(result);
            }
            catch (Exception)
            {
                var result = ApiResponse.Create(HttpStatusCode.BadRequest, null, "InternalServerError_Error");
                return Ok(result);
            }
        }

        [HttpGet("GetFreezedProducts")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> GetFreezedProducts([FromQuery] PaginationSearchDTO paginationSearchDTO)
        {
            List<Product> list = new();

            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                if (String.IsNullOrWhiteSpace(paginationSearchDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                    .Where(x => x.Status == Status.Offline)
                    .Include(x => x.Category)
                    .Include(x => x.Store).OrderByDescending(x => x.Id).ToListAsync();

                else
                    list = await _entityRepository.TableNoTracking
                       .Where(x => x.Name.ToLower().Contains(paginationSearchDTO.SearchText.ToLower()) && x.Status == Status.Offline)
                       .Include(x => x.Category)
                       .Include(x => x.Store)
                       .OrderByDescending(x => x.Id)
                       .ToListAsync();

                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationSearchDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationSearchDTO);

                var page = paginationSearchDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationSearchDTO.RecordsPerPage);

                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, list);

                #region End the watch  
                watch.Stop();
                result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
                #endregion

                return Ok(result);
            }
            catch (Exception)
            {
                var result = ApiResponse.Create(HttpStatusCode.BadRequest, null, "InternalServerError_Error");
                return Ok(result);
            }
        }

        [HttpGet("GetProductsByStore/{storeId}")]
        public async Task<ActionResult> GetProductsByStore([FromQuery] PaginationSearchDTO paginationDTO, long storeId)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion
                List<Product> list = new();

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                    .Where(x => x.StoreId == storeId)
                    .Include(x => x.Category)
                    .Include(x => x.Store)
                    .OrderByDescending(c => c.Id).ToListAsync();

                else
                    list = await _entityRepository.TableNoTracking
                    .Where(x => x.StoreId == storeId && x.Store.Name.ToLower().Contains(paginationDTO.SearchText.ToLower()))
                    .Include(x => x.MeasruingUnit)
                    .Include(x => x.Category)
                    .Include(x => x.Store)
                    .OrderByDescending(c => c.Id).ToListAsync();

                List<ProductDetailsDTO> productVMs = new();
                foreach (var item in list)
                {
                    ProductDetailsDTO itemDTO = new();
                    itemDTO.Product = item;
                    itemDTO.Colors = JsonConvert.DeserializeObject<List<string>>(item.Colors);
                    itemDTO.Images = JsonConvert.DeserializeObject<List<string>>(item.Image);
                    productVMs.Add(itemDTO);
                }
                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationDTO);

                var page = paginationDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationDTO.RecordsPerPage);

                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, productVMs);

                #region End the watch  
                watch.Stop();
                result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
                #endregion

                return Ok(result);
            }
            catch (Exception)
            {
                var result = ApiResponse.Create(HttpStatusCode.BadRequest, null, "InternalServerError_Error");
                return Ok(result);
            }
        }

        [HttpGet("GetProductsByCategory/{categoryId}")]
        public async Task<ActionResult> GetProductsByCategory([FromQuery] PaginationSearchDTO paginationDTO, long categoryId)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion
                List<Product> list = new();

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                    .Where(x => x.CategoryId == categoryId && x.Status == Status.Online)
                    .Include(x => x.Category)
                    .Include(x => x.Store)
                    .OrderByDescending(c => c.Id).ToListAsync();

                else
                    list = await _entityRepository.TableNoTracking
                    .Where(x => x.CategoryId == categoryId && x.Name.ToLower().Contains(paginationDTO.SearchText.ToLower()) && x.Status == Status.Online)
                    .Include(x => x.Category)
                    .Include(x => x.Store)
                    .OrderByDescending(c => c.Id).ToListAsync();

                List<ProductDetailsDTO> productVMs = new();
                foreach (var item in list)
                {
                    ProductDetailsDTO itemDTO = new();
                    itemDTO.Product = item;
                    itemDTO.Colors = JsonConvert.DeserializeObject<List<string>>(item.Colors);
                    itemDTO.Images = JsonConvert.DeserializeObject<List<string>>(item.Image);
                    productVMs.Add(itemDTO);
                }
                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationDTO);

                var page = paginationDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationDTO.RecordsPerPage);

                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, productVMs);

                #region End the watch  
                watch.Stop();
                result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
                #endregion

                return Ok(result);
            }
            catch (Exception)
            {
                var result = ApiResponse.Create(HttpStatusCode.BadRequest, null, "InternalServerError_Error");
                return Ok(result);
            }
        }

        [HttpGet("GetIndexProductUser")]
        public async Task<ActionResult> GetIndexProductUser([FromQuery] PaginationSearchDTO paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<Product> list = new();

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                    .Where(x => x.Status == Status.Online)
                    .Include(x => x.Currency)
                    .Include(x => x.Category)
                    .Include(x => x.Store)
                    .OrderByDescending(c => c.Id).ToListAsync();

                else
                    list = await _entityRepository.TableNoTracking
                    .Where(x => x.Status == Status.Online && x.Name.ToLower().Contains(paginationDTO.SearchText.ToLower()))
                    .Include(x => x.Currency)
                    .Include(x => x.Category)
                    .Include(x => x.Store)
                    .OrderByDescending(c => c.Id).ToListAsync();

                List<ProductUserDTO> productVMs = new();

                Currency UserCurrency = await GetUserCurrency();

                foreach (var item in list)
                {
                    ProductUserDTO itemDTO = new();
                    itemDTO.Product = item;
                    itemDTO.PriceUser = GetPriceInUserCurrenct(item.Currency.Rate, UserCurrency.Rate, item.Price);
                    itemDTO.PriceUser1 = GetPriceInUserCurrenct(item.Currency.Rate, UserCurrency.Rate, item.Price1);
                    itemDTO.PriceUser2 = GetPriceInUserCurrenct(item.Currency.Rate, UserCurrency.Rate, item.Price2);
                    itemDTO.PriceUser3 = GetPriceInUserCurrenct(item.Currency.Rate, UserCurrency.Rate, item.Price3);
                    itemDTO.UserCurrency = UserCurrency;
                    productVMs.Add(itemDTO);
                }
                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationDTO);

                var page = paginationDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationDTO.RecordsPerPage);

                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, productVMs);

                #region End the watch  
                watch.Stop();
                result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
                #endregion

                return Ok(result);
            }
            catch (Exception)
            {
                var result = ApiResponse.Create(HttpStatusCode.BadRequest, null, "InternalServerError_Error");
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(long id)
        {

            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.Get(id);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion
            return Ok(result);

        }

        [HttpGet("GetProductForUser/{id}")]
        public async Task<ActionResult> GetProductForUser(long id)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            Currency UserCurrency = await GetUserCurrency();
            var result = await _entityServices.GetProductForUser(id, UserCurrency);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post(AddProductVMRequest entity)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.Post(entity);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Put(EditProductVMRequest entity)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.Put(entity);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpPut("Freeze_UnFreeze")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Freeze_UnFreeze(FreezeVMRequest entity)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion
            var result = await _entityServices.Freeze_UnFreeze(entity);
            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult> Delete(long id)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.Delete(id);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        #region helper
        private async Task<Currency> GetUserCurrency()
        {
            string jwt = Request.Headers[HeaderNames.Authorization].ToString().Replace("bearer ", "");
            List<Claim> claims = ParseClaimsFromJwt(jwt);

            var currencyId = long.Parse(claims.FirstOrDefault(c => c.Type == "CurrencyId").Value);

            Currency currency = await _currencyRepository.GetByIdAsync(currencyId);
            return currency;

            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //var currencyId = (identity.FindFirst("CurrencyId")?.Value);
            //Claim claim = User.Claims.Where(x => x.Type == ClaimTypes.PostalCode)
            //.SingleOrDefault(); 
        }

        private static decimal GetPriceInUserCurrenct(double ProductCurrency, double UserCurrency, decimal Price)
        {
            decimal priceInBase = decimal.Divide(Price, Convert.ToDecimal(ProductCurrency));
            decimal priceInUserCurrency = decimal.Multiply(priceInBase, Convert.ToDecimal(UserCurrency));
            return priceInUserCurrency;
        }

        private List<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = System.Text.Json.JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
        #endregion
    }
}