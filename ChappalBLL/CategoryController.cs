using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class CategoryController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public DataTable GetCategory(int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID)
            };
            return dbHelper.ExecuteDataTable("uspGetCategory", parameters);
        }

        public int InsertUpdateCategory(int CategoryID, string Name, int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@CategoryID", CategoryID),
                new SqlParameter("@Name", Name),
                new SqlParameter("@TypeID", TypeID)
            };
            return dbHelper.ExecuteNonQuery("uspInsertUpdateCategory", parameters);
        }
    }
}