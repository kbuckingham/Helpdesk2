using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using Microsoft.Win32;
using System.Management;
using System.Configuration;
using System.Security.Principal;


/// <summary>
/// Summary description for Inventory
/// </summary>
namespace HelpDesk
{
    public class Item
    {

        #region Attributes
       
        public string PON = "";                 //BASE
        public string INVN = "";                //BASE
        public string SerialN = "";             //BASE
        public string PartN = "";               //BASE
        public string Description = "";         //BASE
        public string Manufact = "";            //BASE
        public DateTime PurchaseDate = Convert.ToDateTime("01/01/1111 12:00:00 AM");    //BASE
        public DateTime WarrantyEndDate = Convert.ToDateTime("01/01/1111 12:00:00 AM"); //BASE
        public DateTime RefreshDate = Convert.ToDateTime("01/01/1111 12:00:00 AM");     //BASE
        public bool Refresh = false;            //BASE
        public string Dnsname = "";             //NETWORKITEM
        public string Mac = "";                 //NETWORKITEM
        public string IP = "";                  //NETWORKITEM
        public bool Networked = false;          //REMOVE
        public bool IsUpdatedinTI = true;       //BASE
        public string Addedby = "";             //BASE
        public string LastModifiedby = "";      //BASE
        public string ContractN = "";           //BASE
        public string ComputerReplacing = "";   //NETWORKITEM
        public bool found = false;              //BASE ??
        public string UserPhone = "";           //REMOVE
        public double ItemCost = 0;             //BASE
        public double ReplacementCost = 0;      //BASE
        public string Condition = "";           //BASE
        public bool IsSoftware = false;         //BASE

        private int _ItemID = -1;               //BASE

        private string _Type = "";              //BASE
        private int _TypeID = 46;               //BASE
        private string _User = "";              //BASE
        private int _UserID = -1;               //BASE
        private string _Building = "";          //BASE
        private int _BuildingID = -1;           //BASE
        private string _Department = "";        //BASE
        private int _DepartID = -1;             //BASE
        private string _Vend = "";              //BASE
        private int _VendID = 2;               //BASE
        private string _Status = "";            //BASE
        private int _StatusID = -1;             //BASE
        private int _WarrantyID = 7;           //BASE
        private string _Service = "";           //BASE
        private string _ServiceN = "";          //BASE
        private string _ServiceWeb = "";        //BASE

        #endregion

        #region Properties

        public int ItemID
        {
            get { return _ItemID; }
        }


        public int TypeID
        {
            get { return _TypeID; }
            set
            {
                try
                {
                    _TypeID = value;
                    DataTable TypeDB = Sql.CCSelect("Select [TYPE] from [TYPE] where [TYPE].TYPEID = " + value);
                    _Type = TypeDB.Rows[0]["TYPE"].ToString();
                }
                catch
                {
                    _Type = "";
                    _TypeID = -1;
                }
            }
        }

        public string Type
        {
            get { return _Type; }
            set
            {
                try
                {
                    _Type = value;
                    DataTable TypeDB = Sql.CCSelect("Select [TYPEID] from [TYPE] where [TYPE].TYPE = '" + value + "'");
                    _TypeID = Convert.ToInt32(TypeDB.Rows[0]["TYPEID"]);
                }
                catch
                {
                    _Type = "";
                    _TypeID = -1;
                }
            }
        }

        public int UserID
        {
            get { return _UserID; }
            set
            {
                try
                {
                    _UserID = value;
                    DataTable dt = Sql.CCSelect("Select [Name] from [User] where UserID = " + value);
                    _User = dt.Rows[0]["Name"].ToString();
                }
                catch 
                {
                    _User = "";
                    _UserID = -1;
                }
            }
        }

        public string User
        {
            get { return _User; }
            set 
            {
                try
                {
                    _User = value;
                    DataTable dt = Sql.CCSelect("Select UserID from [USER] where Name = '" + value + "'");
                    _UserID = Convert.ToInt32(dt.Rows[0]["UserID"]);
                }
                catch
                {
                    _User = "";
                    _UserID = -1;
                }
            }
        }

        public string Building
        {
            get { return _Building; }
            //set { _Building = value; }
        }

        public string Department
        {
            get { return _Department; }
            //set { _Department = value; }
        }

        public int VendID
        {
            get { return _VendID; }
            set 
            {
                try
                {
                    _VendID = value;
                    DataTable dt = Sql.CCSelect("Select VenderName from Vender where VenderID = " + value);
                    _Vend = dt.Rows[0]["VenderName"].ToString();

                }
                catch
                {
                    _Vend = "";
                    _VendID = -1;
                }
            }
        }

        public string Vend
        {
            get { return _Vend; }
        }
        
        public string Status
        {
            get { return _Status; }
            set
            {
                try
                {
                    _Status = value;
                    DataTable StatusDB = Sql.CCSelect("Select [ItemStatusID] from ItemStatus where StatusName = '" + value + "'");
                    _StatusID = Convert.ToInt32(StatusDB.Rows[0]["ItemStatusID"]);
                }
                catch
                {
                    _Status = "";
                    _StatusID = -1;
                }
            }
        }

        public int StatusID
        {
            get { return _StatusID; }
            set 
            {
                try
                {
                    _StatusID = value;
                    DataTable StatusDB = Sql.CCSelect("Select [StatusName] from ItemStatus where ItemStatusID = " + value);
                    _Status = StatusDB.Rows[0]["StatusName"].ToString();
                }
                catch
                {
                    _Status = "";
                    _StatusID = -1;
                }
            }
        }

        public int ServiceID
        {
            get { return _WarrantyID; }
            set
            {
                try
                {
                    _WarrantyID = value;
                    DataTable dt = Sql.CCSelect("Select * from Warranty_info where WarrantyID_PK = " + value);
                    _Service = dt.Rows[0]["ServiceName"].ToString();
                    _ServiceN = dt.Rows[0]["ContactNumber"].ToString();
                    _ServiceWeb = dt.Rows[0]["ContactWeb"].ToString();
                }
                catch
                {

                }
            }
        }

        public string Service
        {
            get { return _Service; }
        }

        public string ServicePhone
        {
            get { return _ServiceN; }
        }

        public string ServiceWeb
        {
            get { return _ServiceWeb; }
        }

        #endregion

        #region Constructors
        public Item()
        {

        }
        
        public Item(int ItemID)
        {
            DataTable itemTB = Sql.CCSelect("SELECT * FROM ITEM "
                                            + "left JOIN [TYPE] ON ITEM.TYPEID = [TYPE].TYPEID "
                                            + "left JOIN [USER] ON ITEM.USERID = [USER].USERID "
                                            + "left JOIN Dept_Location ON [USER].PID = Dept_Location.PID "
                                            + "left JOIN DEPARTMENT ON Dept_Location.DeptID = DEPARTMENT.DeptID "
                                            + "left JOIN LOCATION ON Dept_Location.BuildingID = LOCATION.BuildingID "
                                            + "left JOIN Vender ON ITEM.VENDERID = Vender.VenderID "
                                            + "left JOIN ItemStatus ON ITEM.ItemStatusIDFK = ItemStatus.ItemStatusID "
                                            + "left JOIN WARRANTY_INFO ON ITEM.WarrantyID_FK = WARRANTY_INFO.WarrantyID_PK "
                                            + "WHERE ITEMID = " + ItemID);

            if (itemTB.Rows.Count > 0)
            {
                found = true;
                DataRow r = itemTB.Rows[0];

                this._ItemID = ItemID;
                PON = r["PON"].ToString();
                INVN = r["INVN"].ToString();
                SerialN = r["SerialN"].ToString();
                PartN = r["PARTN"].ToString();
                Description = r["DESCRIPTION"].ToString();
                Manufact = r["MANUFACT"].ToString();
                Dnsname = r["DNSName"].ToString();
                Mac = r["MAC"].ToString();
                IP = r["IPAddress"].ToString();
                Condition = r["Condition"].ToString();
                Addedby = r["AddingMember"].ToString();
                LastModifiedby = r["LastEditingMember"].ToString();
                ComputerReplacing = r["DNSNameToReplace"].ToString();

                if (r["Networkable"] != DBNull.Value)
                    Networked = Convert.ToBoolean(r["Networkable"]);
                if (r["PurchaseDate"] != DBNull.Value)
                    PurchaseDate = Convert.ToDateTime(r["PurchaseDate"]);
                if (r["WarrantyDate"] != DBNull.Value)
                    WarrantyEndDate = Convert.ToDateTime(r["WarrantyDate"]);
                if (r["RefreshDate"] != DBNull.Value)
                    RefreshDate = Convert.ToDateTime(r["RefreshDate"]);

                if (r["refreshed"] != DBNull.Value)
                    Refresh = Convert.ToBoolean(r["Refreshed"]);
                if (r["AddedToTrackit"] != DBNull.Value)
                    IsUpdatedinTI = Convert.ToBoolean(r["AddedToTrackit"]);
                if (r["IsSoftware"] != DBNull.Value)
                    IsSoftware = Convert.ToBoolean(r["IsSoftware"]);
                
                
                if (r["ItemCost"] == DBNull.Value)
                    ItemCost = 0;
                else
                    ItemCost = Convert.ToDouble(r["ItemCost"]);

                if (r["ReplacementCost"] == DBNull.Value)
                    ReplacementCost = 0;
                else
                    ReplacementCost = Convert.ToDouble(r["ReplacementCost"]);

                if (r["extension"] == DBNull.Value)
                    UserPhone = "";
                else
                    UserPhone = r["extension"].ToString();

                if (r["TYPEID"] != DBNull.Value)
                {
                    _TypeID = Convert.ToInt32(r["TYPEID"]);
                    _Type = r["TYPE"].ToString();
                }

                if (r["USERID"] != DBNull.Value)
                {
                    _UserID = Convert.ToInt32(r["USERID"]);
                    _User = r["Name"].ToString();
                    _BuildingID = Convert.ToInt32(r["BuildingID"]);
                    _Building = r["Location"].ToString();
                    _DepartID = Convert.ToInt32(r["DeptID"]);
                    _Department = r["DeptName"].ToString();
                }

                if (r["VenderID"] != DBNull.Value)
                {
                    _VendID = Convert.ToInt32(r["VenderID"]);
                    _Vend = r["VenderName"].ToString();
                }

                if (r["ItemStatusIDFK"] != DBNull.Value)
                {
                    _StatusID = Convert.ToInt32(r["ItemStatusIDFK"]);
                    _Status = r["StatusName"].ToString();
                }

                if (r["WarrantyID_PK"] != DBNull.Value)
                {
                    _WarrantyID = Convert.ToInt32(r["WarrantyID_PK"]);
                    _Service = r["ServiceName"].ToString();
                    _ServiceN = r["ContactNumber"].ToString();
                    _ServiceWeb = r["ContactWeb"].ToString();
                    ContractN = r["ContractN"].ToString();
                }
                else
                    _WarrantyID = -1;
            }
            else
            {
                found = false;
            }

        }
        #endregion

        #region Member Methods

        public bool SuccessPing()
        {
            return Item.SuccessPing(IP);
        }

        public void Commit()
        {
            if (ItemID == -1)
            {
                _ItemID = AddNewItem(SerialN.ToUpper(), Description, Manufact, PurchaseDate, Refresh, _TypeID, _VendID, _UserID, PON, AD.UserName);
            }
            
            Sql.CCSelect("UPDATE ITEM " +
                            "SET INVN = '" + INVN + "', " +
                            "PON = '" + PON + "', " +
                            "SERIALN = '" + SerialN + "', " +
                            "PARTN = '" + PartN + "', " +
                            "VENDERID = " + _VendID + ", " +
                            "TYPEID = " + _TypeID + ", " +
                            "MANUFACT = '" + Manufact + "', " +
                            "PurchaseDate = '" + PurchaseDate + "', " +
                            "WarrantyDate = '" + WarrantyEndDate + "', " +
                            "ItemCost = '" + ItemCost + "', " +
                            "ReplacementCost = '" + ReplacementCost + "', " +
                            "RefreshDate = '" + RefreshDate + "', " +
                            "DNSName = '" + Dnsname + "', " +
                            "MAC = '" + Mac + "', " +
                            "IPAddress = '" + IP + "', " +
                            "ItemStatusIDFK = " + _StatusID + ", " +
                            "Refreshed = '" + Refresh + "', " +
                            "[DESCRIPTION] = '" + Description.Replace("'","''") + "', " +
                            "UserID = " + _UserID + ", " +
                            "AddedToTrackit = '" + IsUpdatedinTI + "', " +
                            "Condition = '" + Condition + "', " +
                            "ContractN = '" + ContractN + "', " +
                            "DNSNameToReplace = '" + ComputerReplacing + "', " +
                            "WarrantyID_FK = " + _WarrantyID + ", " +
                            "AddingMember = '" + Addedby + "', " +
                            "LastEditingMember = '" + LastModifiedby + "', " +
                            "IsSoftware = '" + IsSoftware + "' " +
                            "WHERE ITEMID = '" + ItemID + "'");
        }

        #endregion

        #region Static Methods

        //add new item to inventory under purchased status
        public static int AddNewItem(string description, string PON, int vendorid, DateTime PurchaseDate, double itemcost, string MemberAdding)
        {
            DataTable dt = Sql.CCSelect("Insert INTO [ITEM] ([DESCRIPTION],PON,VENDERID,PurchaseDate,ItemCost, AddingMember, LastEditingMember, AddedToTrackit, WarrantyID_FK) "
                                   + "VALUES ('" + description + "','" + PON + "'," + vendorid + ",'" + PurchaseDate + "'," + itemcost + ", '" + MemberAdding + "', '" + MemberAdding + "', 1, 7);" + "Select Scope_IDENTITY();");
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        //added to the item page
        public static int AddNewItem(string serial, string description, string manufacturer, DateTime PurchaseDate,
                                      bool refreshed, int typeid, int vendorid, int userid, string PON, string MemberAdding)
        {
            DataTable dt = Sql.CCSelect("Insert INTO [ITEM] (SERIALN,[DESCRIPTION],MANUFACT,PurchaseDate,Refreshed,TYPEID,VENDERID,PON,UserID,ItemStatusIDFK, WarrantyID_FK, ContractN, AddingMember, LastEditingMember, AddedToTrackit) "
                                   + "VALUES ('" + serial + "','" + description + "','" + manufacturer + "','" + PurchaseDate + "','" + refreshed + "'," + typeid + "," + vendorid + ",'" + PON + "'," + userid + ", 1, 7, '', '" + MemberAdding + "', '" + MemberAdding + "', 1);" + "Select Scope_IDENTITY();");
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        public static bool SuccessPing(string target)
        {
            Ping ping = new Ping();

            if (target == "" || target == null)
                return false;

            try
            {
                PingReply reply = ping.Send(IPAddress.Parse(target), 120);  //ping timeout

                if (reply.Status == IPStatus.Success)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }

        }

        //Compare two items. If there are any changes between old and new item, the changes will be
        //written to the LOG table
        public static void CompareItems(Item originalItem, Item changedItem)
        {
            string changes = " ";
            bool changed = false;
            changes = originalItem.ItemID.ToString();

            if (originalItem.StatusID != changedItem.StatusID)
            {
                changes += ", Status = From: " + originalItem.Status.ToString() + " To: " + changedItem.Status.ToString();
                changed = true;
            }

            if (originalItem.INVN != changedItem.INVN)
            {
                changes += ", Inv Number = From: " + originalItem.INVN.ToString() + " To: " + changedItem.INVN.ToString();
                changed = true;
            }

            if (originalItem.PON != changedItem.PON)
            {
                changes += ", PO Number = From: " + originalItem.PON.ToString() + " To: " + changedItem.PON.ToString();
                changed = true;
            }

            if (originalItem.SerialN != changedItem.SerialN)
            {
                changes += ", Serial Number = From: " + originalItem.SerialN.ToString() + " To: " + changedItem.SerialN.ToString();
                changed = true;
            }

            if (originalItem.PartN != changedItem.PartN)
            {
                changes += ", Part Number = From: " + originalItem.PartN.ToString() + " To: " + changedItem.PartN.ToString();
                changed = true;
            }

            if (originalItem.Description != changedItem.Description)
            {
                changes += ", Description = From: " + originalItem.Description.ToString().Replace("'","''") + " To: " + changedItem.Description.ToString().Replace("'","''");
                changed = true;
            }

            if (originalItem.Manufact != changedItem.Manufact)
            {
                changes += ", Manufacturer = From: " + originalItem.Manufact.ToString() + " To: " + changedItem.Manufact.ToString();
                changed = true;
            }

            if (originalItem.TypeID != changedItem.TypeID)
            {
                changes += ", Type = From: " + originalItem.Type.ToString() + " To: " + changedItem.Type.ToString();
                changed = true;
            }

            if (originalItem.Refresh != changedItem.Refresh)
            {
                changes += ", Refresh = From: " + originalItem.Refresh.ToString() + " To: " + changedItem.Refresh.ToString();
                changed = true;
            }

            if (originalItem.RefreshDate != changedItem.RefreshDate)
            {
                changes += ", RefreshDate = From: " + originalItem.RefreshDate.ToString() + " To: " + changedItem.RefreshDate.ToString();
                changed = true;
            }

            if (originalItem.IsUpdatedinTI != changedItem.IsUpdatedinTI)
            {
                changes += ", In TI = From: " + originalItem.IsUpdatedinTI.ToString() + " To: " + changedItem.IsUpdatedinTI.ToString();
                changed = true;
            }

            if (originalItem.Condition != changedItem.Condition)
            {
                changes += ", Condition = From: " + originalItem.Condition.ToString() + " To: " + changedItem.Condition.ToString();
                changed = true;
            }

            if (originalItem.UserID != changedItem.UserID)
            {
                changes += ", User = From: " + originalItem.User.ToString() + " To: " + changedItem.User.ToString();
                changed = true;
            }

            if (originalItem.ItemCost != changedItem.ItemCost)
            {
                changes += ", Item Cost = From: " + originalItem.ItemCost.ToString() + " To: " + changedItem.ItemCost.ToString();
                changed = true;
            }

            if (originalItem.ReplacementCost != changedItem.ReplacementCost)
            {
                changes += ", Replacement Cost = From: " + originalItem.ReplacementCost.ToString() + " To: " + changedItem.ReplacementCost.ToString();
                changed = true;
            }

            if (originalItem.VendID != changedItem.VendID)
            {
                changes += ", Vendor = From: " + originalItem.Vend.ToString() + " To: " + changedItem.Vend.ToString();
                changed = true;
            }

            if (originalItem.PurchaseDate != changedItem.PurchaseDate)
            {
                changes += ", Purch Date = From: " + originalItem.PurchaseDate.ToString() + " To: " + changedItem.PurchaseDate.ToString();
                changed = true;
            }

            if (originalItem.WarrantyEndDate != changedItem.WarrantyEndDate)
            {
                changes += ", Warranty Date = From: " + originalItem.WarrantyEndDate.ToString() + " To: " + changedItem.WarrantyEndDate.ToString();
                changed = true;
            }

            if (originalItem.ServiceID != changedItem.ServiceID)
            {
                changes += ", Warranty Service = From: " + originalItem.Service.ToString() + " To: " + changedItem.Service.ToString();
                changed = true;
            }

            if (originalItem.Dnsname != changedItem.Dnsname)
            {
                changes += ", DNS = From: " + originalItem.Dnsname.ToString() + " To: " + changedItem.Dnsname.ToString();
                changed = true;
            }

            if (originalItem.IP != changedItem.IP)
            {
                changes += ", IP = From: " + originalItem.IP.ToString() + " To: " + changedItem.IP.ToString();
                changed = true;
            }

            if (originalItem.Mac != changedItem.Mac)
            {
                changes += ", MAC = From: " + originalItem.Mac.ToString() + " To: " + changedItem.Mac.ToString();
                changed = true;
            }

            if (changed)
            {
                LOG.NewLog(changes, LogType.ItemModify);
            }


        }

        public static void BindTISyncGrid(GridView TIGrid)
        {
            TIGrid.DataSource = Sql.CCSelect("SELECT ITEM.PON, ITEM.INVN, ITEM.SERIALN, ITEM.PARTN, ITEM.DESCRIPTION, ITEM.MANUFACT, ITEM.PURCHASEDATE, ITEM.WARRANTYDATE, ITEM.Refreshed, LOCATION.Location, DEPARTMENT.DeptName, [USER].Name, TYPE.TYPE, ITEM.AddedToTrackit, ITEM.ITEMID "
                                         + "FROM TYPE left JOIN ITEM ON TYPE.TYPEID = ITEM.TYPEID "
                                         + "left JOIN [USER] ON ITEM.USERID = [USER].USERID "
                                         + "left JOIN [USER] AS USER_1 ON ITEM.USERID = USER_1.USERID "
                                         + "left JOIN Dept_Location ON [USER].PID = Dept_Location.PID AND USER_1.PID = Dept_Location.PID "
                                         + "left JOIN LOCATION ON Dept_Location.BuildingID = LOCATION.BuildingID "
                                         + "left JOIN DEPARTMENT ON Dept_Location.DeptID = DEPARTMENT.DeptID "
                                         + "WHERE (ITEM.AddedToTrackit = 0)");
            TIGrid.DataBind();
        }

        public static void BindStatusDLL(DropDownList DDL)
        {
            DDL.DataSource = Sql.CCSelect("Select * from ItemStatus");
            DDL.DataTextField = "StatusName";
            DDL.DataValueField = "ItemStatusID";
            DDL.DataBind();
        }

        public static void BindTypeDDL(DropDownList DDL)
        {
            DDL.DataSource = Sql.CCSelect("SELECT * FROM [TYPE] ORDER BY TYPE");
            DDL.DataTextField = "TYPE";
            DDL.DataValueField = "TYPEID";
            DDL.DataBind();
        }

        public static void BindVendorDDL(DropDownList DDL)
        {
            DDL.DataSource = Sql.CCSelect("SELECT * FROM [Vender] ORDER BY VenderName");
            DDL.DataTextField = "VenderName";
            DDL.DataValueField = "VenderID";
            DDL.DataBind();
        }

        public static void BindWarServiceDDL(DropDownList DDL)
        {
            DDL.DataSource = Sql.CCSelect("SELECT * FROM [Warranty_INFO] ORDER BY ServiceName");
            DDL.DataTextField = "ServiceName";
            DDL.DataValueField = "WarrantyID_PK";
            DDL.DataBind();
        }

        public static void BindChoiceDDL(DropDownList DDL)
        {
            DDL.Items.Add(new ListItem("Skip", "1"));
            DDL.Items.Add(new ListItem("Inventory", "2"));
            DDL.Items.Add(new ListItem("Receive Hardware", "3"));
            DDL.Items.Add(new ListItem("Receive Software", "4"));
        }

        public static void BindPODDL(DropDownList DDL)
        {
            DataTable dt = Sql.CCSelect("Select distinct PON from ITEM");
            ArrayList polist = new ArrayList();

            foreach (DataRow row in dt.Rows)
                polist.Add(row["PON"].ToString());

            //finds POs for the last 2 months for that are not already in inventory
            DataTable jenztable = Sql.JenzSelect("SELECT Distinct po_header.po_num, (po_header.po_dte) as podate, name_format_view_a.last_first_middle "
                                            + "FROM po_header "
                                            + "LEFT OUTER JOIN name_format_view name_format_view_a ON po_header.requester_id_num = name_format_view_a.id_num "
                                            + "LEFT OUTER JOIN name_format_view name_format_view_b ON po_header.id_num = name_format_view_b.id_num, trans_hist, trans_hist_ext "
                                            + "WHERE ( po_header.grp_num = trans_hist.group_num ) "
                                            + "AND   ( po_header.grp_num = trans_hist_ext.group_num ) "
                                            + "AND   ( ( po_header.pur_agent_id_num in ('33725','176624','34767','34686','31767','34333') ) "
                                            + "AND   ( trans_hist_ext.source_cde = 'po' ) "
                                            + "AND   ( trans_hist.source_cde = 'po' ) ) "
                                            + "AND   ( po_header.po_dte >= DATEADD(month, -2, GetDate())) "
                                            + "AND   (PO_Line_Num = 1) "
                                            //+ "AND   ( po_num not in ('"+PONs+"')) "
                                            + "ORDER BY podate DESC");
            DDL.DataSource = jenztable;
            DDL.DataTextField = "po_num";
            DDL.DataValueField = "po_num";
            DDL.DataBind();

            foreach (ListItem itm in DDL.Items)
            {
                if (polist.Contains(itm.Text.Trim()))
                {
                    itm.Attributes.Add("style", "background-color:#2e8e00"); 
                }
                
            }

            #region sql query
            //SELECT (po_header.po_dte) as podate, po_header.po_num, convert(decimal(10,2), COST_PER_UNIT) as costperunit, ITEM_DESC, trans_hist_ext.rcvd_dte, PO_Line_Num,
            //convert(decimal(10,0), ORDERED_QUANTITY) as qtyordered, convert(decimal(10,0),trans_hist_ext.rcvd_quantity) as qtyreceived, name_format_view_a.last_first_middle as requestedby, name_format_view_b.last_first_middle as vendor, PUR_AGENT_ID_NUM
            //FROM po_header 
            //LEFT OUTER JOIN name_format_view name_format_view_a ON po_header.requester_id_num = name_format_view_a.id_num 
            //LEFT OUTER JOIN name_format_view name_format_view_b ON po_header.id_num = name_format_view_b.id_num, trans_hist, trans_hist_ext 
            //WHERE ( po_header.grp_num = trans_hist.group_num ) 
            //AND   ( po_header.grp_num = trans_hist_ext.group_num ) 
            //AND   ( ( po_header.pur_agent_id_num in ('33725','176624','34767','34686','31767','34333') ) 
            //AND   ( trans_hist_ext.source_cde = 'po' ) 
            //AND   ( trans_hist.source_cde = 'po' ) ) 
            //AND   ( po_header.po_dte >= DATEADD(month, -2, GetDate()))
            //and   (PO_Line_Num = 1)
            //ORDER BY po_header.po_dte DESC
            #endregion
        }

        public static void BindNewDDL(DropDownList DDL)
        {
            DDL.Items.Add(new ListItem("Active", "1"));
            DDL.Items.Add(new ListItem("Storage", "2"));
            DDL.Items.Add(new ListItem("Ready", "3"));
            DDL.Items.Add(new ListItem("Receiving", "4"));
        }

        public static void BindReceiveDDL(DropDownList DDL)
        {
            DDL.DataSource = Sql.CCSelect("Select Distinct PON "
                                        + "from ITEM "
                                        + "Join ItemStatus on ItemStatusID = ITEM.ItemStatusIDFK "
                                        + "where ItemStatus.StatusName = 'Purchased'");
            DDL.DataTextField = "PON";
            DDL.DataValueField = "PON";
            DDL.DataBind();
        }

        public static void BindReceiveGrid(GridView gv, string pon)
        {
            gv.DataSource = Sql.CCSelect("Select * from ITEM "
                                       + "JOIN [USER] ON ITEM.USERID = [USER].USERID "
                                       + "JOIN Vender ON ITEM.VENDERID = Vender.VenderID "
                                       + "JOIN ItemStatus on ItemStatusID = ITEM.ItemStatusIDFK "
                                       + "where PON = '"+pon+"'");
            gv.DataBind();
            foreach (GridViewRow r in gv.Rows)
            {
                Label typeid = (Label)r.FindControl("typeid");
                Label inv = (Label)r.FindControl("inv");

                Label status = (Label)r.FindControl("status");
                Button recbtn = (Button)r.FindControl("recbtn");
                Image checkimg = (Image)r.FindControl("checkimg");

                if (typeid.Text == "59")
                    inv.Text = "No";
                else
                    inv.Text = "Yes";

                if (status.Text != "Purchased")
                {
                    recbtn.Visible = false;
                    checkimg.Visible = true;
                }

            }

        }

        //pull the jenz rows for the give PO # and fill the GridView
        public static void BindJenzItemGrid(GridView JenzGrid, string pon)
        {
            

            DataTable jenzresult = Sql.JenzSelect("SELECT (po_header.po_dte) as podate, po_header.po_num, convert(decimal(10,2), COST_PER_UNIT) as costperunit, ITEM_DESC, trans_hist_ext.rcvd_dte, PO_Line_Num, "
                                                + "convert(decimal(10,0), ORDERED_QUANTITY) as qtyordered, convert(decimal(10,0),trans_hist_ext.rcvd_quantity) as qtyreceived, name_format_view_a.last_first_middle as requestedby, name_format_view_b.last_first_middle as vendor, PUR_AGENT_ID_NUM "
                                                + "FROM po_header "
                                                + "LEFT OUTER JOIN name_format_view name_format_view_a ON po_header.requester_id_num = name_format_view_a.id_num "
                                                + "LEFT OUTER JOIN name_format_view name_format_view_b ON po_header.id_num = name_format_view_b.id_num, trans_hist, trans_hist_ext "
                                                + "WHERE ( po_header.grp_num = trans_hist.group_num ) "
                                                + "AND   ( po_header.grp_num = trans_hist_ext.group_num ) "
                                                + "AND   ( ( po_header.pur_agent_id_num in ('33725','176624','34767','34686','31767','34333') ) "
                                                + "AND   ( trans_hist_ext.source_cde = 'po' ) "
                                                + "AND   ( trans_hist.source_cde = 'po' ) ) "
                                                + "AND   ( po_header.po_num = '"+pon+"') "
                                                + "AND   (PO_Line_Num = 1) "
                                                + "ORDER BY podate DESC");


            #region old query
            //DataTable jenzresult = Sql.JenzSelect("SELECT (po_header.po_dte) as podate, po_header.po_num, trans_hist.trans_amt, trans_hist.trans_desc, trans_hist_ext.rcvd_dte, "
            //                                 + "convert(decimal(10,0),trans_hist_ext.ordered_quantity) as qtyordered, convert(decimal(10,0),trans_hist_ext.rcvd_quantity) as qtyreceived, name_format_view_a.last_first_middle as requestedby, name_format_view_b.last_first_middle as vendor, PUR_AGENT_ID_NUM "
            //                                 + "FROM po_header "
            //                                 + "LEFT OUTER JOIN name_format_view name_format_view_a ON po_header.requester_id_num = name_format_view_a.id_num "
            //                                 + "LEFT OUTER JOIN name_format_view name_format_view_b ON po_header.id_num = name_format_view_b.id_num, trans_hist, trans_hist_ext "
            //                                 + "WHERE ( po_header.grp_num = trans_hist.group_num ) "
            //                                 + "AND   ( po_header.grp_num = trans_hist_ext.group_num ) "
            //                                 + "AND   ( ( po_header.pur_agent_id_num in ('33725','176624','34767','34686','31767','34333') ) "
            //                                 + "AND   ( trans_hist_ext.source_cde = 'po' ) "
            //                                 + "AND   ( trans_hist.source_cde = 'po' ) ) "
            //                                 + "AND   ( po_header.po_dte >= DATEADD(month, -2, GetDate())) "
            //                                 + "AND   ( po_num not in ('"+PONs+"')) "
            //                                 + "ORDER BY po_header.po_dte DESC");
            #endregion

            JenzGrid.DataSource = jenzresult;
            JenzGrid.DataBind();

            DataTable dt = Sql.CCSelect("Select distinct PON from ITEM");
            ArrayList polist = new ArrayList();

            foreach (DataRow row in dt.Rows)
                polist.Add(row["PON"].ToString());

            foreach(GridViewRow r in JenzGrid.Rows)
            {
                Label ponum = (Label)r.FindControl("PONum");
                DropDownList choiceddl = (DropDownList)r.FindControl("choiceddl");

                BindChoiceDDL(choiceddl);

                if (polist.Contains(ponum.Text.Trim()))
                {
                    r.ForeColor = System.Drawing.Color.Gray;
                    choiceddl.Enabled = false;
                }
            }

        }

        public static DataTable ItemSearch(string searchfield)
        {
            return Sql.CCSelect("SELECT ITEM.ITEMID, [USER].Name AS [User], ITEM.INVN AS [Inv #], ITEM.PON AS [PO #], ITEM.SERIALN AS [Serial #],  ITEM.DESCRIPTION AS Description, "
                              + "TYPE.TYPE AS Type, ITEM.PurchaseDate AS [Purchase Date], ITEM.Refreshed AS [Refresh?], ItemStatus.StatusName "
                              + "FROM ITEM "
                              + "LEFT JOIN [USER] ON ITEM.USERID = [USER].USERID "
                              + "LEFT JOIN TYPE ON ITEM.TYPEID = TYPE.TYPEID "
                              + "JOIN ItemStatus ON ITEM.ItemStatusIDFK = ItemStatus.ItemStatusID "
                              + "WHERE (ITEM.INVN LIKE '" + searchfield + "') "
                              + "OR ([USER].Name LIKE '" + searchfield + "') "
                              + "OR (ITEM.SERIALN LIKE '" + searchfield + "') "
                              + "OR (ITEM.DESCRIPTION LIKE '" + searchfield + "')"
                              + "order by [User].Name");
        }

        //Gridscan for Items. Must have an ID label TemplateField
        public static void ItemGridscan(GridView Grid1)
        {
            //General grid scan, take each row, pull out the requester, replace the name, and add the view button

            foreach (GridViewRow gvrow in Grid1.Rows)
            {

                Label ID = (Label)gvrow.FindControl("ID");

                for (int i = 1; i < gvrow.Cells.Count; i++)
                {
                    gvrow.Cells[i].Attributes["onClick"] = "location.href='/Inventory/CompDetails.aspx?ID=" + ID.Text + "'";
                }

                gvrow.Attributes["onmouseover"] = "this.originalstyle=this.style.backgroundColor;this.style.cursor='hand';this.style.backgroundColor='#DBD6C8';";
                gvrow.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.backgroundColor=this.originalstyle;";

            }

        }

        //Gridscan for Items. Must have an ID label TemplateField. Allows for some cells to not be linked to the item. SkipFrontRows, true=skip the columns from the front, false=skip columns from the end.
        public static void ItemGridscan(GridView Grid1, int RowsToSkip, Boolean SkipFrontColumns)
        {
            //General grid scan, take each row, pull out the requester, replace the name, and add the view button

            foreach (GridViewRow gvrow in Grid1.Rows)
            {

                Label ID = (Label)gvrow.FindControl("ID");

                if (SkipFrontColumns)
                {
                    for (int i = RowsToSkip + 1; i < gvrow.Cells.Count; i++)
                    {
                        gvrow.Cells[i].Attributes["onClick"] = "location.href='/Inventory/CompDetails.aspx?ID=" + ID.Text + "'";
                    }
                }
                else
                {
                    for (int i = 1; i < gvrow.Cells.Count - RowsToSkip; i++)
                    {
                        gvrow.Cells[i].Attributes["onClick"] = "location.href='/Inventory/CompDetails.aspx?ID=" + ID.Text + "'";
                    }
                }
                gvrow.Attributes["onmouseover"] = "this.originalstyle=this.style.backgroundColor;this.style.cursor='hand';this.style.backgroundColor='#DBD6C8';";
                gvrow.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.backgroundColor=this.originalstyle;";

            }

        }

        public static void ItemGridscan(GridView Grid1, int RowsToSkip, Boolean SkipFrontColumns, Boolean EditOnly, string redirect)
        {
            //General grid scan, take each row, pull out the requester, replace the name, and add the view button

            foreach (GridViewRow gvrow in Grid1.Rows)
            {

                Label ID = (Label)gvrow.FindControl("ID");

                if (SkipFrontColumns)
                {
                    for (int i = RowsToSkip + 1; i < gvrow.Cells.Count; i++)
                    {
                        if(EditOnly)
                            gvrow.Cells[i].Attributes["onClick"] = "location.href='/Inventory/ItemView.aspx?ID=" + ID.Text + "&redirect=" + redirect + "'";
                        else
                            gvrow.Cells[i].Attributes["onClick"] = "location.href='/Inventory/CompDetails.aspx?ID=" + ID.Text + "'";
                    }
                }
                else
                {
                    for (int i = 1; i < gvrow.Cells.Count - RowsToSkip; i++)
                    {
                        if (EditOnly)
                            gvrow.Cells[i].Attributes["onClick"] = "location.href='/Inventory/ItemView.aspx?ID=" + ID.Text + "&redirect=" + redirect + "'";
                        else
                            gvrow.Cells[i].Attributes["onClick"] = "location.href='/Inventory/CompDetails.aspx?ID=" + ID.Text + "'";
                    }
                }
                gvrow.Attributes["onmouseover"] = "this.originalstyle=this.style.backgroundColor;this.style.cursor='hand';this.style.backgroundColor='#DBD6C8';";
                gvrow.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.backgroundColor=this.originalstyle;";

            }

        }

        #endregion

        
    }


    public class ItemType
    {

        #region static methods



        #endregion
    }

    public class ItemVendor
    {

    }

}