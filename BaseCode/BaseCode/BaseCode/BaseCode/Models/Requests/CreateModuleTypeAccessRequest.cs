using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCode.Models.Requests
{
    public class CreateModuleTypeAccessRequest
    {
        public int UtmaID { get; set; }
        public int ModuleID { get; set; }
        public int UserTypeID { get; set; }
        public string Status { get; set; }
    }
}
