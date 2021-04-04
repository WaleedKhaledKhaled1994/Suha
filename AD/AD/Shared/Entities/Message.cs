using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities
{
    public class Message : BaseEntity
    {
        public long OrderId { get; set; }
        public virtual Order Order { get; set; }
        public long SenderId { get; set; }
        public virtual UserC Sender { get; set; }
        public string Text { get; set; }
        public MessageType MessageType { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}
