using AD.Shared.Entities.Base.Enums;
using AD.Shared.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.ViewModels.Order
{
    public class EditOrderVMRequest
    {
        public long Id { get; set; }
        public int ShippingDate { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public AddNotificationVMRequest AddNotificationVM { get; set; }
    }
}
