using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class MenuController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public DataTable GetMenu(int RoleID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@RoleID", RoleID)
            };
            return dbHelper.ExecuteDataTable("uspGetMenu", parameters);
        }
    }
}