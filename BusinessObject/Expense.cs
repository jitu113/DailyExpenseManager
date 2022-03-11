using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class BaseModel
    {
        public int UserId { get; set; }
    }
     public class SearchItemList
    {
        public DateTime Date { get; set; }
        public List<ExpenseDetails> lstExpenseDtls;
        public int Sum { get; set; }
    }
    public class Item
    {

        public string Particular { get; set; }
        public decimal Price { get; set; }

        public bool IsPaid { get; set; }
        public int MonthId { get; set; }

    }
    public class UserBudget
    {

        public int ItemTypeId { get; set; }
        public string ItemType { get; set; }
        public int Budget { get; set; }

    }
    public class ExpenseDetails : BaseModel
    {
        public int? EntryId { get; set; }

        public string Date { get; set; }

        public string Particular { get; set; }
        public int Quantity { get; set; }

        public int UnitPrice { get; set; }
        public int Totol { get; set; }

        public string Remarks { get; set; }
        public Int16 ItemTypeId { get; set; }
        public string ItemType { get; set; }

    }
    public class FilterDate
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }
    public class DateVal
    {
        public DateTime Date { get; set; }

    }
    public class SearchData : BaseModel
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string item { get; set; }
        public string ItemTypeId { get; set; }
      


    }
    public class DetailsInfo 
    {
        public List<ExpenseDetails> lstExpenseDtls;
       
        //    public FilterDate dateFilter;

    }
    public class BudgetExpense
    {
        public int ItemTypeId { get; set; }
        public string ItemType { get; set; }
        public int Budget { get; set; }
        public int Expense { get; set; }
        public int AmountLeft { get; set; }

        //    public FilterDate dateFilter;

    }
    public class CurrentDetails : BaseModel
    {
        public int ExpectedIncome { get; set; }
        public int CurrentIncome { get; set; }
        public int ExpectedExpense { get; set; }
        public decimal CurrentExpenses { get; set; }

        //    public FilterDate dateFilter;

    }
}
