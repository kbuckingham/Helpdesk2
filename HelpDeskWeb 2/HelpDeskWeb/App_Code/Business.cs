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
using System.Drawing;


/// <summary>
/// Summary description for HelpDesk
/// </summary>
namespace HelpDesk
{
    public class Settings
    {
        #region Static Methods

        public static void CCOnly(HttpResponse r)
        {
            if (!SiteMembers.IsActive)
                r.Redirect("/WorkOrders/");

            if (!SiteMembers.Agreed())
                r.Redirect("/Help/Confidentiality.aspx");

            if (!Feedback.HasSubmittedFeedback() && DateTime.Now.DayOfWeek == DayOfWeek.Friday && SiteMembers.IsHDStudent)
                r.Redirect("/StudentFeedback/");
        }

        public static void HDMOnly(HttpResponse r)
        {
            //If user is not in the computer center redirct to the work orders default page
            if (!SiteMembers.IsActive)
                r.Redirect("/WorkOrders/");
            //If user is a current student redirect to the Sitemembers home page
            if (SiteMembers.IsinStudentRole())
                r.Redirect("/");
        }

        #endregion

    }


    public class HDmenu
    {
        #region Constructor

        public HDmenu()
        {
        }

        #endregion

        #region Static Methods

        public static string createMenu(string adname)
        {
            DataTable permTB = Sql.CCSelect("SELECT SiteMembers.ADName, MemberRoles.[Role], Menu.MenuRoot, Menu.MenuName, Menu.URL, Menu.Ordering "
                                          + "FROM MemberRoles JOIN SiteMembers ON MemberRoles.RID = SiteMembers.RIDFK "
                                          + "JOIN Role_Menu ON MemberRoles.RID = Role_Menu.RIDFK "
                                          + "JOIN Menu ON Role_Menu.MIDFK = Menu.MID "
                                          + "Where SiteMembers.ADName = '" + adname + "' "
                                          + "ORDER BY MenuRoot, Ordering");

            string conMenu = "<div id=\"nav\"><ul style=\"margin: 0px;\">";

            
            try
            {
                if (SiteMembers.IsActive)
                {
                    DataRow[] rootTB = permTB.Select("MenuRoot = '.Root'");
                
                    foreach (DataRow row in rootTB)
                    {
                        string css = "main";

                        if (row == rootTB.First())
                            css = "front";

                        conMenu += "<li class=\"" + css + "\"><a href=\"" + row["URL"].ToString() + "\">" + row["MenuName"] + "</a>";
                        DataRow[] subTB = permTB.Select("MenuRoot = '" + row["MenuName"] + "'", "Ordering ASC");
                        if (subTB != null)
                        {
                            conMenu += "<ul>";
                            foreach (DataRow subrow in subTB)
                            {
                                conMenu += "<li class=\"link\"><a href=\"" + subrow["URL"].ToString() + "\">" + subrow["MenuName"] + "</a></li>";

                            }
                            conMenu += "</ul>";
                        }
                        conMenu += "</li>";
                    }
                }
                else
                {
                    conMenu += "<li class=\"front\"><a href=\"/\">Home</a></li>";
                    conMenu += "<li class=\"main\"><a href=\"/WorkOrders/Request.aspx\">Request</a></li>";
                    conMenu += "<li class=\"main\"><a href=\"/WorkOrders/WorkOrders.aspx\">Work Orders</a></li>";
                }
            }
            catch 
            { 
                conMenu += "<li class=\"front\"><a href=\"/\">Home</a></li>";
                conMenu += "<li class=\"main\"><a href=\"/WorkOrders/Request.aspx\">Request</a></li>";
                conMenu += "<li class=\"main\"><a href=\"/WorkOrders/WorkOrders.aspx\">Work Orders</a></li>";
            }

            conMenu += "</ul></div><div class=\"clear\"></div>";

            return conMenu;
        }

        #endregion

    }

    public class WorkOrder
    {
        #region Properties
        public Int32 WID
        {
            get { return _WID; }
        }
        public string Requester
        {
            get { return _Requester; }
            set { _Requester = value; }
        }
        public string Creator
        {
            get { return _Creator; }
            set { _Creator = value; }
        }
        public string Phone
        {
            get { return _Phone; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
        public DateTime CreationDate
        {
            get { return _CreationDate; }
            set { _CreationDate = value; }
        }
        public DateTime AssignedDate
        {
            get { return _AssignedDate; }
            set { _AssignedDate = value; }
        }
        public DateTime ClosedDate
        {
            get { return _ClosedDate; }
            set { _ClosedDate = value; }
        }
        public DateTime CompletedDate
        {
            get { return _CompletedDate; }
            set { _CompletedDate = value; }
        }
        public bool Active
        {
            get { return _Active; }
        }
        public string Status
        {
            get { return _Status; }
        }
        public Int32 Points
        {
            get { return _Points; }
            set { _Points = value; }
        }
        public bool HDManagerViewStatus
        {
            get { return _HDManagerViewStatus; }
            set { _HDManagerViewStatus = value; }
        }
        public bool NetManagerViewStatus
        {
            get { return _NetManagerViewStatus; }
            set { _NetManagerViewStatus = value; }
        }
        public int Notes
        {
            get { return _Notes; }
        }
        public Array NotesArray
        {
            get { return _NotesArray; }
        }
        public int PriorID
        {
            get { return _PriorID; }
            set { _PriorID = value; }
        }
        public int TagID
        {
            get { return _TagID; }
            set { _TagID = value; }
        }

        public DataTable AssignedTechs
        {
            get { return _AssignedTechnicians; }
            set { _AssignedTechnicians = value;}
        }

        #endregion

        #region Attributes
        private int _WID = -1;
        private string _Requester = "";
        private string _Creator = "";
        private string _Phone = "";
        private string _Description = "";
        private int _PriorID = 1;
        private int _TagID = 1;
        private string _Details = "";
        private DateTime _CreationDate = Convert.ToDateTime("01/01/2000 1:01:01 AM");
        private DateTime _AssignedDate = Convert.ToDateTime("01/01/2000 1:01:01 AM");
        private DateTime _ClosedDate = Convert.ToDateTime("01/01/2000 1:01:01 AM");
        private DateTime _CompletedDate = Convert.ToDateTime("01/01/2000 1:01:01 AM");
        
        private bool _Active = true;
        private string _Status = "Open";
        private int _Points = 0;
        private bool _HDManagerViewStatus = true;
        private bool _NetManagerViewStatus = true;
        private int _Notes = -1;
        private Array _NotesArray;
        
        private DataTable _AssignedTechnicians = new DataTable();

        #endregion
        
        #region Constructors

        //use this to create and object before committing
        public WorkOrder()
        {
            //store assigned technicians of a work order
            _AssignedTechnicians.Columns.Add("FullName");
            _AssignedTechnicians.Columns.Add("ADName");
            _AssignedTechnicians.Columns.Add("Points");
        }

        public WorkOrder(int WID)
        {
            _WID = WID;
            DataTable dt = Sql.CCSelect("SELECT * FROM WorkOrders WHERE WID = " + Convert.ToString(WID));

            //store assigned technicians of a work order
            _AssignedTechnicians.Columns.Add("FullName");
            _AssignedTechnicians.Columns.Add("ADName");
            _AssignedTechnicians.Columns.Add("Points");
            _AssignedTechnicians = Sql.CCSelect("SELECT SiteMembers.FirstName + ' ' + SiteMembers.LastName AS FullName, WO_SiteMembers.ADName, WO_SiteMembers.Points "
                                              + "FROM WO_SiteMembers "
                                              + "JOIN SiteMembers ON WO_SiteMembers.ADName = SiteMembers.ADName "
                                              + "WHERE WOID = " + _WID);
            //instantiate notes
            Note Note = new Note(_WID);

            if (dt.Rows.Count > 0)
            {
                _Requester = dt.Rows[0]["Requester"].ToString();
                _Creator = dt.Rows[0]["Creator"].ToString();
                _Phone = AD.GetPhone(dt.Rows[0]["Requester"].ToString());
                _Description = dt.Rows[0]["Description"].ToString();
                _Details = dt.Rows[0]["Details"].ToString();
                if (!dt.Rows[0]["CreationDate"].ToString().Equals(""))
                    _CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"].ToString());
                if (!dt.Rows[0]["AssignedDate"].ToString().Equals(""))
                    _AssignedDate = Convert.ToDateTime(dt.Rows[0]["AssignedDate"].ToString());
                if (!dt.Rows[0]["CompletedDate"].ToString().Equals(""))
                    _CompletedDate = Convert.ToDateTime(dt.Rows[0]["CompletedDate"].ToString());
                _Active = Convert.ToBoolean(dt.Rows[0]["Active"].ToString());
                if (!dt.Rows[0]["Status"].ToString().Equals(""))
                    _Status = dt.Rows[0]["Status"].ToString();
                if (!dt.Rows[0]["ClosedDate"].ToString().Equals(""))
                    _ClosedDate = Convert.ToDateTime(dt.Rows[0]["ClosedDate"].ToString());
                if (!dt.Rows[0]["Points"].ToString().Equals(""))
                    _Points = Convert.ToInt32(dt.Rows[0]["Points"].ToString());
                if (!dt.Rows[0]["HDManagerViewStatus"].ToString().Equals(""))
                    _HDManagerViewStatus = Convert.ToBoolean(dt.Rows[0]["HDManagerViewStatus"].ToString());
                if (!dt.Rows[0]["NetManagerViewStatus"].ToString().Equals(""))
                    _NetManagerViewStatus = Convert.ToBoolean(dt.Rows[0]["NetManagerViewStatus"].ToString());
                try
                {
                    _PriorID = Convert.ToInt32(dt.Rows[0]["PriorIDFK"]);
                }
                catch { _PriorID = 1; }
                try
                {
                    _TagID = Convert.ToInt32(dt.Rows[0]["TagIDFK"]);
                }
                catch { _TagID = 1; }
            }
            else
                throw new Exception("Invalid WorkOrder ID");

            DataTable intersecdt = Sql.CCSelect("Select * from WO_SiteMembers where WOID = " + Convert.ToString(WID));

            DataTable Notesdt = Sql.CCSelect("SELECT * FROM WONotes WHERE WID = " + WID + " ORDER BY NoteDate DESC");
            if (Notesdt.Rows.Count > 0)
            {
                _Notes = Notesdt.Rows.Count;
                _NotesArray = Array.CreateInstance(typeof(int), _Notes);
                for (int i = 0; i < _Notes; i++)
                    _NotesArray.SetValue(Convert.ToInt32(Notesdt.Rows[i]["NID"].ToString()), i);
            }
            //else
            //throw new Exception("Invalid WorkOrder ID");
        }

        #endregion

        #region Member Methods

        public void Pending()
        {
            RemoveAllTech();
        }

        public void Queue()
        {
            _Status = "Queued";

            _ClosedDate = DateTime.Now;
        }

        public void Open()
        {
            _Status = "Open";
        }

        public void Close()
        {
            _Status = "Closed";
            _CompletedDate = DateTime.Now;
        }

        public void CloseByUser()
        {
            _Status = "Closed by User";
            _ClosedDate = DateTime.Now;
            _CompletedDate = DateTime.Now;
        }
        
        public bool IsaTech(string Tech)
        {
            DataRow[] found = _AssignedTechnicians.Select("ADName = '" + Tech + "'");

            if (found.Length == 0)
                return true;
            else
                return false;
        }

        public bool IsaTech()
        {
            DataRow[] found = _AssignedTechnicians.Select("");

            if (found.Length != 0)
                return true;
            else
                return false;
        }

        public void AddTech(string Tech)
        {
            DataRow row = _AssignedTechnicians.NewRow();
            row["FullName"] = SiteMembers.ToFullName(Tech);
            row["ADName"] = Tech;
            row["Points"] = "2";
            _AssignedTechnicians.Rows.Add(row);

            if (_AssignedTechnicians.Rows.Count == 1)
                _AssignedDate = DateTime.Now;
        }

        public void RemoveTech(string Tech)
        {
            if (_AssignedTechnicians.Rows.Count == 1)
            {
                _Status = "Pending";
                _AssignedDate = Convert.ToDateTime("01/01/2000 1:01:01 AM");
                _CompletedDate = Convert.ToDateTime("01/01/2000 1:01:01 AM");
            }

            DataRow[] found = _AssignedTechnicians.Select("ADName = '" + Tech + "'");

            if (found != null && found.Length == 1)
                _AssignedTechnicians.Rows.Remove(found[0]);
        }

        public void RemoveAllTech()
        {
            _AssignedTechnicians.Rows.Clear();
            _Status = "Pending";
            _AssignedDate = Convert.ToDateTime("01/01/2000 1:01:01 AM");
            _CompletedDate = Convert.ToDateTime("01/01/2000 1:01:01 AM");
        }

        public void ChangePoints(string techname, int points)
        {
            DataRow[] rowtoupdate = _AssignedTechnicians.Select("ADName = '" + techname + "'");
            if (rowtoupdate != null && rowtoupdate.Length == 1)
            {
                rowtoupdate[0]["Points"] = points.ToString();
            }
        }

        public int GetPoints(string techname)
        {
            DataRow[] find = _AssignedTechnicians.Select("ADNAME = '" + techname + "'");
            if (find != null && find.Length == 1)
            {
                return Convert.ToInt32(find[0]["Points"]);
            }
            else
            {
                return 0;
            }

        }

        public void Commit()
        {
            if (_WID == -1)
            {
                _WID = NewWorkOrder(_Requester, _Description);
            }
            
            string _AssignDate = "";
            string _CreateDate = "";
            string _CloseDate = "";
            string _CompleteDate = "";
            int _HDView = Convert.ToInt32(_HDManagerViewStatus);
            int _StuView = Convert.ToInt32(_NetManagerViewStatus);

            if (_AssignedDate.Equals(Convert.ToDateTime("1/1/2000 1:01:01 AM")))
                _AssignDate = "NULL";
            else
                _AssignDate = "'" + _AssignedDate + "'";

            if (_CreationDate.Equals(Convert.ToDateTime("1/1/2000 1:01:01 AM")))
                _CreateDate = "NULL";
            else
                _CreateDate = "'" + _CreationDate + "'";

            if (_ClosedDate.Equals(Convert.ToDateTime("1/1/2000 1:01:01 AM")))
                _CloseDate = "NULL";
            else
                _CloseDate = "'" + _ClosedDate + "'";

            if (_CompletedDate.Equals(Convert.ToDateTime("1/1/2000 1:01:01 AM")))
                _CompleteDate = "NULL";
            else
                _CompleteDate = "'" + _CompletedDate + "'";

            //******************************************************
            //the ToUserName and ToFullName methods of AD are setting it to Unkown User sometimes....this doesn't get reset until someone looks at the work order

            Sql.CCSelect("UPDATE WorkOrders SET Requester = '" + _Requester + "', "
                                            + "Creator = '" + _Creator + "', "
                                            + "Description = '" + _Description.Replace("'", "''") + "', "
                                            + "Details = '" + _Details.Replace("'", "''") + "', "
                                            + "CreationDate = " + _CreateDate + ", "
                                            + "AssignedDate = " + _AssignDate + ", "
                                            + "CompletedDate = " + _CompleteDate + ", "
                                            + "Active = '" + _Active + "', "
                                            + "Status = '" + _Status + "', "
                                            + "ClosedDate = " + _CloseDate + ", "
                                            + "Points = " + _Points + ", "
                                            + "PriorIDFK = " + _PriorID + ", "
                                            + "TagIDFK = " + _TagID + ", "
                                            + "HDManagerViewStatus = " + _HDView.ToString() + ", "
                                            + "NetManagerViewStatus = " + _StuView.ToString() + " "
                                            + "WHERE WID = " + _WID);


            #region update techinicians on database
            DataTable intersecdt = Sql.CCSelect("Select SiteMembers.FirstName + '' + SiteMembers.LastName AS FullName, WO_SiteMembers.ADName, WO_SiteMembers.Points FROM WO_SiteMembers Join SiteMembers ON WO_SiteMembers.ADName = SiteMembers.ADName where WOID = " + Convert.ToString(_WID));

            ArrayList currentdb = new ArrayList();
            ArrayList listobject = new ArrayList();
            
            foreach (DataRow tech in intersecdt.Rows)
                currentdb.Add(tech["ADName"].ToString());

            foreach (DataRow tech in _AssignedTechnicians.Rows)
                listobject.Add(tech["ADName"].ToString());


            foreach (string tech in listobject)
            {
                DataRow[] row = _AssignedTechnicians.Select("ADName = '" + tech + "'");

                row[0]["Points"].ToString();

                if (currentdb.Contains(tech))
                    Sql.CCSelect("UPDATE WO_SiteMembers set Points = " + row[0]["Points"].ToString() + " where ADName = '"+tech+"' and WOID = " + _WID);
                else
                    Sql.CCSelect("Insert INTO WO_SiteMembers (WOID, ADName, Points) VALUES (" + _WID + ", '" + tech + "', " + row[0]["Points"].ToString() + ")");
                
            }

            foreach (string tech in currentdb)
            {
                if (!listobject.Contains(tech))
                {
                    //delete the row from the server
                    Sql.CCSelect("DELETE FROM WO_SiteMembers where WOID = " + _WID + " AND ADName = '" + tech + "'");
                }
            }
            #endregion

        }
        #endregion

        #region Static Methods

        public static int NewWorkOrder(string requester, string Description)
        {
            DataTable dt = Sql.CCSelect("INSERT INTO WorkOrders (Requester,Description,CreationDate,Active) VALUES ('" + requester + "','" + Description.Replace("'", "''") + "','" + DateTime.Now + "','True');" + "SELECT SCOPE_IDENTITY();");

            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }

        public static DataTable GetRequestedWOs(string username)
        {
            return Sql.CCSelect("SELECT [WID], [Status], [Requester], [Description], [AssignedDate] "
                                + "FROM [WorkOrders] "
                                + "WHERE (([Status] = 'Open') OR ([Status] = 'Pending') OR ([Status] = 'Queued')) "
                                + "AND (Requester = '" + AD.UserName + "') "
                                + "AND (CreationDate <= GETDATE()) "
                                + "ORDER BY CASE [Status] WHEN 'Pending' THEN 1 WHEN 'Open' THEN 2 WHEN 'Queued' THEN 3 END, CreationDate DESC");
        }

        public static DataTable GetDepartmentWOs(string username)
        {
            return Sql.CCSelect("SELECT [WID], [Requester], [Description], [CreationDate], (Select LastName + ', ' + FirstName FROM [USER] where ADNAME = WorkOrders.Requester) as LastFirstName "
                                + "from [WorkOrders] "
                                + "JOIN [USER] ON WorkOrders.Requester = [USER].ADName "
                                + "JOIN Dept_Location ON [USER].PID = Dept_Location.PID "
                                + "JOIN DEPARTMENT ON Dept_Location.DeptID = DEPARTMENT.DeptID "
                                + "Where (([Status] = 'Open') OR ([Status] = 'Pending') OR ([Status] = 'Queued')) "
                                + "AND ADName != '" + username + "' "
                                + "AND (CreationDate <= GETDATE()) "
                                + "AND DEPARTMENT.DeptID = (Select Department.DeptID "
                                + "						   FROM Dept_Location "
                                + "						   JOIN DEPARTMENT ON Dept_Location.DeptID = DEPARTMENT.DeptID "
                                + "                          JOIN [USER] ON Dept_Location.PID = [USER].PID "
                                + "                          Where ADName = '" + username + "') "
                                + "ORDER BY CASE [Status] WHEN 'Pending' THEN 1 WHEN 'Open' THEN 2 WHEN 'Queued' THEN 3 END, CreationDate DESC ");
        }

        public static DataTable GetPastWOs(string username)
        {
            return Sql.CCSelect("SELECT [WID], [Status], [Requester], [Description], [AssignedDate] "
                                + "FROM [WorkOrders] "
                                + "WHERE (([Status] = 'Closed') OR ([Status] = 'Closed by User')) "
                                + "AND (CreationDate <= GETDATE()) "
                                + "AND (CompletedDate > Dateadd(m, -3, getdate())) "
                                + "AND (Requester = '" + AD.UserName + "') "
                                + "ORDER BY [CompletedDate] DESC");
        }

        public static DataTable GetAssignedWOs(string username)
        {
            return Sql.CCSelect("SELECT [WID], TAGS.[Tagname], [Requester], [Description], [CreationDate], (Select LastName + ', ' + FirstName FROM [USER] where ADNAME = WorkOrders.Requester) as LastFirstName "
                                + "FROM WorkOrders "
                                + "JOIN WO_SiteMembers ON WorkOrders.WID = WO_SiteMembers.WOID "
                                + "LEFT JOIN TAGS ON WorkOrders.TagIDFK = TAGS.TagID "
                                + "WHERE (WorkOrders.Status = 'Open') AND (WO_SiteMembers.ADName = '" + username + "') "
                                + "AND (CreationDate <= GETDATE()) "
                                + "ORDER BY WorkOrders.CreationDate");
        }

        public static DataTable GetPendingWOs()
        {
            return Sql.CCSelect("SELECT [WID], TAGS.[Tagname], [Requester], [Description], [CreationDate], (Select LastName + ', ' + FirstName FROM [USER] where ADNAME = WorkOrders.Requester) as LastFirstName "
                                + "FROM [WorkOrders] "
                                + "LEFT JOIN TAGS ON WorkOrders.TagIDFK = TAGS.TagID "
                                + "WHERE ([Status] = 'Pending') "
                                + "AND (CreationDate <= GETDATE()) "
                                + "ORDER BY CreationDate");
        }

        public static DataTable GetFutureWOs()
        {
            return Sql.CCSelect("SELECT [WID], TAGS.[Tagname], [Requester], [Description], [CreationDate], (Select LastName + ', ' + FirstName FROM [USER] where ADNAME = WorkOrders.Requester) as LastFirstName "
                                + "FROM [WorkOrders] "
                                + "LEFT JOIN TAGS ON WorkOrders.TagIDFK = TAGS.TagID "
                                + "WHERE (CreationDate >= GETDATE()) "
                                + "ORDER BY CreationDate");
        }

        public static DataTable GetQueuedWOs()
        {
            return Sql.CCSelect("SELECT [WID], TAGS.[Tagname], [Requester], [Description], [CreationDate], (Select LastName + ', ' + FirstName FROM [USER] where ADNAME = WorkOrders.Requester) as LastFirstName "
                                + "FROM [WorkOrders] "
                                + "LEFT JOIN TAGS ON WorkOrders.TagIDFK = TAGS.TagID "
                                + "WHERE ([Status] = 'Queued') ORDER BY CreationDate");
        }

        public static DataTable GetOpenWOs(string username)
        {
            return Sql.CCSelect("Select [WID], TAGS.[Tagname], [Requester], [Description], [CreationDate], (Select LastName + ', ' + FirstName FROM [USER] where ADNAME = WorkOrders.Requester) as LastFirstName "
                                + "FROM WorkOrders "
                                + "LEFT JOIN TAGS ON WorkOrders.TagIDFK = TAGS.TagID "
                                + "WHERE WID IN (Select WOID FROM WO_SiteMembers "
                                + "Where ADName != '" + username + "' "
                                + "and WOID NOT IN (SELECT WOID from WO_SiteMembers "
                                + "WHERE ADName = '" + username + "')) "
                                + "AND [Status] = 'Open' "
                                + "AND (CreationDate <= GETDATE()) "
                                + "ORDER BY TagName, LastFirstName");
        }

        public static DataTable GetPrioritys()
        {
            return Sql.CCSelect("SELECT [PriorityName], [PriorID] FROM [Priority]");
        }

        public static DataTable GetCategories()
        {
            return Sql.CCSelect("SELECT * FROM TAGS");
        }

        public static DataTable GetStatuses()
        {
            return Sql.CCSelect("SELECT DISTINCT [Status] FROM [WorkOrders] WHERE [Status] is not null ORDER BY [Status]");
        }


        //Scans and adds additional features to the Gridview. Must have a WID Label TemplateField
        public static void WOGridscan(GridView Grid1)
        {
            foreach (GridViewRow gvrow in Grid1.Rows)
            {
                Label WID = (Label)gvrow.FindControl("WID");
                Label LFN = (Label)gvrow.FindControl("LastFirstName");
                Label requester = (Label)gvrow.FindControl("Requester");

                if (LFN != null)
                {
                    if (String.IsNullOrEmpty(LFN.Text))
                        requester.Visible = true;
                }

                gvrow.Attributes["onClick"] = "location.href='ViewOrder.aspx?WID=" + Cryptography.NumberEncrypt(WID.Text) + "'";

                DataTable detailsdt = Sql.CCSelect("Select Details from WorkOrders where WID = " + WID.Text);

                HtmlToText html2txt = new HtmlToText();

                try { gvrow.Attributes["title"] = html2txt.Convert(detailsdt.Rows[0]["Details"].ToString()); }
                catch { }

                if (SiteMembers.IsActive)
                {
                    Label userName = (Label)gvrow.FindControl("Requester");

                    DataTable result = Sql.CCSelect("SELECT NetManagerViewStatus, HDManagerViewStatus, PriorIDFK FROM WorkOrders WHERE WID = " + WID.Text);
                    DataTable TagID = Sql.CCSelect("Select TagIDFK from WorkOrders Where WID = " + WID.Text);       //find what tag for this work order 

                    foreach (DataRow tbrow in result.Rows)
                    {
                        if (SiteMembers.IsHDM)
                        {
                            if (tbrow["HDManagerViewStatus"].ToString() == "False")
                            {
                                gvrow.Font.Bold = true;
                            }
                        }

                        if (SiteMembers.IsNetM)
                        {
                            if (tbrow["NetManagerViewStatus"].ToString() == "False")
                            {
                                gvrow.Font.Bold = true;
                            }
                        }

                        if (tbrow["PriorIDFK"].ToString() == "2")
                        {
                            gvrow.ForeColor = Color.Red;
                            gvrow.Font.Size = 14;
                        }

                        if (tbrow["PriorIDFK"].ToString() == "3")
                        {
                            gvrow.ForeColor = System.Drawing.ColorTranslator.FromHtml("#353535");
                            gvrow.Font.Italic = true;
                        }

                    }

                    gvrow.Attributes["onmouseover"] = "this.originalstyle=this.style.backgroundColor;this.style.cursor='hand';this.style.backgroundColor='#FFFFFF';";
                    gvrow.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.backgroundColor=this.originalstyle;";

                    try
                    {
                        WOTag tag = new WOTag(Convert.ToInt32(TagID.Rows[0]["TagIDFK"]));
                        gvrow.BackColor = System.Drawing.ColorTranslator.FromHtml(tag.TagColor);                    //change the background to that tag color
                    }
                    catch
                    {
                        WOTag tag = new WOTag(1);
                        gvrow.BackColor = System.Drawing.ColorTranslator.FromHtml(tag.TagColor);                    //change the background to the General tag color
                    }
                }
                else
                {
                    gvrow.Attributes["onmouseover"] = "this.originalstyle=this.style.backgroundColor;this.style.cursor='hand';this.style.backgroundColor='#FFFFFF';";
                    gvrow.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.backgroundColor=this.originalstyle;";
                    
                    WOTag tag = new WOTag(1);
                    gvrow.BackColor = System.Drawing.ColorTranslator.FromHtml(tag.TagColor);                        //change the background to the General tag color
                }
            }

        }

        #endregion

    }

    public class WOTag
    {
        #region Properties
        public int TagID
        {
            get { return _TagID; }
        }

        public string TagName
        {
            get { return _TagName; }
            set { _TagName = value; }
        }

        public string TagColor
        {
            get { return _TagColor; }
            set { _TagColor = value; }
        }
        #endregion

        #region Attributes

        private int _TagID = -1;
        private string _TagName = "";
        private string _TagColor = "";

        #endregion

        #region Constructors

        public WOTag(int TagID)
        {
            _TagID = TagID;
            DataTable dt = Sql.CCSelect("Select * from Tags WHERE TAGID = " + TagID.ToString());

            if (dt.Rows.Count > 0)
            {
                _TagName = dt.Rows[0]["TagName"].ToString();
                _TagColor = dt.Rows[0]["TagColor"].ToString();
            }
            else
                throw new Exception("Invalid Tag ID");
        }

        #endregion

        #region Member Methods

        public void Commit()
        {
            Sql.CCSelect("UPDATE TAGS SET TagName = '" + _TagName + "', "
                                      + "TagColor = '" + _TagColor + "', "
                                      + "WHERE TagID = " + _TagID);
        }

        #endregion

        #region Static Methods

        #endregion
    }

    public class Staff
    {
        #region Properties

        #endregion

        #region Attributes

        private int _UserID = -1;
        private string _DisplayName = "";
        private string _FirstName = "";
        private string _LastName = "";
        private int _PID = -1;
        private int _DeptID = -1;
        private int _BuildingID = -1;
        private string _DeptName = "";
        private string _BuildingName = "";
        private string _ADName = "";
        private string _Email = "";
        private string _extension = "";
        private bool _InAD = false;

        #endregion

        #region Constructors

        public Staff(int UserID)
        {
            _UserID = UserID;
            DataTable data = Sql.CCSelect("SELECT * FROM [USER] "
                                      + "JOIN Dept_Location ON [USER].PID = Dept_Location.PID "
                                      + "JOIN DEPARTMENT ON Dept_Location.DeptID = DEPARTMENT.DeptID "
                                      + "JOIN LOCATION ON Dept_Location.BuildingID = LOCATION.BuildingID "
                                      + "WHERE USERID = " + UserID.ToString());
            fill(data);            
        }

        public Staff(string ADName)
        {
            _ADName = ADName;
            DataTable data = Sql.CCSelect("SELECT * FROM [USER] "
                                      + "JOIN Dept_Location ON [USER].PID = Dept_Location.PID "
                                      + "JOIN DEPARTMENT ON Dept_Location.DeptID = DEPARTMENT.DeptID "
                                      + "JOIN LOCATION ON Dept_Location.BuildingID = LOCATION.BuildingID "
                                      + "WHERE ADName = '" + ADName + "'");
            fill(data);          
        }

        #endregion

        #region Member Methods

        private void fill(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                _UserID = Convert.ToInt32(dt.Rows[0]["USERID"]);
                _DisplayName = dt.Rows[0]["Name"].ToString();
                _FirstName = dt.Rows[0]["FirstName"].ToString();
                _LastName = dt.Rows[0]["LastName"].ToString();
                _PID = Convert.ToInt32(dt.Rows[0]["PID"]);
                _DeptID = Convert.ToInt32(dt.Rows[0]["DeptID"]);
                _BuildingID = Convert.ToInt32(dt.Rows[0]["BuildingID"]);
                _DeptName = dt.Rows[0]["DeptName"].ToString();
                _BuildingName = dt.Rows[0]["Location"].ToString();
                _Email = dt.Rows[0]["Email"].ToString();
                _extension = dt.Rows[0]["extension"].ToString();
                if (!dt.Rows[0]["InAD"].ToString().Equals(""))
                    _InAD = Convert.ToBoolean(dt.Rows[0]["InAD"].ToString());
            }
            else
            {
                throw new Exception("Invalid User ID");
            }
        }

        public void Commit()
        {
            
        }

        #endregion

        #region Static Methods

        public static bool IsAUser(string adname)
        {
            DataTable dt = Sql.CCSelect("Select * from [USER] where ADNAME = '" + adname + "'");

            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }


        public static string ToLastFirstName(string username)
        {
            DataTable result = Sql.CCSelect("Select Lastname + ', ' + FirstName as LastFirstName from [user] where ADNAME = '" + username + "'");
            try { return result.Rows[0]["LastFirstName"].ToString(); }
            catch { return username; }
        }

        public static string ToDisplayName(string username)
        {
            DataTable result = Sql.CCSelect("Select [Name] from [user] where ADNAME = '" + username + "'");

            try { return result.Rows[0]["Name"].ToString(); }
            catch { return username; }
        }

        public static string ToUserName(string displayname)
        {
            DataTable result = Sql.CCSelect("Select ADName from [USER] where Name = '" + displayname + "'");

            try { return result.Rows[0]["ADName"].ToString(); }
            catch { return displayname; }
        }

        public static string ToUserNameLF(string LastFirstName)
        {
            DataTable result = Sql.CCSelect("Select ADName from [USER] where Lastname + ', ' + FirstName = '" + LastFirstName + "'");

            try { return result.Rows[0]["ADName"].ToString(); }
            catch { return LastFirstName; }
            
        }

        public static string GetPhone(string username)
        {
            DataTable result = Sql.CCSelect("Select extension from [USER] where ADName = '" + username + "'");

            return result.Rows[0]["extension"].ToString();
        }

        public static string GetEmail(string username)
        {
            DataTable result = Sql.CCSelect("Select Email from [USER] where ADName = '" + username + "'");

            return result.Rows[0]["Email"].ToString();
        }

        public static DataTable StaffFacultyDDL()
        {
            return Sql.CCSelect("Select ADName, LastName + ', ' + FirstName as LastFirstName "
                              + "from [User] "
                              + "Where ADName <> '' and ADName is not null "
                              + "and LastName is not null and LastName <> '' "
                              + "Order by LastFirstName ");
        }

        #endregion
    }
    
    public class Note
    {
        #region Properties
        public int NID
        {
            get { return _NID; }
        }
        public int WID
        {
            get { return _WID; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public bool Hidden
        {
            get { return _Hidden; }
            set { _Hidden = value; }
        }
        public string User
        {
            get { return _User; }
            set { _User = value; }
        }
        public string EditUser
        {
            get { return _EditUser; }
            set { _EditUser = value; }
        }

        public DateTime NoteDate
        {
            get { return _NoteDate; }
        }


        public DateTime EditDate
        {
            get { return _EditDate; }
            set { _EditDate = value; }
        }

        #endregion

        #region Attributes
        private int _NID = -1;
        private int _WID = -1;
        private string _Description = "";
        private bool _Hidden = false;
        private string _User = "";
        private string _EditUser = "";
        private DateTime _NoteDate = DateTime.Now;
        private DateTime _EditDate = DateTime.Now;
        #endregion

        #region Constructors
        public Note(int NID)
        {
            DataTable Check = Sql.CCSelect("SELECT * FROM WONotes WHERE NID = " + NID);
            if (Check.Rows.Count > 0)
            {
                _NID = NID;
                _WID = Convert.ToInt32(Check.Rows[0]["WID"].ToString());
                _Description = Check.Rows[0]["Note"].ToString();
                _Hidden = Convert.ToBoolean(Check.Rows[0]["Hidden"].ToString());
                _User = Check.Rows[0]["User"].ToString();
                _NoteDate = Convert.ToDateTime(Check.Rows[0]["NoteDate"].ToString());
                _EditDate = Convert.ToDateTime(Check.Rows[0]["EditDate"].ToString());
                _EditUser = Check.Rows[0]["EditUser"].ToString();
            }
            //else
            //    throw new Exception("Invalid Note ID");
        }
        #endregion

        #region Member Methods
        public void Commit()
        {

            Sql.CCSelect("UPDATE WONotes SET Note = '" + _Description.Replace("'", "''") + "', "
                + "Hidden = '" + _Hidden + "', "
                + "[User] = '" + _User + "', "
                + "EditUser = '" + _EditUser + "', "
                + "NoteDate = '" + _NoteDate + "', "
                + "EditDate = '" + _EditDate + "' "
                + "WHERE NID = " + _NID + ";");
        }
        #endregion

        #region Static Methods

        public static int NewNote(int WID, string NoteDesc, string User, bool Hidden)
        {
            //DataTable Check = Sql.CCSelect("SELECT COUNT(*) AS Total FROM WorkOrders WHERE WID = " + WID);
            //if (Check.Rows[0][0].Equals("0"))
            //throw new Exception("Invalid WorkOrder ID");
            int IntHidden = 0;
            DateTime currentdatetime = DateTime.Now;

            if (Hidden == true)
                IntHidden = 1;
            DataTable dt = Sql.CCSelect("INSERT INTO WONotes (WID, Note, Hidden, [User], NoteDate, EditDate) VALUES (" + WID + ",'" + NoteDesc.Replace("'", "''") + "'," + IntHidden + ",'" + User + "','" + currentdatetime + "','" + currentdatetime + "'); SELECT SCOPE_IDENTITY();");

            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }

        public static int DeleteNote(int NID)
        {
            DataTable dt = Sql.CCSelect("DELETE FROM WONotes WHERE NID = " + NID + "; SELECT SCOPE_IDENTITY();");

            return NID;
        }

        #endregion

    }

    public class HDLogs
    {
        #region Properties
        public int Total
        {
            get { return _total; }
        }
        #endregion

        #region Attributes
        int _total = 0;
        #endregion

        #region Constructors
        public HDLogs()
        {
            DataTable dt = Sql.CCSelect("Select * From Logs");
            _total = dt.Rows.Count;
        }
        #endregion

        #region Member Methods
        #endregion

        #region Static Methods
        public static int NewDBLog(string User, string IP)
        {
            DataTable dt = Sql.CCSelect("INSERT INTO LOGS (Note) VALUES('User " + User + " on IP " + IP + " visited the WorkOrders.aspx page');" + "SELECT SCOPE_IDENTITY();");

            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }
        public static int NewRequestLog(string User, string IP)
        {
            DataTable dt = Sql.CCSelect("INSERT INTO LOGS (Note) VALUES('User " + User + " on IP " + IP + " visited the Request.aspx page');" + "SELECT SCOPE_IDENTITY();");

            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }
        public static int ErrorLog(string User, string IP, string Error)
        {
            DataTable dt = Sql.CCSelect("INSERT INTO LOGS (Note) VALUES('-Error- User: " + User + " - IP: " + IP + " - Page where error occured: " + Error + "')");

            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }
        public static bool CleanLogs()
        {
            try
            {
                // get a configured DbCommand object
                DbCommand comm = GenericDataAccess.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "CleanLogs";

                GenericDataAccess.ExecuteNonQuery(comm);
                return true;
            }
            catch
            { return false; }
        }
        #endregion

    }

    public class HDNews
    {
        #region Properties
        #endregion

        #region Attributes
        #endregion

        #region Constructors
        public HDNews()
        {
        }
        #endregion

        #region Member Methods
        #endregion

        #region Static Methods

        public static int NewNews(string Desc)
        {
            // get a configured DbCommand object
            DbCommand comm2 = GenericDataAccess.CreateCommand();
            // set the stored procedure name
            comm2.CommandText = "AddNews";

            string preNews = Desc.Replace("\r\n", "<br />");
            preNews = preNews.Replace("[link]", "<a href=\"");
            preNews = preNews.Replace("[name]", "\">");
            preNews = preNews.Replace("[/link]", "</a>");

            // create a new parameter for PO Number
            DbParameter param = comm2.CreateParameter();
            param.ParameterName = "@news";
            param.Value = preNews;
            param.DbType = DbType.String;
            param.Size = 1000;
            comm2.Parameters.Add(param);

            return GenericDataAccess.ExecuteNonQuery(comm2);
        }

        public static string Top4()
        {
            DataTable dtResult = Sql.CCSelect("SELECT TOP 4 * FROM NEWS WHERE [Public] = 1 ORDER BY NewsDate DESC");

            string Results = "<div>";

            foreach (DataRow drRow in dtResult.Rows)
            {
                Results += drRow["News"].ToString() + "<br />";
                Results += drRow["NewsDate"].ToString() + "<br /><br />";
            }

            Results += "</div>";
            return Results;
        }
        #endregion

    }

    public class Semester
    {
        #region Properties

        public static DataTable CurrentID()
        {
            return Sql.CCSelect("Select SemesterID From Semester Where(GETDATE() >= BeginDate and GETDATE() <= EndDate)");
        }

        public static string CurrentIDstring()
        {
            return Sql.CCSelect("Select SemesterID From Semester Where(GETDATE() >= BeginDate and GETDATE() <= EndDate)").Rows[0]["SemesterID"].ToString();
        }

        #endregion

        #region Attributes

        #endregion

        #region Constructors
            public Semester()
            {

            }

        #endregion

        #region Member Methods
        #endregion

        #region Static Methods

        public static void BindSemesterDD(DropDownList SemesterDD)
        {
            DataTable SemesterTable = Sql.CCSelect("SELECT Semester.SemesterID, Season.Season + ' ' + Semester.Year as SemesterName "
                                                    + "FROM Semester JOIN Season ON Semester.SeasonID = Season.SeasonID Order by SemesterID desc");
            SemesterDD.DataSource = SemesterTable;
            SemesterDD.DataTextField = "SemesterName";
            SemesterDD.DataValueField = "SemesterID";
            SemesterDD.DataBind();
            SemesterDD.Items.Insert(0, new ListItem("---Select Semester---", "-1"));
            SemesterDD.SelectedIndex = 0;
        }

        public static bool IsFinalWeek(int SemesterID)
        {
            DataTable IsFinalDB = Sql.CCSelect("SELECT Season.IsFinalWeek "
                                             + "FROM Season JOIN Semester ON Season.SeasonID = Semester.SeasonID "
                                             + "WHERE SemesterID = " + SemesterID);

            return Convert.ToBoolean(IsFinalDB.Rows[0]["IsFinalWeek"]);
        }

        #endregion
    }

    public class SiteMembers
    {
        #region Attributes

        private string _ADname = "";
        private int _ID = -1;
        private string _FirstName = "";
        private string _LastName = "";
        string _CellPhone = "";
        string Dorm = "";
        string _RoomNumber = "";
        string _HomeTown = "";
        string _State = "";
        private string _Email = "";
        string _PictureLink = "";
        bool Active = true;
        DateTime _RegisterDate = Convert.ToDateTime("01/01/1111 1:01:01 AM");
        DateTime _BirthDate = Convert.ToDateTime("01/01/1111 1:01:01 AM");
        string Comments = "";

        int _RoleID = -1;
        string _Role = "";
        bool _IsStudentRole = false;

        #endregion

        #region Properties

        public int ID
        {
            get { return _ID; }
        }
        public string ADname
        {
            get { return _ADname; }
        }
        public string Email
        {
            get { return _Email; }
        }
        public string FirstName
        {
            get { return _FirstName; }
        }
        public string LastName
        {
            get { return _LastName;}
        }
        public string HomeTown
        {
            get { return _HomeTown; }
            set { _HomeTown = value; }
        }
        public string PictureLink
        {
            get { return _PictureLink; }
            set { _PictureLink = value; }
        }
        public DateTime RegisterDate
        {
            get { return _RegisterDate; }
        }
        public string RoomNumber
        {
            get { return _RoomNumber; }
            set { _RoomNumber = value; }
        }
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        public string Phone
        {
            get { return _CellPhone; }
            set { _CellPhone = value; }
        }
            

        #endregion

        #region Constructors
        public SiteMembers(string username)
        {
            DataTable dt = Sql.CCSelect("SELECT SiteMembers.*, MemberRoles.Role, MemberRoles.IsStudentRole FROM SiteMembers JOIN MemberRoles ON SiteMembers.RIDFK = MemberRoles.RID WHERE ADName = '" + username + "'");

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    _ADname = row["ADName"].ToString();

                    _ID = Convert.ToInt32(row["ID"]);
                    _FirstName = row["FirstName"].ToString();
                    _LastName = row["LastName"].ToString();
                    _CellPhone = row["CellPhone"].ToString();
                    Dorm = row["Dorm"].ToString();
                    _RoomNumber = row["RoomNumber"].ToString();
                    _HomeTown = row["HomeTown"].ToString();
                    _State = row["State"].ToString();
                    _Email = row["email"].ToString();
                    _PictureLink = row["PicLink"].ToString();
                    Active = Convert.ToBoolean(row["Active"].ToString());
                    if (row["RegisterDate"] != DBNull.Value)
                        _RegisterDate = Convert.ToDateTime(row["RegisterDate"].ToString());
                    if (row["BirthDate"] != DBNull.Value)
                        _BirthDate = Convert.ToDateTime(row["BirthDate"].ToString());
                    Comments = row["Comments"].ToString();

                    _RoleID = Convert.ToInt32(row["RIDFK"]);
                    _Role = row["Role"].ToString();
                    _IsStudentRole = Convert.ToBoolean(row["IsStudentRole"]);
                }
            }
        }

        #endregion

        #region Member Methods

        public void Commit()
        {

        }

        #endregion

        #region Static Methods

        public static int AddSiteMember(string ADName, string FirstName, string LastName, string Email, int RID, string pictureurl, int ID)
        {
            DataTable dt = Sql.CCSelect("Insert INTO SiteMembers (FirstName, LastName, ADName, email, Active, OnTheClock, RIDFK, RegisterDate, PicLink, ID) "
                    + "VALUES ('" + FirstName + "','" + LastName + "','" + ADName + "','" + Email + "','true','false'," + RID + ", GETDATE(), '" + pictureurl + "', " + ID + ")" + "Select Scope_IDENTITY();");
            return Convert.ToInt32(dt.Rows[0][0]);

        }

        public static bool IsAdmin
        {
            get { return IsMemberOf(AD.UserName, 1); }
        }

        public static bool IsanAdmin(string adname)
        {
            return IsMemberOf(adname, 1);
        }

        public static bool IsHDStudent
        {
            get { return IsMemberOf(AD.UserName, 2); }
        }

        public static bool IsNetworkStudent
        {
            get { return IsMemberOf(AD.UserName, 4); }
        }

        public static bool IsWebStudent
        {
            get { return IsMemberOf(AD.UserName, 6); }
        }

        public static bool IsHDM
        {
            get { return IsMemberOf(AD.UserName, 5); }
        }

        public static bool IsNetM
        {
            get
            { return IsMemberOf(AD.UserName, 7); }
        }

        public static bool IsActive
        {
            get { return IsAMember(AD.UserName); }
        }

        public static string HDM
        {
            get { return GetManagerAD("Help Desk Manager"); }
        }

        public static string NetM
        {
            get { return GetManagerAD("Network Manager"); }
        }

        public static bool Agreed()
        {
            //CHECK IF AGREED IF NOT REDIRECT
            if (AD.IsinCCStudents)
            {
                try
                {

                    DataTable agreed = Sql.CCSelect("SELECT SemesterSiteMembers.Agreed "
                                                + "FROM Semester JOIN SemesterSiteMembers ON Semester.SemesterID = SemesterSiteMembers.SemesterID "
                                                + "Where (GETDATE() >= Semester.BeginDate and GETDATE() <= Semester.EndDate) and SemesterSiteMembers.ADName = '" + AD.UserName + "'");

                    if (!Convert.ToBoolean(agreed.Rows[0]["Agreed"]))
                        return false;
                    else
                        return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static bool IsAMember(string username)
        {
            ArrayList members = new ArrayList();

            DataTable roleresult = Sql.CCSelect("Select ADName from SiteMembers where (ADName != 'Administrator' AND ADName != 'unassigned') AND (Active = 1)");

            foreach (DataRow row in roleresult.Rows)
            {
                members.Add(row["ADName"].ToString());
            }

            if (members.Contains(username))
                return true;
            else
                return false;
        }

        public static string GetManagerAD(string Role)
        {
            DataTable adname = Sql.CCSelect("SELECT SiteMembers.ADName "
                                            + "FROM MemberRoles "
                                            + "JOIN SiteMembers ON MemberRoles.RID = SiteMembers.RIDFK "
                                            + "WHERE MemberRoles.Role = '" + Role + "'");

            return adname.Rows[0]["ADName"].ToString();
        }

        public static DataTable ActiveNonStudents()
        {
            return Sql.CCSelect("Select ADName, FirstName, LastName, LastName + ', ' + FirstName as LastFirstName, Email "
                                        + "from Sitemembers "
                                        + "Join MemberRoles ON SiteMembers.RIDFK = MemberRoles.RID "
                                        + "where Active = 1 "
                                        + "AND IsStudentRole = 0 order by LastFirstName");
        }

        //[ISMemberOf]
        //       Gets: Username to check and the role to check against
        //    Returns: True or False
        //Description: This function is used to check if a given username i.e. "pwinkle" is
        //  a user of a certain group or role. Roles for each user are stored in 
        //  the sitemembers table of the HDWEB database.
        public static bool IsMemberOf(string username, int roleid)
        {
            ArrayList members = new ArrayList();

            DataTable roleresult = Sql.CCSelect("SELECT SiteMembers.ADName FROM MemberRoles JOIN SiteMembers ON MemberRoles.RID = SiteMembers.RIDFK Where MemberRoles.RID = '" + roleid + "'");

            foreach (DataRow row in roleresult.Rows)
            {
                members.Add(row["ADName"].ToString());
            }

            if (members.Contains(username))
                return true;
            else
                return false;
        }

        public static DataTable ActiveStudents()
        {
            return Sql.CCSelect("Select *, LastName + ', ' + FirstName As LastFirstName from SiteMembers JOIN MemberRoles ON SiteMembers.RIDFK = MemberRoles.RID Where (IsStudentRole = 1) and (Active = 1) Order By LastName");
        }

        public static DataTable ActiveMembers()
        {
            return Sql.CCSelect("Select *, FirstName + ' ' + LastName As Fullname, LastName + ', ' + FirstName As LastFirstName from SiteMembers JOIN MemberRoles ON SiteMembers.RIDFK = MemberRoles.RID Where (Active = 1) Order By LastName");
        }

        //[IsinStudentRole]
        //    Returns: True or False
        //Description: This function is used to find if the currently login user
        //is in a student role
        public static bool IsinStudentRole()
        {
            DataTable isinroleTB = Sql.CCSelect("SELECT MemberRoles.IsStudentRole "
                                                + "FROM SiteMembers JOIN MemberRoles ON SiteMembers.RIDFK = MemberRoles.RID "
                                                + "Where SiteMembers.ADName = '" + AD.UserName + "'");
            try { return Convert.ToBoolean(isinroleTB.Rows[0]["IsStudentRole"]); }
            catch { return false; }
        }

        //[IsinStudentRole]
        //       Gets: Username to check if in a student role
        //    Returns: True or False
        //Description: This function is used to find if the given login user
        //is in a student role.
        public static bool IsinStudentRole(string username)
        {

            DataTable isinroleTB = Sql.CCSelect("SELECT MemberRoles.IsStudentRole "
                        + "FROM SiteMembers JOIN MemberRoles ON SiteMembers.RIDFK = MemberRoles.RID "
                        + "Where SiteMembers.ADName = '" + username + "'");
            try   {return Convert.ToBoolean(isinroleTB.Rows[0]["IsStudentRole"]); }
            catch { return false; }

        }

        //[BindStudentDD]
        //       Gets: Username to check if in a student role
        //    Returns: True or False
        //Description: This function is used to find if the given login user
        //is in a student role.
        public static void BindStudentDD(DropDownList StudentDD, string SemesterID)
        {
            DataTable StudentTable = Sql.CCSelect("SELECT SiteMembers.LastName + ', ' + SiteMembers.FirstName as FullName, SiteMembers.ADName "
                                                + "FROM SemesterSiteMembers JOIN SiteMembers ON SemesterSiteMembers.ADName = SiteMembers.ADName "
                                                + "where SemestersiteMembers.SemesterID = '" + SemesterID + "' ORDER BY LastName ");
            StudentDD.DataSource = StudentTable;
            StudentDD.DataTextField = "FullName";
            StudentDD.DataValueField = "ADName";
            StudentDD.DataBind();
            StudentDD.Items.Insert(0, new ListItem("---Select Student---", "-1"));
            StudentDD.SelectedIndex = 0;
        }

        public static void BindStudentDD(DropDownList StudentDD)
        {
            DataTable StudentTable = Sql.CCSelect("SELECT LastName + ', ' + FirstName as FullName, ADName "
                                                + "FROM SiteMembers JOIN MemberRoles ON SiteMembers.RIDFK = MemberRoles.RID "
                                                + "Where (IsStudentRole = 1) and (Active = 1) ORDER BY LastName ");
            StudentDD.DataSource = StudentTable;
            StudentDD.DataTextField = "FullName";
            StudentDD.DataValueField = "ADName";
            StudentDD.DataBind();
            StudentDD.Items.Insert(0, new ListItem("---Select Student---", "-1"));
            StudentDD.SelectedIndex = 0;
        }

        public static void BindRole(DropDownList RoleDD)
        {
            DataTable RoleTable = Sql.CCSelect("Select * from MemberRoles where IsStudentRole = 'true'");
            RoleDD.DataSource = RoleTable;
            RoleDD.DataTextField = "Role";
            RoleDD.DataValueField = "RID";
            RoleDD.DataBind();
        }

        public static void BindFeedbackDD(DropDownList FBDD, string ADName)
        {
            if(ADName != null)
            {
                FBDD.DataSource = Sql.CCSelect("SELECT LogID, DateStamp FROM FEEDBACK WHERE ADNameFK LIKE '" + ADName + "' AND DateStamp IS NOT NULL Order by DateStamp Desc");
                FBDD.DataValueField = "LogID";
                FBDD.DataTextField = "DateStamp";
                FBDD.DataBind();
            }
        }

        public static string ToFullName(string username)
        {
            try
            {
                DataTable dt = Sql.CCSelect("Select FirstName + ' ' + LastName AS FullName from SiteMembers where ADName = '" + username + "'");
                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["FullName"].ToString();
                else
                    return "not found";
            }
            catch { return "not found"; }
        }

        public static string ToLastFirstName(string username)
        {
            try
            {
                DataTable dt = Sql.CCSelect("Select LastName + ' ' + FirstName AS FullName from Users where ADName = '" + username + "'");
                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["FullName"].ToString();
                else
                    return "not found";
            }
            catch { return "not found"; }
        }

        public static string ToUserName(string fullname)
        {
            string[] name = fullname.Split(' ');
            try
            {
                DataTable dt = Sql.CCSelect("Select ADName from SiteMembers where FirstName = '" + name[0] + "' and LastName = '" + name[1] + "'");
                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["ADName"].ToString();
                else
                    return "not found";
            }
            catch { return "not found"; }
        }

        public static string GetEmail(string username)
        {
            DataTable dt = Sql.CCSelect("Select email from SiteMembers where ADName = '" + username + "'");
            return dt.Rows[0]["email"].ToString();
        }

        public static DataTable WWOrderedStuList()
        {
            return Sql.CCSelect("Select SiteMembers.ADName, FirstName + ' ' + LastName as FullName , RegisterDate, WWAssigned, SUM(WO_SiteMembers.Points) AS Points "
                                + "from SiteMembers " 
                                + "left JOIN WO_SiteMembers ON SiteMembers.ADName = WO_SiteMembers.ADName "
                                + "join MemberRoles ON SiteMembers.RIDFK = MemberRoles.RID "
                                + "Where Active = 1 "
                                + "and MemberRoles.IsStudentRole = 'true' "
                                + "Group by SiteMembers.ADName, FirstName, LastName, RegisterDate, WWAssigned "
                                + "Order By CONVERT(DATE, RegisterDate), Points desc ");
        }

        #endregion
    }

    public class WorkWeeks
    {
        #region Properties

        public int WWID
        {
            get { return _WWID; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public string ShortDesc
        {
            get { return _ShortDesc; }
            set { _ShortDesc = value; }
        }

        public int Maxstudents
        {
            get { return _maxstudents; }
            set { _maxstudents = value; }
        }

        public DateTime Startdate
        {
            get { return _startdate; }
            set { _startdate = value; }
        }

        public DateTime Enddate
        {
            get { return _enddate; }
            set { _enddate = value; }
        }

        #endregion

        #region Attributes

        private int _WWID = -1;
        private string _Description = "";
        private string _ShortDesc = "";
        private int _maxstudents = -1;
        private DateTime _startdate = Convert.ToDateTime("01/01/2000 1:01:01 AM");
        private DateTime _enddate = Convert.ToDateTime("01/01/2000 1:01:01 AM");
        
        #endregion

        #region constructor
         public WorkWeeks(int WWID)
            {
                try
                {
                    DataTable dt = Sql.CCSelect("SELECT * FROM WorkWeeks WHERE WWID = '" + WWID + "'");
                    
                    foreach (DataRow row in dt.Rows)
                    {
                        _WWID = WWID;
                        _Description = row["Description"].ToString();
                        _ShortDesc = row["ShortDesc"].ToString();
                        _maxstudents = Convert.ToInt32(row["NumAllowed"]);
                        _startdate = Convert.ToDateTime(row["StartDate"]);
                        _enddate = Convert.ToDateTime(row["EndDate"]);
                    }
                }
                catch
                {
                    //throw new Exception("Invalid Username");
                }
            }
        #endregion

        #region Member Methods

         public void Commit()
         {
             Sql.CCSelect("UPDATE WorkWeeks SET Description = '" + _Description + "', "
                                            + "ShortDesc = '" + _ShortDesc + "', "
                                            + "NumAllowed = " + _maxstudents.ToString() + ", "
                                            + "StartDate = '" + _startdate + "', "
                                            + "EndDate = '" + _enddate + "' "
                                            + "where WWID = " + _WWID);
         }

        #endregion

        #region Static Methods

        //Used to create a new work order
        //returns WWID created as int
        public static int NewWorkWeek(string Description, string Shortdesc, int maxstudents, DateTime startdate, DateTime enddate)
        {
            DataTable dt = Sql.CCSelect("INSERT INTO WorkWeeks (Description,ShortDesc,NumAllowed,StartDate,EndDate) VALUES ('" + Description + "','" + Shortdesc + "'," + maxstudents.ToString() + ",'" + startdate + "','" + enddate + "');" + "SELECT SCOPE_IDENTITY();");

            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }

        //Used to delete a work week and any related sitemembers
        public static void DeleteWorkWeek(int WWID)
        {

            Sql.CCSelect("If Exists(Select * from WorkWeeks where WWID = " + WWID + ") "
                    + "Begin "
                    + "Delete from WW_SiteMembers where WWIDFK = " + WWID
                    + " Delete From WorkWeeks where WWID = " + WWID
                    + " End");
        }

        //This function checks to see if work week assignment is in progress
        //returns true or false
        public static bool AssignInProgress()
        {

            if (SiteMembers.WWOrderedStuList().Rows[0]["WWAssigned"] is DBNull)
                return false;
            else
                return true;
        }

        //Used to find the current student who needs to pick work weeks 
        //returns ADNAME as string
        public static string StudentInProgress()
        {
            DataTable dt = Sql.CCSelect("Select SiteMembers.ADName, FirstName + ' ' + LastName as FullName , RegisterDate, WWAssigned, SUM(WO_SiteMembers.Points) AS Points "
                                    + "from SiteMembers "
                                    + "left JOIN WO_SiteMembers ON SiteMembers.ADName = WO_SiteMembers.ADName "
                                    + "join MemberRoles ON SiteMembers.RIDFK = MemberRoles.RID "
                                    + "Where Active = 1 "
                                    + "and MemberRoles.IsStudentRole = 'true' "
                                    + "AND WWAssigned = 'False' "
                                    + "Group by SiteMembers.ADName, FirstName, LastName, RegisterDate, WWAssigned "
                                    + "Order By CONVERT(DATE, RegisterDate), Points desc ");

            try { return dt.Rows[0]["ADNAME"].ToString(); }
            catch { return "assignment not in progress"; }
        }

        //Used to start the work week assignment process
        public static void StartAssignment()
        {
            Sql.CCSelect("Update Sitemembers set WWAssigned = 'False'");
        }

        //Used to stop the work week assignment process
        public static void StopAssignment()
        {
            Sql.CCSelect("update Sitemembers set WWAssigned = NULL");
        }

        public static void NextStudent()
        {
            if (AssignInProgress())
            {
                Sql.CCSelect("Update Sitemembers set WWAssigned = 'true' where ADName = '" + StudentInProgress() + "'");

                if (StudentInProgress().Equals("assignment not in progress"))
                    StopAssignment();
            }            
        }

        public static DataTable GetCurrentList()
        {
            return Sql.CCSelect("SELECT * "
                              + "FROM WorkWeeks "
                              + "WHERE EndDate > GETDATE()");
        }

        public static DataTable GetCurrentList(string adname)
        {
            return Sql.CCSelect("SELECT WWID, [Description], NumAllowed, StartDate, EndDate, ShortDesc "
                              + "FROM WorkWeeks "
                              + "JOIN WW_SiteMembers ON WorkWeeks.WWID = WW_SiteMembers.WWIDFK "
                              + "WHERE (WorkWeeks.EndDate > GETDATE()) "
                              + "AND (ADNAMEFK = '"+adname+"')");
        }


        public static DataTable GetOpenSpotsList()
        {
            return Sql.CCSelect("SELECT * "
                              + "FROM WorkWeeks ww "
                              + "WHERE EndDate > GETDATE() "
                              + "AND (Select (Select NumAllowed from WorkWeeks where WWID = ww.WWID) - Count(*) as Available from WW_SiteMembers Where WWIDFK = ww.WWID) > 0");
        }


        public static string GetAvail(int wwid)
        {
            string wwidstring = wwid.ToString();

            DataTable avail = Sql.CCSelect("Select (Select NumAllowed from WorkWeeks where WWID = " + wwidstring + ") - Count(*) as Available from WW_SiteMembers Where WWIDFK = " + wwidstring);
            return avail.Rows[0]["Available"].ToString();
        }

        public static void AssignStuToWW(string adname, int wwid)
        {
            Sql.CCSelect("If not exists (Select * from WW_SiteMembers where ADNAMEFK = '" + adname + "' and WWIDFK = " + wwid + ") "
                       + "Begin "
                       + "Insert into WW_SiteMembers (ADNAMEFK, WWIDFK) Values ('" + adname + "', " + wwid + ") "
                       + "End ");
        }

        public static void UnassignStuToWW(string adname, int wwid)
        {
            Sql.CCSelect("if exists (Select * from WW_SiteMembers where ADNAMEFK = '" + adname + "' and WWIDFK = " + wwid + ") "
                       + "Begin "
                       + "Delete from WW_SiteMembers where ADNAMEFK = '" + adname + "' and WWIDFK = " + wwid + " "
                       + "End");
        }

        public static bool IsAssigned(string adname, int wwid)
        {
            DataTable intersection = Sql.CCSelect("Select * from WW_SiteMembers where ADNAMEFK = '" + adname + "' AND WWIDFK = " + wwid.ToString());

            if (intersection.Rows.Count > 0)
                return true;
            else
                return false;
        }
        #endregion
    }

    public class Feedback
    {
        #region Properties

        public int EntryID
        {
            get {return _LogID;}
        }
                
        #endregion

        #region Attributes

            private int _LogID = -1;
            public string ADName = "";
            public string Good = "";
            public string Bad = "";
            public string GoalsMet = "";
            public string GoalsNotMet = "";
            public DateTime TimeStamp = Convert.ToDateTime("01/01/2000 1:01:01 AM");

        #endregion

        #region Constructors
        public Feedback()
        { }

        public Feedback(int LogID)
        {
            DataTable dt = Sql.CCSelect("SELECT * FROM FEEDBACK WHERE LogID = " + LogID + "");

            Internal(dt);            
        }

        public Feedback(string ADName, DateTime TimeStamp)
        {
            DataTable dt = Sql.CCSelect("Select LogID from FEEDBACK WHERE ADNAMEFK = '" + ADName + "' AND DataStamp = '" + TimeStamp + "'");

            Internal(dt);
        }

        private void Internal(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];

                if (r["LogID"] != DBNull.Value)
                    _LogID = Convert.ToInt32(r["LogID"]);

                if (r["ADNameFK"] != DBNull.Value)
                    ADName = r["ADNameFK"].ToString();

                if (r["Good"] != DBNull.Value)
                    Good = r["Good"].ToString();

                if (r["Bad"] != DBNull.Value)
                    Bad = r["Bad"].ToString();

                if (r["DateStamp"] != DBNull.Value)
                    TimeStamp = Convert.ToDateTime(r["DateStamp"]);

                if (r["GoalsMet"] != DBNull.Value)
                    GoalsMet = r["GoalsMet"].ToString();

                if (r["GoalsNotMet"] != DBNull.Value)
                    GoalsNotMet = r["GoalsNotMet"].ToString();
            }
        }

        #endregion

        #region Member Methods
        public void Commit()
        {
            Sql.CCSelect("UPDATE FEEDBACK SET Good='" + Good + "', " +
                                             "Bad='" + Bad + "', " +
                                             "GoalsMet='" + GoalsMet + "', " +
                                             "GoalsNotMet='" + GoalsNotMet + "', " +
                                             "DateStamp='" + TimeStamp + "' " +
                                             "WHERE LogID = " + _LogID);
        }



        public void SubmitFeedback()
        {
            if (HasSavedFeedback())
            {
                Sql.CCSelect("UPDATE FEEDBACK SET Good='" + Good + "',Bad='" + Bad + "',GoalsMet='" + GoalsMet + "',GoalsNotMet='" + GoalsNotMet + "',DateStamp='" + TimeStamp + "' WHERE ADNameFK='" + AD.UserName + "' AND DateStamp IS NULL");
            }
            else
            {
                Sql.CCSelect("INSERT INTO FEEDBACK (ADNameFK,Good,Bad,GoalsMet,GoalsNotMet,DateStamp) VALUES ('" + AD.UserName + "','" + Good + "','" + Bad + "','" + GoalsMet + "','" + GoalsNotMet + "','" + TimeStamp + "')");
            }
        }

        public void SaveFeedback()
        {
            if (HasSavedFeedback())
            {
                Sql.CCSelect("UPDATE FEEDBACK SET Good='" + Good + "',Bad='" + Bad + "',GoalsMet='" + GoalsMet + "',GoalsNotMet='" + GoalsNotMet + "' WHERE ADNameFK='" + AD.UserName + "' AND DateStamp IS NULL");
            }
            else
            {
                Sql.CCSelect("INSERT INTO FEEDBACK (ADNameFK,Good,Bad,GoalsMet,GoalsNotMet,DateStamp) VALUES ('" + AD.UserName + "','" + Good + "','" + Bad + "','" + GoalsMet + "','" + GoalsNotMet + "',NULL)");
            }
        }

        #endregion

        #region Static Methods

            public static DataTable GetGood(int logid, string UserName)
            {
                return Sql.CCSelect("SELECT * FROM FEEDBACK WHERE LogID = " + logid + " AND ADNameFK = '" + UserName + "'");
            }

            public static bool HasSubmittedFeedback()
            {
                DateTime LastSubmissionDate = DateTime.Now.AddDays(-5);
                DataTable dt = Sql.CCSelect("SELECT top 1 (DateStamp) FROM FEEDBACK WHERE ADNameFK = '" + AD.UserName + "' AND DateStamp IS NOT NULL ORDER BY DateStamp DESC");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        LastSubmissionDate = Convert.ToDateTime(row["DateStamp"]);
                    }
                }
                if (LastSubmissionDate >= DateTime.Now.AddDays(-4))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static bool HasEntry(string username)
            {
                DataTable dt = Sql.CCSelect("Select * from FEEDBACK WHERE ADNameFK = '" + username + "'");
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }

            public static bool HasSavedFeedback()
            {
                DataTable dt = Sql.CCSelect("SELECT * FROM FEEDBACK WHERE ADNameFK = '" + AD.UserName + "' AND DateStamp IS NULL");
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }

        #endregion
    }

    public delegate void InstantiateTemplateDelegate(Control container);

    public class TemplateHandler : ITemplate
    {
        private InstantiateTemplateDelegate m_instantiateTemplate;

        public TemplateHandler(InstantiateTemplateDelegate instantiateTemplate)
        {
            m_instantiateTemplate = instantiateTemplate;
        }

        public void InstantiateIn(Control container)
        {
            m_instantiateTemplate(container);
        }
    }

    public class Software
    {

        #region Attributes
        public int SID = -1;
        public string Name = "";
        public string Company = "";
        public string Version = "";
        public string Serial = "";
        public string InstallLocation = "";
        public string InstallScript = "";
        public string RegKey = "";
        public bool Current = false;
        #endregion

        #region Properties

        #endregion

        #region Constructors
        Software(int SID)
        {
            DataTable Check = Sql.CCSelect("SELECT * FROM SOFTWARE WHERE SID = " + SID);
            if (Check.Rows.Count > 0)
            {
                this.SID = SID;
                Name = Check.Rows[0]["Name"].ToString();
                Company = Check.Rows[0]["Company"].ToString();
                Version = Check.Rows[0]["Version"].ToString();
                Serial = Check.Rows[0]["Serial"].ToString();
                InstallLocation = Check.Rows[0]["InstallLocation"].ToString();
                InstallScript = Check.Rows[0]["InstallScript"].ToString();
                RegKey = Check.Rows[0]["RegKey"].ToString();
                Current = Convert.ToBoolean(Check.Rows[0]["Current"].ToString());
            }
        }
        #endregion

        #region Member Methods


        #endregion

        #region Static Methods

        public static int AddSoftwareToDB(string _name, string _company, string _version, bool _current)
        {
           //DataTable sid =  Sql.CCSelect("IF NOT EXISTS(SELECT * FROM SOFTWARE WHERE name = '" + _name + "' and company = '" + _company + "' and version = '" + _version + "') INSERT INTO SOFTWARE (Name, Company, Version, [Current]) VALUES ('" + _name + "','" + _company + "','" + _version + "','"  + _current + "');" + "SELECT SCOPE_IDENTITY() AS SID; ");
            DataTable sid = Sql.CCSelect("IF NOT EXISTS(SELECT * FROM SOFTWARE WHERE name = '" + _name + "' and company = '" + _company + "' and version = '" 
                + _version + "') BEGIN INSERT INTO SOFTWARE (Name, Company, Version, [Current]) VALUES ('" + _name + "','" + _company + "','" 
                + _version + "','" + _current + "');" + "SELECT SCOPE_IDENTITY() AS SID; END ELSE BEGIN SELECT SID FROM SOFTWARE WHERE name = '" 
                + _name + "' and company = '" + _company + "' and version = '" + _version + "' END ");
           return Convert.ToInt32(sid.Rows[0]["SID"]);
        }

        public static void AddSoftwareToItem(int sid, string target)
        {
            DataTable Item = Sql.CCSelect("SELECT * FROM ITEM WHERE IPAddress = " + "'" + target + "'");
            DataTable ItemSoftware = Sql.CCSelect("IF NOT EXISTS(SELECT * FROM ITEM_SOFTWARE WHERE SID = '" + sid + "' and ITEMID = '" + Item.Rows[0]["ITEMID"].ToString() + "') INSERT INTO ITEM_SOFTWARE(SID, ITEMID) VALUES ('" + sid.ToString() + "','" + Item.Rows[0]["ITEMID"].ToString() + "')");
        }

        public static bool PingIT(string target)
        {
            Ping ping = new Ping();

            if (target == "" || target == null)
                return false;

            try
            {
                ping.Send(target);
                PingReply reply = ping.Send(target);

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

        public static DataTable GetIP()
        {
            DataTable ip = Sql.CCSelect("SELECT it.ITEMID, it.IPAddress FROM ITEM it WHERE it.TYPEID = 1 AND (it.IPAddress Is Not Null AND it.IPAddress <> '')");
            return ip;
        }

        public static int GetItemByIPAddress(string target)
        {
            DataTable item = Sql.CCSelect("SELECT it.ITEMID FROM ITEM it WHERE it.IPAddress = " + "'" + target + "'");
            return Convert.ToInt32(item.Rows[0]["ITEMID"]);
        }

        public static int GetInvByIPAddress(string target)
        {
            DataTable item = Sql.CCSelect("SELECT it.InvN FROM ITEM it WHERE it.IPAddress = " + "'" + target + "'");
            return Convert.ToInt32(item.Rows[0]["InvN"]) ;
        }

        public static string GetIPAddressByItemID(int itemID)
        {
            DataTable iptable = Sql.CCSelect("SELECT it.IPAddress FROM ITEMS it WHERE ITEMID = " + "'" + itemID + "'");
            return iptable.Rows[0]["IPAddress"].ToString();
        }

        public static string GetDNSByIPAddress(string ip)
        {
            DataTable dnsTable = Sql.CCSelect("SELECT IT.DNSNAME FROM ITEM IT WHERE IPADDRESS = " + "'" + ip + "'");
            return dnsTable.Rows[0]["DNSName"].ToString();
        }

        public static int GetItemByInvNumber(int invn)
        {
            DataTable item = Sql.CCSelect("SELECT it.ItemID FROM ITEM it WHERE it.INVN = " + "'" + invn + "'");
            return Convert.ToInt32(item.Rows[0]["ITEMID"]);
        }

        public static DataTable GetSoftwareByItemID(string target)
        {
            DataTable software = Sql.CCSelect("SELECT s.* FROM SOFTWARE s JOIN ITEM_SOFTWARE its ON its.SID = s.SID JOIN ITEM it ON it.ITEMID = its.ITEMID WHERE it.ITEMID = " + "'" + target + "' ORDER BY s.Name" );
            return software;
        }

        public static DataTable GetSoftwareBySID(int sid)
        {
            DataTable software = Sql.CCSelect("SELECT s.* FROM SOFTWARE s WHERE s.SID = " + "'" + sid + "'");
            return software;
        }

        public static string GetBatchBySoftwareID(string target)
        {
            DataTable batch = Sql.CCSelect("SELECT s.Batch FROM SOFTWARE s WHERE s.SID = " + "'" + target + "'");
            return batch.ToString();
        }

        public static string GetIPAddressByInv(int inv)
        {
            DataTable ip = Sql.CCSelect("SELECT I.IPADDRESS FROM ITEM I WHERE INVN = " + "'" + inv + "'");
            return ip.Rows[0]["IPADDRESS"].ToString();
        }

        public static string GetIPAddressByDNS(string dns)
        {
            DataTable ip = Sql.CCSelect("SELECT I.IPADDRESS FROM ITEM I WHERE DNSNAME = " + "'" + dns + "'");
            return ip.Rows[0]["IPADDRESS"].ToString();
        }

        public static int GetItemIDByDNS(string dns)
        {
            DataTable item = Sql.CCSelect("SELECT I.ITEMID FROM ITEM I WHERE DNSNAME = " + "'" + dns + "'");
            return Convert.ToInt32(item.Rows[0]["ITEMID"]);
        }

        public static int NewSoftware(string DisplayName, string Company, string ver, string regkey)
        {
            DataTable dt = Sql.CCSelect("INSERT INTO Software (Name, Company, Version, RegKey) "
                                      + "Values ('" + DisplayName + "','" + Company + "','" + ver + "','" + regkey + "'); SELECT SCOPE_IDENTITY();");

            return Convert.ToInt32(dt.Rows[0][0]);
        }

        public static DataTable GetFromPC(string target)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Company");
            dt.Columns.Add("Version");
            dt.Columns.Add("RegKey");

            // Impersonate, automatically release the impersonation.
            using (new Tools.Impersonator("helpdeskweb", "COFO", "2010Bolger"))
            {
                //target = "172.16.12.40";
                if (Item.SuccessPing(target))
                {
                    try//to access remote computer
                    {


                        RegistryKey rk = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, target).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

                        foreach (string skName in rk.GetSubKeyNames())
                        {
                            using (RegistryKey sk = rk.OpenSubKey(skName))
                            {
                                try
                                {
                                    //If the key has value, continue, if not, skip it:
                                    if (sk.GetValue("DisplayName") != null)
                                    {
                                        int SID = -1;
                                        string Name = sk.GetValue("DisplayName").ToString();
                                        string Company = sk.GetValue("Publisher").ToString();
                                        string Version = "";
                                        if (sk.GetValue("VersionMajor").ToString() != "")
                                            Version = sk.GetValue("VersionMajor").ToString();
                                        string RegKey = skName;

                                        //--------------------------------------------
                                        DataTable sql = Sql.CCSelect("SELECT SID FROM SOFTWARE WHERE Name = '" + Name + "' AND Company = '" + Company + "' AND Version = '" + Version + "'");

                                        if (sql.Rows.Count < 1)//DOES NOT EXIST
                                        {

                                        }
                                        else
                                        {
                                            SID = Convert.ToInt32(sql.Rows[0]["SID"]);
                                        }

                                        //---------------------------------------------

                                        DataRow _row = dt.NewRow();
                                        _row["SID"] = SID.ToString();
                                        _row["Name"] = Name;
                                        _row["Company"] = Company;
                                        _row["Version"] = Version;
                                        _row["RegKey"] = RegKey;

                                        dt.Rows.Add(_row);

                                    }
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                    catch { }
                }

            }

            return dt;
            
        }


        #endregion
    }

    public class Schedule
    {
        #region Properties
        
        //GET AND SET ADNAME
        public string ADName
        {
            get { return _ADName; }
            set { _ADName = value; }
        }

        //GET AND SET DAY
        public string Day
        {
            get { return _Day; }
            set { _Day = value; }
        }

        //GET AND SET SEMESTERID
        public string SemesterID
        {
            get { return _SemesterID; }
            set { _SemesterID = value; }
        }
        
        //GET AND SET SSMID
        public int SSMID
        {
            get { return _SSMID; }
            set { _SSMID = value; }
        }

        //RETRIEVE HTML CREATED TABLE
        public string ScheduleView
        {
            get 
            {
                if (_ScheduleLayout == "master")
                {
                    _scheduleView = consMasterSchedule();
                }
                return _scheduleView;
            }
        }

        //GET DataTable
        public DataTable ScheduleDT
        {
            get { return _DT ; }
        }

        //get error
        public bool error
        {
            get { return _error; }
        }

        public int MasterRoleID
        {
            get { return _MasterRoleID; }
            set { _MasterRoleID = value; }
        }

        #endregion

        #region Attributes

        string _ADName = AD.UserName;
        string _Day = "Monday";
        string _SemesterID = Semester.CurrentIDstring();
        string _scheduleView;
        int _SSMID = 0;
        DataTable _DaysDT = Sql.CCSelect("Select * from [Day]");
        DataTable _TimeDT = Sql.CCSelect("Select * from [Time]");
        DataTable _DT = new DataTable();
        int _MasterRoleID = -1;
        bool _error = false;
        string _ScheduleLayout = "student";

        #endregion

        #region Constructor

        //MASTER TABLE CONSTRUCTOR
        public Schedule(string SemesterID)
        {
            _SemesterID = SemesterID;
            _ScheduleLayout = "master";
        }

        //STUDENT TABLE CONSTRUCTOR
        public Schedule(string SemesterID, string StudentName)
        {
            _SemesterID = SemesterID;
            _ADName = StudentName;
            
            if (_SSMID == 0)
            {
                DataTable SSMIDTB = Sql.CCSelect("Select SemSiteMemID From SemesterSiteMembers where ADName = '" + _ADName + "' and SemesterID = " + _SemesterID);

                //if SSMID Does not exists
                if (SSMIDTB.Rows.Count == 0)
                {
                    _scheduleView = "SSMID not found";
                    _error = true;
                }
                else
                {
                    _SSMID = Convert.ToInt32(SSMIDTB.Rows[0]["SemSiteMemID"]);
                    
                }
                
            }

            //CREATE DATATABLE TO BIND TO GRIDVIEW
            _DT = bindStuSchedule(_SSMID);

            //CREATE STRING TO FEED TO LITERAL
            _scheduleView = consStuSchedule(_SSMID);

            _ScheduleLayout = "student";

        }

        //DAY TABLE CONSTRUCTOR
        public Schedule(string SemesterID, DayOfWeek Day)
        {
            _ScheduleLayout = "day";
        }
                
        #endregion

        #region Member Methods

        public DataTable DaysDT()
        {
            return _DaysDT;
        }

        public DataTable TimeDT()
        {
            return _TimeDT;
        }
        
        #endregion
        
        #region Static Methods

        private string consStuSchedule(int ssmid)
        {
            StringBuilder scheduletext = new StringBuilder();

            scheduletext.Append("<table id='ScheduleTable' class='ScheduleStyle'><th></th>");
            
            foreach (DataRow day in _DaysDT.Rows)
            {
                scheduletext.Append("<th>" + day["Day"].ToString() + "</th>");
            }

            scheduletext.Append("</tr>");

            foreach (DataRow time in _TimeDT.Rows)
            {
                int t = Convert.ToInt32(_TimeDT.Rows.IndexOf(time)) + 1;
                scheduletext.Append("<tr>");
                for (int d = 0; d < 6; d++)
                {
                    if (d == 0)
                    {
                        scheduletext.Append("<td class='hourTD'>" + time["Time"].ToString() + "</td>");
                    }
                    else
                    {
                        DataTable cellDT = Sql.CCSelect("SELECT Schedule.SID, Schedule.SemSiteMemID, Schedule.TimeID, Schedule.DayID, Schedule.STIDFK, Schedule.Text, Schedule.FN, ScheduleType.DisplayName "
                                                      + "FROM Schedule JOIN ScheduleType ON Schedule.STIDFK = ScheduleType.STID "
                                                      + "WHERE (Schedule.SemSiteMemID = " + ssmid + ") AND (Schedule.TimeID = " + time["TimeID"].ToString() + ") AND (Schedule.DayID = " + d + ")");

                        //cellDT.Rows[0][""].ToString();

                        if (cellDT.Rows.Count != 0)
                        {
                            switch (cellDT.Rows[0]["DisplayName"].ToString())
                            {
                                case "Work":
                                    scheduletext.Append("<td class='dayTD workDiv' ID='t" + t + "d" + d + "' onclick='changetheme(" + t + "," + d + ")'>Work</td>");
                                    break;
                                case "Lunch":
                                    scheduletext.Append("<td class='dayTD lunchDiv' ID='t" + t + "d" + d + "' onclick='changetheme(" + t + "," + d + ")'>Lunch</td>");
                                    break;
                                case "Phone":
                                    scheduletext.Append("<td class='dayTD phoneDiv' ID='t" + t + "d" + d + "' onclick='changetheme(" + t + "," + d + ")'>Phone</td>");
                                    break;
                                case "Class":
                                    scheduletext.Append("<td  title='" + cellDT.Rows[0]["FN"].ToString().Trim() + "' class='dayTD classDiv' ID='t" + t + "d" + d + "' >" + cellDT.Rows[0]["Text"].ToString() + "</td>");
                                    break;
                            }


                        }
                        else
                        {
                            scheduletext.Append("<td ID='t" + t + "d" + d + "' class='dayTD' onclick='changetheme(" + t + "," + d + ")'></td>");
                        }

                    }

                }
                scheduletext.Append("</tr>");
            }
            return scheduletext.ToString();
        }

        private DataTable bindStuSchedule(int ssmid)
        {
            //SETUP DATATABLE TO RETURN
            DataTable ScheduleDT = new DataTable();

            //CONSTRUCT COLUMNS
            ScheduleDT.Columns.Add("Time");
            foreach (DataRow day in _DaysDT.Rows)
            {
                ScheduleDT.Columns.Add(day["Day"].ToString());
            }
            
            //CONSTRUCT ROWS
            foreach (DataRow time in _TimeDT.Rows)
            {
                DataRow DTrow = ScheduleDT.NewRow();

                foreach (DataColumn d in ScheduleDT.Columns)
                {
                    if (DataColumn.Equals(d, ScheduleDT.Columns["Time"]))
                    {
                        DTrow[d] = time["Time"].ToString();
                    }
                    else
                    {
                        DataTable celldt = Sql.CCSelect("SELECT Schedule.Text, Schedule.FN, ScheduleType.DisplayName AS Type, Time.Time, Day.Day "
                                   + "FROM Schedule "
                                   + "JOIN ScheduleType ON Schedule.STIDFK = ScheduleType.STID "
                                   + "JOIN Time ON Schedule.TimeID = Time.TimeID "
                                   + "JOIN Day ON Schedule.DayID = Day.DayID "
                                   + "WHERE (Schedule.SemSiteMemID = '" + ssmid + "') AND (Time.Time = '" + time["Time"].ToString() + "') AND (Day.Day = '" + d.ColumnName.ToString() + "') "
                                   + "ORDER BY Schedule.DayID, Schedule.TimeID");


                        if(celldt.Rows.Count > 0)
                        {
                            switch (celldt.Rows[0]["Type"].ToString())
                            {
                                case "Work":
                                    DTrow[d] = "Work";
                                    break;
                                case "Lunch":
                                    DTrow[d] = "Lunch";
                                    break;
                                case "Phone":
                                    DTrow[d] = "Phone";
                                    break;
                                case "Class":
                                    DTrow[d] = celldt.Rows[0]["Text"].ToString();
                                    break;
                            }
                        }
                        
                    }
                }

                ScheduleDT.Rows.Add(DTrow);
            }

            return ScheduleDT;
        }

        private string consMasterSchedule()
        {
            StringBuilder scheduletext = new StringBuilder();

            scheduletext.Append("<table id='scheduleTable' class='stuschedule'><tr class='bottomLine'><th></th>");

            foreach (DataRow day in _DaysDT.Rows)
            {
                scheduletext.Append("<th>" + day["Day"].ToString() + "</th>");
            }

            scheduletext.Append("</tr>");

            foreach (DataRow time in _TimeDT.Rows)
            {
                int t = Convert.ToInt32(_TimeDT.Rows.IndexOf(time)) + 1;
                scheduletext.Append("<tr>");
                for (int d = 0; d < 6; d++)
                {
                    if (d == 0)
                    {
                        scheduletext.Append("<td class='hourTD'>" + time["Time"].ToString() + "</td>");
                    }
                    else
                    {

                        DataTable Students = Sql.CCSelect("SELECT SiteMembers.FirstName, SiteMembers.LastName "
                                                        + "FROM SiteMembers "
                                                        + "JOIN SemesterSiteMembers ON SiteMembers.ADName = SemesterSiteMembers.ADName "
                                                        + "JOIN Schedule ON SemesterSiteMembers.SemSiteMemID = Schedule.SemSiteMemID "
                                                        + "JOIN MemberRoles ON SiteMembers.RIDFK = MemberRoles.RID "
                                                        + "WHERE (Schedule.TimeID = " + t + ") AND (Schedule.DayID = " + d + ") AND (SemesterSiteMembers.SemesterID = " + _SemesterID + ") "
                                                        + "AND (Schedule.STIDFK = 1) AND (MemberRoles.RID = " + _MasterRoleID + ")");

                        DataTable Studentsall = Sql.CCSelect("SELECT SiteMembers.FirstName, SiteMembers.LastName "
                                                       + "FROM SiteMembers JOIN SemesterSiteMembers ON SiteMembers.ADName = SemesterSiteMembers.ADName "
                                                       + "JOIN Schedule ON SemesterSiteMembers.SemSiteMemID = Schedule.SemSiteMemID "
                                                       + "WHERE (Schedule.TimeID = " + t + ") AND (Schedule.DayID = " + d + ") AND (SemesterSiteMembers.SemesterID = " + _SemesterID + ") AND (Schedule.STIDFK = 1)");


                        scheduletext.Append("<td ID='t" + t + "d" + d + "' class='dayTD'>");

                        if (MasterRoleID == -1)
                        {
                            try
                            {
                                foreach (DataRow row in Studentsall.Rows)
                                {
                                    scheduletext.Append(row["FirstName"].ToString() + " " + row["LastName"].ToString() + "<br />");
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            try
                            {
                                foreach (DataRow row in Students.Rows)
                                {
                                    scheduletext.Append(row["FirstName"].ToString() + " " + row["LastName"].ToString() + "<br />");
                                }
                            }
                            catch { }
                        }

                        scheduletext.Append("</td>");
                    }

                }//Day for loop end

            }//Time foreach loop end


            return scheduletext.ToString();
        }
     
        #endregion
    }

    public class ScheduleCell
    {
        #region Properties

        public int CellID
        {
            get { return _CellID; }
        }

        public int TimeID
        {
            get { return _TimeID; }
            set
            {
                try
                {
                    _TimeID = value;
                    DataTable TimeTB = Sql.CCSelect("Select [Time] from [Time] where [Time].TimeID = " + value);
                    _Time = TimeTB.Rows[0]["Time"].ToString();
                }
                catch
                {
                    _Time = "";
                    _TimeID = -1;
                }
            }
        }

        public string Time
        {
            get { return _Time; }
            set
            {
                try
                {
                    _Time = value;
                    DataTable TimeIDTB = Sql.CCSelect("Select TimeID from [Time] where [Time].[Time] = '" + value + "'");
                    _TimeID = Convert.ToInt32(TimeIDTB.Rows[0]["TimeID"]);
                }
                catch
                {
                    _Time = "";
                    _TimeID = -1;
                }
            }
        }

        public int DayID
        {
            get { return _DayID; }
            set
            {
                try
                {
                    _DayID = value;
                    DataTable DayTB = Sql.CCSelect("Select Day from [Day] where [Day].DayID = " + value);
                    _Day = DayTB.Rows[0]["Day"].ToString();
                }
                catch
                {
                    _Day = "";
                    _DayID = -1;
                }

            }
        }

        public string Day
        {
            get { return _Day; }
            set
            {
                try
                {
                    _Day = value;
                    DataTable DayIDTB = Sql.CCSelect("Select DayID from [Day] where [Day] ='" + value + "'");
                    _DayID = Convert.ToInt32(DayIDTB.Rows[0]["DayID"]);
                }
                catch
                {
                    _Day = "";
                    _DayID = -1;
                }
            }
        }

        public int ScheduleTypeID
        {
            get { return _ScheduleTypeID; }
            set
            {
                try
                {
                    _ScheduleTypeID = value;
                    DataTable ScheduleTypeTB = Sql.CCSelect("Select DisplayName from ScheduleType where STID = " + value);
                    _ScheduleType = ScheduleTypeTB.Rows[0]["DisplayName"].ToString();
                    if (_ScheduleType != "Class")
                    {
                        _ClassID = "";
                        _ClassFullName = "";
                    }
                }
                catch
                {
                    _ScheduleType = "";
                    _ScheduleTypeID = -1;
                }
            }
        }

        public string ScheduleType
        {
            get { return _ScheduleType; }
            set
            {
                try
                {
                    _ScheduleType = value;
                    DataTable ScheduleTypeIDTB = Sql.CCSelect("Select STID from ScheduleType where Displayname = '" + value + "'");
                    _ScheduleTypeID = Convert.ToInt32(ScheduleTypeIDTB.Rows[0]["DisplayName"]);
                    if (value != "Class")
                    {
                        _ClassID = "";
                        _ClassFullName = "";
                    }
                }
                catch
                {
                    _ScheduleType = "";
                    _ScheduleTypeID = -1;
                }
            }
        }

        public string ClassID
        {
            get { return _ClassID; }
            set { _ClassID = value; }
        }

        public string ClassFullName
        {
            get { return _ClassFullName; }
        }

        public int SSMID
        {
            get { return _SSMID; }
            set { _SSMID = value; }
        }

        public int numberofstudentsworking
        {
            get
            {

                DataTable number = Sql.CCSelect("SELECT COUNT(*) AS Working "
                                              + "FROM Schedule JOIN SemesterSiteMembers ON Schedule.SemSiteMemID = SemesterSiteMembers.SemSiteMemID "
                                              + "where STIDFK = 1"
                                              + "and DayID = " + _DayID
                                              + "and TimeID = " + _TimeID
                                              + "and SemesterID = (SELECT SemesterID FROM SemesterSiteMembers where SemSiteMemID = " + _SSMID + ")");

                return Convert.ToInt32(number.Rows[0]["Working"]);
            }
        }

        #endregion

        #region Attributes

        int _CellID = -1;
        int _TimeID = -1;
        string _Time = "";
        int _DayID = -1;
        string _Day = "";
        int _ScheduleTypeID = -1;
        string _ScheduleType = "";
        string _ClassID = "";
        string _ClassFullName = "";
        int _SSMID = 0;
        bool _OnDB = true;

        #endregion

        #region Constructor

        public ScheduleCell(int CellID)
        {
            DataTable Check = Sql.CCSelect("SELECT * "
                                         + "FROM Schedule JOIN ScheduleType ON Schedule.STIDFK = ScheduleType.STID "
                                         + "JOIN [Day] ON Schedule.DayID = [Day].DayID "
                                         + "JOIN [Time] ON Schedule.TimeID = [Time].TimeID "
                                         + "WHERE    Sid = " + CellID);
            if (Check.Rows.Count > 0)
            {
                _OnDB = true;
                _CellID = CellID;
                _TimeID = Convert.ToInt32(Check.Rows[0]["TimeID"]);
                _Time = Check.Rows[0]["Time"].ToString();
                _DayID = Convert.ToInt32(Check.Rows[0]["DayID"]);
                _Day = Check.Rows[0]["Day"].ToString();
                _ScheduleTypeID = Convert.ToInt32(Check.Rows[0]["STIDFK"]);
                _ScheduleType = Check.Rows[0]["DisplayName"].ToString();
                _ClassID = Check.Rows[0]["Text"].ToString();
                _ClassFullName = Check.Rows[0]["FN"].ToString();
                _SSMID = Convert.ToInt32(Check.Rows[0]["SemSiteMemID"]);
            }
            else
            {
                _OnDB = false;
            }
            
        }

        public ScheduleCell(int ssmid, string Time, string Day)
        {
            DataTable Check = Sql.CCSelect("SELECT * "
                                         + "FROM Schedule JOIN ScheduleType ON Schedule.STIDFK = ScheduleType.STID "
                                         + "JOIN [Day] ON Schedule.DayID = [Day].DayID "
                                         + "JOIN [Time] ON Schedule.TimeID = [Time].TimeID "
                                         + "WHERE SemSiteMemID = " + ssmid + " AND [Time] = '" + Time + "' AND [DAY] = '" + Day + "'");
            if (Check.Rows.Count > 0)
            {
                _CellID = Convert.ToInt32(Check.Rows[0]["SID"]);
                _TimeID = Convert.ToInt32(Check.Rows[0]["TimeID"]);
                _Time = Check.Rows[0]["Time"].ToString();
                _DayID = Convert.ToInt32(Check.Rows[0]["DayID"]);
                _Day = Check.Rows[0]["Day"].ToString();
                _ScheduleTypeID = Convert.ToInt32(Check.Rows[0]["STIDFK"]);
                _ScheduleType = Check.Rows[0]["DisplayName"].ToString();
                _ClassID = Check.Rows[0]["Text"].ToString();
                _ClassFullName = Check.Rows[0]["FN"].ToString();
                _SSMID = Convert.ToInt32(Check.Rows[0]["SemSiteMemID"]);
            }
            else
            {
                _OnDB = false;
                _SSMID = ssmid;
                this.Time = Time;
                this.Day = Day;
            }
        }

        public ScheduleCell(int ssmid, int TimeID, int DayID)
        {
            DataTable Check = Sql.CCSelect("SELECT * "
                                         + "FROM Schedule JOIN ScheduleType ON Schedule.STIDFK = ScheduleType.STID "
                                         + "JOIN [Day] ON Schedule.DayID = [Day].DayID "
                                         + "JOIN [Time] ON Schedule.TimeID = [Time].TimeID "
                                         + "WHERE SemSiteMemID = " + ssmid + " AND Schedule.TimeID = '" + TimeID + "' AND Schedule.DayID = '" + DayID + "'");
            if (Check.Rows.Count > 0)
            {
                _OnDB = true;
                _CellID = Convert.ToInt32(Check.Rows[0]["SID"]);
                _TimeID = Convert.ToInt32(Check.Rows[0]["TimeID"]);
                _Time = Check.Rows[0]["Time"].ToString();
                _DayID = Convert.ToInt32(Check.Rows[0]["DayID"]);
                _Day = Check.Rows[0]["Day"].ToString();
                _ScheduleTypeID = Convert.ToInt32(Check.Rows[0]["STIDFK"]);
                _ScheduleType = Check.Rows[0]["DisplayName"].ToString();
                _ClassID = Check.Rows[0]["Text"].ToString();
                _ClassFullName = Check.Rows[0]["FN"].ToString();
                _SSMID = Convert.ToInt32(Check.Rows[0]["SemSiteMemID"]);
            }
            else
            {
                _OnDB = false;
                _SSMID = ssmid;
                this.TimeID = TimeID;
                this.DayID = DayID;
            }
        }

        #endregion

        #region Member Methods

        public string commit()
        {
            
                if (_OnDB)
                {
                    if (_ScheduleTypeID != -1)
                    {
                        Sql.CCSelect("Update Schedule set stidfk = " + _ScheduleTypeID
                                                   + ", TimeID = " + _TimeID
                                                   + ", DayID = " + _DayID
                                                   + ", SemSiteMemID = " + _SSMID
                                                   + ", [Text] = '" + _ClassID
                                                   + "', FN = '" + _ClassFullName
                                                   + "' where SID = " + _CellID);
                        return "U";
                    }
                    else
                    {
                        Sql.CCSelect("Delete FROM Schedule where SID = " + _CellID);
                        return "D";
                    }
                }
                else
                {
                    if (_ScheduleTypeID != -1)
                    {
                        DataTable NewID = Sql.CCSelect("Insert INTO Schedule (SemSiteMemID, TimeID, DayID, STIDFK, [Text], FN) "
                                   + "Values (" + _SSMID + "," + _TimeID + "," + _DayID + "," + _ScheduleTypeID + ",'" + _ClassID + "','" + _ClassFullName + "'); SELECT SCOPE_IDENTITY() AS NewID");
                        _CellID = Convert.ToInt32(NewID.Rows[0]["NewID"]);
                        _OnDB = true;
                        return "C";
                    }
                    else
                    {
                        return "R";
                    }
                }
        }

        #endregion

        #region Static Methods

  
        #endregion
    }

    public class CMS
    {
        #region Properties
        public int CID
        {
            get { return _CID; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
        public string EditedContent
        {
            get { return _EditedContent; }
            set { _EditedContent = value; }
        }
        public bool IsPublished
        {
            get { return _IsPublished; }
            set { _IsPublished = value; }
        }
        public string LastModifiedBy
        {
            get { return _LastModifiedBy; }
            set { _LastModifiedBy = value; }
        }
        public bool IsHelpDesk
        {
            get { return _IsHelpDesk; }
            set { _IsHelpDesk = value; }
        }
        public int CategoryFK
        {
            get { return _CategoryFK;}
            set { _CategoryFK = value; }
        }
        #endregion
    
        #region Attributes
        private int _CID;
        private string _Content = "";
        private string _EditedContent = "";
        private bool _IsPublished = true;
        private string _LastModifiedBy = "";
        private DateTime _LastModifiedTime = DateTime.Now;
        private string _Description = "";
        private bool _IsHelpDesk = false;
        private int _CategoryFK = 4;
        #endregion
    
        #region Constructors
        public CMS(int CID)
        {
            DataTable Check = Sql.CCSelect("SELECT * FROM CMS_CONTENT WHERE CMS_ID = " + CID);
            if (Check.Rows.Count > 0)
            {
                _CID = CID;
                _Content = Check.Rows[0]["Content"].ToString();
                _EditedContent = Check.Rows[0]["EditedContent"].ToString();
                _IsPublished = Convert.ToBoolean(Check.Rows[0]["IsPublished"]);
                _LastModifiedBy = Check.Rows[0]["LastModifiedBy"].ToString();
                _LastModifiedTime = Convert.ToDateTime(Check.Rows[0]["LastModifiedTime"]);
                _Description = Check.Rows[0]["Description"].ToString();
                _IsHelpDesk = Convert.ToBoolean(Check.Rows[0]["IsHelpDesk"]);
                _CategoryFK = Convert.ToInt32(Check.Rows[0]["CatFK"]);
            }
            //else
            //    throw new Exception("Invalid Note ID");
        }
        #endregion
    
        #region Member Methods
        public void Commit()
        {

            Sql.CCSelect("UPDATE CMS_Content SET "
                + "EditedContent = '" + _EditedContent.Replace("'","''") + "', "
                + "IsPublished = 'false', "
                + "LastModifiedTime = '" + DateTime.Now + "', "
                + "LastModifiedBy = '" + AD.UserName + "', "
                + "Description = '" + _Description + "', "
                + "IsHelpDesk = '" + _IsHelpDesk + "', "
                + "CatFK = " + _CategoryFK + " "
                + "WHERE CMS_ID = " + _CID); 
        }

        public void Publish()
        {
            this.Commit();
            Sql.CCSelect("UPDATE CMS_CONTENT SET "
                + "Content ='" + _EditedContent.Replace("'","''") + "',"
                + "IsPublished='true'"
                + "WHERE CMS_ID = " + _CID);
        }

        public void Revert()
        {
            this.Commit();
            Sql.CCSelect("UPDATE CMS_CONTENT SET "
                + "EditedContent ='" + _Content + "',"
                + "IsPublished='true'"
                + "WHERE CMS_ID = " + _CID);
        }

        public void Delete()
        {
            Sql.CCSelect("DELETE FROM CMS_CONTENT "
                        + "WHERE CMS_ID = " + _CID); 
        }

        #endregion
    
        #region Static Methods
    
        public static int NewContent(string Content, string Description, bool HelpDeskCheck, int CategoryFK)
        {
            //if (Check.Rows[0][0].Equals("0"))
            //throw new Exception("Invalid CMS_Content ID");
            DataTable dt = Sql.CCSelect("INSERT INTO CMS_Content (EditedContent, IsPublished, LastModifiedTime, LastModifiedBy, [Description], IsHelpDesk, CatFK) " +
                                        "VALUES ('" + Content.Replace("'", "''") + "','FALSE','" + DateTime.Now + "','" + AD.UserName + "', '" + Description + "', '" + HelpDeskCheck + "', " + CategoryFK + "); SELECT SCOPE_IDENTITY();");
    
            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }
    
        public static int DeleteContent(int CID)
        {
            DataTable dt = Sql.CCSelect("DELETE FROM CMS_Content WHERE CID = " + CID + "; SELECT SCOPE_IDENTITY();");
    
            return CID;
        }

        public static string SearchBar(string search)
        {
            StringBuilder searchText = new StringBuilder();

            DataTable dt = Sql.CCSelect("SELECT * FROM CMS_CONTENT WHERE [Description] LIKE '%"+search+"%' OR Content LIKE '%"+search+"%'");

            foreach (DataRow row in dt.Rows)
            {
                searchText.Append("<a href='/ContentManagement/Content.aspx?id=" + row["CMS_ID"].ToString() + "'>" + row["Description"].ToString() + "</a><br /> ");
            }

            return searchText.ToString();
        }
    

        #endregion
    }

    public class CMSCategory
    {
        #region Properties

        #endregion

        #region Attributes

        #endregion

        #region Constructors

        public CMSCategory(int CatID)
        {


        }
        #endregion

        #region Member Methods

        #endregion

        #region Static Methods

        public static int NewCategory(string CategoryName)
        {

            DataTable dt = Sql.CCSelect("INSERT INTO CONTENT_CATEGORY (CategoryName) " +
                                        "VALUES ('" + CategoryName + "'); SELECT SCOPE_IDENTITY();");

            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }

        public static void BindCategoryDD(DropDownList CategoryDD)
        {
            DataTable CatTable = Sql.CCSelect("SELECT * FROM CONTENT_CATEGORY WHERE CategoryID <> 4");
            CategoryDD.DataSource = CatTable;
            CategoryDD.DataTextField = "CategoryName";
            CategoryDD.DataValueField = "CategoryID";
            CategoryDD.DataBind();
        }

        #endregion

    }

    public enum LogType
    {
        Default = -1,
        ItemModify = 1
    }
    
    public class LOG
    {
        #region Properties

        #endregion

        #region Attributes
        public int ID = -1;
        public string Note = "";
        public DateTime LogTime = DateTime.Now;
        public LogType TypeID = LogType.Default;
        public int typenum = -1;
        public string Who = " ";
        #endregion

        #region Constructor
        public LOG(int LogID)
        {
            DataTable logTable = Sql.CCSelect("SELECT * FROM Logs WHERE LogID = " + LogID);
            {
               if(logTable.Rows.Count > 0)
               {
                   
                   Note = logTable.Rows[0]["Note"].ToString();
                   LogTime = Convert.ToDateTime(logTable.Rows[0]["LogTime"]);
                   typenum = Convert.ToInt32(logTable.Rows[0]["TypeID"]);

                   if(typenum == 1)
                   {
                       TypeID = LogType.ItemModify;
                   }

                   else
                   {
                       TypeID = LogType.Default;
                   }

                   Who = logTable.Rows[0]["Who"].ToString();
                   
               }
            }
        }
        #endregion

        #region Member Methods

        #endregion

        #region Static Methods

        public static int NewLog(string note, LogType LT)
        {


            DataTable dt = Sql.CCSelect("Insert into Logs (Note, LogTime, TypeID, Who) " +
                                        "VALUES ('"+note+"', GETDATE(), " + (int)LT + ", '" + AD.UserName + "'); SELECT SCOPE_IDENTITY();");

            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }



        #endregion
    }


    public class EmailNotification
    {


        #region Static Methods

        //Notify student that they are next to signup for their work weeks
        public static void StuWWAssignment(string to_adname)
        {
            string To = SiteMembers.GetEmail(to_adname);
            string From = "helpdesk@cofo.edu";
            string Subject = "Important! Sign Up For Your Work Weeks!";
            string Body = "It is your turn to sign up for your work weeks!<br />"
                        + "Please visit the link below when you are at the computer center. "
                        + "Work weeks are available based on seniority and total points accumulated. "
                        + "The next student in the queue will be notified when you have completed your sign up. <br /><br />"
                        + "Thank you.<br /><br />"
                        + "<a style=\"color:#660000\" href=\"http://helpdesk/WorkWeeks/\">Work Week Sign up</a>";
            HDEmail Email = new HDEmail(From, Subject, Body);
            Email.Add(To);
            Email.Send();
        }


        //Notify HelpDesk Manager that a student wants to change a work week
        public static void RequestWWChange(string adname, string fromwwid, string towwid, string reason)
        {
            DataTable fromtb = Sql.CCSelect("Select * from WorkWeeks where wwid = " + fromwwid);
            DataTable totb = Sql.CCSelect("Select * from WorkWeeks where wwid = " + towwid);
            DataTable stuid = Sql.CCSelect("Select ID from SiteMembers where adname = '"+adname+"'");

            string To = SiteMembers.GetEmail(SiteMembers.HDM);
            string From = "helpdesk@cofo.edu";
            string Subject = "Computer Center Work Week Change";
            string Body = "This is " + SiteMembers.ToFullName(adname) + "("+stuid.Rows[0]["ID"].ToString()+") and I would like "
                        + "to change the work week I'm suppose to work (" + fromtb.Rows[0]["Description"] + ") to the work week (" + totb.Rows[0]["Description"] + "). <br/>" 
                        + reason;
            HDEmail Email = new HDEmail(From, Subject, Body);
            Email.Add(To);
            Email.Send();
        }

        //Sends a completion notification to to_adname about information from the workorder with WOID. 
        //The email address is pulled from AD instead of the database to account for any username given.
        //If any error occurs it will send back false else it will return true
        public static bool WOCompletion(int WOID, string to_adname)
        {
            try
            {
                WorkOrder WO = new WorkOrder(WOID);

                string Techstring = "";
                foreach (DataRow tech in WO.AssignedTechs.Rows)
                {
                    Techstring += tech["FullName"].ToString() + ", ";
                }

                string from = "helpdesk@cofo.edu";
                string subject = "Completed Work Order - \"" + WO.Description + "\"";
                string body = "<head><style>#title a:visited{color:#ffffff}</style></head><div class=\"title\""
                            + "style=\"padding:10px;width:100%;background-color:#660000;color:#ffffff\"><i><h2>"
                            + "<a style=\"color:#ffffff\" href=\"http://helpdesk/\">College of the Ozarks :: HelpDesk</a></h2>"
                            + "</i></div><div style=\"background-color:#F7F6F3;padding:10px;border:2px solid gray\"><br />"
                            + "<br />This work order has been labeled as completed by the technician at: " + WO.ClosedDate + "<br />"
                            + "<br /><b>Technician(s):</b> " + Techstring + "<br />"
                            + "<b>Description:</b> " + WO.Description + "<br />"
                            + "<b>Details:</b> " + WO.Details + "<br />"
                            + "<br />You can view the work order and any notes <a href=\"http://helpdesk/WorkOrders/ViewOrder.aspx?WID="
                            + Cryptography.NumberEncrypt(WO.WID.ToString()) + "\">here</a><br /><br /> If you have any questions feel free to call us at "
                            + "2222 or E-Mail us at helpdesk@cofo.edu<br /><br /></div>";

                HDEmail Mail = new HDEmail(from, subject, body);

                Mail.Add(AD.GetEmail(to_adname));
                Mail.Send();

                return true;
            }
            catch { return false; }
        }

        public static bool WONew(int WOID)
        {
            try
            {
                WorkOrder WO = new WorkOrder(WOID);

                
                string from = "helpdesk@cofo.edu";
                string subject = "New Work Order - \"" + WO.Description + "\"";
                string body = "<head><style>#title a:visited{color:#ffffff}</style></head><div class=\"title\""
                            + "style=\"padding:10px;width:100%;background-color:#660000;color:#ffffff\"><i><h2>"
                            + "<a style=\"color:#ffffff\" href=\"http://helpdesk/\">College of the Ozarks :: HelpDesk</a></h2>"
                            + "</i></div><div style=\"background-color:#F7F6F3;padding:10px;border:2px solid gray\"><br />"
                            + "A new work order was <b>created by:</b> " + WO.Creator + "<br /><br />"
                            + "<b>Requester:</b> " + WO.Requester + "<br/>"
                            + "<b>Creation Date:</b> " + WO.CreationDate + "<br />"
                            + "<b>Description:</b> " + WO.Description + "<br />"
                            + "<b>Details:</b> " + WO.Details + "<br />"
                            + "<br />You can view the work order and any notes <a href=\"http://helpdesk/WorkOrders/ViewOrder.aspx?WID="
                            + Cryptography.NumberEncrypt(WO.WID.ToString()) + "\">here</a><br /></div>";

                HDEmail Mail = new HDEmail(from, subject, body);

                Mail.Add(AD.GetEmail(SiteMembers.HDM));
                Mail.Send();

                return true;
            }
            catch { return false; }
        }

        #endregion
    }
}

