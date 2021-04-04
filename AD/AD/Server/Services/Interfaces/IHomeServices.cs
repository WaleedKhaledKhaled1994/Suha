using AD.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IHomeServices
    {
        Task LogToDB(LogDTO log);
        Task CurrencyService();
        Task UpdateCurrencies();
        Task SendNotificationsConfirmToReceivedOrders();
        Task SendNotifications();
    }
}
