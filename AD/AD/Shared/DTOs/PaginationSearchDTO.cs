using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.DTOs
{
    public class PaginationSearchDTO : PaginationDTO
    {
        public string SearchText { get; set; } = "";
    }
}
