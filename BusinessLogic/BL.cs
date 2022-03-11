using BusinessObject;
using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Data;
using System.Net;
using System.IO;

namespace BusinessLogic
{
    public static class Logger
    {


        public static void WriteLog(string Method, string Msg)
        {

            string myfile = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["AuditPath"]);

            // Appending the given texts
            string file = "-------Audit start  Time " + DateTime.Now.ToString() + " -----\n"
                 + "Method " + Method + "\nMessage " + Msg + "\n-----------Audit end----------\n";

            File.AppendAllText(myfile, file);

        }
    }
    public class BL
    {
        public ReturnObject<int> CreateUser(UserDetails userData)
        {
            ReturnObject<int> ro = new ReturnObject<int>();

            ro.Response = DALMySql.CreateUser(userData);

            if (ro.Response > 0)
            {
                ro.StatusCode = HttpStatusCode.Created;
            }
            else
            {
                ro.StatusCode = HttpStatusCode.BadRequest;
            }
            return ro;
        }
        public ReturnObject<List<Avatars>> GetAvatars()
        {
            ReturnObject<List<Avatars>> ro = new ReturnObject<List<Avatars>>();
           
                ro.Response = DALMySql.GetAvatars();
             
                if(ro.Response.Count>0)
                {
                    ro.StatusCode = HttpStatusCode.Created;
                }
                else
                {
                    ro.StatusCode = HttpStatusCode.BadRequest;
                }
            return ro;
        }
        public ReturnObject<int> UserLogin(UserDetails userData)
        {
            ReturnObject<int> ro = new ReturnObject<int>();

            //Logger.WriteLog("UserLogin", "UserData " + userData.UserName +" "+ userData.Password);
                ro.Response = DALMySql.UserLogin(userData);

                if (ro.Response > 0)
                {
                    ro.StatusCode = HttpStatusCode.OK;
                }
                else if(ro.Response==0)
                {
                    ro.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {

                    ro.StatusCode = HttpStatusCode.BadRequest;
                }
                return ro;

          
        }
        public ReturnObject<List<UserBudget>> GetUserBudget( int UserId)
        {
            ReturnObject<List<UserBudget>> ro = new ReturnObject<List<UserBudget>>();


            List<UserBudget> objDtlInfo = new List<UserBudget>();
            objDtlInfo = DALMySql.GetUserBudget(UserId);
            //IEnumerable<ExpenseDetails> ineumQuery = objDtlInfo.lstExpenseDtls.Where(x => x.ItemTypeId == 2);
            //objDtlInfo.lstExpenseDtls

            FilterDate clsFilterDate = new FilterDate();

            if (objDtlInfo.Count == 0)
            {
                ro.StatusCode = HttpStatusCode.NoContent;


            }
            else
            {

                ro.Response = objDtlInfo;
                ro.StatusCode = HttpStatusCode.OK;
            }
            return ro;
        }

        public ReturnObject<DetailsInfo> GetExpenseDetails(DateTime fromDate, DateTime toDate, string ItemTypeId, string item,int UserId)
        {
            ReturnObject<DetailsInfo> ro = new ReturnObject<DetailsInfo>();

            
            DetailsInfo objDtlInfo = new DetailsInfo();
                objDtlInfo.lstExpenseDtls =DALMySql.SearhItem(fromDate,
                 toDate, ItemTypeId, item, UserId);
                //IEnumerable<ExpenseDetails> ineumQuery = objDtlInfo.lstExpenseDtls.Where(x => x.ItemTypeId == 2);
                //objDtlInfo.lstExpenseDtls

                FilterDate clsFilterDate = new FilterDate();
              
                if (objDtlInfo.lstExpenseDtls.Count == 0)
                {
                    ro.StatusCode = HttpStatusCode.NoContent;
                    

                }
                else
                {
                    
                    ro.Response = objDtlInfo;
                    ro.StatusCode = HttpStatusCode.OK;
                }
                return ro;
        }
        public static ReturnObject<CurrentDetails> GetCurrentTransctions()
        {
            ReturnObject<CurrentDetails> ro = new ReturnObject<CurrentDetails>();
           
            ro.Response = new CurrentDetails();
            int monthId = DateTime.Now.Month;
            int Year = DateTime.Now.Year;
            var data = DAL.GetCurrentTransctions(monthId, Year);
            ro.Response.ExpectedIncome = data.Tables[0].AsEnumerable().Sum(o => o.Field<int>("Amount"));
            ro.Response.CurrentIncome = ro.Response.ExpectedIncome;
            ro.Response.CurrentExpenses = data.Tables[1].AsEnumerable().Sum(o => o.Field<decimal>("Totol"));
            ro.Response.ExpectedExpense = data.Tables[2].AsEnumerable().Sum(o => o.Field<int>("Amount"));
            ro.StatusCode = HttpStatusCode.OK;
           
            return ro;
        }
        public  ReturnObject<bool> DeleteItem(long id,int userId)
        {
            ReturnObject<bool> ro = new ReturnObject<bool>();
            
                var data = DALMySql.DeleteItem(id, userId);
                ro.Response = data>0?true:false;
                if (ro.Response)
                { 
                    ro.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    ro.StatusCode = HttpStatusCode.BadRequest;
                }
            
            return ro;
        }
        public  ReturnObject<long> CreateTransaction(ExpenseDetails Data)
        {
            ReturnObject<long> ro = new ReturnObject<long>();
           
                ro.Response = DALMySql.SaveData(Data);
                if(ro.Response > 0)
                {
                    ro.StatusCode = Data.EntryId > 0 ? HttpStatusCode.OK : HttpStatusCode.Created;

                }
                else
                {
                    ro.StatusCode = HttpStatusCode.BadRequest;

                }
            return ro;
        }
    }
    
}