using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ChappalPrintService
{
    public partial class InvoicePrint : ServiceBase
    {
        string PrinterName = string.Empty;
        string ConactNumber = string.Empty;
        string InvoiceFooterNoteShort = string.Empty;
        string AddressShort = string.Empty;

        #region Form Variables 
        static string path = string.Empty;
        static string conString = "";
        Timer timer = new Timer();
        static DataTable dtValue = new DataTable();
        static DataTable dtValueInvoice = new DataTable();
        #endregion

        #region Windows Service Functions
        public InvoicePrint()
        {
            InitializeComponent();
            path = AppDomain.CurrentDomain.BaseDirectory;
            conString = Decrypt(System.Configuration.ConfigurationManager.AppSettings["connString"].ToString(), "b0tin@74");
            timer.Elapsed += PerformTimerOperationCrystalReport;
            timer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            timer.Start();

        }
        protected override void OnStart(string[] args)
        {
            WriteLog("Service Started.", "OnStart(string[] args)");
            WriteLog("Version: 10-Dec-2025 06:20 PM", "OnStart");
            WriteLog("PerformTimerOperationCrystalReport", "OnStart");
            PrinterName = System.Configuration.ConfigurationManager.AppSettings["PrinterName"].ToString();
            DataTable dtShop = GetShopInfo(1);
            if(dtShop.Rows.Count > 0)
            {
                ConactNumber = dtShop.Rows[0]["ConactNumber"].ToString();
                InvoiceFooterNoteShort = dtShop.Rows[0]["InvoiceFooterNoteShort"].ToString();
                AddressShort = dtShop.Rows[0]["AddressShort"].ToString();
            }
        }
        protected override void OnStop()
        {
            WriteLog("Service Stopped.", "OnStop()");
        }
        #endregion

        #region Timer Functions
        #region Crystal Report function
        void PerformTimerOperationCrystalReport(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                PrintCrystalReportInvoice();
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "PerformTimerOperationCrystalReport(object sender, ElapsedEventArgs e)");
            }
            finally
            {
                timer.Start();
            }
        }
        #endregion
        #endregion

        #region Printing Functions
        private void PrintCrystalReportInvoice()
        {
            try
            {
                DataTable dtOrders = GetPrintingInvoices(1);
                if (dtOrders.Rows.Count > 0)
                {                    
                    WriteLog(string.Format("Order No-{0}- Started Printing.", dtOrders.Rows[0]["SaleID"].ToString()), string.Empty);
                    if (PrintInvoiceReport(dtOrders, PrinterName))
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "PrintCrystalReportInvoice");
            }
        }
        #endregion

        #region Log Files
        private static void WriteLog(string Msg, string FunctionName)
        {
            string logFile = path + "PrintLog.txt";
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine($"{DateTime.Now}: {Msg}");
                        sw.Flush();
                        fs.Flush(true);
                    }
                    break;
                }
                catch (IOException)
                {
                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Logging failed: " + ex.Message);
                    break;
                }
            }
        }

        #endregion

        #region Decryp
        public static string Decrypt(string EncryptedText, string Key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(Key);
            byte[] buffer = Convert.FromBase64String(EncryptedText);
            MemoryStream stream = new MemoryStream();
            try
            {
                DES des = new DESCryptoServiceProvider();
                CryptoStream stream2 = new CryptoStream(stream, des.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "Decrypt(string EncryptedText, string Key)");
            }
            return Encoding.UTF8.GetString(stream.ToArray());
        }
        #endregion

        #region Get Data
        public DataTable GetPrintingInvoices(int TypeID)
        {
            DataTable dtOrders = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 120;
                        cmd.CommandText = "uspGetPrintInvoices";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter()
                        {
                            ParameterName = "@TypeID",
                            DbType = DbType.Int32,
                            Value = TypeID
                        };
                        pparams.Add(parameter);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dtOrders.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "GetPrintingInvoices(int TypeID, long SaleInvoiceID, long CustomerID)");
            }
            return dtOrders;
        }
        public DataTable GetShopInfo(int ShopID)
        {
            DataTable dtOrders = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 120;
                        cmd.CommandText = "uspGetShop";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter()
                        {
                            ParameterName = "@ShopID",
                            DbType = DbType.Int32,
                            Value = ShopID
                        };
                        pparams.Add(parameter);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dtOrders.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "GetPrintingInvoices(int TypeID, long SaleInvoiceID, long CustomerID)");
            }
            return dtOrders;
        }
        #endregion

        #region Update Data
        public bool UpdatePrinted(int SaleID)
        {
            bool flag = false;
            int maxRetries = 3;
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("uspUpdatePrinted", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 120;

                            cmd.Parameters.Add(new SqlParameter("@SaleID", SqlDbType.Int) { Value = SaleID });                            

                            cmd.ExecuteNonQuery();
                            flag = true;
                        }
                    }
                    break;
                }
                catch (SqlException ex)
                {

                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString(), "UpdatePrinted");
                    flag = false;
                    break;
                }
            }

            return flag;
        }
        #endregion                

        #region Crystal Reports
        private bool PrintInvoiceReport(DataTable dtValueOrder, string PrinterName)
        {
            string ServiceExecutionType = "0";
            bool flag = true;
            if (dtValueOrder.Rows.Count > 0)
            {
                if (PrinterName.Length > 0)
                {
                    try
                    {
                        using (ReportDocument report = (ReportDocument)new CrpInvoice())
                        {
                            report.SetDataSource(dtValueOrder);
                            report.Refresh();
                            if (ServiceExecutionType == "0")
                            {
                                report.PrintOptions.PrinterName = PrinterName;
                            }
                            report.SetParameterValue("ConactNumber", ConactNumber);
                            report.SetParameterValue("InvoiceFooterNoteShort", InvoiceFooterNoteShort);
                            report.SetParameterValue("AddressShort", AddressShort);
                            string InvoiceChar = DateTime.Now.ToString("dddd").Substring(0, 1);
                            string dummyinvoiceno =  (1718 + Convert.ToInt32(dtValueOrder.Rows[0]["SaleID"])).ToString();
                            report.SetParameterValue("InvoiceNo", dtValueOrder.Rows[0]["InvoiceNo"].ToString() + InvoiceChar + dummyinvoiceno);
                            // --- Print ---
                            if (ServiceExecutionType == "1")
                            {
                                string pdfPath = @"C:\Reports\Invoices_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
                                ExportOptions exportOptions = new ExportOptions();
                                DiskFileDestinationOptions diskOptions = new DiskFileDestinationOptions();
                                diskOptions.DiskFileName = pdfPath;
                                exportOptions = report.ExportOptions;
                                exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                                exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                                exportOptions.DestinationOptions = diskOptions;
                                report.Export();                                
                            }
                            else
                            {
                                report.PrintToPrinter(1, false, 0, 0);
                            }
                            if (!UpdatePrinted(Convert.ToInt32(dtValueOrder.Rows[0]["SaleID"])))
                            {
                                WriteLog("KOT Print not updated.", string.Empty);
                            }

                        }
                        WriteLog($"Order No-{dtValueOrder.Rows[0]["SaleID"].ToString()}-  Printed.", string.Empty);
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Print Error: " + ex.Message, "PrintInvoiceReport");
                        flag = false;
                    }

                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }
        #endregion
    }
}