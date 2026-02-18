using System;
using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class PurchaseController
    {
        private string connectionString = System.Configuration.ConfigurationSettings.AppSettings["connString"].ToString();
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public bool InsertPurchase(string PurchaseNo, int SupplierID, int GrossAmount, int Discount, int NetAmount, DataTable dtItems, int CreatedBy,DateTime DocDate,int DBServer)
        {
            int PurchaseID = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    SqlParameter[] parameters = {
                new SqlParameter("@PurchaseNo",PurchaseNo),
                new SqlParameter("@SupplierID",SupplierID),
                new SqlParameter("@GrossAmount", GrossAmount),
                new SqlParameter("@Discount",Discount),
                new SqlParameter("@NetAmount",NetAmount),
                new SqlParameter("@CreatedBy", CreatedBy),
                new SqlParameter("@DocDate",DocDate),
                new SqlParameter("@DBServer",DBServer)
            };
                    PurchaseID = Convert.ToInt32(dbHelper.ExecuteScalar("uspInsertPurchase", parameters,conn,transaction));
                    if (PurchaseID > 0)
                    {
                        foreach (DataRow dr in dtItems.Rows)
                        {
                            SqlParameter[] itemParams = {
                    new SqlParameter("@PurchaseID", PurchaseID),
                    new SqlParameter("@ItemID", dr["ItemID"]),
                    new SqlParameter("@ColorID", dr["ColorID"]),
                    new SqlParameter("@Price", dr["Price"]),
                    new SqlParameter("@Quantity",dr["Quantity"]),
                    new SqlParameter("@Amount",Convert.ToInt32(dr["Quantity"]) * Convert.ToInt32(dr["Price"])),
                    new SqlParameter("@SizeID",dr["SizeID"]),
                    new SqlParameter("@StockDate",DocDate)
                };
                            dbHelper.ExecuteNonQuery("uspInsertPurchaseDetail", itemParams,conn,transaction);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            return true;
        }

        public bool UpdatePurchase(int PurchaseID, string PurchaseNo, int SupplierID, int GrossAmount, int Discount, int NetAmount, DataTable dtItems, int UpdatedBy,int DBServer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                {
                    try
                    {
                        SqlParameter[] parameters = {
                    new SqlParameter("PurchaseID",PurchaseID),
                new SqlParameter("@PurchaseNo",PurchaseNo),
                new SqlParameter("@SupplierID",SupplierID),
                new SqlParameter("@GrossAmount", GrossAmount),
                new SqlParameter("@Discount",Discount),
                new SqlParameter("@NetAmount",NetAmount),
                new SqlParameter("@UpdatedBy", UpdatedBy),
                new SqlParameter("@DBServer",DBServer)
            };
                        dbHelper.ExecuteNonQuery("uspUpdatePurchase", parameters,conn,transaction);
                        foreach (DataRow dr in dtItems.Rows)
                        {
                            SqlParameter[] itemParams = {
                    new SqlParameter("@PurchaseID", PurchaseID),
                    new SqlParameter("@ItemID", dr["ItemID"]),
                    new SqlParameter("@ColorID", dr["ColorID"]),
                    new SqlParameter("@Price", dr["Price"]),
                    new SqlParameter("@Quantity",dr["Quantity"]),
                    new SqlParameter("@Amount",Convert.ToInt32(dr["Quantity"]) * Convert.ToInt32(dr["Price"])),
                    new SqlParameter("@SizeID",dr["SizeID"]),
                    new SqlParameter("@StockDate",dr["StockDate"])
                };
                            dbHelper.ExecuteNonQuery("uspInsertPurchaseDetail", itemParams,conn,transaction);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        public DataTable GetPurchase(int TypeID,int PurchaseID,int DBServer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID),
                new SqlParameter("@PurchaseID", PurchaseID),
                new SqlParameter("@DBServer",DBServer)
            };
            return dbHelper.ExecuteDataTable("uspGetPurchase", parameters);
        }
    }
}