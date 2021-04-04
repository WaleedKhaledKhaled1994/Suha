using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AD.Shared.Entities.Base
{
    public class ApiResponse
    {
        public static ApiResponse Create(HttpStatusCode statusCode, object result = null, string message = null)
        {
            return new ApiResponse(statusCode, result, message);
        }
        [Display(Name = "meta")]
        public ResponseMeta Meta { get; set; }

        [Display(Name = "data")]
        public object Data { get; set; }

        protected ApiResponse(HttpStatusCode status_number, object result = null, string errorMessage = null)
        {
            Meta = new ResponseMeta
            {
                StatusNumber = (int)status_number,
                Message = errorMessage
            };
            Data = result;
        }

        public ApiResponse()
        {
        }

        public class ResponseMeta
        {
            public bool Status => StatusNumber == 200 || StatusNumber == 201 ? true : false;

            public int StatusNumber { get; set; }

            public string Message { get; set; }

            public double TotalProcessingTime { get; set; }
        }
    }
}
