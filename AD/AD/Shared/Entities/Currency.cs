using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ResourceFiles;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AD.Shared.Entities
{
    public class Currency : AuditableEntity
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Code { get; set; }
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name_en { get; set; }
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name_ar { get; set; }
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name_fr { get; set; }
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name_tr { get; set; }
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public string Name_ru { get; set; }
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        public double Rate { get; set; }
        public string Symbol { get; set; }
        public Status Status { get; set; } = Status.Online;

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resource))]
        //[Column(TypeName = "decimal(18, 5)")]
        //public decimal Worth { get; set; }
    }
}
