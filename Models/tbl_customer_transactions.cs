//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BankCusManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_customer_transactions
    {
        public int Transaction_Id { get; set; }
        public int Cust_Id { get; set; }
        public string TypeOf_Transaction { get; set; }
        public Nullable<double> Transaction_Amount { get; set; }
        public Nullable<System.DateTime> Transaction_Date { get; set; }
    
        public virtual tbl_customers tbl_customers { get; set; }
    }
}
