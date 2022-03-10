using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCode.Models.Requests
{
    public class GetModuleListRequest
    {
        public int ModuleID { get; set; }
        public string Name { get; set; }
    }
}
