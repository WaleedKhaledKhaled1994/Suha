using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs.Store
{
    public class IndexStoresUser
    {
        public Entities.Store Store { get; set; }
        public FollowStatus Status { get; set; }
    }
}
