using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HelpDesk;
using HelpDeskWeb.ContentManagement;

namespace HelpDeskWeb.ContentManagement
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Start up the Search Box
            //Searchbox.Attributes.Add("onKeyPress", "doClick('" + Searchbutton.ClientID + "', event) ");

            //Bind the data to the Gridview
            BindTopicGrid();
        }

        protected void BindTopicGrid()
        {
            TopicGrid.DataSource = Sql.CCSelect("SELECT CMS_ID, Description, CONTENT_CATEGORY.CategoryName " +
                                                "FROM CMS_CONTENT " +
                                                "JOIN CONTENT_CATEGORY " +
                                                "ON CMS_CONTENT.CatFK = CONTENT_CATEGORY.CategoryID " + 
                                                "ORDER BY CONTENT_CATEGORY.CategoryName");

                                                
            TopicGrid.DataBind();
        }

        protected void Create_Click(object sender, EventArgs e)
        {
            Response.Redirect("/ContentManagement/Create.aspx");
        }

        protected void GridScan(object sender, EventArgs e)
        {
            foreach (GridViewRow row in TopicGrid.Rows)
            {
                Label ID = (Label)row.FindControl("CMS_ID");
                Label Description = (Label)row.FindControl("Description");
                Label Category = (Label)row.FindControl("CategoryName");

                row.Attributes["onClick"] = "location.href='Content.aspx?ID=" + ID.Text + "'";
                row.Attributes["onmouseover"] = "this.originalstyle=this.style.backgroundColor;this.style.cursor='hand';this.style.backgroundColor='#EFAAAA';";
                row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.backgroundColor=this.originalstyle;";
            }
        }

        //protected void Searchbutton_Click(object sender, EventArgs e)
        //{

        //        ContentIndex.Text = CMS.SearchBar(Searchbox.Text);

        //}
    }
}