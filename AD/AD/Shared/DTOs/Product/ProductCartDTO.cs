using AD.Shared.Entities.MTM;
using AD.Shared.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Product
{
    public class ProductCartDTO
    {
        public Entities.Product Product { get; set; }
        public string Color { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public int? Quantity { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal Total { get; set; }
        public string CustomerNote { get; set; }
        public StorePaymentMethods StorePaymentMethod { get; set; }
    }
}
