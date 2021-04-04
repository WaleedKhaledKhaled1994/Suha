using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using AD.Shared.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities
{
    public class Order : AuditableEntity
    {
        public long UserCId { get; set; }
        public virtual UserC UserC { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public long ProductId { get; set; }
        public virtual Product Product { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public int? Quantity { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [Column(TypeName = "decimal(18, 5)")]
        public decimal Total { get; set; }
        public long StorePaymentMethodId { get; set; }
        public virtual StorePaymentMethods StorePaymentMethod { get; set; }
        public DateTime ShippingDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public int Rate { get; set; }
    }
}
