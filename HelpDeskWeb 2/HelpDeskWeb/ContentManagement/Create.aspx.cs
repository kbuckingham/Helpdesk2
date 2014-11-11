using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using HelpDesk;

namespace HelpDeskWeb.ContentManagement
{
    public partial class Create : System.Web.UI.Page
    {
        CMS CMS;

        protected void Page_Load(object sender, EventArgs e)
        {

            //Hide the New Category TextBox and the Description Error on Page Load
            if (!IsPostBack)
            {
                CMSCategory.BindCategoryDD(CatDDL);
                NewCatTB.Visible = false;
                //DesError.Visible = false;
            }
        }

        protected void SaveNew_Click(object sender, EventArgs e)
        {
            ////Checks the Description Field to make sure something was entered
            //if (DescriptionTB.Text != "")
            //{
            int CatID = -1;

            //Logic for showning and hiding the Category Drop Down List or the New Category TextBox
            if (NewCatTB.Visible)
            {
                CatID = CMSCategory.NewCategory(NewCatTB.Text);
            }
            else
            {
                CatID = Convert.ToInt32(CatDDL.SelectedValue);
            }

            //Logic Adds the Content, Description, HelpDesk Only Checkbox Results, and Category Information to 
            //the database; then Redirects to the Default ContentManagement Page
            CMS = new CMS(Convert.ToInt32(Request.QueryString["id"]));
            int cid = CMS.NewContent(Editor1.Content, DescriptionTB.Text, HDCheck.Checked, CatID);
            Response.Redirect("/ContentManagement");
        }
        ////If nothing was entered in the Description Field, show an error
        //else
        //{
        //    DesError.Visible = true;
        //}


        protected void upload_Click(object sender, EventArgs e)
        {
            try
            {
                //Checks if the name of the file being uploaded is available and saves it to the server
                if (!File.Exists(Server.MapPath("/ContentManagement/Images/") + piclocation.FileName))
                {
                    piclocation.SaveAs(Server.MapPath("/ContentManagement/Images/") + piclocation.FileName);
                }
                //Pastes the file into the Content Editor below current content
                Editor1.Content += "<img src='" + "/ContentManagement/Images/" + piclocation.FileName + "' />";
            }
            catch { }
        }

        protected void AddCat_Click(object sender, EventArgs e)
        {
            //Logic gives the user the ability to switch between the Drop Down List and the New Category TextBox
            if (CatDDL.Visible)
            {
                CatDDL.Visible = false;
                NewCatTB.Visible = true;
            }
            else
            {
                CatDDL.Visible = true;
                NewCatTB.Visible = false;
            }
        }
    }
}