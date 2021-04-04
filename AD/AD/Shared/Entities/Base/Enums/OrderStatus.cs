using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities.Base.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        AwaitingPayment = 1,
        Processing = 2,
        InShipping = 3,
        Received = 4,
        Rejected =5
    }
}
