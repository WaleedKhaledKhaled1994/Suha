using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities
{
    public class PaymentMethod : AuditableEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
