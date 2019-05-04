using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jQueryAjaxInAsp.NETMVC.Models;
using System.Data.Entity;

namespace jQueryAjaxInAsp.NETMVC.Controllers
{
    public class UserController : Controller
    {

        public ActionResult Index()
        {
            using (DBModel dbModel = new DBModel())
            {
                return View(dbModel.Users.ToList());
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                User userModel = new User();
                return View(userModel);
            }
            else
            {
                using (DBModel db = new DBModel())
                {
                    return View(db.Users.Where(x => x.UserID == id).FirstOrDefault());
                }
            }
            
        }

        [HttpPost]
	public ActionResult AddOrEdit(User userModel)
        {
            using (DBModel db = new DBModel())
            {
                if(db.Users.Any(x => x.Username == userModel.Username))
                {
                    ViewBag.DuplicateMessage = "User already exists.";
                    return View("AddOrEdit", userModel);
                }
                db.Users.Add(userModel);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful.";
            return View("AddOrEdit", new User());
        }

        // GET: /User/Create

        /* public ActionResult Create()
         {
             return View();
         } */
        [HttpPost]
       // [Route("User/{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                using (DBModel db = new DBModel())
                {
                    User usr = db.Users.Where(x => x.UserID == id).FirstOrDefault<User>();
                   
                    // delete from db
                    db.Users.Remove(usr);
                    db.SaveChanges();

                }
                return Json(new { success = true}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}