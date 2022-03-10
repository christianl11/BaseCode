using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseCode.Models.Tables;

namespace BaseCode.Models.Responses
{
    public class GetUserAccessResponse
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public List<UserAccessModule> UserAccess { get; set; }
    }
}
