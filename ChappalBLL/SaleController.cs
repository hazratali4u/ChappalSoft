using System;
using System.Data;
using System.Data.SqlClient;
using ChappalDAL;
using Newtonsoft.Json;

namespace ChappalBLL
{
    public class SaleController
    {
        private string connectionString = System.Configuration.ConfigurationSettings.AppSettings["connString"].ToString();
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public int InsertSale(int SaleType, int CustomerID, int PaymentMode, int GrossAmount, int Discount, int AmountPaid, DataTable dtItems, int CreatedBy,DateTime DocDate,bool IsPrinted, int DBServer)
        {
            int SaleID = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    SqlParameter[] parameters = {
                new SqlParameter("@SaleType",SaleType),
                new SqlParameter("@CustomerID",CustomerID),
                new SqlParameter("@PaymentMode", PaymentMode),
                new SqlParameter("@GrossAmount", GrossAmount),
                new SqlParameter("@Discount",Discount),
                new SqlParameter("@AmountPaid",AmountPaid),
                new SqlParameter("@CreatedBy", CreatedBy),
                new SqlParameter("@DocDate",DocDate),
                new SqlParameter("@IsPrinted",IsPrinted),
                new SqlParameter("@DBServer",DBServer)
            };
                    SaleID = Convert.ToInt32(dbHelper.ExecuteScalar("uspInsertSale", parameters, conn, transaction));
                    if (SaleID > 0)
                    {
                        foreach (DataRow dr in dtItems.Rows)
                        {
                            SqlParameter[] itemParams = {
                    new SqlParameter("@SaleID", SaleID),
                    new SqlParameter("@ItemID", dr["ItemID"]),
                    new SqlParameter("@ColorID", dr["ColorID"]),
                    new SqlParameter("@Price", dr["Price"]),
                    new SqlParameter("@Quantity",dr["Quantity"]),
                    new SqlParameter("@Amount",Convert.ToInt32(dr["Quantity"]) * Convert.ToInt32(dr["Price"])),
                    new SqlParameter("@SizeID",dr["SizeID"]),
                    new SqlParameter("@StockDate",DocDate)
                };
                            dbHelper.ExecuteNonQuery("uspInsertSaleDetail", itemParams, conn, transaction);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return 0;
                }
            }
            return SaleID;
        }

        public bool UpdateSale(int SaleID, DataTable dtItems, int UpdateBy, DateTime DocDate, int DBServer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    foreach (DataRow dr in dtItems.Rows)
                    {
                        if (Convert.ToInt32(dr["Quantity"]) > 0)
                        {
                            SqlParameter[] itemParams = {
                    new SqlParameter("@SaleID", SaleID),
                    new SqlParameter("@ItemID", dr["ItemID"]),
                    new SqlParameter("@ColorID", dr["ColorID"]),
                    new SqlParameter("@SizeID",dr["SizeID"]),
                    new SqlParameter("@Price", dr["Price"]),
                    new SqlParameter("@Quantity",dr["Quantity"]),
                    new SqlParameter("@QuantityStock",dr["QuantityStock"]),
                    new SqlParameter("@Amount",dr["Amount"]),
                    new SqlParameter("@StockDate",DocDate)
                };
                            dbHelper.ExecuteNonQuery("uspInsertSaleDetailOnEdit", itemParams, conn, transaction);
                        }
                    }

                    SqlParameter[] parameters = {
                new SqlParameter("@SaleID",SaleID),
                new SqlParameter("@UpdateBy", UpdateBy),
                new SqlParameter("@DBServer",DBServer)
            };
                    dbHelper.ExecuteNonQuery("uspuUpdateSale", parameters, conn, transaction);

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

        public bool UpdateSale(int SaleID, int LedgerAmount, int TypeID)
        {
            
            try
            {
                SqlParameter[] parameters = {
                new SqlParameter("@SaleID",SaleID),
                new SqlParameter("@LedgerAmount",LedgerAmount),
                new SqlParameter("@TypeID", TypeID)
            };
                dbHelper.ExecuteNonQuery("uspUpdateSale", parameters);                
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public DataTable GetSale(int SaleID,int DBServer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@SaleID", SaleID),
                new SqlParameter("@DBServer",DBServer)
            };
            return dbHelper.ExecuteDataTable("uspGetSale", parameters);
        }

        public DataTable GetSaleDetail(int SaleID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@SaleID", SaleID)
            };
            return dbHelper.ExecuteDataTable("uspGetSaleDetail", parameters);
        }
    }
}