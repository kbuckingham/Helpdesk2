using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HelpDesk;

namespace HelpDeskWeb.Members
{
    public partial class PointWinners : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //redirect if not allowed to see page.
            Settings.CCOnly(Response);

            before.Text = "Point Data for ";
            after.Text = "Semester";

            //if the page refresh is the initial page load
            if (!IsPostBack)
            {
                //BIND SEMESTER DROPDOWNLIST
                HelpDesk.Semester.BindSemesterDD(SemesterDD);
                SemesterDD.SelectedValue = Semester.CurrentIDstring();

                BindPoints();
            }
        }

        protected void BindPoints()
        {

            DataTable SemIDTB = Sql.CCSelect("Select BeginDate, EndDate From Semester Where SemesterID = " + SemesterDD.SelectedValue);

            try
            {

                DataTable PointsDB = Sql.CCSelect("Select ADName as AssignedTechnician , SUM(wosm.Points) AS Points "
                                                + "from WO_SiteMembers wosm "
                                                + "JOIN WorkOrders wo ON wo.WID = wosm.WOID "
                                                + "where (CompletedDate BETWEEN '" + SemIDTB.Rows[0]["BeginDate"] + "' and '" + SemIDTB.Rows[0]["EndDate"] + "')"
                                                + "and (ADName <> 'unassigned' AND ADName <> 'pwinkle' AND ADName <> 'pettit' AND ADName <> 'dthompson' AND ADName <> 'fyoungblood' AND ADName <> 'kpettit' AND ADName <> 'henderson') "
                                                + "GROUP BY ADName Order by Points desc ");

                PointsGV.DataSource = PointsDB;
                PointsGV.DataBind();

                foreach (GridViewRow row in PointsGV.Rows)
                {
                    Literal imagelit = (Literal)row.FindControl("Pic");
                    Label usernameL = (Label)row.FindControl("NameL");


                    SiteMembers student = new SiteMembers(usernameL.Text);
                    imagelit.Text = "<img src=\"" + student.PictureLink + "\" alt=\"" + student.FirstName + "\" width=\"20%\" />";
                }

            }
            catch { }

        }
        protected void SemesterDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPoints();
        }
    }
}