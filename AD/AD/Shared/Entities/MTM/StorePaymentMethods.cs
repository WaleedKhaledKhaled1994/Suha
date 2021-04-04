using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities.MTM
{
    public class StorePaymentMethods : AuditableEntity
    {
        public long StoreId { get; set; }
        public virtual Store Store { get; set; }
        public long PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public string Details { get; set; }
    }
}
