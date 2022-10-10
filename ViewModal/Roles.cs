using BankCusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankCusManagement.ViewModal
{
    public class Roles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Roles()
        {
            this.tbl_role_menu = new HashSet<tbl_role_menu>();
            this.tbl_users = new HashSet<tbl_users>();
        }

        public int RoleI_d { get; set; }
        public string Role_Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_role_menu> tbl_role_menu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_users> tbl_users { get; set; }
    }
}