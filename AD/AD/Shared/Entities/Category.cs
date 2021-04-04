using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.Entities.MTM;
using AD.Shared.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities
{
    public class Category : AuditableEntity 
    {
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name_en { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name_ar { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name_fr { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name_tr { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name_ru { get; set; }
        public string Description_en { get; set; }
        public string Description_ar { get; set; }
        public string Description_fr { get; set; }
        public string Description_tr { get; set; }
        public string Description_ru { get; set; }
        public string Image { get; set; }
        [Range(0, long.MaxValue, ErrorMessageResourceName = "SelectList", ErrorMessageResourceType = typeof(Resource))]
        public CategoryLevel CategoryLevel { get; set; }
        public Status Status { get; set; } = Status.Online;
        public long? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<CategoryFollowers> CategoryFollowers { get; set; }

        //public virtual ICollection<Product> Products { get; set; }
        //public virtual ICollection<Category> Categories { get; set; }
    }
}
