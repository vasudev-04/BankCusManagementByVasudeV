using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace BankCusManagement.ViewModal
{
    public class ResetPassword
    {
        //[Remote("ResetPwd", "Customer" ,"Login Id didn't found try again with correct details")]
        [Display(Name ="Login Id")]
        public string LoginId { get; set; }
        [Required(ErrorMessage ="Password is Required")]
        [Display(Name = "Current Password")]
        public string currentPassword { get; set; }
        //[Required]
        //public string token { get; set; }
        [Required(ErrorMessage = "New Password is Required")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "confirm New Password is Required")]
        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "New Password and confirmNew Password Should be Same!!!!!")]
        public string confirmNewPassword { get; set; }
    }
}