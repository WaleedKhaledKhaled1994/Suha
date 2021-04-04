using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities.MTM
{
    public class StoreTags : AuditableEntity
    {
        public long StoreId { get; set; }
        public virtual Store Store { get; set; }
        public long TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
