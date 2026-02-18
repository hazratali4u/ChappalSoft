using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class SizeController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public DataTable GetSize(int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID)
            };
            return dbHelper.ExecuteDataTable("uspGetSize", parameters);
        }

        public int InsertUpdateSize(int SizeID, int Name,int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@SizeID", SizeID),
                new SqlParameter("@Name", Name),
                new SqlParameter("@TypeID", TypeID),
            };
            return dbHelper.ExecuteNonQuery("uspInsertUpdateSize", parameters);
        }
    }
}