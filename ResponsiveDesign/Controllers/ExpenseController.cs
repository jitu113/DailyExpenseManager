using BusinessLogic;
using BusinessObject;
using Helper;
using ResponsiveDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;

namespace ResponsiveDesign.Controllers
{
    [MyHandelErrrAttribute]
    public class ExpenseController : Controller
    {
        // GET: Expense
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetExpenseDetails(SearchData searchData)
        {
            searchData.fromDate = DateTime.Now.AddDays(-5);
            ReturnObject<DetailsInfo> ro = new BL().GetExpenseDetails(searchData.fromDate, searchData.toDate, searchData.ItemTypeId, searchData.item, searchData.UserId);
            if(ro.StatusCode== HttpStatusCode.OK)
            {
                return Json(new { status = ro.StatusCode, lstExpenseDtls = ro.Response.lstExpenseDtls.GroupBy(x => x.Date).Select(group => new { Date = group.Key, Sum = group.Sum(x => x.Totol), Items = group.ToList(), GrandTotol = ro.Response.lstExpenseDtls.Sum(x => x.Totol) }) });
            }
            else
            {
                return Json(new { status = ro.StatusCode });
            }
            
        }
        public JsonResult CreateTransaction(ExpenseDetails Data)
        { 
            ReturnObject<long> ro = new BL().CreateTransaction(Data);
            return Json(new { status = ro.StatusCode, response = ro.Response });
            
        }
        [HttpGet]
        public JsonResult DeleteItem(long id,int userId)
        {
            ReturnObject<bool> ro = new BL().DeleteItem(id, userId);
            return Json(new { status = ro.StatusCode, response = ro.Response });

        }
        public ActionResult CreateBudget()
        {

            return View();
        }
        [HttpGet]
        public string TestUrl(long id)
        {
            return "Hello dear";

        }

    }
}