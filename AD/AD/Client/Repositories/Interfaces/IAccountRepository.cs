using AD.Shared.DTOs;
using AD.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserToken> Login(LoginDTO loginDTO);
        Task<UserToken> Register(RegisterDTO registerDTO);
        Task<UserToken> RenewToken();

    }
}
