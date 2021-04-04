using Microsoft.EntityFrameworkCore;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class MessageServices : IMessageServices
    {
        private readonly IRepository<Message> _entityRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IUserCServices _userCServices;

        public MessageServices(IRepository<Message> entityRepository, IUserCServices userCServices, IRepository<Order> orderRepository)
        {
            _entityRepository = entityRepository;
            _userCServices = userCServices;
            _orderRepository = orderRepository;
        }

        public async Task<ApiResponse> Get(long orderId, string userName)
        {
            try
            {
                long userId = (await _userCServices.GetCurrent(userName)).Id;

                var entities = await _entityRepository.TableNoTracking
                                .Where(x => x.OrderId == orderId)
                                .Include(x => x.Order).ThenInclude(x => x.UserC)
                                .Include(x => x.Order).ThenInclude(x => x.Product).ThenInclude(x => x.Store)
                                .OrderBy(x => x.MessageDateTime).ToListAsync();
                return ApiResponse.Create(HttpStatusCode.OK, entities);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }

        public async Task<ApiResponse> Post(AddMessageVMRequest entity, string userName)
        {
            try
            {
                long userId = (await _userCServices.GetCurrent(userName)).Id;
                Order order = await _orderRepository.GetByIdAsync(entity.OrderId);
                MessageType messageType;

                if (order.UserCId == userId)
                    messageType = MessageType.FromCustomer;
                else
                    messageType = MessageType.FromStore;

                Message message = new()
                {
                    OrderId = entity.OrderId,
                    SenderId = userId,
                    Text = entity.Text,
                    MessageType = messageType,
                    MessageDateTime = DateTime.UtcNow
                };
                var newEntity = await _entityRepository.InsertAsync(message);

                
                return ApiResponse.Create(HttpStatusCode.OK, newEntity.Id);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
    }
}
