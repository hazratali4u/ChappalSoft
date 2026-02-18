using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class ItemPriceController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public DataTable GetItemPrice(int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID)
            };
            return dbHelper.ExecuteDataTable("uspGetItemPrice", parameters);
        }

        public int InsertUpdateItemPrice(int ItemID,int ItemPrice,int ItemWSPrice, int UpdatedBy,int DBServer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@ItemID", ItemID),
                new SqlParameter("@ItemPrice",ItemPrice),
                new SqlParameter("@ItemWSPrice",ItemWSPrice),
                new SqlParameter("@UpdatedBy", UpdatedBy),
                new SqlParameter("@DBServer",DBServer)
            };
            return dbHelper.ExecuteNonQuery("uspInsertUpdateItemPrice", parameters);
        }
    }
}