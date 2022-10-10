using BankCusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankCusManagement.ViewModal
{
    public class RoleMenu
    {
        public int Menu_Id { get; set; }
        public string Menu_Name { get; set; }
        public int Role_Id { get; set; }

        public virtual tbl_Roles tbl_Roles { get; set; }
    }
}