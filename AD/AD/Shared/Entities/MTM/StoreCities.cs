using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities.MTM
{
    public class StoreCities : AuditableEntity
    {
        public long StoreId { get; set; }
        public virtual Store Store { get; set; }
        public long CityId { get; set; }
        public virtual City City { get; set; }
    }
}
