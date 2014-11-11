using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HelpDesk;

namespace HelpDeskWeb.ContentManagement
{
    public partial class Confidentiality : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Agree_Click(object sender, EventArgs e)
        {
            DataTable SemesterIDTable = Sql.CCSelect("Select SemesterID from Semester where GETDATE() >= BeginDate and GETDATE() <= EndDate");
            DataTable SemesterSiteMembers = Sql.CCSelect("Select * from SemesterSiteMembers where SemesterID = " + SemesterIDTable.Rows[0]["SemesterID"].ToString() + " and ADName = '" + AD.UserName + "'");

            //try
            //{
                if (SemesterSiteMembers.Rows.Count == 0)
                {
                    Sql.CCSelect("Insert into SemesterSiteMembers (SemesterID, ADName, Agreed) "
                           + "Values (" + SemesterIDTable.Rows[0]["SemesterID"].ToString() + ", '" + AD.UserName + "','true')");
                }
                else
                {
                    Sql.CCSelect("Update SemesterSiteMembers Set Agreed = 'true' where SemesterID = " + SemesterIDTable.Rows[0]["SemesterID"].ToString() + " and ADName = '" + AD.UserName + "'");
                }

            //}
            //catch { ErrorInfo.Text = "Could not commit to database"; }

            Response.Redirect("/");
        }

        protected void Decline_Click(object sender, EventArgs e)
        {
            DataTable SemesterIDTable = Sql.CCSelect("Select SemesterID from Semester where GETDATE() >= BeginDate and GETDATE() <= EndDate");
            DataTable SemesterSiteMembers = Sql.CCSelect("Select * from SemesterSiteMembers where SemesterID = " + SemesterIDTable.Rows[0]["SemesterID"].ToString() + " and ADName = '" + AD.UserName + "'");

            //try
            //{
                if (SemesterSiteMembers.Rows.Count == 0)
                {
                    Sql.CCSelect("Insert into SemesterSiteMembers (SemesterID, ADName, Agreed) "
                           + "Values (" + SemesterIDTable.Rows[0]["SemesterID"].ToString() + ", '" + AD.UserName + "','false')");
                }
                else
                {
                    Sql.CCSelect("Update SemesterSiteMembers Set Agreed = 'false' where SemesterID = " + SemesterIDTable.Rows[0]["SemesterID"].ToString() + " and ADName = '" + AD.UserName + "'");
                }

            //}
            //catch { ErrorInfo.Text = "Could not commit to database"; }

            Response.Redirect("/");
        }
    }
}