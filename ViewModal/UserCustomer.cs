using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankCusManagement.ViewModal
{
    public class UserCustomer
    {
        [Required(ErrorMessage = "First Name is Required")]
        public string Customer_FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        public string Customer_LastName { get; set; }
        [Required(ErrorMessage = "City is Required")]
        public string Customer_City { get; set; }
        [Required(ErrorMessage = "Account Type is Required")]
        public string Account_Type { get; set; }
        public Nullable<double> Account_BalanceAmount { get; set; }
        [Display(Name = "Login Id")]
        [Required(ErrorMessage ="login Id is Required")]
        [RegularExpression("[A-za-z0-9]{6,20}", ErrorMessage = "Entered User Id must be in the range of 6 to 20 Alpha - numeric characters")]
        public string Login_Id { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [RegularExpression("[A-Z]{1}[a-z0-9@#$%_-]{7,16}", ErrorMessage = "Entered Password must be in the range of 7 to 16 Alpha-numeric characters and must Contain atleast One Special Character")]
       
        public string Password { get; set; }
        [Required(ErrorMessage = "User Name is Required")]
        public string User_Name { get; set; }
    }
}