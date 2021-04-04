using AD.Client.Helpers;
using AD.Client.Repositories.Interfaces;
using AD.Shared.Entities.MTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/favourites";
        public FavouriteRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task Follow_UnFollow_Store(StoreFollowers entity)
        {
            var response = await _httpService.Post<StoreFollowers>($"{url}/Follow_UnFollow_Store", entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
        public async Task Follow_UnFollow_Category(CategoryFollowers entity)
        {
            var response = await _httpService.Post<CategoryFollowers>($"{url}/Follow_UnFollow_Category", entity);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
