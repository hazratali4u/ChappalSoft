using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class ShopController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        public DataTable GetShop(int ShopID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@ShopID", ShopID)            };
            return dbHelper.ExecuteDataTable("uspGetShop", parameters);
        }
        public int UpdateShop(int ShopID, string Name,string Address,string AddressShort, string ConactPerson,string ConactNumber,string InvoiceFooterNote,string InvoiceFooterNoteShort, int UserID,int DBServer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@ShopID", ShopID),
                new SqlParameter("@Name", Name),
                new SqlParameter("@Address", Address),
                new SqlParameter("@AddressShort",AddressShort),
                new SqlParameter("@ConactPerson", ConactPerson),
                new SqlParameter("@ConactNumber", ConactNumber),
                new SqlParameter("@InvoiceFooterNote",InvoiceFooterNote),
                new SqlParameter("@InvoiceFooterNoteShort",InvoiceFooterNoteShort),
                new SqlParameter("@UserID", UserID),
                new SqlParameter("@DBServer",DBServer)
            };
            return dbHelper.ExecuteNonQuery("uspUpdateShop", parameters);
        }
    }
}