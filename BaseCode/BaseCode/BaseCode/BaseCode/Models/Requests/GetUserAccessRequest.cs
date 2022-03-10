using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCode.Models.Requests
{
    public class GetUserAccessRequest
    {
        public string UserName { get; set; }
        public string UserTypeDesc { get; set; }
        public string Modulename { get; set; }
    }
}
