using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCode.Models.Requests
{
    public class CreateUserTypeRequest
    {
        public int UserTypeId { get; set; }
        public string UserTypeDesc { get; set; }
        public string Status { get; set; }
    }
}
