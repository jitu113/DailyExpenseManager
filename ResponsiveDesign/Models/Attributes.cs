using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ResponsiveDesign.Models
{
    public class Attributes
    {
    }
    public class  MyHandelErrrAttribute: HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            ErrorHadler.HandalError(filterContext.RouteData.Values["controller"].ToString()+ " "+
                filterContext.RouteData.Values["action"].ToString() +"Stack trace : "+ filterContext.Exception.StackTrace, filterContext.Exception.Message.ToString());

            filterContext.HttpContext.Response.Redirect("~/Home/Error");
            base.OnException(filterContext);
        }

    }

    public static class ErrorHadler
    {
        public static void HandalError(string Method, string Msg)
        {

            string myfile = HostingEnvironment.ApplicationPhysicalPath + @"\Error.log";


              //  Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["ErrorPath"]);

            // Appending the given texts
            string file = "-------Error start  Time " + DateTime.Now.ToString() + " -----\n"
                 + "URL " + Method + "\nMessage " + Msg + "\n-----------Error end----------\n";

            File.AppendAllText(myfile, file);


        }



    }
    //public class MyAuthnticationFilter 
    //{
    //    public void OnAuthorization(AuthorizationContext filterContext)
    //    {
    //        base.ON(filterContext);
    //    }
    //}

}