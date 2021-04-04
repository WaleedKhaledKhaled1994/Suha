using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities.MTM
{
    public class FixerCurrencies : BaseEntity
    {
        public long FixerId { get; set; }
        public virtual Fixer Fixer { get; set; }
        public long CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }
        public double Rate { get; set; }
    }
}
