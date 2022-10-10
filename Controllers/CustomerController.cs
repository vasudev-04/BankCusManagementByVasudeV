using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Security;
using BankCusManagement.Models;
using BankCusManagement.ViewModal;


namespace BankCusManagement.Controllers
{
    public class CustomerController : Controller
    {
       
       // GET: Customer
       BankCusManagementEntity db = new BankCusManagementEntity();
        public ActionResult Index()
        {
            if (Session["LoginId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult MyProfile()
        {
            var Uid = Convert.ToInt32(Session["Uid"]);
            Customers CustProfile =  db.tbl_customers.Where(z => z.Uid == Uid).Select(x => new Customers(){ 
                                                                                           Customer_Code = x.Customer_Code,
                                                                                           Customer_FirstName = x.Customer_FirstName,
                                                                                           Customer_LastName = x.Customer_LastName,
                                                                                           Customer_FullName = x.Customer_FirstName + x.Customer_LastName,
                                                                                           Customer_City = x.Customer_City
                                                                                           }).FirstOrDefault();

            if (CustProfile == null)
            {
                TempData["Error"] = "No Details Found";
                return View();
            }
        return View(CustProfile);
        }

        public ActionResult ResetPwd()
        {
           

            return View();
        }
       


        [HttpPost]
        public ActionResult ResetPwd(ResetPassword chgPwd)
        {
            var loginId = Session["LoginId"];
            var encryptPass = "";
            var encodePwd = (string)chgPwd.confirmNewPassword;
            byte[] pwdEncode = new byte[encodePwd.Length];
            pwdEncode = Encoding.UTF8.GetBytes(encodePwd);
            encryptPass = Convert.ToBase64String(pwdEncode);



            var changePasswordUser = db.tbl_users.Where(c => c.Login_Id == loginId && c.Password == encryptPass).FirstOrDefault();

            if (changePasswordUser == null)
            {
                TempData["Error"] = "No Matching Details Were Found Try Again With Correct Details";
                return View();
                //ModelState.AddModelError(string.Empty, "No Matching Details Were Found Try Again With Correct Details");
            }

            else
            {
                changePasswordUser.Password = chgPwd.confirmNewPassword;
                db.SaveChanges();
                ModelState.Clear();
                TempData["success"] = "PASSWORD UPDATED SUCCESSFULLY";
                //ModelState.AddModelError(String.Empty, "PASSWORD UPDATED SUCCESSFULLY");
            }
                
               
                return View();
            
           

        }


        public ActionResult MyTransByDate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MyTransByDate(CustAndHisTransactions obj)
        {
            var custCode = (string)Session["Ccode"];
            if (obj.Transaction_Date.Value.Year >= DateTime.Now.Year && obj.Transaction_Date.Value.Month >= DateTime.Now.Month && obj.Transaction_Date.Value.Day > DateTime.Now.Day)
            {
                TempData["Error"] = "Entered Date should not greater than Today Date"; 
                //ViewBag.message = "Entered Date should not greater than Today's Date";
                //ModelState.AddModelError(string.Empty, "Entered Date should not greater than Today's Date");
                return View();
            }

            var dateOfTrans = db.tbl_customer_transactions.Where(c => c.Transaction_Date.Value.Year == obj.Transaction_Date.Value.Year && c.Transaction_Date.Value.Month == obj.Transaction_Date.Value.Month && c.Transaction_Date.Value.Day == obj.Transaction_Date.Value.Day).Select(p => p.Transaction_Date).FirstOrDefault();
            if (dateOfTrans == null)
            {
                TempData["Error"] = "No Details were found with the given date";
                //ViewBag.message = "No Details were found with the given date";
                return View();
            }

            var customerTransactions = db.tbl_customers.Join(db.tbl_customer_transactions, customer => customer.Cust_Id, transactions => transactions.Cust_Id, (customer, transactions) => new CustAndHisTransactions()
            {
                Customer_Code = customer.Customer_Code,
                //Customer_FullName = customer.Customer_FirstName + " " + customer.Customer_LastName,
                TypeOf_Transaction = transactions.TypeOf_Transaction,
                Transaction_Amount = transactions.Transaction_Amount,
                Transaction_Date = transactions.Transaction_Date
            }).Where(c => c.Transaction_Date.Value.Year == obj.Transaction_Date.Value.Year && c.Transaction_Date.Value.Month == obj.Transaction_Date.Value.Month && c.Transaction_Date.Value.Day == obj.Transaction_Date.Value.Day && c.Customer_Code == custCode);

            return View("MyTransViewByDate", customerTransactions);
            
        }
        public ActionResult MyTransViewByDate()
        {
            return View();
        }

      
        public ActionResult MyAccountBal()
        {
            var custCode = (string)Session["Ccode"];

            tbl_customers amount = db.tbl_customers.Where(z => z.Customer_Code == custCode).FirstOrDefault();
            return View(amount);
        }


        //-----------------------------------------------------------------------------------------------------

      

        public ActionResult DepositCustDetails()
        {
            var cusCode = (string)Session["Ccode"];
            var cust = db.tbl_customers.Where(x => x.Customer_Code == cusCode).FirstOrDefault();
           // return View("DepositCustDetails", cust);
           return View(cust);
        }

        public ActionResult DepositIntoCustAcc(double? Deposit)
        {
            if (Deposit < 0)
            {
                TempData["Error"] = "Amount should greater than 0";
                return RedirectToAction("DepositCustDetails");
            }
            var uid = Convert.ToInt32(Session["Uid"]);
            var CustUser = db.tbl_customers.Where(x =>x.Uid== uid).FirstOrDefault();
            
            double? amount = (double)CustUser.Account_BalanceAmount;
            
            if (CustUser != null)
            {
                amount += Deposit;
                CustUser.Account_BalanceAmount = amount;
                CustUser.Last_TransactionDate = DateTime.Now;
                tbl_customer_transactions DepositTransaction = new tbl_customer_transactions()
                {
                    Cust_Id = CustUser.Cust_Id,
                    TypeOf_Transaction = "D",
                    Transaction_Amount = Deposit,
                    Transaction_Date = DateTime.Now
                };


                TempData["message"] = "Deposited Successfully";
                db.tbl_customer_transactions.Add(DepositTransaction);
                db.SaveChanges();
                return RedirectToAction("DepositCustDetails");
                //return RedirectToAction("DepositCustDetails", "Staff");
            }

            return RedirectToAction("DepositCustDetails");
        }


        //-----------------------------------------------

        

        public ActionResult WithdrawlCustDetails()
        {
            var cusCode = (string)Session["Ccode"];
            var CustDetails = db.tbl_customers.Where(x => x.Customer_Code == cusCode).FirstOrDefault();
            if (CustDetails == null)
            {
                TempData["Error"] = "No matching details were found";
                return View();
            }
           return View("WithdrawlCustDetails", CustDetails);
            //return View();
        }
        [HttpPost]
        public ActionResult WithdrawlCustDetails(int? amt)
        {
            if (amt == null)
            {
                TempData["Error"] = "Invalid Attempt";
                return View();
            }
            var uId = (int)Session["Uid"];
            var amount = db.tbl_customers.Where(c => c.tbl_users.Uid == uId).Select(p => p.Account_BalanceAmount).FirstOrDefault();

            tbl_customers res = new tbl_customers();
            res = db.tbl_customers.Where(c => c.Uid == uId).SingleOrDefault();

            if (amt > amount)
            {
                TempData["Error"] = "Insufficient Balance";
                return View(res);
            }
            if (amt == amount)
            {
                TempData["Error"] = "Entire Amount cannot be withdrawl";
                return RedirectToAction("WithdrawlCustDetails", "Customer");
            }
          

            res.Account_BalanceAmount = res.Account_BalanceAmount-amt;
            res.Last_TransactionDate = DateTime.Now;
           
           

            db.SaveChanges();

            tbl_customer_transactions WithdrawlTransaction = new tbl_customer_transactions()
            {
                Cust_Id = res.Cust_Id,
                TypeOf_Transaction = "W",
                Transaction_Amount = amt,
                Transaction_Date = DateTime.Now
            };

            
            db.tbl_customer_transactions.Add(WithdrawlTransaction);
            db.SaveChanges();

            ModelState.Clear();

            TempData["Success"] = "WithDraw successfull";
            return View(res);
        }



    }
}