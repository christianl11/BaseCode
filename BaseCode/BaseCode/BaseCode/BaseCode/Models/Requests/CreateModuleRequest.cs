using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCode.Models.Requests
{
    public class CreateModuleRequest
    {
        public int ModuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

    }
}
