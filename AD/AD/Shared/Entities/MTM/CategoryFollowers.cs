using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities.MTM
{
    public class CategoryFollowers : AuditableEntity
    {
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public long UserCId { get; set; }
        public virtual UserC UserC { get; set; }
        public FollowStatus Status { get; set; } = FollowStatus.UnFollow;
    }
}
