using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.Entities;
using AD.Shared.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/messages";
        public MessageRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Message>> Get(long orderId)
        {
            var response = await _httpService.Get2<List<Message>>($"{url}/Chat/{orderId}");

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task<long> Create(AddMessageVMRequest entity)
        {
            var response = await _httpService.Post2<AddMessageVMRequest, long>(url, entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
    }
}
