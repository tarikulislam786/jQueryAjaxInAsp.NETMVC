using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQueryAjaxInAsp.NETMVC
{
    public class GlobalClass
    {
        public static string RenderRazorViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                ViewEngineResult ViewResult;
                ViewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, ViewResult.View, controller.ViewData, controller.TempData, sw);
                ViewResult.View.Render(viewContext, sw);
                ViewResult.ViewEngine.ReleaseView(controller.ControllerContext, ViewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}