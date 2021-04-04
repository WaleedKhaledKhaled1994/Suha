using AD.Shared.DTOs;
using AD.Shared.Entities.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.ViewModels.Order
{
    public class IndexOrderVMRequest 
    {
        public OrderStatus? OrderStatus { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public string SearchText { get; set; } = "";
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 10;
        public PaginationSearchDTO Pagination
        {
            get { return new PaginationSearchDTO() { Page = Page, RecordsPerPage = RecordsPerPage, SearchText = SearchText }; }
        }
        //public string Title { get; set; }
        //public int GenreId { get; set; }
        //public bool InTheaters { get; set; }
        //public bool UpcomingReleases { get; set; }

    }
}
