using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data.Entity;
using BankCusManagement.Models;
using BankCusManagement.ViewModal;


namespace BankCusManagement.Controllers
{
    public class LoginController : Controller
    {
        BankCusManagementEntity db = new BankCusManagementEntity();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(tbl_users user)
        {
            if (!ModelState.IsValid)
            {

                return View(user);
            }
            else
            {
               // var u = db.tbl_users.Where(x => x.Login_Id == user.Login_Id).Select(x => x.Password);


                var UserPwd = "";
                var password = (string)user.Password;
                byte[] encode = new byte[password.Length];
                encode = Encoding.UTF8.GetBytes(password);
                UserPwd = Convert.ToBase64String(encode);

                var users = db.tbl_users.Where(x => x.Login_Id == user.Login_Id).Where(c=>c.Password == UserPwd).SingleOrDefault();

                //var staff = db.tbl
                if (users == null)
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                  

                    return View();
                }

                else if (users.Status.ToString().ToLower() == "inactive")
                {
                    ModelState.AddModelError("", "Inactive Users cannot login Please contact Bank");
                   
                    return View();
                }

                else
                {
                   
                    if (users.Role_ID == 1)
                    {
                        Session["UserKey"] = Guid.NewGuid();
         
                        Session["LastLogin"] = users.Last_Login;
                        users.Last_Login = DateTime.Now;
                        Session["RoleId"] = users.Role_ID;
                        db.SaveChanges();
                        var res = users.tbl_Roles.tbl_role_menu.Where(x => x.Role_Id == users.Role_ID).Select(m=>m.Menu_Name).ToList();

                        Session["Option1"]=res[0].ToString();
                        Session["Option2"] = res[1].ToString();
                        Session["Option3"] = res[2].ToString();
                        Session["Option4"] = res[3].ToString();
                        Session["Option5"] = res[4].ToString();
                        Session["Option6"] = res[5].ToString();

                        Session["UserName"] = users.User_Name;
                        return RedirectToAction("Index", "Staff");
                    }

                    else 
                    {
                        Session["LoginId"] = users.Login_Id;
                        
                        Session["LastLogin"] = users.Last_Login;
                        users.Last_Login = DateTime.Now;
                        var res = users.tbl_Roles.tbl_role_menu.Where(x => x.Role_Id == 2).Select(m => m.Menu_Name).ToList();

                        Session["Option1"] = res[0].ToString();
                        Session["Option2"] = res[1].ToString();
                        Session["Option3"] = res[2].ToString();
                        Session["Option4"] = res[3].ToString();
                        
                        db.SaveChanges();
                        Session["UserName"] = users.User_Name;
                        Session["Uid"] = users.Uid;
                        Session["Ccode"] = users.tbl_customers.Where(x => x.Uid == users.Uid).Select(z => z.Customer_Code).FirstOrDefault();


                        return RedirectToAction("Index", "Customer");
                    }
                }
                
            }
             
        }
    }
}