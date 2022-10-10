using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankCusManagement.ViewModal
{
    public class CustAndHisTransactions
    {
        //[Display(Name ="Cust Id")]
        //public int Cust_Id { get; set; }
        [Display(Name = "Customer Code")]
        public string Customer_Code { get; set; }
        [Display(Name = "Full Name")]
        public string Customer_FullName { get; set; }
        [Display(Name = "Type Of Transaction")]
        public string TypeOf_Transaction { get; set; }
        [Display(Name = "Transaction Amount")]
        public Nullable<double> Transaction_Amount { get; set; }
        [Display(Name = "Transaction Date")]
        public Nullable<System.DateTime> Transaction_Date { get; set; }
    }
}