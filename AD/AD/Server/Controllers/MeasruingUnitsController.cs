using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ViewModels.MeasruingUnit;
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
    public class MeasruingUnitsController : ControllerBase
    {
        private readonly IMeasruingUnitServices _entityServices;
        private readonly IRepository<MeasruingUnit> _entityRepository;
        public MeasruingUnitsController(IMeasruingUnitServices entityServices, IRepository<MeasruingUnit> entityRepository)
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

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Get([FromQuery] PaginationSearchDTO paginationSearchDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<MeasruingUnit> list = new();

                if (String.IsNullOrWhiteSpace(paginationSearchDTO.SearchText))
                    list = await _entityRepository.TableNoTracking.Where(x => x.Status == Status.Online).OrderByDescending(x => x.Id).ToListAsync();

                else
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.Name_en.ToLower().Contains(paginationSearchDTO.SearchText.ToLower()) && x.Status == Status.Online)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Post(AddMeasruingUnitVMRequest entity)
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
        public async Task<ActionResult> Put(EditMeasruingUnitVMRequest entity)
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
