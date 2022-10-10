using BankCusManagement.Models;
using BankCusManagement.ViewModal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankCusManagement.Controllers
{
    public class StaffController : Controller
    {
        BankCusManagementEntity db = new BankCusManagementEntity();
        // GET: Staff
        
        public ActionResult Index()
        {
            if (Session["UserKey"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            
            return View();
        }


        public ActionResult Log()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult CreateCustomer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCustomer(UserCustomer obj)
        {
            var msg = "";
            var password = (string)obj.Password;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);

            tbl_users users = new tbl_users()
            {

                User_Name = obj.User_Name,
                Login_Id = obj.Login_Id,
                Password = msg /*obj.Password*/,
                Status = "Active",
                Role_ID = 2


            };
            db.tbl_users.Add(users);
            db.SaveChanges();


            tbl_customers cust = new tbl_customers() {
                Customer_FirstName = obj.Customer_FirstName,
                Customer_LastName = obj.Customer_LastName,
                Customer_City = obj.Customer_City,
                Account_Type = obj.Account_Type,
                Account_BalanceAmount = 5000,
                Uid = users.Uid,
                 Last_TransactionDate = DateTime.Now



            };
            db.tbl_customers.Add(cust);
            db.SaveChanges();


            tbl_customer_transactions custTransac = new tbl_customer_transactions() { 
            
              Cust_Id = cust.Cust_Id,
               Transaction_Amount = cust.Account_BalanceAmount,
                Transaction_Date = DateTime.Now,
                 TypeOf_Transaction = "D"
            };

            db.tbl_customer_transactions.Add(custTransac);
            db.SaveChanges();

            ModelState.Clear();

            TempData["Success"] = "Details Saved Successfully";
            return View();
        }

        public ActionResult ViewAllCustomers()
        {
            var custList = db.tbl_customers.ToList();
            if (custList.Count == 0)
            {
                TempData["Error"] = "No Details were found";
                //ModelState.AddModelError(string.Empty, "No Customers List were found to show");
                return View();
            }
            else
            {
                return View(custList);
            }

        }

        public ActionResult Edit(string custCode)
        {
            var cust = db.tbl_customers.Where(x => x.Customer_Code == custCode).FirstOrDefault();
             Session["cCode"] = cust.Customer_Code;

            return View(cust);
        }
        [HttpPost]
        public ActionResult Edit(tbl_customers obj)
        {
            var cCode = (string)Session["cCode"];
            
            var cust = db.tbl_customers.Where(x => x.Customer_Code == cCode).FirstOrDefault();
            if (cust.Account_BalanceAmount > 0 && obj.tbl_users.Status.ToString().ToLower() == "inactive")
            {
                TempData["Error"] = "Account having Enough Balance So, Unable to InActive the Account";
                return View();
            }
            cust.Customer_FirstName = obj.Customer_FirstName;
            cust.Customer_LastName = obj.Customer_LastName;
            cust.Account_Type = obj.Account_Type;
            cust.Customer_City = obj.Customer_City;
            cust.tbl_users.Status = obj.tbl_users.Status;

            db.SaveChanges();

            TempData["Success"] = "Details Updated Successfully";
            ModelState.Clear();
            return View();
            //return RedirectToAction("ViewAllCustomers");
        }
        public ActionResult DeleteCustomer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteCustomer(tbl_customers cust)
        {
            var custInCustomer = db.tbl_customers.Where(x => x.Customer_Code == cust.Customer_Code).FirstOrDefault();

            if (custInCustomer != null)
            {

                var custInUser = db.tbl_users.Where(x => x.Uid == custInCustomer.Uid).FirstOrDefault();
                var custInTransaction = db.tbl_customer_transactions.Where(x => x.Cust_Id == custInCustomer.Cust_Id).FirstOrDefault();
                if (custInCustomer != null && custInCustomer.Account_BalanceAmount == 0 && custInUser.Status != "Active")
                {

                    return View("DeleteCustomerView", custInCustomer);
                    


                }

                else if (custInCustomer != null && custInCustomer.Account_BalanceAmount > 0 && custInUser.Status == "Active")
                {
                    Session["Successful"] = "Inactive";
                    ModelState.AddModelError(string.Empty, "User having Enough balance in his A/C and he is in ACTIVE state so unable to delete the customer");

                }

                else
                {
                    TempData["Error"] = "N";
                    ModelState.AddModelError("", "ACTIVE state Users can't be Deleted");
                    return View();
                }
            }


            else
            {
                TempData["Error"] = "No Details Found to Delete Customer";
                return View();
                //ModelState.AddModelError(string.Empty, "No Details Found to Delete Customer");
            }
            return View();
        }

        public ActionResult DeleteCustomerView()
        {
            return View();
        }


        public ActionResult DelInactiveCust(string cust)
        {
            var custInCustomer = db.tbl_customers.Where(x => x.Customer_Code == cust).FirstOrDefault();

            var custInTransaction = db.tbl_customer_transactions.Where(x => x.Cust_Id == custInCustomer.Cust_Id).FirstOrDefault();
            db.tbl_customer_transactions.Remove(custInTransaction);
            db.SaveChanges();

           
            db.tbl_customers.Remove(custInCustomer);
            db.SaveChanges();

            var custInUser = db.tbl_users.Where(x => x.Uid == custInCustomer.Uid).FirstOrDefault();

            db.tbl_users.Remove(custInUser);
            db.SaveChanges();

            

            Session["Successful"] = "Successful";
            ModelState.AddModelError(string.Empty, "User Deleted Successfully");

            return RedirectToAction("ViewAllCustomers");
        }
       
        public ActionResult TransReportByCusCode()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TransReportByCusCode(string CustCode)
        {

            var res = CustCode;
            var cust = db.tbl_customers.Where(x => x.Customer_Code == res).FirstOrDefault();

            if (res != null && cust != null)
            {
               var custTransac =  cust.tbl_customer_transactions.Where(x => x.Cust_Id == cust.Cust_Id).ToList();
                if (custTransac == null)
                {

                    TempData["Nodata"] = "No Transactions found";
                    return View();
                }

                var customerTransactions = db.tbl_customers.Join(db.tbl_customer_transactions, customer => customer.Cust_Id, transactions => transactions.Cust_Id, (customer, transactions) => new CustAndHisTransactions() {
                    Customer_Code = customer.Customer_Code,
                    Customer_FullName = customer.Customer_FirstName + " " + customer.Customer_LastName,
                    TypeOf_Transaction = transactions.TypeOf_Transaction,
                    Transaction_Amount = transactions.Transaction_Amount,
                    Transaction_Date = transactions.Transaction_Date }).Where(c => c.Customer_Code == res);

                return View(customerTransactions);


            }

            else if (res == "")
            {
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return View();
            }

            else if (cust == null)
            {
                TempData["Error"] = "No Details Found with the Given Customer Code";
               // ModelState.AddModelError(string.Empty, "No Customer found with the Given Customer Code");
                return View();
            }
            return View();
        }

        public ActionResult TransReportByDate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransReportByDate(DateTime TransactionDate)
        {
            if (TransactionDate.Year >= DateTime.Now.Year && TransactionDate.Month >= DateTime.Now.Month && TransactionDate.Day > DateTime.Now.Day)
            {
                TempData["Error"] = "Entered Date should not greater than Today Date";
                //ModelState.AddModelError(string.Empty, "Entered Date should not greater than Today's Date");
                return View();
            }

            var dateOfTrans = db.tbl_customer_transactions.Where(c => c.Transaction_Date.Value.Year == TransactionDate.Year && c.Transaction_Date.Value.Month == TransactionDate.Month && c.Transaction_Date.Value.Day == TransactionDate.Day).Select(p => p.Transaction_Date).FirstOrDefault();
            if (dateOfTrans == null)
            {
                TempData["Error"] = "No Details Found with the Given Date";
                //ModelState.AddModelError(string.Empty, "No Details were found with the given date");
                return View();
            }
             DateTime? TransDate = null;
            double? TotalDeposit = 0.0;
            double? TotalWithdrawl = 0.0;
            var Totalcount = db.tbl_customer_transactions.Where(c => c.Transaction_Date.Value.Year == TransactionDate.Year && c.Transaction_Date.Value.Month == TransactionDate.Month && c.Transaction_Date.Value.Day == TransactionDate.Day).Select(p => p.Cust_Id).Distinct().Count();
            //var TransctionReportByDate = db.GetTransactionsByDate(TransactionDate).ToList();

            var DepositTransactions= db.tbl_customer_transactions.Where(c => c.Transaction_Date.Value.Year == TransactionDate.Year && c.Transaction_Date.Value.Month == TransactionDate.Month && c.Transaction_Date.Value.Day == TransactionDate.Day && c.TypeOf_Transaction.ToLower() == "d").ToList();

            

            foreach (var i in DepositTransactions)
            {
                TotalDeposit += i.Transaction_Amount;
                TransDate = i.Transaction_Date;

            }


            var WithdrawlTransactions = db.tbl_customer_transactions.Where(c => c.Transaction_Date.Value.Year == TransactionDate.Year && c.Transaction_Date.Value.Month == TransactionDate.Month && c.Transaction_Date.Value.Day == TransactionDate.Day && c.TypeOf_Transaction.ToLower() == "w").ToList();
            foreach (var i in WithdrawlTransactions)
            {
                TotalWithdrawl += i.Transaction_Amount;
                TransDate =  i.Transaction_Date;


            }

            TransDetByDate transDetByDate = new TransDetByDate()
            {
                TDeposit = TotalDeposit,
                TWithdrawl = TotalWithdrawl,
                Tcount = Totalcount,
                TransactionDate = TransDate,
                  
            };
            return View("TransReportViewByDate", transDetByDate);

        }

        public ActionResult TransReportViewByDate()
        {
           
            return View();
        }

        public ActionResult DetailTransReportByDate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DetailTransReportByDate(DateTime date)
        {
            if (date.Year >= DateTime.Now.Year && date.Month >= DateTime.Now.Month && date.Day > DateTime.Now.Day)
            {
                TempData["Error"] = "Entered Date should not greater than Today Date";
                //ModelState.AddModelError(string.Empty, "Entered Date should not greater than Today's Date");
                return View();
            }

            var dateOfTrans = db.tbl_customer_transactions.Where(c => c.Transaction_Date.Value.Year == date.Year && c.Transaction_Date.Value.Month == date.Month && c.Transaction_Date.Value.Day == date.Day).Select(p => p.Transaction_Date).FirstOrDefault();
            if (dateOfTrans == null)
            {
                TempData["Error"] = "No Details Found with the Given Date";
                //ModelState.AddModelError(string.Empty, "No Details were found with the given date");
                return View();
            }
            var dateOfTransaction = date;
            var customerTransactions = db.tbl_customers.Join(db.tbl_customer_transactions, customer => customer.Cust_Id, transactions => transactions.Cust_Id, (customer, transactions) => new CustAndHisTransactions()
            {
                Customer_Code = customer.Customer_Code,
                Customer_FullName = customer.Customer_FirstName + " " + customer.Customer_LastName,
                TypeOf_Transaction = transactions.TypeOf_Transaction,
                Transaction_Amount = transactions.Transaction_Amount,
                Transaction_Date = transactions.Transaction_Date
            }).Where(c => c.Transaction_Date.Value.Year == dateOfTransaction.Year && c.Transaction_Date.Value.Month == dateOfTransaction.Month && c.Transaction_Date.Value.Day == dateOfTransaction.Day);

            return View("DetailedTransViewByDate", customerTransactions);
        }

        public ActionResult DetailedTransViewByDate()
        {
            return View();
        }

        //public ActionResult Deposit()
        //{

        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Deposit(tbl_customers obj)
        //{
        //    var cust = db.tbl_customers.Where(x => x.Customer_Code == obj.Customer_Code).FirstOrDefault();
        //    return View("DepositCustDetails", cust);
        //}
        
        //public ActionResult DepositCustDetails()
        //{
        //    return View();
        //}
        
        //public ActionResult DepositIntoCustAcc(string CustCode, double? Deposit)
        //{
        //    var res = CustCode.Trim();
        //     var CustUser = db.tbl_customers.Where(x => x.Customer_Code == res).FirstOrDefault();
        //    double? amount = (double)CustUser.Account_BalanceAmount;
        //    if (CustUser != null)
        //    {
        //        amount += Deposit;
        //        CustUser.Account_BalanceAmount = amount;

        //        db.SaveChanges();
        //        return RedirectToAction("ViewAllCustomers", "Staff");
        //    }

        //    return View();
        //}
    }
}