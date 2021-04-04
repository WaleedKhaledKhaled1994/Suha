using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Product
{
    public class ProductDetailsUserDTO
    {
        public ProductDetailsDTO ProductDetailsDTO { get; set; }
        public Entities.Currency UserCurrency { get; set; }

        [Column(TypeName = "decimal(18, 5)")]
        public decimal PriceUser { get; set; }

        [Column(TypeName = "decimal(18, 5)")]
        public decimal PriceUser1 { get; set; }

        [Column(TypeName = "decimal(18, 5)")]
        public decimal PriceUser2 { get; set; }

        [Column(TypeName = "decimal(18, 5)")]
        public decimal PriceUser3 { get; set; }
    }
}
