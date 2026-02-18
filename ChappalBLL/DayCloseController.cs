using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class DayCloseController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        public int InsertDayClose(System.DateTime WorkingDate, int AddedBy,int DBServer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@WorkingDate", WorkingDate),
                new SqlParameter("@AddedBy", AddedBy),
                new SqlParameter("@DBServer",DBServer)
            };
            return dbHelper.ExecuteNonQuery("uspInsertWorkingDate", parameters);
        }
    }
}