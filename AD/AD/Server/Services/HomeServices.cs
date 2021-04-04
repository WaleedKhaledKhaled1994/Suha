using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Currency;
using AD.Shared.Entities;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using AD.Shared.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using System.Reflection;
using System.Resources;
using System.Globalization;

namespace AD.Server.Services
{
    public class HomeServices : IHomeServices
    {
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly ILogger<HomeServices> _logger;

        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IRepository<Fixer> _fixerRepository;
        private readonly IRepository<FixerCurrencies> _fixerCurrenciesRepository;

        public HomeServices(IRecurringJobManager recurringJobManager, ILogger<HomeServices> logger, IRepository<Fixer> fixerRepository, IRepository<FixerCurrencies> fixerCurrenciesRepository, IRepository<Currency> currencyRepository, IRepository<Order> orderRepository, IRepository<Notification> notificationRepository)
        {
            _logger = logger;
            _recurringJobManager = recurringJobManager;
            _currencyRepository = currencyRepository;
            _fixerRepository = fixerRepository;
            _fixerCurrenciesRepository = fixerCurrenciesRepository;
            _orderRepository = orderRepository;
            _notificationRepository = notificationRepository;
        }
        public Task LogToDB(LogDTO log)
        {
            string errorLog = $"PAGE: {log.Page} - MESSAGE: {log.Message}";
            _logger.LogError(errorLog);

            return Task.CompletedTask;
        }

        public Task CurrencyService()
        {
            _recurringJobManager.AddOrUpdate(
              "CurrencyService",
              () => UpdateCurrencies(),
              "*/45 * * * *");
            return Task.CompletedTask;
        }
        public async Task UpdateCurrencies()
        {
            _logger.LogInformation($"Currency Service Has Started at: {DateTime.Now}");

            try
            {
                Root root;

                using (var httpClient = new HttpClient())
                {
                    using var response = await httpClient.GetAsync("http://data.fixer.io/api/latest?access_key=9cc3d535f5d051c64225b33e90835ff0&format=1");
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    root = JsonConvert.DeserializeObject<Root>(apiResponse);
                }
                try
                {
                    Fixer fixer = new()
                    {
                        Success = root.success,
                        TimeStamp = root.timestamp,
                        Base = root.@base,
                        Date = root.date
                    };
                    await _fixerRepository.InsertAsync(fixer);
                    foreach (var item in root.rates.GetType().GetProperties())
                    {
                        // do stuff here
                        string Code = item.Name;
                        Currency currency = _currencyRepository.Table.Where(x => x.Code == Code).SingleOrDefault();

                        double rate;
                        if (currency != null)
                        {
                            rate = Convert.ToDouble(item.GetValue(root.rates, null));
                            currency.Rate = rate;
                            await _currencyRepository.UpdateAsync(currency);

                            FixerCurrencies fixerCurrencies = new()
                            {
                                FixerId = fixer.Id,
                                CurrencyId = currency.Id,
                                Rate = rate
                            };
                            await _fixerCurrenciesRepository.InsertAsync(fixerCurrencies);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An Error has occured while Update Currencies Rates: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error has occured while Request Currency Api: {ex.Message}");
            }
        }

        public Task SendNotificationsConfirmToReceivedOrders()
        {
            // at 12:00 of every day "0 12 * * *"
            _recurringJobManager.AddOrUpdate(
              "SendNotificationsConfirmToReceivedOrders",
              () => SendNotifications(),
              "0 12 * * *");
            return Task.CompletedTask;
        }
        public async Task SendNotifications()
        {
            _logger.LogInformation($"Send Notifications Confirm To Received Orders Service Has Started at: {DateTime.Now}");
            
            try
            {
                var orders = await _orderRepository.TableNoTracking
                    .Include(x=>x.Product).Where(x => x.ShippingDate.Day == DateTime.Now.Day && x.Status == OrderStatus.InShipping).ToListAsync();
                foreach (var item in orders)
                {
                    Notification notification = new()
                    {
                        Title = "Received Confirmation",
                        Body = $"{item.Product.Name}/ {item.Quantity}/ Please, Go to Orders Page and Confirm Receiving your Order!",
                        Type = NotificationType.OrderEdit,
                        UserCId = item.UserCId,
                        Link = "/Orders"
                    };
                    var newEntity = await _notificationRepository.InsertAsync(notification);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An Error has occured while Send Notifications Confirm To Received Orders: {ex.Message}");
            }
        }
    }
}
