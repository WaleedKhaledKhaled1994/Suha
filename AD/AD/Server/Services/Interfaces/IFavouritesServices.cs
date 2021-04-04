using AD.Shared.DTOs;
using AD.Shared.Entities;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.MTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Server.Services.Interfaces
{
    public interface IFavouritesServices
    {
        Task<ApiResponse> Follow_UnFollow_Store(StoreFollowers entity);
        Task<ApiResponse> Follow_UnFollow_Category(CategoryFollowers entity);
    }
}
