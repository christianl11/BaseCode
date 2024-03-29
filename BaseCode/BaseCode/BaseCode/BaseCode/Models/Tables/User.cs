﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCode.Models.Tables
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string password { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string EmailAddress { get; set; }
        public string UserTypeId { get; set; }
        public string UserTypeDesc { get; set; }
    }
}
