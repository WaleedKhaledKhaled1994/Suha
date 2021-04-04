using AD.Shared.Entities;
using AD.Shared.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> Get(long orderId);
        Task<long> Create(AddMessageVMRequest input);
    }
}
