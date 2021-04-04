using Microsoft.EntityFrameworkCore;
using AD.Server.Repositories;
using AD.Server.Services.Interfaces;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.MTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AD.Server.Services
{
    public class FavouritesServices : IFavouritesServices
    {
        private readonly IRepository<StoreFollowers> _storeFollowersRepository;
        private readonly IRepository<CategoryFollowers> _categoryFollowersRepository;

        public FavouritesServices(IRepository<StoreFollowers> storeFollowersRepository, IRepository<CategoryFollowers> categoryFollowersRepository)
        {
            _storeFollowersRepository = storeFollowersRepository;
            _categoryFollowersRepository = categoryFollowersRepository;
        }
      
        public async Task<ApiResponse> Follow_UnFollow_Store(StoreFollowers input)
        {
            try
            {
                var storeFollowerDB = await _storeFollowersRepository.Table.Where(x => x.UserCId == input.UserCId && x.StoreId == input.StoreId).SingleOrDefaultAsync();
                if (storeFollowerDB != null)
                {
                    storeFollowerDB.Status = input.Status;
                    await _storeFollowersRepository.SaveChangesAsync();
                }
                else
                    await _storeFollowersRepository.InsertAsync(new StoreFollowers { UserCId = input.UserCId, StoreId = input.StoreId, Status = input.Status });
                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }

        }

        public async Task<ApiResponse> Follow_UnFollow_Category(CategoryFollowers input)
        {
            try
            {
                var categoryFollowerDB = await _categoryFollowersRepository.Table.Where(x => x.UserCId == input.UserCId && x.CategoryId == input.CategoryId).SingleOrDefaultAsync();
                if (categoryFollowerDB != null)
                {
                    categoryFollowerDB.Status = input.Status;
                    await _storeFollowersRepository.SaveChangesAsync();
                }
                else
                    await _categoryFollowersRepository.InsertAsync(new CategoryFollowers { UserCId = input.UserCId, CategoryId = input.CategoryId, Status = input.Status });

                return ApiResponse.Create(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                return ApiResponse.Create(HttpStatusCode.InternalServerError, null, "InternalServerError_Error");
            }
        }
    }
}
