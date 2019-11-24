using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightManagment___WebApi___Part_3
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}