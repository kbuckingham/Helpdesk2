using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.Data;
using System.Data.Common;
using System.IO;
using HelpDesk;

namespace HelpDeskWeb.Members
{
    public partial class EditProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FNameTB.Enabled = false;
            LNameTB.Enabled = false;

            //redirect if not allowed to see page.
            Settings.CCOnly(Response);


            if (!IsPostBack)
            {

                DataTable ProfileTable = Sql.CCSelect("SELECT * FROM SiteMembers WHERE ADName = '" + AD.UserName + "'");

                try
                {
                    ProfilePic.ImageUrl = ProfileTable.Rows[0]["PicLink"].ToString();
                    FNameTB.Text = ProfileTable.Rows[0]["FirstName"].ToString();
                    LNameTB.Text = ProfileTable.Rows[0]["LastName"].ToString();
                    CellTB.Text = ProfileTable.Rows[0]["CellPhone"].ToString();
                    DormTB.Text = ProfileTable.Rows[0]["Dorm"].ToString();
                    RoomTB.Text = ProfileTable.Rows[0]["RoomNumber"].ToString();
                    CityTB.Text = ProfileTable.Rows[0]["HomeTown"].ToString();
                    StateTB.Text = ProfileTable.Rows[0]["State"].ToString();
                    EmailTB.Text = ProfileTable.Rows[0]["Email"].ToString();
                    DOBTB.Text = ProfileTable.Rows[0]["DOB"].ToString();
                    CommentsTB.Text = ProfileTable.Rows[0]["Comments"].ToString().Replace("<br />", "\r\n");
                }
                catch { ErrorL.Text = "Some data fields were blank or the fields could not be pulled from the database."; }

            }
        }

        

        protected void Submit_Click(object sender, EventArgs e)
        {
            try
            {
                Sql.CCSelect("UPDATE SiteMembers "
                           + "SET FirstName = '" + FNameTB.Text + "', "
                           + "LastName = '" + LNameTB.Text + "', "
                           + "CellPhone = '" + CellTB.Text + "', "
                           + "Dorm = '" + DormTB.Text + "', "
                           + "RoomNumber = '" + RoomTB.Text + "', "
                           + "HomeTown = '" + CityTB.Text + "', "
                           + "[State] = '" + StateTB.Text + "', "
                           + "email = '" + EmailTB.Text + "', "
                           + "DOB = '" + DOBTB.Text + "', "
                           + "Comments = '" + CommentsTB.Text + "' "
                           + "Where ADName = '" + AD.UserName + "'");
            }
            catch { ErrorL.Text = "One or more entries where not saved to the database."; }

        }
    }
}