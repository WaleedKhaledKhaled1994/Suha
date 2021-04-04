using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/home";
        
        public HomeRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task LogToFile(LogDTO log)
        {
            var response = await _httpService.Post<LogDTO>($"{url}/LogToFile", log);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
        public async Task LogToDB(LogDTO log)
        {
            var response = await _httpService.Post<LogDTO>($"{url}/LogToDB", log);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
