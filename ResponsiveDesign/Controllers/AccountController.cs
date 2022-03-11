using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ResponsiveDesign.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult>  Index(UserDetails product)
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response =await  client.PostAsJsonAsync<UserDetails>(
                "api/products", product);
            response.EnsureSuccessStatusCode();
            string res = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                res =await  response.Content.ReadAsStringAsync();
                HttpCookie wtrCookies = new HttpCookie("UserName");
                wtrCookies.Value = "Jayanta";
                Response.Cookies.Add(wtrCookies);
                return View();
            }
            else
            {
                return View();

            }


        
        }
        public class UserDetails
        {


            public string UserName { get; set; }
            public string Password { get; set; }
            //public string IpAddress { get; set; }

        }
    }
}