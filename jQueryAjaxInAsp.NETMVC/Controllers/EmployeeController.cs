﻿using jQueryAjaxInAsp.NETMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQueryAjaxInAsp.NETMVC.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAllEmployee());
        }

        IEnumerable<Employee> GetAllEmployee()
        {
            using (DBModel db = new DBModel())
            {
                return db.Employees.ToList<Employee>();
            }
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            
            Employee emp = new Employee();
            if(id != 0)
            {
                using (DBModel db = new DBModel())
                {
                    emp = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault<Employee>();
                }
            }
            return View(emp);
        }

        [HttpPost]
        public ActionResult AddOrEdit(Employee emp)
        {
            try
            {
                if (!String.IsNullOrEmpty(emp.Name) && !String.IsNullOrEmpty(emp.Position)) {
                    if (emp.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(emp.ImageUpload.FileName);
                        string extension = Path.GetExtension(emp.ImageUpload.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        emp.ImagePath = "~/AppFiles/Images/" + fileName;
                        emp.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                    }
                    using (DBModel db = new DBModel())
                    {
                        if (emp.EmployeeID == 0)
                        {
                            db.Employees.Add(emp); // Insert
                            db.SaveChanges();
                        }
                        else
                        {
                            db.Entry(emp).State = EntityState.Modified; // Update
                            db.SaveChanges();
                        }

                    }
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                    ////return RedirectToAction("ViewAll");
                }
                else
                {
                    return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Please complete the form." }, JsonRequestBehavior.AllowGet);
                   // return "";
                }
                
               
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (DBModel db = new DBModel())
                {
                    Employee emp = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault<Employee>();
                    // unlink file from directory
                    string photoName = @Url.Content(emp.ImagePath);
                    string fullPath = Request.MapPath(photoName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    // delete from db
                    db.Employees.Remove(emp);
                    db.SaveChanges();
                    
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}