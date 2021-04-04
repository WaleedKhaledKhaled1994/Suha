using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.ViewModels.Favourite
{
    public class FollowVMRequest
    {
        [Required]
        public long UserCId { get; set; }
        [Required]
        public long EntityId { get; set; }
    }
}
