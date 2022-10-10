using BankCusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankCusManagement.ViewModal
{
    public class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            this.tbl_customers = new HashSet<tbl_customers>();
        }

        public int Uid { get; set; }
        [Display(Name ="Login Id")]
        [Required(ErrorMessage ="Login Id is Required")]
        [RegularExpression("[A-za-z0-9]{6,20}", ErrorMessage ="Invalid Login Id")]
        public string Login_Id { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [RegularExpression("[A-Z]{1}[a-z0-9@#$%_-]{7,16}", ErrorMessage = "Invalid Password")]
       
        public string Password { get; set; }
        public string User_Name { get; set; }
        public int Role_ID { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Last_Login { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_customers> tbl_customers { get; set; }
        public virtual tbl_Roles tbl_Roles { get; set; }
    }
}