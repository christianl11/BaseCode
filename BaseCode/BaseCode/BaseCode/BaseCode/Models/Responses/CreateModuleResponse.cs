using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCode.Models.Responses
{
    public class CreateModuleResponse
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public int Moduleid { get; set; }
    }
}
