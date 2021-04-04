using Microsoft.AspNetCore.Identity;
using AD.Server.Models;
using AD.Shared.DTOs;
using AD.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IUserServices
    {
        string GetUserId();

        bool IsInRole(string role);

        bool IsSameUser(string userId);
    }
}
