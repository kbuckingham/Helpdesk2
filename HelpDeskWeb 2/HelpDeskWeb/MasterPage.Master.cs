using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HelpDesk;

namespace HelpDeskWeb
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteMembers loggedon = new SiteMembers(AD.UserName);

            profilepic.Attributes["src"] = loggedon.PictureLink;
            fullname.Text = loggedon.FirstName + "! ";
            nowdate.Text = DateTime.Now.Year.ToString();
        }
    }
}