using AD.Shared.DTOs;
using AD.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities
{
    public class Fixer : BaseEntity
    {
        public bool Success { get; set; }
        public int TimeStamp { get; set; }
        public string @Base { get; set; }
        public string Date { get; set; }
    }
}
