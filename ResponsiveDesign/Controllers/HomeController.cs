using BusinessLogic;
using BusinessObject;
using Helper;
using ResponsiveDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ResponsiveDesign.Controllers
{
    [MyHandelErrrAttribute]
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            string uname = string.Empty;
            int id = 0;
            if (Request.Cookies.Get("UserName") == null && Request.Cookies.Get("UserId") == null)
            {

                return RedirectToAction("Register", "Login");
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.Cookies.Get("UserName").Value) && !string.IsNullOrEmpty(Request.Cookies.Get("UserId").Value))
                {
                    uname = Request.Cookies.Get("UserName").Value;
                    id = Convert.ToInt32(Request.Cookies.Get("UserId").Value);
                }
                else
                {
                    return RedirectToAction("Register", "Login");
                }

            }
            ViewBag.UserName = uname;
            ViewBag.UserId = id;
            return View();
        }

        public ActionResult Trail()
        {

            HttpCookie wtrCookies = new HttpCookie("UserName");
            wtrCookies.Value = "Guest";
            Response.Cookies.Add(wtrCookies);
            wtrCookies = new HttpCookie("UserId");
            wtrCookies.Value = "0";
            Response.Cookies.Add(wtrCookies);
            return RedirectToAction("Index");


        }
        public ActionResult LogOut()
        {
            var cun = Response.Cookies.Get("UserName");
            var cid = Response.Cookies.Get("UserId");
            cun.Expires = DateTime.Now.AddSeconds(1);
            cid.Expires= DateTime.Now.AddSeconds(1);
            Session["UserName"] = null;
            Session["UserId"] = null;

            return RedirectToAction("Index");

        }
        public async Task<ActionResult> Register(UserDetails user)
        {

            HttpClient client = new HttpClient();
            var url=Convert.ToString(  System.Configuration.ConfigurationManager.AppSettings["APIBaseURL"]);
            HttpResponseMessage response = await client.PostAsJsonAsync<UserDetails>(
                url+ "api/Account/CreateUser", user);
            response.EnsureSuccessStatusCode();
            string res = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    res = await response.Content.ReadAsStringAsync();
                    HttpCookie wtrCookies = new HttpCookie("UserName");
                    wtrCookies.Value = user.UserName;
                    Response.Cookies.Add(wtrCookies);
                    wtrCookies = new HttpCookie("UserId");
                    wtrCookies.Value = res;
                    Response.Cookies.Add(wtrCookies);
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.Error = "UserId alreary exists";
                }
                else
                {
                    ViewBag.Error = response.StatusCode;

                }
            }
            else
            {
                ViewBag.Error = response.StatusCode;
               

            }
            return View("Login");
         



        }
        public ActionResult ViewExpenseGraph()
        {
            List<BudgetExpense> budgetExpenses = new List<BudgetExpense>();
            ReturnObject<DetailsInfo> ro = new BL().GetExpenseDetails(DateTime.Now.AddDays(-DateTime.Now.Day+1), DateTime.Now, "", "", Convert.ToInt32(Request.Cookies.Get("UserId").Value));
            ReturnObject<List<UserBudget>> ro1 = new BL().GetUserBudget(Convert.ToInt32(Request.Cookies.Get("UserId").Value));

            if (ro1.StatusCode == HttpStatusCode.OK)
            {
                for (int i = 0; i < ro1.Response.Count; i++)
                {

                    BudgetExpense budgetExpense = new BudgetExpense();
                    budgetExpense.ItemTypeId = ro1.Response[i].ItemTypeId;
                    budgetExpense.Budget = ro1.Response[i].Budget;
                    budgetExpense.ItemType = ro1.Response[i].ItemType;
                    budgetExpense.Expense = ro.Response.lstExpenseDtls.Where(x => x.ItemTypeId == ro1.Response[i].ItemTypeId).Sum(x => x.Totol);
                    budgetExpense.AmountLeft = budgetExpense.Budget - budgetExpense.Expense;

                    budgetExpenses.Add(budgetExpense);
                }
            }
            return View(budgetExpenses);


        }
        public ActionResult Create()
        {
            return View();

        }
        public async Task<ActionResult> UserLogin(UserDetails user)
        {

            HttpClient client = new HttpClient();
            var url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["APIBaseURL"]);
            HttpResponseMessage response = await client.PostAsJsonAsync<UserDetails>(
                url + "api/Account/UserLogin", user);
           // response.EnsureSuccessStatusCode();
            string res = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadAsStringAsync();
                HttpCookie wtrCookies = new HttpCookie("UserName");
                wtrCookies.Value = user.UserName;
                Response.Cookies.Add(wtrCookies);
                wtrCookies = new HttpCookie("UserId");
                wtrCookies.Value = res;
                Response.Cookies.Add(wtrCookies);
            }
            
            return RedirectToAction("Index");
        }
        public class UserDetails
        {


            public string UserName { get; set; }
            public string Password { get; set; }
            //public string IpAddress { get; set; }

        }
        public ActionResult About()
        {
            
            return View();
        }
        public ActionResult Contact()
        {

            return View();
        }
        public ActionResult Services()
        {

            return View();
        }
        public ActionResult Error()
        {

            return View();
        }
        public ActionResult Settings()
        {

            return View();
        }

    }

}