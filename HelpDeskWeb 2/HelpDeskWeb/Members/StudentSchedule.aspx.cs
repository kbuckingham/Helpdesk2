using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HelpDesk;

namespace HelpDeskWeb.Members
{
    public partial class StudentSchedule : System.Web.UI.Page{
    
    string username = "";                                                   //Global username
    string SID = "";                                                        //Global Semester ID
    string SSMID = "";                                                      //Global Semester SiteMember ID

    protected void Page_Load(object sender, EventArgs e)
    {
        //redirect if not allowed to see page.
        Settings.CCOnly(Response);

        if (AD.IsHDM)
        {
            StudentDD.Enabled = true;
            EditB.Visible = true;
            MasterB.Visible = true;
        }

        //if the page refresh is the initial page load
        if (!IsPostBack)
        {
            //BIND Members DROPDOWNLIST
            HelpDesk.SiteMembers.BindStudentDD(StudentDD);

            //FIND USER
            findusername();

            //BIND SEMESTER DROPDOWNLIST
            HelpDesk.Semester.BindSemesterDD(SemesterDD);

            //FIND SEMESTER and CONSTRUCT SCHEDULE
            findsemestercreateschedule();
        }

    }

    private void findusername()
    {
        //if no student id specified in the querystring
        if (Request.QueryString["id"] == null)
            username = AD.UserName;     //set current logged in user as global var
        else
            username = Request.QueryString["id"];   //get querystring and set it to global username var

        StudentDD.SelectedValue = username;
        StudentName.Text = "'s";  //set the name text with the 's
    }

    private void findsemestercreateschedule()
    {
        //to store semester ID
        DataTable SemIDTB = null;

        //if there is no sid in address string
        if (Request.QueryString["sid"] == null)
            SemIDTB = Semester.CurrentID();         //use the current semesters ID
        else
            SemIDTB = Sql.CCSelect("Select SemesterID From Semester Where SemesterID = " + Request.QueryString["sid"]);   //use the sid from the address bar and put in DataTable

        //if no data found in Semester ID table
        if (SemIDTB.Rows.Count == 0)
        {
            StudentName.Text = "";   //set student name to nothing
            afterName.Text = "--No Semester Schedule found--"; //State there is no Semester found
        }
        else //semester is found
        {
            SID = SemIDTB.Rows[0]["SemesterID"].ToString();  //set semester ID Global var
            SemesterDD.SelectedValue = SID;                  //set drop down list to correct semester
            

            afterName.Text = " Schedule";                   //set text after drop down list

            //Get SSMID for intersection relation table
            DataTable SSMIDTB = Sql.CCSelect("Select SemSiteMemID From SemesterSiteMembers where ADName = '" + username + "' and SemesterID = " + SID);

            //if SSMID Does not exists
            if (SSMIDTB.Rows.Count == 0)
            {
                //good chance it hasn't been created yet
                afterName.Text = " not yet created";

            }
            else //Semester_SiteMember ID found
            {

                //set global semester site member ID
                SSMID = SSMIDTB.Rows[0]["SemSiteMemID"].ToString();

                //CONSTRUCT SCHEDULE------

                //create new schedule object as a student schedule and give it Semester and Student Name
                Schedule individualschedule = new Schedule(SID, username);

                //feed the table to literal
                schedulelit.Text = individualschedule.ScheduleView;
                
            }
        }
    }
    
    protected void SemesterDD_SelectedIndexChanged(object sender, EventArgs e)
    {
        //redirect with new querystring
        Response.Redirect("/CCStudents/StudentSchedule.aspx?id=" + StudentDD.SelectedValue + "&sid=" + SemesterDD.SelectedValue);
    }

    protected void StudentDD_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("/CCStudents/StudentSchedule.aspx?id=" + StudentDD.SelectedValue + "&sid=" + SemesterDD.SelectedValue);
    }

    protected void EditB_Click(object sender, EventArgs e)
    {
        Response.Redirect("/SiteManager/Schedules.aspx?id=" + StudentDD.SelectedValue + "&sid=" + SemesterDD.SelectedValue);
    }

    protected void MasterB_Click(object sender, EventArgs e)
    {
        Response.Redirect("/SiteManager/MasterSchedule.aspx");
    }
}
}