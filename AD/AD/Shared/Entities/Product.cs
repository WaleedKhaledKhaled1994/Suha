using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities
{
    public class Product : AuditableEntity
    {
        public string Code { get; set; }

        public string CodeStore { get; set; }

        public string Brand { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [Range(1, long.MaxValue, ErrorMessageResourceName = "SelectList", ErrorMessageResourceType = typeof(Resource))]
        public long MeasruingUnitId { get; set; }
        public virtual MeasruingUnit MeasruingUnit { get; set; }
        public int? MinValue { get; set; }
        public string Colors { get; set; } = "[]";

        public string Image { get; set; } = "[]";

        [Column(TypeName = "decimal(18, 5)")]
        public decimal Price { get; set; }

        public int? Quantity1 { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal Price1 { get; set; }

        public int? Quantity2 { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal Price2 { get; set; }

        public int? Quantity3 { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal Price3 { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [Range(1, long.MaxValue, ErrorMessageResourceName = "SelectList", ErrorMessageResourceType = typeof(Resource))]
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [Range(1, long.MaxValue, ErrorMessageResourceName = "SelectList", ErrorMessageResourceType = typeof(Resource))]
        public long CurrencyId { get; set; } = 9;
        public virtual Currency Currency { get; set; }

        public string Note { get; set; }

        public Status Status { get; set; } = Status.Online;

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        [Range(1, long.MaxValue, ErrorMessageResourceName = "SelectList", ErrorMessageResourceType = typeof(Resource))]
        public long StoreId { get; set; }
        public virtual Store Store { get; set; }
    }
}
