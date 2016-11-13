using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace AddressBook.Controllers
{
    
    public class AccountController : Controller
    {//[Authorize]
        [HttpGet]
        public ActionResult Index(int id = 0)
        {
            ViewBag.btnSaveTitle = true;
            ViewBag.btnCancelEnable = false;

            if (id == 0)
                return View();
            using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
            {
                ViewBag.btnSaveTitle = false;
                ViewBag.btnCancelEnable = true;
                return View(ABEntity.Users.Find(id));
            }
        }

        [HttpPost]
        public ActionResult Index(User user, string btnCancel)
        {
            if (btnCancel != null)
                return RedirectToAction("Index", new { id = 0 });
            using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
            {
                if (ModelState.IsValid)
                {
                    if (user.UserID == 0)
                    {

                        //user.Password = Helpers.Encode(user.Password);
                        ABEntity.Users.Add(user);
                    }
                    else
                    {
                        var selUser = ABEntity.Users.First(c => c.UserID == user.UserID);
                        selUser.FirstName = user.FirstName;
                        selUser.LastName = user.LastName;
                        selUser.UserName = user.UserName;
                        selUser.Password = user.Password;
                        selUser.UserType = user.UserType;
                    }

                    ABEntity.SaveChanges();
                    ModelState.Clear();
                    user = null;
                    ViewBag.Message = "Successfuly Registration Done.";
                }

                return View(user);
            }
        }

       public JsonResult IsUsernameAvailable(string username)
        {
            using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
            {
                var user = ABEntity.Users.FirstOrDefault(c => c.UserName == username);
                return Json(user == null);
            }
        }
        
        public ActionResult List()
        {
            using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
            {
                return PartialView(ABEntity.Users.ToList());
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
            {
                ViewBag.Message = "";

                var selUser = ABEntity.Users.First(c => c.UserID == id);
                ABEntity.Users.Remove(selUser);
                ABEntity.SaveChanges();

                ModelState.Clear();
                selUser = null;

                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Login(Models.User user)
        {

            using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
            {
                //user.Password = Helpers.Encode(user.Password);
                var userobj = ABEntity.Users.Where(m => m.UserName == user.UserName && m.Password == user.Password).FirstOrDefault();
                if (userobj != null)
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    Session["LoginedUser"] = userobj;
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Login data is incorrect!");
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Account", "Login");
        }

        [HttpGet]
        public ActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forgot(ForgotPasswordModel forgotpassword)
        {
            if (ModelState.IsValid)
            {
                using (AddressBookDBEntities ABEntity = new AddressBookDBEntities())
                {
                    MembershipUser user;
                    var foundUser = (from u in ABEntity.Users
                                     where u.UserName == forgotpassword.Email
                                     select u).FirstOrDefault();
                    if (foundUser != null)
                    {
                        user = Membership.GetUser(foundUser.UserName.ToString());
                        var token = WebSecurity.GeneratePasswordResetToken(foundUser.UserName);
                        string resetLink = "<a href='"
                      + Url.Action("ResetPassword", "Account", new { rt = "token" }, "http")
                      + "'>Reset Password Link</a>";

                        string subject = "Reset your password for jmqtnonsense.ca";
                        string body = "You link: " + resetLink;
                        string from = "no-reply@jmqtnonsense.ca";

                        MailMessage message = new MailMessage(from, forgotpassword.Email);
                        message.Subject = subject;
                        message.Body = body;
                        SmtpClient client = new SmtpClient();
                        try
                        {
                            client.Send(message);
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("", "Issue sending email: " + e.Message);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "No user found by that email.");
                    }
                }
             }
            return View(forgotpassword);
        }
    }
}
