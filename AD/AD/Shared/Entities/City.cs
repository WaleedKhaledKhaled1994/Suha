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
    public class City :AuditableEntity
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
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public long CountryId { get; set; }
        public virtual Country Country { get; set; }
        public Status Status { get; set; } = Status.Online;
        //public virtual ICollection<Store> Stores { get; set; }
        //public virtual ICollection<StoreCities> StoreCountries { get; set; }
    }
}
