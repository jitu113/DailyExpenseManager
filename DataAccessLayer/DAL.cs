using BusinessObject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class DAL
    {
       
            public static long SaveData(ExpenseDetails objExpensdtl)
            {
                string connectionString =
                           ConfigurationManager.ConnectionStrings["ExpenseContext"].ConnectionString;

                List<ExpenseDetails> employees = new List<ExpenseDetails>();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    try
                    {
                        long id=0;
                        SqlCommand cmd = new SqlCommand("[MyExpense].[InsertInfo]", con);
                        cmd.Parameters.Add("@date", SqlDbType.Date).Value = objExpensdtl.Date;
                        cmd.Parameters.Add("@particular", SqlDbType.VarChar).Value = objExpensdtl.Particular;
                        cmd.Parameters.Add("@quantity", SqlDbType.TinyInt).Value = objExpensdtl.Quantity;
                        cmd.Parameters.Add("@unitPrice", SqlDbType.Int).Value = objExpensdtl.UnitPrice;
                        cmd.Parameters.Add("@totol ", SqlDbType.Int).Value = objExpensdtl.Totol;
                        cmd.Parameters.Add("@remarks ", SqlDbType.VarChar).Value = objExpensdtl.Remarks;
                        cmd.Parameters.Add("@entryId ", SqlDbType.Int).Value = objExpensdtl.EntryId==null?0: objExpensdtl.EntryId;
                        cmd.Parameters.Add("@ItemTypeId", SqlDbType.TinyInt).Value = objExpensdtl.ItemTypeId;
                        cmd.Parameters.Add("@UserId", SqlDbType.TinyInt).Value = objExpensdtl.UserId;
                       cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader rdr= cmd.ExecuteReader();
                        while(rdr.Read())
                        {
                        id = Convert.ToInt32(rdr["ID"]);
                        }
                     con.Close();
                    return id;
                    }
                    catch (Exception ex)
                    {
                        return 0;
                    }
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
            public static List<ExpenseDetails> GetDetailsByDate(DateTime fromDate, DateTime todate, int itemTypeId)
            {
                try
                {
                    string connectionString =
                       ConfigurationManager.ConnectionStrings["ExpenseContext"].ConnectionString;

                    List<ExpenseDetails> employees = new List<ExpenseDetails>();

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("[MyExpense].GetDetailsByDate", con);
                        cmd.Parameters.Add("@fromdate", SqlDbType.Date).Value = fromDate;
                        cmd.Parameters.Add("@todate", SqlDbType.Date).Value = todate;
                        cmd.Parameters.Add("@itemTypeId", SqlDbType.TinyInt).Value = itemTypeId;

                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            ExpenseDetails expenseDetails = new ExpenseDetails();
                            expenseDetails.EntryId = Convert.ToInt32(rdr["EntryId"]);
                            expenseDetails.Particular = rdr["Particular"].ToString();
                            expenseDetails.Quantity = Convert.ToInt32(rdr["Quantity"]);
                            expenseDetails.Totol = Convert.ToInt32(rdr["Totol"]);
                         //   expenseDetails.Date = Convert.ToDateTime(rdr["Date"]);
                            expenseDetails.Remarks = Convert.ToString(rdr["Remarks"]);
                            expenseDetails.UnitPrice = Convert.ToInt32(rdr["UnitPrice"]);
                            expenseDetails.ItemType = rdr["ItemType"] == DBNull.Value ? "Not Set" : Convert.ToString(rdr["ItemType"]);
                            employees.Add(expenseDetails);
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
        public static int DeleteItem(long id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseContext"].ConnectionString;
            List<Item> Items = new List<Item>();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[MyExpense].[DeleteEntry]", con);
                    cmd.Parameters.Add("@entryId", SqlDbType.BigInt).Value = id;
                  
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    con.Close();
                    return res;

                }
            }
            catch (Exception ex)
            {
                return 0;

            }

           
        }
        public static int CreateUser(UserDetails userData)
        {
            try
            {
                string connectionString =
                   ConfigurationManager.ConnectionStrings["ExpenseContext"].ConnectionString;

                List<ExpenseDetails> employees = new List<ExpenseDetails>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[MyExpense].[CreateUser]", con);
                    cmd.Parameters.Add("@UserName", SqlDbType.Text).Value = userData.UserName;
                    cmd.Parameters.Add("@Password", SqlDbType.Text).Value = userData.Password;//
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value =-1;
                    cmd.Parameters["@id"].Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    int rdr = cmd.ExecuteNonQuery();
                    con.Close();
                    if (rdr>0)
                    {

                        return (int)   cmd.Parameters["@id"].Value;
                    }
                   else
                    {
                        return 0;
                    }
                }
               
            }


            catch (Exception ex)
            {
              
                return 0;
            }
        }
      
        public static int UserLogin(UserDetails userData)
        {
           
                string connectionString =
                   ConfigurationManager.ConnectionStrings["ExpenseContext"].ConnectionString;

                List<ExpenseDetails> employees = new List<ExpenseDetails>();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[MyExpense].[UserLogin]", con);
                    cmd.Parameters.Add("@UserName", SqlDbType.Text).Value = userData.UserName;
                    cmd.Parameters.Add("@Password", SqlDbType.Text).Value = userData.Password;//
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = -1;
                    cmd.Parameters["@id"].Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteScalar();
                    int UserId = 0;
                   
                    UserId=  Convert.ToInt32(cmd.Parameters["@id"].Value);
                    
                    con.Close();
                    return UserId;
                }



        }
        public static List<ExpenseDetails> SearhItem(DateTime fromDate, DateTime todate, string itemTypeIds, string item, int UserId)
            {
                try
                {
                    string connectionString =
                       ConfigurationManager.ConnectionStrings["ExpenseContext"].ConnectionString;

                    List<ExpenseDetails> employees = new List<ExpenseDetails>();

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand("[MyExpense].[SearchItem]", con);
                        cmd.Parameters.Add("@fromdate", SqlDbType.Date).Value = fromDate;
                        cmd.Parameters.Add("@todate", SqlDbType.Date).Value = todate;
                       cmd.Parameters.Add("@userId", SqlDbType.Int).Value = UserId;
                    if (!string.IsNullOrEmpty(itemTypeIds)&&Convert.ToInt32(itemTypeIds)!=0)
                        {
                            cmd.Parameters.Add("@itemTypeIds", SqlDbType.VarChar).Value = itemTypeIds;
                        }
                        cmd.Parameters.Add("@item", SqlDbType.VarChar).Value = item;

                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            ExpenseDetails expenseDetails = new ExpenseDetails();
                            expenseDetails.EntryId = Convert.ToInt32(rdr["EntryId"]);
                            expenseDetails.Particular = rdr["Particular"].ToString();
                            expenseDetails.Quantity = Convert.ToInt32(rdr["Quantity"]);
                            expenseDetails.Totol = Convert.ToInt32(rdr["Totol"]);
                            expenseDetails.Date = rdr["Date"].ToString();
                            expenseDetails.Remarks = Convert.ToString(rdr["Remarks"]);
                            expenseDetails.UnitPrice = Convert.ToInt32(rdr["UnitPrice"]);
                            expenseDetails.ItemType = rdr["ItemType"] == DBNull.Value ? "Not Set" : Convert.ToString(rdr["ItemType"]);
                            employees.Add(expenseDetails);
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