using System.Data;
using System.Data.SqlClient;
using ChappalDAL;
using System;

namespace ChappalBLL
{
    public class ExpenseController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public DataTable GetExpenseHead(int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID)
            };
            return dbHelper.ExecuteDataTable("uspGetExpenseHead", parameters);
        }

        public int InsertUpdateExpenseHead(int ExpenseHeadID, string Name, int UserID, int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@ExpenseHeadID", ExpenseHeadID),
                new SqlParameter("@Name", Name),
                new SqlParameter("@UserID",UserID),
                new SqlParameter("@TypeID", TypeID)
            };
            return dbHelper.ExecuteNonQuery("uspInsertUpdateExpenseHead", parameters);
        }

        public DataTable GetExpense(int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID)
            };
            return dbHelper.ExecuteDataTable("uspGetExpense", parameters);
        }
        public int InsertUpdateExpense(int ExpenseID, int ExpenseHeadID, DateTime ExpenseDate, decimal Amount, string Remarks, int UserID, int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@ExpenseID",ExpenseID),
                new SqlParameter("@ExpenseHeadID", ExpenseHeadID),
                new SqlParameter("@ExpenseDate", ExpenseDate),
                new SqlParameter("@Amount",Amount),
                new SqlParameter("@Remarks",Remarks),
                new SqlParameter("@UserID",UserID),
                new SqlParameter("@TypeID", TypeID)
            };
            return dbHelper.ExecuteNonQuery("uspInsertUpdateExpense", parameters);
        }
    }
}