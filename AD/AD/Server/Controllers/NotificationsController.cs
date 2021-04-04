using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels.Notification;
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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationServices _entityServices;
        private readonly IRepository<Notification> _entityRepository;
        private readonly IUserCServices _userCServices;

        public NotificationsController(INotificationServices entityServices, IRepository<Notification> entityRepository, IUserCServices userCServices)
        {
            _entityServices = entityServices;
            _entityRepository = entityRepository;
            _userCServices = userCServices;
        }

        [HttpGet("all")]
        public async Task<ActionResult> Get()
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.Get(User.Identity.Name);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationSearchDTO paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<Notification> list = new();

                var user = await _userCServices.GetCurrent(User.Identity.Name);
                long userId = user != null ? user.Id : 0;

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.UserCId == userId)
                        .OrderByDescending(x => x.NotificationTime).ToListAsync();

                else
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.Body.ToLower().Contains(paginationDTO.SearchText.ToLower()) && x.UserCId == userId)
                        .OrderByDescending(x => x.NotificationTime)
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

        [HttpGet("GetNewCount")]
        public async Task<ActionResult> GetNewCount()
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.GetNewCount(User.Identity.Name);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post(AddNotificationVMRequest entity)
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
        public async Task<ActionResult> Put(List<long> listNotificationToUpdateRead)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.Put(listNotificationToUpdateRead);

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
