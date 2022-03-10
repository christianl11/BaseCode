using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCode.Models.Requests
{
    public class GetUserTypeListRequest
    {
        public int UserTypeId { get; set; }
        public string UserTypeDesc { get; set; }
    }
}
