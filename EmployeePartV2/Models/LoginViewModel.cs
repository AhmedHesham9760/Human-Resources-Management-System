using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeePartV2.Models
{
    public class LoginViewModel
    {
        public int GroupId { get; set; }
        public List<int> screenIds { get; set; }
    }
}