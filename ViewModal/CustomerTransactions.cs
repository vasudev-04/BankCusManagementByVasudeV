using BankCusManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankCusManagement.ViewModal
{
    public class CustomerTransactions
    {
        public int Transaction_Id { get; set; }
        public int Cust_Id { get; set; }
        public string TypeOf_Transaction { get; set; }
        public Nullable<double> Transaction_Amount { get; set; }
        public Nullable<System.DateTime> Transaction_Date { get; set; }

        public virtual tbl_customers tbl_customers { get; set; }
    }
}