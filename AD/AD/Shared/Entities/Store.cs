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
    public class Store : AuditableEntity
    {
        public long userCId { get; set; }
        public UserC UserC { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public long CityId { get; set; }
        public City City { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [Range(1, long.MaxValue, ErrorMessageResourceName = "SelectList", ErrorMessageResourceType = typeof(Resource))]
        public long CurrencyId { get; set; }
        public string Code { get; set; }
        public virtual Currency Currency { get; set; }
        public string Image { get; set; }
        public Status Status { get; set; } = Status.Online;
        public double AverageRate { get; set; }
        public virtual ICollection<StoreTags> StoreTags { get; set; }
        public virtual ICollection<StoreCountries> StoreCountries { get; set; }
        public virtual ICollection<StoreCities> StoreCities { get; set; }
        public virtual ICollection<StoreUsers> StoreUsers { get; set; }
        public virtual ICollection<StoreFollowers> StoreFollowers { get; set; }
        public virtual ICollection<StorePaymentMethods> StorePaymentMethods { get; set; }
    }
}
