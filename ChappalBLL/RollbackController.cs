using System;
using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class RollbackController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        public DataTable GetRollbackData(int TypeID,DateTime DocumentDate,DateTime ToDate)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID", TypeID),
                new SqlParameter("@DocumentDate",DocumentDate),
                new SqlParameter("@ToDate",ToDate)
            };
            return dbHelper.ExecuteDataTable("uspGetRollbackData", parameters);
        }
        public int RollbackData(int TypeID, int DocumentID,string RecordID, DateTime StockDate, int UserID,int DBServer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID", TypeID),
                new SqlParameter("@DocumentID", DocumentID),
                new SqlParameter("@RecordID",RecordID),
                new SqlParameter("@StockDate",StockDate),
                new SqlParameter("@UserID", UserID),
                new SqlParameter("@DBServer",DBServer)
            };
            return dbHelper.ExecuteNonQuery("uspRollbackData", parameters);
        }
    }
}