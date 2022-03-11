using BusinessObject;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class DALMySql
    {

        public static long SaveData(ExpenseDetails objExpensdtl)
        {
            string connectionString =
                       ConfigurationManager.ConnectionStrings["ExpenseContextMySql"].ConnectionString;

            List<ExpenseDetails> employees = new List<ExpenseDetails>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {


                long id = 0;
                MySqlCommand cmd = new MySqlCommand("InsertInfo", con);

                cmd.Parameters.AddWithValue("p_date",Convert.ToDateTime(   objExpensdtl.Date).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("p_particular", objExpensdtl.Particular);
                cmd.Parameters.AddWithValue("p_quantity", objExpensdtl.Quantity);
                cmd.Parameters.AddWithValue("p_unitPrice", objExpensdtl.UnitPrice);
                cmd.Parameters.AddWithValue("p_totol", objExpensdtl.Totol);
                cmd.Parameters.AddWithValue("p_remarks", objExpensdtl.Remarks);
                cmd.Parameters.AddWithValue("p_entryId", objExpensdtl.EntryId==null?0 : objExpensdtl.EntryId);
                cmd.Parameters.AddWithValue("p_ItemTypeId", objExpensdtl.ItemTypeId);
                cmd.Parameters.AddWithValue("p_UserId", objExpensdtl.UserId);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    id = Convert.ToInt32(rdr["ID"]);
                }
                con.Close();
                return id;
            }
            

        }

        public static List<Item> GetFixedExpenses(int monthId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseContext"].ConnectionString;
            List<Item> Items = new List<Item>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(connectionString);
                cmd.Parameters.Add("@MonthId", SqlDbType.TinyInt).Value = monthId;
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Item Item = new Item();
                    Item.Particular = Convert.ToString(rdr["Particular"]);
                    Item.IsPaid = Convert.ToBoolean(rdr["Particular"]);
                    Item.Price = Convert.ToDecimal(rdr["Particular"]);
                    Items.Add(Item);
                }
                conn.Close();
            }
            return Items;
        }
        public static DataSet GetCurrentTransctions(int monthId, int year)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseContext"].ConnectionString;
            List<Item> Items = new List<Item>();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[GetCurrentExpense]", conn);
                    cmd.Parameters.Add("@MonthId", SqlDbType.TinyInt).Value = monthId;
                    cmd.Parameters.Add("@year", SqlDbType.Int).Value = year;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    SqlDataAdapter dp = new SqlDataAdapter();
                    dp.SelectCommand = cmd;
                    dp.Fill(ds);

                }
            }
            catch (Exception ex)
            {
                return null;

            }

            return ds;
        }
     
        public static int DeleteItem(long id,int userid)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseContextMySql"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("DeleteEntry", con);
                cmd.Parameters.AddWithValue("p_entryId", id);
                cmd.Parameters.AddWithValue("p_userId", userid);

                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                int res = cmd.ExecuteNonQuery();
                con.Close();
                return res;
            }

        }
        public static List<Avatars> GetAvatars()
        {
            string connectionString =
                   ConfigurationManager.ConnectionStrings["ExpenseContextMySql"].ConnectionString;

            List<Avatars> lstAvatar = new List<Avatars>();

    


                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("GetAvatar", con);



                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        lstAvatar.Add(new Avatars()
                        {
                            AvatarId = Convert.ToInt32(rdr["AvatarId"]),
                            AvatarName = Convert.ToString(rdr["AvatarName"])

                        });

                    }


                }
                return lstAvatar;

            }
        
            public static int CreateUser(UserDetails userData)
        {
           
                string connectionString =
                   ConfigurationManager.ConnectionStrings["ExpenseContextMySql"].ConnectionString;

                List<ExpenseDetails> employees = new List<ExpenseDetails>();

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("CreateUser", con);
                    cmd.Parameters.AddWithValue("p_username", userData.UserName);
                    cmd.Parameters.AddWithValue("p_password", userData.Password);
                    cmd.Parameters.AddWithValue("p_op_id", 0).Direction= ParameterDirection.Output;
                  

                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return  Convert.ToInt32( cmd.Parameters["p_op_id"].Value);
                    
                }
        }

        public static int UserLogin(UserDetails userData)
        {

            string connectionString =
                   ConfigurationManager.ConnectionStrings["ExpenseContextMySql"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("UserLogin", con);
                cmd.Parameters.AddWithValue("p_username", userData.UserName);
                cmd.Parameters.AddWithValue("p_password", userData.Password);
                cmd.Parameters.AddWithValue("p_op_id", 0).Direction = ParameterDirection.Output;


                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Convert.ToInt32(cmd.Parameters["p_op_id"].Value);

            }
        }
        
         public static List<UserBudget> GetUserBudget(int UserId)
        {
            string connectionString =
                   ConfigurationManager.ConnectionStrings["ExpenseContextMySql"].ConnectionString;

            List<UserBudget> lstBudget = new List<UserBudget>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand("GetBudget", con);
                
                cmd.Parameters.AddWithValue("p_userId", UserId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    UserBudget budget = new UserBudget();
                    budget.ItemTypeId = Convert.ToInt32(rdr["ItemTypeId"]);
                    budget.ItemType= Convert.ToString(rdr["ItemType"]);
                    budget.Budget = Convert.ToInt32(rdr["Budget"]);
             
                    lstBudget.Add(budget);
                }
            }
            return lstBudget;

        }
        public static List<ExpenseDetails> SearhItem(DateTime fromDate, DateTime todate, string itemTypeIds, string item, int UserId)
        {
            string connectionString =
                   ConfigurationManager.ConnectionStrings["ExpenseContextMySql"].ConnectionString;

                List<ExpenseDetails> employees = new List<ExpenseDetails>();

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand("SearchItem", con);
                    cmd.Parameters.AddWithValue("p_item", item);
                    cmd.Parameters.AddWithValue("p_itemTypes", itemTypeIds== "0"?"":itemTypeIds);
                    cmd.Parameters.AddWithValue("p_fromDate", fromDate);
                    cmd.Parameters.AddWithValue("p_toDate", todate);
                    cmd.Parameters.AddWithValue("p_userId",UserId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ExpenseDetails expenseDetails = new ExpenseDetails();
                        expenseDetails.EntryId = Convert.ToInt32(rdr["EntryId"]);
                        expenseDetails.Particular = rdr["Particular"].ToString();
                        expenseDetails.Quantity = Convert.ToInt32(rdr["Quantity"]);
                        expenseDetails.Totol = Convert.ToInt32(rdr["Totol"]);
                        expenseDetails.Date = Convert.ToDateTime(rdr["Date"]).ToString("MM-dd-yyyyy");
                        expenseDetails.Remarks = Convert.ToString(rdr["Remarks"]);
                        expenseDetails.UnitPrice = Convert.ToInt32(rdr["UnitPrice"]);
                        expenseDetails.ItemType = rdr["ItemType"] == DBNull.Value ? "Not Set" : Convert.ToString(rdr["ItemType"]);
                         expenseDetails.ItemTypeId= Convert.ToInt16(rdr["ItemTypeId"]);
                    employees.Add(expenseDetails);
                    }
                }
                return employees;
           
        }
        public static List<ExpenseDetails> expenses(DateTime? date)
        {

            {
                try
                {
                    string connectionString =
                       ConfigurationManager.ConnectionStrings["ExpenseContext"].ConnectionString;

                    List<ExpenseDetails> employees = new List<ExpenseDetails>();

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("[MyExpense].[GetCurrentMonthExpense]", con);
                        cmd.Parameters.Add("@date", SqlDbType.Date).Value = date;
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            ExpenseDetails employee = new ExpenseDetails();
                            employee.EntryId = Convert.ToInt32(rdr["EntryId"]);
                            employee.Particular = rdr["Particular"].ToString();
                            employee.Quantity = Convert.ToInt32(rdr["Quantity"]);
                            employee.Totol = Convert.ToInt32(rdr["Totol"]);
                           // employee.Date = Convert.ToDateTime(rdr["Date"]);
                            employee.Remarks = Convert.ToString(rdr["Remarks"]);
                            employee.UnitPrice = Convert.ToInt32(rdr["UnitPrice"]);
                            employees.Add(employee);
                        }
                    }
                    return employees;
                }


                catch (Exception ex)
                {
                    string s = "";
                    throw;
                }
            }
        }
    }
}