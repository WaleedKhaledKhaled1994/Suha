using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AD.Client.Helpers;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Category;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _entityServices;
        private readonly IRepository<Category> _entityRepository;
        private readonly IRepository<CategoryFollowers> _categoryFollowers;
        private readonly IUserCServices _userCServices;

        public CategoriesController(ICategoryServices entityServices, IRepository<Category> entityRepository, IRepository<CategoryFollowers> categoryFollowers, IUserCServices userCServices)
        {
            _entityServices = entityServices;
            _entityRepository = entityRepository;
            _categoryFollowers = categoryFollowers;
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

        [HttpGet("GetCategoriesWithoutProducts")]
        public async Task<ActionResult<List<Category>>> GetCategoriesWithoutProducts()
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.GetCategoriesWithoutProducts();

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpGet("GetAllCategoriesAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> GetAllCategoriesAdmin([FromQuery] PaginationSearchDTO paginationSearchDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion
                List<CategoryDTO> categoryVMList = new();
                List<Category> list = new();

                if (String.IsNullOrWhiteSpace(paginationSearchDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.Status == Status.Online && x.CategoryLevel == CategoryLevel.Category)
                        .Include(x => x.Parent).OrderByDescending(x => x.Id).ToListAsync();
                else
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.Name_en.ToLower().Contains(paginationSearchDTO.SearchText.ToLower()) &&
                               x.Status == Status.Online && x.CategoryLevel == CategoryLevel.Category)
                        .Include(x => x.Parent)
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();

                foreach (var category in list)
                {
                    CategoryDTO categoryVM = new()
                    {
                        Category = category,
                        UpperCategories = await _entityServices.GetUpperCategories(category),
                        ChildCategories = await _entityServices.GetChildCategories(category)
                    };
                    categoryVMList.Add(categoryVM);
                }
                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationSearchDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationSearchDTO);

                var page = paginationSearchDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationSearchDTO.RecordsPerPage);

                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, categoryVMList);

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

        [HttpGet("GetAllCategoriesUser")]
        public async Task<ActionResult> GetAllCategoriesUser([FromQuery] PaginationSearchDTO paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<Category> list = new();
                List<CategoryForUser> categoryVMList = new();

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                       .Where(x => x.Status == Status.Online && x.CategoryLevel == CategoryLevel.Category)
                       .Include(x => x.Parent)
                      .OrderByDescending(x => x.Id).ToListAsync();
                else
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.Name_en.ToLower().Contains(paginationDTO.SearchText.ToLower()) && x.CategoryLevel == CategoryLevel.Category && x.Status == Status.Online)
                        .Include(x => x.Parent)
                        .OrderByDescending(x => x.Id).ToListAsync();

                var user = await _userCServices.GetCurrent(User.FindFirst(ClaimTypes.Name).Value);
                long userId = user != null ? user.Id : 0;
                var myFavouriteCategoriesIds = (await _categoryFollowers.TableNoTracking
                    .Where(x => x.UserCId == userId && x.Status == FollowStatus.Follow).ToListAsync()).Select(x => x.CategoryId).ToList();

                foreach (var item in list)
                {
                    CategoryForUser category = new();

                    category.Category = item;

                    category.Status = myFavouriteCategoriesIds.Contains(item.Id) ? FollowStatus.Follow : FollowStatus.UnFollow;

                    categoryVMList.Add(category);
                }
                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationDTO);

                var page = paginationDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationDTO.RecordsPerPage);

                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, categoryVMList);

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

        [HttpGet("GetFreezedCategories")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> GetFreezedCategories([FromQuery] PaginationSearchDTO paginationSearchDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<CategoryDTO> categoryVMList = new();
                List<Category> list = new();
                List<Category> searchList = new();

                if (String.IsNullOrWhiteSpace(paginationSearchDTO.SearchText))
                {
                    list = await _entityRepository.TableNoTracking.Where(x => x.Status == Status.Offline).Include(x => x.Parent).OrderByDescending(x => x.Id).ToListAsync();
                    foreach (var category in list)
                    {
                        CategoryDTO categoryVM = new()
                        {
                            Category = category,
                            UpperCategories = await _entityServices.GetUpperCategories(category),
                            ChildCategories = await _entityServices.GetChildCategories(category)
                        };
                        categoryVMList.Add(categoryVM);
                    }
                }
                else
                {
                    searchList = await _entityRepository.TableNoTracking
                        .Where(x => x.Name_en.ToLower().Contains(paginationSearchDTO.SearchText.ToLower()) && x.Status == Status.Offline)
                        .Include(x => x.Parent)
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();
                    foreach (var category in searchList)
                    {
                        CategoryDTO categoryVM = new()
                        {
                            Category = category,
                            UpperCategories = await _entityServices.GetUpperCategories(category),
                            ChildCategories = await _entityServices.GetChildCategories(category)
                        };
                        categoryVMList.Add(categoryVM);
                    }
                }
                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationSearchDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationSearchDTO);

                var page = paginationSearchDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationSearchDTO.RecordsPerPage);

                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, categoryVMList);

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

        [HttpGet("GetDetailsForUser/{id}")]
        public async Task<ActionResult> GetDetailsForUser(long id)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.GetDetailsForUser(id, User.Identity.Name);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Post(AddCategoryVMRequest entity)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Put(EditCategoryVMRequest entity)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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
