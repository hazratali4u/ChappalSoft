using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using System.Data.SQLite;
using System.Threading;
using System.Management;
using System.IO;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace ChappalUtility
{
    public partial class frmStock : Form
    {
        string conString = System.Configuration.ConfigurationSettings.AppSettings["connString"].ToString();
        private BackgroundWorker backgroundWorker;

        public frmStock(Main Parent)
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            InitializeComponent();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("ItemID", typeof(int));
            dtItem.Columns.Add("Name", typeof(string));

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT ItemID, Name FROM chappal.tblItem WHERE ItemID IN (" + txtItemID.Text + ")"), con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DataRow drNew = dtItem.NewRow();
                            drNew["ItemID"] = dr[0];
                            drNew["Name"] = dr[1];
                            dtItem.Rows.Add(drNew);
                        }
                    }
                }
                con.Close();
            }

            DataTable dtColor = new DataTable();
            dtColor.Columns.Add("ColorID", typeof(int));
            dtColor.Columns.Add("Name", typeof(string));

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT ColorID, Name FROM chappal.tblColor"), con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DataRow drNew = dtColor.NewRow();
                            drNew["ColorID"] = dr[0];
                            drNew["Name"] = dr[1];
                            dtColor.Rows.Add(drNew);
                        }
                    }
                }
                con.Close();
            }

            DataTable dtSize = new DataTable();
            dtSize.Columns.Add("SizeID", typeof(int));
            dtSize.Columns.Add("Name", typeof(string));

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT SizeID, Name FROM chappal.tblSize"), con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DataRow drNew = dtSize.NewRow();
                            drNew["SizeID"] = dr[0];
                            drNew["Name"] = dr[1];
                            dtSize.Rows.Add(drNew);
                        }
                    }
                }
                con.Close();
            }
            
            int itemID = 0;
            int colorID = 0;
            int sizeID = 0;
            foreach (DataRow dr in dtItem.Rows)
            {
                //Delete Existing Stock
                itemID = Convert.ToInt32(dr["ItemID"]);

                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(string.Format("DELETE FROM chappal.tblStock WHERE ItemID =" + itemID), con))
                    {
                        if (cmd.ExecuteNonQuery() > 0)
                        {

                        }
                    }
                    con.Close();
                }
                DataTable dtSale = new DataTable();
                dtSale.Columns.Add("Sold", typeof(int));

                DataTable dtPurchase = new DataTable();
                dtPurchase.Columns.Add("Purchase", typeof(int));
                int Purchase = 0;
                int Sold = 0;

                DateTime StartDate = Convert.ToDateTime("2025-05-20");
                while (StartDate < System.DateTime.Now.AddDays(-1))
                {
                    foreach (DataRow drColor in dtColor.Rows)
                    {
                        colorID = Convert.ToInt32(drColor["ColorID"]);
                        foreach (DataRow drSize in dtSize.Rows)
                        {
                            sizeID = Convert.ToInt32(drSize["SizeID"]);

                            dtPurchase.Rows.Clear();
                            using (SqlConnection con = new SqlConnection(conString))
                            {
                                con.Open();
                                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT ISNULL(D.Quantity,0) AS Quantity \r\nFROM chappal.tblPurchase P\r\nINNER JOIN chappal.tblPurchaseDetail D ON P.PurchaseID = D.PurchaseID\r\nWHERE CONVERT(DATE, P.TimeStamp) = CONVERT(DATE,'" + StartDate.ToString("yyyy-MM-dd") + "')\r\nAND D.ItemID = " + itemID + " AND D.ColorID = " + colorID + " AND D.SizeID = " + sizeID + ""), con))
                                {
                                    using (IDataReader drPur = cmd.ExecuteReader())
                                    {
                                        while (drPur.Read())
                                        {
                                            DataRow drNew = dtPurchase.NewRow();
                                            drNew["Purchase"] = drPur[0];
                                            dtPurchase.Rows.Add(drNew);
                                        }
                                    }
                                }
                                con.Close();
                            }
                            Purchase = 0;
                            if (dtPurchase.Rows.Count > 0)
                            {
                                Purchase = Convert.ToInt32(dtPurchase.Rows[0]["Purchase"]);
                            }
                            dtSale.Rows.Clear();
                            using (SqlConnection con = new SqlConnection(conString))
                            {
                                con.Open();
                                using (SqlCommand cmd = new SqlCommand(string.Format("SELECT ISNULL(D.Quantity,0) AS Quantity \r\nFROM chappal.tblSale P\r\nINNER JOIN chappal.tblSaleDetail D ON P.SaleID = D.SaleID\r\nWHERE CONVERT(DATE, P.TimeStamp) = CONVERT(DATE,'" + StartDate.ToString("yyyy-MM-dd") + "')\r\nAND D.ItemID = " + itemID + " AND D.ColorID = " + colorID + " AND D.SizeID = " + sizeID + ""), con))
                                {
                                    using (IDataReader drPur = cmd.ExecuteReader())
                                    {
                                        while (drPur.Read())
                                        {
                                            DataRow drNew = dtSale.NewRow();
                                            drNew["Sold"] = drPur[0];
                                            dtSale.Rows.Add(drNew);
                                        }
                                    }
                                }
                                con.Close();
                            }
                            Sold = 0;
                            if (dtSale.Rows.Count > 0)
                            {
                                Sold = Convert.ToInt32(dtSale.Rows[0]["Sold"]);
                            }
                            if (Sold > 0 || Purchase > 0)
                            {
                                StringBuilder sbScript = new StringBuilder();
                                sbScript.Append("IF EXISTS(\r\nSELECT 1\r\nFROM [chappal].[tblStock]\r\nWHERE StockDate = CONVERT(DATE,'" + StartDate.ToString("yyyy-MM-dd") + "')\r\nAND ItemID = " + itemID + "\r\nAND ColorID = " + colorID + "\r\nAND SizeID = " + sizeID + "\r\n)");
                                sbScript.Append("BEGIN\r\n\tUPDATE [chappal].[tblStock]\r\n\tSET Purchased = " + Purchase + ",Sold=" + Sold + "\r\n\tWHERE StockDate = CONVERT(DATE,'" + StartDate.ToString("yyyy-MM-dd") + "')\r\n\tAND ItemID = " + itemID + "\r\n\tAND ColorID = " + colorID + "\r\n\tAND SizeID = " + sizeID + "\r\nEND ");
                                sbScript.Append("ELSE\r\nBEGIN\r\nINSERT INTO [chappal].[tblStock](StockDate\r\n,ItemID\r\n,ColorID\r\n,SizeID\r\n,Opening\r\n,Sold\r\n,Purchased\r\n,Closing\r\n)\r\nVALUES(\r\nCONVERT(DATE,'" + StartDate.ToString("yyyy-MM-dd") + "')\r\n," + itemID + "\r\n," + colorID + "\r\n," + sizeID + "\r\n,0\r\n," + Sold + "\r\n," + Purchase + "\r\n,0\r\n)\r\nEND");

                                using (SqlConnection con = new SqlConnection(conString))
                                {
                                    con.Open();
                                    using (SqlCommand cmd = new SqlCommand(string.Format(sbScript.ToString()), con))
                                    {
                                        if (cmd.ExecuteNonQuery() > 0)
                                        {

                                        }
                                    }
                                    con.Close();
                                }                                
                            }

                            StringBuilder sbClosingStock = new StringBuilder();
                            sbClosingStock.Append("UPDATE chappal.tblStock  \r\nSET Closing = Opening + Purchased - Sold  \r\nWHERE StockDate = CONVERT(DATE,'" + StartDate.ToString("yyyy-MM-dd") + "')");
                            sbClosingStock.Append(" AND ItemID=" + itemID + " AND ColorID=" + colorID + " AND SizeID=" + sizeID);
                            using (SqlConnection con = new SqlConnection(conString))
                            {
                                con.Open();
                                using (SqlCommand cmd = new SqlCommand(string.Format(sbClosingStock.ToString()), con))
                                {
                                    if (cmd.ExecuteNonQuery() > 0)
                                    {

                                    }
                                }
                                con.Close();
                            }

                            StringBuilder sbOpening = new StringBuilder();
                            sbOpening.Append("INSERT INTO chappal.tblStock  \r\n(  \r\nStockDate  \r\n,ItemID  \r\n,ColorID  \r\n,SizeID  \r\n,Opening  \r\n,Sold  \r\n,Purchased  \r\n,Closing)  \r\nSELECT DATEADD(DAY,1,CONVERT(DATE,'" + StartDate.ToString("yyyy-MM-dd") + "'))  \r\n,ItemID  \r\n,ColorID  \r\n,SizeID  \r\n,Closing  \r\n,0  \r\n,0  \r\n,0  \r\nFROM chappal.tblStock  \r\nWHERE StockDate = CONVERT(DATE,'" + StartDate.ToString("yyyy-MM-dd") + "')");
                            sbOpening.Append(" AND ItemID=" + itemID + " AND ColorID=" + colorID + " AND SizeID=" + sizeID);
                            using (SqlConnection con = new SqlConnection(conString))
                            {
                                con.Open();
                                using (SqlCommand cmd = new SqlCommand(string.Format(sbOpening.ToString()), con))
                                {
                                    if (cmd.ExecuteNonQuery() > 0)
                                    {

                                    }
                                }
                                con.Close();
                            }
                        }
                    }

                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        lblStatus.Text = "Item ID:" + dr["ItemID"].ToString() + " | Item Name:" + dr["Name"].ToString() + " | Stock Date: " + StartDate.ToString("dd-MMM-yyyy");
                    }));

                    StartDate = StartDate.AddDays(1);
                }                
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ZKMessgeBox.Show("Stock updated.");
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            if(txtItemID.Text.Trim().Length == 0)
            {
                ZKMessgeBox.Show("Enter Item IDs.");
                txtItemID.Focus();
                return;
            }
            var confirmResult = ZKMessgeBox.Show("Are you sure to update stock??", "Confirm Stock Update!!", ZKMessgeBox.I8Buttons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }
    }
}