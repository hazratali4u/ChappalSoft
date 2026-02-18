using System;
using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class ReceiptController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public DataTable GetReceipt(int TypeID,int CustomerID, DateTime Date)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID),
                new SqlParameter("@CustomerID",CustomerID),
                new SqlParameter("@Date",Date)
            };
            return dbHelper.ExecuteDataTable("uspGetReceipt", parameters);
        }

        public int InsertUpdateReceipt(string RecordID, int CustomerID, int SaleID,int PaymentMode,int Amount,string Remarks, int CreatedBy, int TypeID,DateTime DocDate,int DBServer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@RecordID", RecordID),
                new SqlParameter("@CustomerID",CustomerID),
                new SqlParameter("@SaleID", SaleID),
                new SqlParameter("@PaymentMode",PaymentMode),
                new SqlParameter("@Amount",Amount),
                new SqlParameter("@Remarks",Remarks),
                new SqlParameter("@CreatedBy",CreatedBy),
                new SqlParameter("@TypeID", TypeID),
                new SqlParameter("@DocDate",DocDate),
                new SqlParameter("@DBServer",DBServer)
            };
            return dbHelper.ExecuteNonQuery("uspInsertUpdateReceipt", parameters);
        }
    }
}