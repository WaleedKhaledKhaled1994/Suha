using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities
{
    public class Notification : BaseEntity
    {
        public DateTime NotificationTime { get; set; } = DateTime.UtcNow;
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationType Type { get; set; }
        public long UserCId { get; set; }
        public virtual UserC UserC { get; set; }
        public bool IsRead { get; set; } = false;
        public string Link { get; set; }
    }
}
