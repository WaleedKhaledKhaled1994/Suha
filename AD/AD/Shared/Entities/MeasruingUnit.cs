﻿using AD.Shared.Entities.Base;
using AD.Shared.Entities.Base.Enums;
using AD.Shared.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities
{
    public class MeasruingUnit : AuditableEntity
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
        public string Symbol { get; set; }
        public Status Status { get; set; } = Status.Online;
    }
}
