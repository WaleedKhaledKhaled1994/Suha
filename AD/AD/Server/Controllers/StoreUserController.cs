using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Employee;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.MTM;
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
    public class StoreUserController : ControllerBase
    {
        private readonly IStoreUserServices _entityServices;
        private readonly IRepository<StoreUsers> _entityRepository;

        public StoreUserController(IStoreUserServices entityServices, IRepository<StoreUsers> entityRepository)
        {
            _entityServices = entityServices;
            _entityRepository = entityRepository;
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

        [HttpGet("NotUsers/{storeId}")]
        public async Task<ActionResult> GetUsersForAddEmployees(long storeId)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion
            var result = await _entityServices.GetUsersForAddEmployees(storeId);
            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpGet("GetStoreEmployeesByStore/{storeId}")]
        public async Task<ActionResult> GetStoreEmployeesByStore([FromQuery] PaginationSearchDTO paginationDTO, long storeId)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<StoreUsers> list = new();
                List<EmployeeDTO> listEmployees = new();

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.StoreId == storeId)
                        .Include(x => x.User).ThenInclude(x => x.City).ThenInclude(x => x.Country)
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();

                else
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.StoreId == storeId && x.User.FirstName.ToLower().Contains(paginationDTO.SearchText.ToLower()))
                        .Include(x => x.User).ThenInclude(x => x.City).ThenInclude(x => x.Country)
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();
                foreach (var item in list)
                {
                    EmployeeDTO employee = new();
                    employee.StoreUser = item;

                    List<int> roleDeserialize = JsonConvert.DeserializeObject<List<int>>(item.Role);

                    if (roleDeserialize.Contains(0))
                        employee.Admin = true;
                    if (roleDeserialize.Contains(1))
                        employee.ManageProducts = true;
                    if (roleDeserialize.Contains(2))
                        employee.ManageOrders = true;
                    if (roleDeserialize.Contains(3))
                        employee.ManageComments = true;
                    listEmployees.Add(employee);
                }

                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(listEmployees, paginationDTO.RecordsPerPage);
                listEmployees = listEmployees.PaginateSearch(paginationDTO);

                var page = paginationDTO.Page;
                var totalPages = TotalPagesCount.Value(listCount, paginationDTO.RecordsPerPage);

                var result = ApiPaginationResponse.Create(HttpStatusCode.OK, page, totalPages, listEmployees);

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

        [HttpGet("GetCurrentStoreUsersByStore/{storeId}")]
        public async Task<ActionResult> GetCurrentStoreUsersByStore(long storeId)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.GetCurrentStoreUsersByStore(storeId);

            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpGet("GetStoreUserByStore/{storeId}")]
        public async Task<ActionResult> GetStoreUserByStore(long storeId)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion
            var result = await _entityServices.GetStoreUserByStore(storeId, User.Identity.Name);
            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpGet("GetStoreUser/{storeUserId}")]
        public async Task<ActionResult> GetStoreUser(long storeUserId)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion
            var result = await _entityServices.GetStoreUser(storeUserId);
            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post(EmployeeDTO entity)
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
        public async Task<ActionResult> Put(EmployeeDTO entity)
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

        [HttpPut("Accept_Reject")]
        public async Task<ActionResult> Accept_Reject(StoreUsers entity)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion
            var result = await _entityServices.Accept_Reject(entity);
            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }
    }
}
