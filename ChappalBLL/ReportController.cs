using System;
using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class ReportController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        public DataTable GetStockReport(int CategoryID, int ItemID, DateTime dtFrom, DateTime dtTo,int ReportType)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@CategoryID", CategoryID),
                new SqlParameter("@ItemID", ItemID),
                new SqlParameter("@dtFrom", dtFrom),
                new SqlParameter("@dtTo", dtTo),
                new SqlParameter("@ReportType",ReportType)
            };
            return dbHelper.ExecuteDataTable("uspGetStockReport", parameters);
        }
        public DataSet GetLedgerReport(int CustomerID, DateTime dtFrom, DateTime dtTo, int ReportType)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@CustomerID", CustomerID),
                new SqlParameter("@dtFrom", dtFrom),
                new SqlParameter("@dtTo", dtTo),
                new SqlParameter("@ReportType",ReportType)
            };
            return dbHelper.ExecuteDataSet("uspGetCustomerLedger", parameters);
        }
        public DataTable GetSalesReport(int CategoryID, int ItemID, DateTime dtFrom, DateTime dtTo, int ReportType)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@CategoryID", CategoryID),
                new SqlParameter("@ItemID", ItemID),
                new SqlParameter("@dtFrom", dtFrom),
                new SqlParameter("@dtTo", dtTo),
                new SqlParameter("@ReportType",ReportType)
            };
            return dbHelper.ExecuteDataTable("uspGetSalesReport", parameters);
        }
        public DataTable GetPurchaseReport(int CategoryID, int ItemID, DateTime dtFrom, DateTime dtTo, int ReportType)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@CategoryID", CategoryID),
                new SqlParameter("@ItemID", ItemID),
                new SqlParameter("@dtFrom", dtFrom),
                new SqlParameter("@dtTo", dtTo),
                new SqlParameter("@ReportType",ReportType)
            };
            return dbHelper.ExecuteDataTable("uspGetPurchaseReport", parameters);
        }
        public DataTable GetDocumentReport(int PartyID, DateTime dtFrom, DateTime dtTo, int ReportType,int ViewType)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@PartyID", PartyID),
                new SqlParameter("@dtFrom", dtFrom),
                new SqlParameter("@dtTo", dtTo),
                new SqlParameter("@ReportType",ReportType),
                new SqlParameter("@ViewType",ViewType)
            };
            return dbHelper.ExecuteDataTable("uspGetDocumentReport", parameters);
        }
    }
}