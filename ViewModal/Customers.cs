using BankCusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankCusManagement.ViewModal
{
    public class Customers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customers()
        {
            this.tbl_customer_transactions = new HashSet<tbl_customer_transactions>();
        }

        public int Cust_Id { get; set; }
        public int Uid { get; set; }
        [Display(Name = "First Name")]
        public string Customer_FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string Customer_LastName { get; set; }
        [Display(Name = "Full Name")]
        public string Customer_FullName { get; set; }
        [Display(Name = "City")]
        public string Customer_City { get; set; }
        public string Account_Type { get; set; }
        public Nullable<double> Account_BalanceAmount { get; set; }
        public Nullable<System.DateTime> Last_TransactionDate { get; set; }
        [Display(Name = "Customer Code")]
        public string Customer_Code { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_customer_transactions> tbl_customer_transactions { get; set; }
        public virtual tbl_users tbl_users { get; set; }
    }
}