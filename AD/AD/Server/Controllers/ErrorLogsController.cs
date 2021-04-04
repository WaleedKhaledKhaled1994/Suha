using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
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
    public class ErrorLogsController : ControllerBase
    {
        private readonly IErrorLogsServices _entityServices;
        private readonly IRepository<WebApiLogs> _entityRepository;

        public ErrorLogsController(IErrorLogsServices entityServices, IRepository<WebApiLogs> entityRepository)
        {
            _entityServices = entityServices;
            _entityRepository = entityRepository;
        }

        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<WebApiLogs>>> Get()
        {
            var result = await _entityServices.Get();
            return result;
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
                List<WebApiLogs> list = new();

                if (String.IsNullOrWhiteSpace(paginationSearchDTO.SearchText))
                    list = await _entityRepository.TableNoTracking.OrderByDescending(x => x.Id).ToListAsync();

                else
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.Message.ToLower().Contains(paginationSearchDTO.SearchText.ToLower()))
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<WebApiLogs>> Get(long id)
        {
            var entity = await _entityServices.Get(id);
            return entity;
        }

        //[HttpPost]
        //public async Task<ActionResult<long>> Post(WebApiLogs entity)
        //{
        //    var result = await _entityServices.Post(entity);
        //    return result;
        //}

        //[HttpPut]
        //public async Task<ActionResult> Put(WebApiLogs entity)
        //{
        //    await _entityServices.Put(entity);
        //    return Ok();
        //}

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Delete(long id)
        {
            await _entityServices.Delete(id);
            return NoContent();
        }
    }
}
