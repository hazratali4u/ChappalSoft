using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class DashboardController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public DataSet GetDashboardData(int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID)
            };
            return dbHelper.ExecuteDataSet("uspGetDashBoardData", parameters);
        }
    }
}