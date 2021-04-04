using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using AD.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using AD.Server.Models;
using Microsoft.Extensions.Logging;
using AD.Server.Services.Interfaces;
using System.Net.Http;
using Newtonsoft.Json;
using AD.Server.Data;
using AD.Server.Repositories;
using AD.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using AD.Shared.DTOs.Currency;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AD.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeServices _homeServices;
        private readonly IRepository<Currency> _currencyRepository;

        public HomeController(IHomeServices homeServices, IRepository<Currency> currencyRepository)
        {
            _homeServices = homeServices;
            _currencyRepository = currencyRepository;
        }

        /// <summary>
        /// hello summary
        /// </summary>
        /// <param name="log">
        /// The content of the log
        /// </param>
        /// <returns>A string</returns>
        [HttpPost("LogToDB")]
        public Task LogToDB(LogDTO log)
        {
            return _homeServices.LogToDB(log);
        }

        [HttpPut("UpdateCurrencies")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task UpdateCurrencies()
        {
            await _homeServices.UpdateCurrencies();
        }

        [HttpPost("InsertCurrencies")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task InsertCurrencies()
        {
            AllCurrencies allCurrencies;
            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync("https://free.currconv.com/api/v7/currencies?apiKey=2b1c11a9fb3f05508fd6");
                string apiResponse = await response.Content.ReadAsStringAsync();
                allCurrencies = JsonConvert.DeserializeObject<AllCurrencies>(apiResponse);
            }

            foreach (var item in allCurrencies.results.GetType().GetProperties())
            {
                string Code = item.Name;

                Currency currencyDB = _currencyRepository.Table.Where(x => x.Code == Code).SingleOrDefault();
                if (currencyDB == null)
                {
                    Currency currency = new Currency()
                    {
                        Code = Code
                    };
                    await _currencyRepository.InsertAsync(currency);
                }
            }
        }
    }
}
