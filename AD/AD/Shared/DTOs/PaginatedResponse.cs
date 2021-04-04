using System;
using System.Collections.Generic;
using System.Text;

namespace AD.Shared.DTOs
{
    public class PaginatedResponse<T>
    {
        public T Response { get; set; }
        public int TotalAmountPages { get; set; }
        public long TotalAmount { get; set; }
    }
}
