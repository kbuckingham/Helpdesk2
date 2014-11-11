using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.Data;
using System.Data.Common;
using HelpDesk;
//using CooAcademics;

namespace HelpDeskWeb.Members
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //redirect if not allowed to see page.
            Settings.CCOnly(Response);
            
            //bind data from Sitemembers to MemberGrid
            BindMemberGrid();
        }

        protected void BindMemberGrid()
        {
            MemberGrid.DataSource = Sql.CCSelect("SELECT PicLink, FirstName, LastName, CellPhone, Dorm, RoomNumber, HomeTown, State" +
                                                  " FROM  dbo.SiteMembers " +
                                                  "WHERE Active='True'");
            MemberGrid.DataBind();
        }
    }
}