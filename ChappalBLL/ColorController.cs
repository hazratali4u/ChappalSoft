using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class ColorController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public DataTable GetColor(int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID)
            };
            return dbHelper.ExecuteDataTable("uspGetColor", parameters);
        }

        public int InsertUpdateColor(int ColorID, string Name,string ColorCode,bool ShowName, int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@ColorID", ColorID),
                new SqlParameter("@Name", Name),
                new SqlParameter("@TypeID", TypeID),
                new SqlParameter("@ColorCode",ColorCode),
                new SqlParameter("@ShowName",ShowName)
            };
            return dbHelper.ExecuteNonQuery("uspInsertUpdateColor", parameters);
        }
    }
}