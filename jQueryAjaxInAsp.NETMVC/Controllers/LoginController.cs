using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jQueryAjaxInAsp.NETMVC.Models;
namespace jQueryAjaxInAsp.NETMVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(User userModel)
        {
            using (DBModel db = new DBModel())
            {
                var userDetails = db.Users.Where(x => x.Username == userModel.Username && x.Password == userModel.Password).FirstOrDefault();
                if(userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }
                else
                {
                    Session["UserID"] = userDetails.UserID;
                    Session["Username"] = userDetails.Username;
                    return RedirectToAction("Index", "Employee");
                }
            }
        }
        
        public ActionResult LogOut()
        {
            int userId = (int)Session["UserID"];
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        } 
    }
}