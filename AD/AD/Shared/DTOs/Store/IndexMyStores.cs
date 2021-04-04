using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Store
{
    public class IndexMyStores
    {
        public Entities.Store Store { get; set; }
        public bool Admin { get; set; } = false;
        public bool ManageProducts { get; set; } = false;
        public bool ManageOrders { get; set; } = false;
        public bool ManageComments { get; set; } = false;
    }
}
