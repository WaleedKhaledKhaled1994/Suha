using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.ViewModels.Home
{
    public class HomePageVMRequest
    {
        //Filter
        public string Product { get; set; }
        public string Brand { get; set; }
        public long StoreId { get; set; }
        public long CategoryId { get; set; }
        public long CountryId { get; set; }
        public long CityId { get; set; }
        public int StoreRate { get; set; } = 0;

        //Sort
        public SortType SortType { get; set; } = SortType.ProductRelease;
        
        //Sort Type: 1
        public DateTime? ProductRelease { get; set; } = DateTime.UtcNow;
        public OrderByDate OrderByDate { get; set; } = OrderByDate.Desc;
        
        //Sort Type:2 Best Seller
    }
}
