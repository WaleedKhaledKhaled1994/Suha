using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.ViewModels.Order
{
    public class OrderRateVMRequest
    {
        public long OrderId { get; set; }
        public int Rate { get; set; }
    }
}
