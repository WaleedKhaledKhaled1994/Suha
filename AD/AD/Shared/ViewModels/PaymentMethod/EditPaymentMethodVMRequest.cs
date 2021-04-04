using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.ViewModels.PaymentMethod
{
    public class EditPaymentMethodVMRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
