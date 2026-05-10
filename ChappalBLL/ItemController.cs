using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class ItemController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public DataTable GetItem(int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID),
                new SqlParameter("@StockDate",System.DateTime.Now)
            };
            return dbHelper.ExecuteDataTable("uspGetItem", parameters);
        }

        public DataTable GetItem(int TypeID,System.DateTime StockDate)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID),
                new SqlParameter("@StockDate",StockDate)
            };
            return dbHelper.ExecuteDataTable("uspGetItem", parameters);
        }

        public int InsertUpdateItem(int ItemID,int CategoryID, string Name, int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@ItemID", ItemID),
                new SqlParameter("@CategoryID",CategoryID),
                new SqlParameter("@Name", Name),
                new SqlParameter("@TypeID", TypeID)
            };
            return dbHelper.ExecuteNonQuery("uspInsertUpdateItem", parameters);
        }

        public int InsertStocker(byte [] BarCodeImage, int ItemID, int ColorID, int SizeID,string ColorName)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@BarCodeImage", BarCodeImage),
                new SqlParameter("@ItemID",ItemID),
                new SqlParameter("@ColorID", ColorID),
                new SqlParameter("@SizeID", SizeID),
                new SqlParameter("@ColorName",ColorName)
            };
            return dbHelper.ExecuteNonQuery("uspInsertStickerPrinting ", parameters);
        }
    }
}