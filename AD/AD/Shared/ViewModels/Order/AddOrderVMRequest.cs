using AD.Shared.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.ViewModels.Order
{
    public class AddOrderVMRequest
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public long ProductId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public int? Quantity { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public decimal Total { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [Range(1, long.MaxValue, ErrorMessageResourceName = "SelectList", ErrorMessageResourceType = typeof(Resource))]
        public long StorePaymentMethodId { get; set; }
    }
}
