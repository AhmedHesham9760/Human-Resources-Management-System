using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeePartV2.Models;

namespace EmployeePartV2.Models
{
    public class viewmodel
    {
        public viewmodel()
        {
            modperm = new ModPerm();
        }
        public Group group { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<Module> modules { get; set; }
        public List<ModPerm> modperms { get; set; }
        public ModPerm modperm { get; set; }


    }
}