using WebApplication1;
using WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        DataContext db = new DataContext();


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tblUser model, string command)
        {
            try
            {
                var User = db.tblUsers.Where(u => u.FullName == model.FullName).FirstOrDefault();
                if (User != null)
                {
                    if (HelperClass.Decrypt(User.Password) == model.Password)
                    {
                        if (command == "Login")
                        {
                            FormsAuthentication.SetAuthCookie(User.Id.ToString(), false);
                            Session["UserId"] = User.Id.ToString();
                            if (User.Role == "Admin")
                            {
                                Session["Role"] = "Admin";
                                //return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                Session["Role"] = "Normal";
                               // return RedirectToAction("UserDashboard", "Home");
                            }
                            return RedirectToAction("Index", "LeaseDetails");
                        }
                        //else if (command == "Clockout")
                        //{
                        //    Session["UserId"] = User.Id.ToString();
                        //    if (User.Role != "Admin")
                        //    {
                        //        var todayaDate = DateTime.Now.Date;
                        //        Attendance attendance = db.Attendances.Where(p => p.UserId == User.Id && p.AttendanceDate == todayaDate && p.CheckOut == null).FirstOrDefault();
                        //        if (attendance != null)
                        //        {
                        //            attendance.CheckOut = DateTime.Now.TimeOfDay;
                        //            attendance.CreatedDate = DateTime.Now;
                        //            //db.Attendances.Add(attendance);
                        //            db.Entry(attendance).State = EntityState.Modified;
                        //            db.SaveChangesAsync();
                        //        }
                        //        Session["Role"] = "Normal";
                        //        return RedirectToAction("Index", "Attendances");
                        //    }
                        //}
                    }
                    else
                    {
                        TempData["Message"] = "Login Failed! You have entered wrong password.";
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("Logout")]
        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            Session["UserId"] = null;
            Session["Role"] = null;
            return Redirect("Login");
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(tblUser model, string command)
        {
            try
            {
                var User = db.tblUsers.Where(u => u.Email == model.Email).FirstOrDefault();
                if (User != null)
                {
                    var encryptedData = HelperClass.Encrypt(User.Id.ToString());
                    var body = "<p>Email From: </p><p>Message:</p><p>{2}</p> Click link : ";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.Email));  // replace with valid value 
                    message.From = new MailAddress("admin@lab1472.com");  // replace with valid value
                    message.Subject = "Your email subject";
                    message.Body = body + Environment.NewLine + Environment.NewLine + "https://attendancems.lab1472.com/Auth/ResetPassword/" + encryptedData;
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        smtp.UseDefaultCredentials = false;
                        var credential = new NetworkCredential
                        {
                            UserName = "admin@lab1472.com",  // replace with valid value
                            Password = "3hM!7d8y"  // replace with valid value
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "mail.lab1472.com";
                        smtp.Port = 25;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = false;

                        smtp.Send(message);
                    }

                    return View(model);
                }
                else
                {
                    TempData["Message"] = "Email sending failed! Email not found.";
                }

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error! " + ex.Message;
                return View(model);
            }
        }

        //public ActionResult ResetPassword(string id)
        //{
        //    var origId = HelperClass.Decrypt(id);
        //    var User = db.tblUsers.Find(new Guid(origId));
        //    if (User != null)
        //    {
        //        //UserVM uservm = new UserVM();
        //        //uservm.Email = User.Email;
        //        //uservm.OldPassword = User.Password;
        //        //return View(uservm);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ResetPassword(UserVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var User = db.tblUsers.Where(u => u.Email == model.Email).FirstOrDefault();
        //            if (User != null)
        //            {
        //                User.Password = HelperClass.Encrypt(model.ConfirmPassword);
        //                db.Entry(User).State = EntityState.Modified;
        //                db.SaveChangesAsync();
        //                return RedirectToAction("Login");
        //            }
        //            else
        //            {
        //                TempData["Message"] = "Error!";
        //            }

        //            return View(model);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    else
        //    {
        //        TempData["Message"] = "Error! Password Not match";
        //        return View(model);
        //    }

        //}
    }
}