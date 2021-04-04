using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Product
{
    public class ProductWithBasePriceDTO
    {
        public Entities.Product Product { get; set; }

        [Column(TypeName = "decimal(18, 5)")]
        public decimal Price { get; set; }
    }
}
