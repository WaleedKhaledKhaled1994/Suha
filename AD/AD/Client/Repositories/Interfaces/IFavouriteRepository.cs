using AD.Shared.Entities.MTM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Repositories.Interfaces
{
    public interface IFavouriteRepository
    {
        Task Follow_UnFollow_Store(StoreFollowers storeFollowers);
        Task Follow_UnFollow_Category(CategoryFollowers entity);
    }
}
