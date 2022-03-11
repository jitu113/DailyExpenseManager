using BusinessLogic;
using BusinessObject;
using Helper;
using ResponsiveDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ResponsiveDesign.Controllers
{
    [MyHandelErrrAttribute]
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Register()
        {


           // List<Avatars> avatars = new List<Avatars>();
           // avatars = new BL().GetAvatars().Response;
           //// List<SelectListItem>  selectListItems = new List<SelectListItem>();
           ////foreach( var avatar in avatars)
           //// {
           ////     var guid = Guid.NewGuid();
           ////     SelectListItem selectListItem = new SelectListItem() { Text = avatar.AvatarName + guid.ToString().Substring(0, 4), Value = avatar.AvatarId.ToString() };
           ////     selectListItems.Add(selectListItem);
           //// }
           //// SelectList selectLists = new SelectList(selectListItems) { };
           // ViewData["Avatars"] = avatars;
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [ActionName("UserRegister")]
        public async Task<ActionResult> UserRegister(UserDetails user)
        {

            HttpResponseMessage msg = new HttpResponseMessage();

            ReturnObject<int> ro = new BL().CreateUser(user);
       
            if (ro.StatusCode == System.Net.HttpStatusCode.Created)
            {
                
                    HttpCookie wtrCookies = new HttpCookie("UserName");
                    wtrCookies.Value = user.UserName;
                    Response.Cookies.Add(wtrCookies);
                    wtrCookies = new HttpCookie("UserId");
                    wtrCookies.Value =Convert.ToString( ro.Response);
                    Response.Cookies.Add(wtrCookies);
                    return RedirectToAction("Index", "Home");
             
            }
            else
            {

                if (ro.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.Error = "UserId alreary exists";
                }
                else
                {
                    ViewBag.Error = ro.StatusCode;

                }


            }
            return View("Register");



            //HttpClient client = new HttpClient();
            //var url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["APIBaseURL"]);
            //HttpResponseMessage response = await client.PostAsJsonAsync<UserDetails>(
            //    url + "api/Account/CreateUser", user);

            //string res = string.Empty;
            //if (ro.StatusCode== System.Net.HttpStatusCode.OK)
            //{
            //    if (response.StatusCode == System.Net.HttpStatusCode.Created)
            //    {
            //        res = await response.Content.ReadAsStringAsync();
            //        HttpCookie wtrCookies = new HttpCookie("UserName");
            //        wtrCookies.Value = user.UserName;
            //        Response.Cookies.Add(wtrCookies);
            //        wtrCookies = new HttpCookie("UserId");
            //        wtrCookies.Value = res;
            //        Response.Cookies.Add(wtrCookies);
            //        return RedirectToAction("Index","Home");
            //    }

            //    else
            //    {
            //        ViewBag.Error = response.StatusCode;

            //    }
            //}
            //else
            //{

            //    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            //    {
            //        ViewBag.Error = "UserId alreary exists";
            //    }
            //    else
            //    {
            //        ViewBag.Error = response.StatusCode;

            //    }


            //}
            //return View("Register");




        }

      
        [MyHandelErrrAttribute]
        public async Task<ActionResult> UserLogin(UserDetails user)
        {

            ReturnObject<int> ro = new BL().UserLogin(user);
            if (ro.StatusCode== System.Net.HttpStatusCode.OK)
            {
               
                    HttpCookie wtrCookies = new HttpCookie("UserName");
                    wtrCookies.Value = user.UserName;
                    Response.Cookies.Add(wtrCookies);
                    wtrCookies = new HttpCookie("UserId");
                    wtrCookies.Value = ro.Response.ToString();
                    Response.Cookies.Add(wtrCookies);
                    return RedirectToAction("Index", "Home");
               
            }
            else
            {

                if (ro.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.Error = "Invalid Credentials";
                }
                else
                {
                    ViewBag.Error = ro.StatusCode;

                }
            }



            return View("Login");



            //HttpClient client = new HttpClient();
            //var url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["APIBaseURL"]);
            //HttpResponseMessage response = await client.PostAsJsonAsync<UserDetails>(
            //    url + "api/Account/UserLogin", user);
            //// response.EnsureSuccessStatusCode();
            //string res = string.Empty;
            //if (response.IsSuccessStatusCode)
            //{
            //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //    {
            //        res = await response.Content.ReadAsStringAsync();
            //        HttpCookie wtrCookies = new HttpCookie("UserName");
            //        wtrCookies.Value = user.UserName;
            //        Response.Cookies.Add(wtrCookies);
            //        wtrCookies = new HttpCookie("UserId");
            //        wtrCookies.Value = res;
            //        Response.Cookies.Add(wtrCookies);
            //        return RedirectToAction("Index","Home");
            //    }

            //    else
            //    {
            //        ViewBag.Error = response.StatusCode;

            //    }
            //}
            //else
            //{

            //     if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            //    {
            //        ViewBag.Error = "Invalid Credentials";
            //    }
            //    else
            //    {
            //        ViewBag.Error = response.StatusCode;

            //    }
            //}



            //return View("Login");



        }
        
    }
}