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
    public partial class Content : System.Web.UI.Page
    {
        CMS CMS;

        protected void Page_Load(object sender, EventArgs e)
        {
            CMS = new CMS(Convert.ToInt32(Request.QueryString["id"]));
            //Show either content or 'under construction' message on page load
            if (CMS.Content == "")
            {
                StaticContent.Text = "<center>This Page Is Currently Under Construction</center>";
            }
            else
            {
                StaticContent.Text = CMS.Content;
            }

            Description.Text = CMS.Description;

            if (CMS.IsPublished)
                Revert.Visible = false;
            else
                Revert.Visible = true;

            if (!IsPostBack)
            {

                //Execute the following only if the entered page ID is valid
                if (Request.QueryString["id"] != null)
                {


                    #region viewlogic

                    //Check if the content is currently at a 'Published' stage
                    if (CMS.IsPublished)
                    {
                        PubView.Visible = true;
                        EditView.Visible = false;
                        //Check if the user is a CCMember-- Viewable content and Editing options are based on this
                        if (AD.IsHDM || AD.IsinCCStudents || AD.IsinDomainAdmins)
                        {

                            //Make Delete & Publish options invisible for anyone but the HDM
                            if (!AD.IsHDM)
                            {
                                Delete.Visible = false;
                                Publish.Visible = false;
                            }
                        }
                        //Make Editing View Button invisible for faculty
                        else
                        {
                            ViewEditor.Visible = false;
                        }

                    }
                    //If the content is not currently at the 'Published' Stage
                    else
                    {

                        //Check if the current user is the HDM or the Last Person to Edit the Content
                        //Make editing view the default for those two users
                        if (AD.IsHDM || CMS.LastModifiedBy == AD.UserName)
                        {
                            EditView.Visible = true;
                            PubView.Visible = false;
                            //Make Delete & Publish options invisible for anyone but the HDM
                            if (!AD.IsHDM)
                            {
                                Delete.Visible = false;
                                Publish.Visible = false;
                            }
                        }
                        //Everyone else's View 
                        else
                        {
                            EditView.Visible = false;
                            PubView.Visible = true;
                            ViewEditor.Visible = false;
                        }
                    }

                    #endregion

                    //Set Variables
                    Editor1.Content = CMS.EditedContent;
                    DescriptionTB.Text = CMS.Description;
                }

                //Show message when the page ID isn't found in the database
                else
                {
                    StaticContent.Text = "No content found";
                    EditView.Visible = false;
                    ViewEditor.Visible = false;
                }

                HDCheck.Checked = CMS.IsHelpDesk;

                CMSCategory.BindCategoryDD(CatDDL);
                CatDDL.SelectedValue = CMS.CategoryFK.ToString();
                NewCatTB.Visible = false;


            }


        }

        //Switch between Editing and Published view
        protected void ViewPublished_Click(object sender, EventArgs e)
        {
            EditView.Visible = false;
            PubView.Visible = true;
        }

        protected void ViewEditor_Click(object sender, EventArgs e)
        {
            EditView.Visible = true;
            PubView.Visible = false;
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            ////Checks the Description Field to make sure something was entered
            //if (DescriptionTB.Text != "")
            //{
            int CatID = -1;

            if (NewCatTB.Visible)
            {
                CatID = CMSCategory.NewCategory(NewCatTB.Text);
            }
            else
            {
                CatID = Convert.ToInt32(CatDDL.SelectedValue);
            }

            CMS = new CMS(Convert.ToInt32(Request.QueryString["id"]));
            CMS.EditedContent = Editor1.Content;
            CMS.Description = DescriptionTB.Text;
            CMS.IsHelpDesk = HDCheck.Checked;
            CMS.CategoryFK = CatID;
            CMS.Commit();
            Response.Redirect("/ContentManagement/Content.aspx?id=" + Request.QueryString["id"]);
        }
        //else
        //{
        //    DesError.Visible = true;
        //}

        protected void Publish_Click(object sender, EventArgs e)
        {
            CMS = new CMS(Convert.ToInt32(Request.QueryString["id"]));
            CMS.EditedContent = Editor1.Content;
            CMS.Description = DescriptionTB.Text;
            CMS.Publish();
            Response.Redirect("/ContentManagement/Content.aspx?id=" + Request.QueryString["id"]);
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            CMS.Delete();
            EditView.Visible = false;
            PubView.Visible = false;
            Response.Redirect("/ContentManagement/");
        }

        protected void GoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ContentManagement/");
        }

        protected void upload_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(Server.MapPath("/ContentManagement/Images/") + piclocation.FileName))
                {

                    piclocation.SaveAs(Server.MapPath("/ContentManagement/Images/") + piclocation.FileName);
                }
                Editor1.Content += "<img src='" + "/ContentManagement/Images/" + piclocation.FileName + "' />";
                //Response.Redirect("/ContentManagement/Images/" + piclocation.FileName);
            }
            catch { }
        }

        protected void AddCat_Click(object sender, EventArgs e)
        {
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

        protected void Revert_Click(object sender, EventArgs e)
        {
            CMS.Revert();
            Response.Redirect("/ContentManagement/Content.aspx?id=" + Request.QueryString["id"]);
        }
    }
}