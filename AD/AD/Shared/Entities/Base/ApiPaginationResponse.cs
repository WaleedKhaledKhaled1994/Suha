using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities.Base
{
    public class ApiPaginationResponse
    {
        public static ApiPaginationResponse Create(HttpStatusCode statusCode, long page, long totalPages, object result = null, string message = null)
        {
            return new ApiPaginationResponse(statusCode, page, totalPages, result, message);
        }
        [Display(Name = "meta")]
        public ResponseMeta Meta { get; set; }

        [Display(Name = "data")]
        public object Data { get; set; }

        protected ApiPaginationResponse(HttpStatusCode status_number, long page, long totalPages, object result = null, string errorMessage = null)
        {
            Meta = new ResponseMeta
            {
                StatusNumber = (int)status_number,
                Message = errorMessage,
                Page = page,
                TotalPages = totalPages
            };
            Data = result;
        }

        public ApiPaginationResponse()
        {
        }

        public class ResponseMeta
        {
            public bool Status => StatusNumber == 200 || StatusNumber == 201 ? true : false;

            public int StatusNumber { get; set; }

            public string Message { get; set; }

            public double TotalProcessingTime { get; set; }
            public long Page { get; set; }
            public long TotalPages { get; set; }
        }
    }
}
