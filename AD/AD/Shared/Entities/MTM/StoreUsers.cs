using AD.Shared.DTOs;
using AD.Shared.DTOs.Auth;
using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities.MTM
{
    public class StoreUsers : AuditableEntity
    {
        public long StoreId { get; set; }
        public virtual Store Store { get; set; }
        public long UserCId { get; set; }
        public virtual UserC User { get; set; }
        public StoreUserStatus Status { get; set; } = StoreUserStatus.Pending;
        public string Role { get; set; } = "[]";
    }
}
