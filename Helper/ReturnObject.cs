using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public  class ReturnObject<T>
    {
  
        
        public T Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
