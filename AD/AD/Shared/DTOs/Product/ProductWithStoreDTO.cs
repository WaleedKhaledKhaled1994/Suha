using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Product
{
    public class ProductWithStoreDTO
    {
        public Entities.Product Product { get; set; }
        public Entities.Store Store { get; set; }
    }
}
