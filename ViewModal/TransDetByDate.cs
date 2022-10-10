using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BankCusManagement.ViewModal
{
    public class TransDetByDate
    {
        [Display(Name ="Total Deposited Amount")]
        public Nullable<double> TDeposit { get; set; }
        [Display(Name = "Total Withdrawl Amount")]
        public Nullable<double> TWithdrawl { get; set; }
        [Display(Name = "Total Customer Count")]
        public Nullable<int> Tcount { get; set; }
        [Required(ErrorMessage ="Transaction Date is Required")]
        [Display(Name = "Transaction Date")]
        [DataType(DataType.DateTime)]
        public Nullable<DateTime> TransactionDate { get; set; }


    }
}