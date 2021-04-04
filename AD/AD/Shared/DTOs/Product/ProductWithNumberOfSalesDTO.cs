using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Product
{
    public class ProductWithNumberOfSalesDTO
    {
        public long ProductId { get; set; }
        public int? NumberOfSales { get; set; }
    }
}
