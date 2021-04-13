using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AspNetCoreLogging.Middleware
{
    public class ApiError
    {
        public string Id { get; set; }
        public HttpStatusCode Status { get; set; }
        public int Code { get; set; }
        public string Links { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}
