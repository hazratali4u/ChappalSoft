using ChappalBLL;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Web;

public partial class MasterPage : System.Web.UI.MasterPage
{
    MenuController menu = new MenuController();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadJSFile();            
            int RoleID = CookieContext.UserID;
            ltrlEmployeeName.Text = CookieContext.EmployeeName;
            GetMenu(RoleID);
        }
    }

    private void GetMenu(int RoleID)
    {
        int no = 0;        
        DataTable dtMenu = menu.GetMenu(Convert.ToInt32(RoleID));
        System.Text.StringBuilder sbMenu = new System.Text.StringBuilder();

        //Main ul started
        sbMenu.Append("<ul class='nav'>");
        foreach (DataRow dr in dtMenu.Rows)
        {
            if (dr["MenuType"].ToString() == "1")
            {
                DataTable dtSubMenu = GetSubMenu(dtMenu, dr["MenuID"].ToString());
                if (dtSubMenu.Rows.Count > 0)
                {
                    //li 
                    sbMenu.Append("<li class='nav-item'>");
                    sbMenu.Append("<a class='nav-link' data-toggle='collapse' href='#ui-basic" + no + "' aria-expanded='false' aria-controls='ui-basic" + no + "'>");
                    switch (dr["MenuID"].ToString())
                    {
                        case "1":
                            sbMenu.Append("<i class='mdi mdi-application menu-icon'></i>");
                            break;
                        case "2":
                            sbMenu.Append("<i class='mdi mdi-settings menu-icon'></i>");                            
                            break;
                        case "3":
                            sbMenu.Append("<i class='mdi mdi-table-edit menu-icon'></i>");                            
                            break;
                        case "4":
                            sbMenu.Append("<i class='mdi mdi-table-large menu-icon'></i>");                            
                            break;
                        case "5":
                            sbMenu.Append("<i class='mdi mdi-bank menu-icon'></i>");                            
                            break;
                        case "6":
                            sbMenu.Append("<i class='mdi mdi-matrix menu-icon'></i>");
                            break;
                    }
                    sbMenu.Append("<span class='menu-title' title='" + dr["MenuName"].ToString() + "'>" + dr["MenuName"].ToString() + "</span>");
                    sbMenu.Append("<i class='menu-arrow'></i>");
                    sbMenu.Append("</a>");
                    sbMenu.Append("<div class='collapse' id='ui-basic" + no + "'>");
                    sbMenu.Append("<ul class='nav flex-column sub-menu'>");
                    //Load Sub Menu                
                    foreach (DataRow drSubMenu in dtSubMenu.Rows)
                    {
                        sbMenu.Append("<li class='nav-item'>");
                        sbMenu.Append("<a title='" + drSubMenu["MenuName"].ToString() + "' class='nav-link' href=" + drSubMenu["MenuURL"].ToString() + ">" + drSubMenu["MenuName"].ToString() + "</a>");
                        sbMenu.Append("</li>");
                    }
                    sbMenu.Append("</ul>");
                    sbMenu.Append("</div>");
                    //li 
                    sbMenu.Append("</li>");
                    no++;
                }
            }
        }
        //Main ul ended
        sbMenu.Append("</ul>");
        ltrlMenu.Text = sbMenu.ToString();
    }

    private DataTable GetSubMenu(DataTable dtMenu, string MenuParentID)
    {
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add("MenuName", typeof(string));
        dtResult.Columns.Add("MenuURL", typeof(string));
        DataRow drResult;
        foreach (DataRow dr in dtMenu.Rows)
        {
            if (dr["MenuParentID"].ToString() == MenuParentID)
            {
                drResult = dtResult.NewRow();
                drResult["MenuName"] = dr["MenuName"];
                drResult["MenuURL"] = dr["MenuURL"];
                dtResult.Rows.Add(drResult);
            }
        }
        return dtResult;
    }

    private void LoadJSFile()
    {
        string url = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
        switch(url)
        {
            case "Sale.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/Sale6.js'></script> <script src='AjazLibrary/moment-with-locales.js'></script>";
                break;
            case "SaleBarCode.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/SaleBarCode6.js'></script> <script src='AjazLibrary/moment-with-locales.js'></script>";
                break;
            case "SaleCoat.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/SaleCoat6.js'></script> <script src='AjazLibrary/moment-with-locales.js'></script>";
                break;
            case "Shop.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/Shop.js'></script>";
                break;
            case "User.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/User.js'></script>";
                break;
            case "Category.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/Category.js'></script>";
                break;
            case "Color.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/Color.js'></script>";
                break;
            case "Size.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/Size.js'></script>";
                break;
            case "Item.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/Item.js'></script>";
                break;
            case "Price.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/Price.js'></script>";
                break;
            case "Purchase.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/Purchase.js'></script>";
                break;
            case "Party.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/Party.js'></script>";
                break;
            case "rptItem.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/ReportItem.js'></script>";
                break;
            case "rptCustomer.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/ReportCustomer.js'></script>";
                break;
            case "rptSales.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/ReportSales.js'></script>";
                break;
            case "rptPurchase.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/ReportPurchase.js'></script>";
                break;
            case "Reprint.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/Reprint2.js'></script>";
                break;
            case "BarCode.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/BarCodeSticker.js'></script>";
                break;
            case "BarCode2.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/BarCodeSticker3.js'></script>";
                break;
            case "rptDocument.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/ReportDocument.js'></script>";
                break;
            case "POSBarCode.aspx":
                ltrlFiles.Text = "<script src='AjazLibrary/POSBarCode.js'></script>";
                break;
        }        
    }   
}