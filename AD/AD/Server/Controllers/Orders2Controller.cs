using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AD.Server.Helpers;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.ViewModels;
using AD.Shared.ViewModels.Notification;
using AD.Shared.ViewModels.Order;
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
    public class Orders2Controller : ControllerBase
    {
        private readonly IOrderSrevices _entityServices;
        private readonly IRepository<Order> _entityRepository;

        private readonly IUserCServices _userCServices;

        public Orders2Controller(IOrderSrevices entityServices, IRepository<Order> entityRepository, IUserCServices userCServices)
        {
            _entityServices = entityServices;
            _entityRepository = entityRepository;
            _userCServices = userCServices;
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

        [HttpPost("GetUserOrders")]
        public async Task<ActionResult> UserOrders(IndexOrderVMRequest paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<Order> list = new();
                var user = await _userCServices.GetCurrent(User.Identity.Name);
                long userId = user != null ? user.Id : 0;

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                        .Where(x => x.UserCId == userId)
                        .Include(x => x.Product).ThenInclude(x=>x.Store)
                        .Include(x => x.StorePaymentMethod).ThenInclude(x => x.PaymentMethod)
                        .ToListAsync();
                else
                    list = await _entityRepository.TableNoTracking
                        .Include(x => x.Product)
                        .Include(x => x.StorePaymentMethod).ThenInclude(x => x.PaymentMethod)
                        .Where(x => x.Product.Name.ToLower().Contains(paginationDTO.SearchText.ToLower()) && x.UserCId == userId)
                        .ToListAsync();

                if (paginationDTO.OrderStatus != null)
                    list = list.Where(x => x.Status == paginationDTO.OrderStatus).ToList();

                if (paginationDTO.From != null)
                    list = list.Where(x => x.OrderDate >= paginationDTO.From).ToList();

                if (paginationDTO.To != null)
                    list = list.Where(x => x.OrderDate <= paginationDTO.To).ToList();

                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationDTO.Pagination);

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

        [HttpPost("GetStoreOrders")]
        public async Task<ActionResult> GetStoreOrders(IndexStoreOrderVMRequest paginationDTO)
        {
            try
            {
                #region Start the watch   
                var watch = new Stopwatch();
                watch.Start();
                #endregion

                List<Order> list = new();

                if (String.IsNullOrWhiteSpace(paginationDTO.SearchText))
                    list = await _entityRepository.TableNoTracking
                        .Include(x=>x.UserC)
                        .Include(x => x.Product)
                        .Include(x => x.StorePaymentMethod).ThenInclude(x => x.PaymentMethod)
                        .Where(x => x.Product.StoreId == paginationDTO.StoreId)
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();
                else
                    list = await _entityRepository.TableNoTracking
                        .Include(x => x.UserC)
                        .Include(x => x.Product)
                        .Include(x => x.StorePaymentMethod).ThenInclude(x => x.PaymentMethod)
                        .Where(x => x.Product.Name.ToLower().Contains(paginationDTO.SearchText.ToLower()) && x.Product.StoreId == paginationDTO.StoreId)
                        .OrderByDescending(x => x.Id)
                        .ToListAsync();

                if (paginationDTO.OrderStatus != null)
                    list = list.Where(x => x.Status == paginationDTO.OrderStatus).ToList();

                if (paginationDTO.From != null)
                    list = list.Where(x => x.OrderDate >= paginationDTO.From).ToList();

                if (paginationDTO.To != null)
                    list = list.Where(x => x.OrderDate <= paginationDTO.To).ToList();

                var listCount = list.Count;
                await HttpContext.InsertPaginationParametersInResponseList(list, paginationDTO.RecordsPerPage);
                list = list.PaginateSearch(paginationDTO.Pagination);

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

        [HttpGet("GetOrderDetailsForUser/{id}")]
        public async Task<ActionResult> GetOrderDetailsForUser(long id)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion
            var result = await _entityServices.GetOrderDetailsForUser(id);
            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpGet("GetOrderDetailsForStore/{id}")]
        public async Task<ActionResult> GetOrderDetailsForStore(long id)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.GetOrderDetailsForStore(id);
            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post(AddOrderVMRequest entity)
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

        [HttpPost("CreateAll")]
        public async Task<ActionResult> PostAll(List<AddOrderVMRequest> entities)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion
            var result = await _entityServices.PostAll(entities, User.Identity.Name);
            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Put(EditOrderVMRequest entity)
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

        [HttpPut("PutRate")]
        public async Task<ActionResult> PutRate(OrderRateVMRequest entity)
        {
            #region Start the watch   
            var watch = new Stopwatch();
            watch.Start();
            #endregion

            var result = await _entityServices.PutRate(entity, User.Identity.Name);
          
            #region End the watch  
            watch.Stop();
            result.Meta.TotalProcessingTime = watch.ElapsedMilliseconds;
            #endregion

            return Ok(result);
        }
    }
}
