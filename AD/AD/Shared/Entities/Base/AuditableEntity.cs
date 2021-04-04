using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities.Base
{
    public abstract partial class AuditableEntity : BaseEntity, IBaseEntity
    {
        [StringLength(450)]
        public string CreatedById { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        [StringLength(450)]
        public string LastModifiedById { get; set; }

        public DateTime? LastModifiedOnUtc { get; set; }
    }
}
