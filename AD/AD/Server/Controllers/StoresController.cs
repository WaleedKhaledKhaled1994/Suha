using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Store;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Store;
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
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreServices _entityServices;
        private readonly IRepository<Store> _entityRepository;
        private readonly IUserCServices _userCServices;
        private readonly IRepository<StoreFollowers> _storeFollowersRepository;
        private readonly IRepository<StoreUsers> _storeUsersRepository;

        public StoresController(IRepository<Store> entityRepository, IStoreServices entityServices, IRepository<StoreFollowers> storeFollowersRepository, IRepository<StoreUsers> storeUsersRepository, IUserCServices userCServices)
        {
            _entityRepository = entityRepository;
            _entityServices = entityServices;
            _storeFollowersRepository = storeFollowersRepository;
            _storeUsersRepository = storeUsersRepository;
            _userCServices = userCServices;
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

        [HttpGet("GetAllStoresAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> GetAllStoresAdmin([FromQuery] PaginationSearchDTO paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<Store> list = new();

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.Status == Status.Online)
                       .Include(x => x.Currency)
                       .Include(x => x.City).ThenInclude(x => x.Country)
                       .Include(x => x.StoreTags).ThenInclude(x => x.Tag)
                       .Include(x => x.StoreCountries).ThenInclude(x => x.Country)
                       .Include(x => x.StoreCities).ThenInclude(x => x.City)
                       .Include(x => x.StoreUsers).ThenInclude(x => x.User)
                       .OrderByDescending(x => x.Id)
                       .ToListAsync();
                else
                    list = await _entityRepository.TableNoTracking
                       .Where(x => x.Name.ToLower().Contains(paginationDTO.SearchText.ToLower()) && x.Status == Status.Online)
                       .Include(x => x.Currency)
                       .Include(x => x.City).ThenInclude(x => x.Country)
                       .Include(x => x.StoreTags).ThenInclude(x => x.Tag)
                       .Include(x => x.StoreCountries).ThenInclude(x => x.Country)
                       .Include(x => x.StoreCities).ThenInclude(x => x.City)
                       .Include(x => x.StoreUsers).ThenInclude(x => x.User)
                       .OrderByDescending(x => x.Id)
                       .ToListAsync();
                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationDTO);

                var page = paginationDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationDTO.RecordsPerPage);

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

        [HttpGet("GetAllFreezedStoresAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> GetAllFreezedStoresAdmin([FromQuery] PaginationSearchDTO paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<Store> list = new();

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.Status == Status.Offline)
                       .Include(x => x.Currency)
                       .Include(x => x.City).ThenInclude(x => x.Country)
                       .Include(x => x.StoreTags).ThenInclude(x => x.Tag)
                       .Include(x => x.StoreCountries).ThenInclude(x => x.Country)
                       .Include(x => x.StoreCities).ThenInclude(x => x.City)
                       .Include(x => x.StoreUsers).ThenInclude(x => x.User)
                       .OrderByDescending(x => x.Id)
                       .ToListAsync();
                else
                    list = await _entityRepository.TableNoTracking
                       .Where(x => x.Name.ToLower().Contains(paginationDTO.SearchText.ToLower()) && x.Status == Status.Offline)
                       .Include(x => x.Currency)
                       .Include(x => x.City).ThenInclude(x => x.Country)
                       .Include(x => x.StoreTags).ThenInclude(x => x.Tag)
                       .Include(x => x.StoreCountries).ThenInclude(x => x.Country)
                       .Include(x => x.StoreCities).ThenInclude(x => x.City)
                       .Include(x => x.StoreUsers).ThenInclude(x => x.User)
                       .OrderByDescending(x => x.Id)
                       .ToListAsync();

                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationDTO);

                var page = paginationDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationDTO.RecordsPerPage);

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

        [HttpGet("GetAllStoresUser")]
        public async Task<ActionResult> GetAllStoresUser([FromQuery] PaginationSearchDTO paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<Store> list = new();
                List<IndexStoresUser> storeVMList = new();

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.Status == Status.Online)
                       .Include(x => x.Currency)
                       .Include(x => x.City).ThenInclude(x => x.Country)
                       .Include(x => x.StoreTags).ThenInclude(x => x.Tag)
                       .Include(x => x.StoreCountries).ThenInclude(x => x.Country)
                       .Include(x => x.StoreCities).ThenInclude(x => x.City)
                       .OrderByDescending(x => x.Id).ToListAsync();
                else
                    list = await _entityRepository.TableNoTracking
                       .Where(x => x.Name.ToLower().Contains(paginationDTO.SearchText.ToLower()) && x.Status == Status.Online)
                       .Include(x => x.Currency)
                       .Include(x => x.City).ThenInclude(x => x.Country)
                       .Include(x => x.StoreTags).ThenInclude(x => x.Tag)
                       .Include(x => x.StoreCountries).ThenInclude(x => x.Country)
                       .Include(x => x.StoreCities).ThenInclude(x => x.City)
                       .OrderByDescending(x => x.Id).ToListAsync();

                var user = await _userCServices.GetCurrent(User.Identity.Name);
                long userId = user != null ? user.Id : 0;

                var myFavouriteStoresIds = (await _storeFollowersRepository.TableNoTracking
                    .Where(x => x.UserCId == userId && x.Status == FollowStatus.Follow).ToListAsync()).Select(x => x.StoreId).ToList();

                foreach (var item in list)
                {
                    IndexStoresUser storeVM = new();

                    storeVM.Store = item;

                    storeVM.Status = myFavouriteStoresIds.Contains(item.Id) ? FollowStatus.Follow : FollowStatus.UnFollow;

                    storeVMList.Add(storeVM);
                }
                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationDTO);

                var page = paginationDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationDTO.RecordsPerPage);

                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, storeVMList);

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

        //[HttpGet("GetMyStoresAll")]
        //public async Task<ActionResult> GetMyStoresAll()
        //{
        //    #region Start the watch   
        //    var watch = new Stopwatch();
        //    watch.Start();
        //    #endregion

        //    var result = await _entityServices.GetMyStoresAll(User.FindFirst(ClaimTypes.Name).Value);
        //    #region End the watch  
        //    watch.Stop();
        //    result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
        //    #endregion

        //    return Ok(result);
        //}

        [HttpGet("GetMyStores")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult> GetMyStores([FromQuery] PaginationSearchDTO paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion
                var user = await _userCServices.GetCurrent(User.Identity.Name);
                long userId = user != null ? user.Id : 0;
                List<StoreUsers> list = new();
                List<IndexMyStores> listIndexMyStores = new();
                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _storeUsersRepository.TableNoTracking
                        .Where(x => x.UserCId == userId && x.Status==StoreUserStatus.Accepted)
                        .Include(x => x.Store).ThenInclude(x => x.StoreTags).ThenInclude(x => x.Tag)
                        .OrderByDescending(x => x.Id).ToListAsync();
                else
                    list = await _storeUsersRepository.TableNoTracking
                        .Where(x => x.UserCId == userId && x.Status==StoreUserStatus.Accepted && x.Store.Name.ToLower().Contains(paginationDTO.SearchText.ToLower()))
                        .Include(x => x.Store).ThenInclude(x => x.StoreTags).ThenInclude(x => x.Tag)
                        .OrderByDescending(x => x.Id).ToListAsync();
                foreach (var item in list)
                {
                    IndexMyStores indexMyStores = new() { Store = item.Store };
                    if (item.Role != null)
                    {
                        List<int> roleDeserialize = JsonConvert.DeserializeObject<List<int>>(item.Role);
                        if (roleDeserialize.Contains(0))
                            indexMyStores.Admin = true;
                        if (roleDeserialize.Contains(1))
                            indexMyStores.ManageProducts = true;
                        if (roleDeserialize.Contains(2))
                            indexMyStores.ManageOrders = true;
                        if (roleDeserialize.Contains(3))
                            indexMyStores.ManageComments = true;
                    }
                    listIndexMyStores.Add(indexMyStores);
                }
                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationDTO);
                var page = paginationDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationDTO.RecordsPerPage);
                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, listIndexMyStores);
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

        [HttpGet("GetRequestAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult> GetRequestAdmin([FromQuery] PaginationSearchDTO paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion
                List<IndexRequestAdminStores> storeVMList = new();
                List<StoreUsers> list = new();
                long userId = (await _userCServices.GetCurrent(User.Identity.Name)).Id;

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _storeUsersRepository.TableNoTracking
                        .Where(x => x.UserCId == userId&& x.Status== StoreUserStatus.Pending)
                        .Include(x => x.Store).ThenInclude(x => x.StoreTags).ThenInclude(x => x.Tag)
                        .OrderByDescending(x => x.Id).ToListAsync();
                else
                    list = await _storeUsersRepository.TableNoTracking
                        .Where(x => x.UserCId == userId && x.Status==StoreUserStatus.Pending && x.Store.Name.ToLower().Contains(paginationDTO.SearchText.ToLower()))
                        .Include(x => x.Store).ThenInclude(x => x.StoreTags).ThenInclude(x => x.Tag)
                        .OrderByDescending(x => x.Id).ToListAsync();

                foreach (var item in list)
                {
                    IndexRequestAdminStores storeVM = new()
                    {
                        Store = item.Store,
                        Status = item.Status
                    };

                    List<int> roleDeserialize = JsonConvert.DeserializeObject<List<int>>(item.Role);

                    if (roleDeserialize.Contains(0))
                        storeVM.Admin = true;
                    if (roleDeserialize.Contains(1))
                        storeVM.ManageProducts = true;
                    if (roleDeserialize.Contains(2))
                        storeVM.ManageOrders = true;
                    if (roleDeserialize.Contains(3))
                        storeVM.ManageComments = true;

                    storeVMList.Add(storeVM);
                }
                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationDTO);

                var page = paginationDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationDTO.RecordsPerPage);

                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, storeVMList);

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

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult> Post(AddStoreVMRequest entity)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion


            var result = await _entityServices.Post(entity, User.Identity.Name);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult> Put(EditStoreVMRequest entity)
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

    }
}
