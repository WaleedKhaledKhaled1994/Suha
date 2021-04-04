using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repository
{
    public class AccountRepository: IAccountRepository
    {
        private readonly IHttpService _httpService;
        private readonly string baseURL = "api/accounts";

        public AccountRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<UserToken> Register(RegisterDTO registerDTO)
        {
            var httpResponse = await _httpService.Post2<RegisterDTO, UserToken>($"{baseURL}/create", registerDTO);

            if (!httpResponse.Success)
                throw new ApplicationException(await httpResponse.GetBody());

            return httpResponse.Response;
        }

        public async Task<UserToken> Login(LoginDTO loginDTO)
        {
            var httpResponse = await _httpService.Post2<LoginDTO, UserToken>($"{baseURL}/login", loginDTO);

            if (!httpResponse.Success)
                throw new ApplicationException(await httpResponse.GetBody());

            return httpResponse.Response;
        }

        public async Task<UserToken> RenewToken()
        {
            var response = await _httpService.Get<UserToken>($"{baseURL}/RenewToken");

            if (!response.Success)
                throw new ApplicationException(await response.GetBody());

            return response.Response;
        }
    }
}
