using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Auth;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCController : ControllerBase
    {
        private readonly IUserCServices _entityServices;
        private readonly IRepository<UserC> _entityRepository;

        public UserCController(IUserCServices entityServices, IRepository<UserC> entityRepository)
        {
            _entityServices = entityServices;
            _entityRepository = entityRepository;
        }
        
        [HttpGet("GetUserCBySearch/{searchText}")]
        public async Task<ActionResult> GetUserCBySearch(string searchText)
        {

            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion
            var result = await _entityServices.GetUserCBySearch(searchText);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpGet("GetCurrent")]
        public async Task<ActionResult> GetCurrent()
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion
            var result = await _entityServices.GetCurrentAPI(User.Identity.Name);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Get([FromQuery] PaginationSearchDTO paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<UserC> list = new();

            if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                list = await _entityRepository.TableNoTracking
                .Where(x => x.Status == Status.Online)
                .Include(n => n.City).ThenInclude(n => n.Country).OrderByDescending(x => x.Id).ToListAsync();

            else
                list = await _entityRepository.TableNoTracking
                    .Where(x => x.UserName.ToLower().Contains(paginationDTO.SearchText.ToLower()) && x.Status==Status.Online)
                    .Include(n => n.City).ThenInclude(n => n.Country)
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

        [HttpGet("GetFreezedUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> GetFreezedUsers([FromQuery] PaginationSearchDTO paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<UserC> list = new();

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                    .Where(x => x.Status == Status.Offline)
                    .Include(n => n.City).ThenInclude(n => n.Country).OrderByDescending(x => x.Id).ToListAsync();

                else
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.UserName.ToLower().Contains(paginationDTO.SearchText.ToLower()) && x.Status == Status.Offline)
                        .Include(n => n.City).ThenInclude(n => n.Country)
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
        public async Task<ActionResult> Post(UserC entity)
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
        public async Task<ActionResult> Put(UserC entity)
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
