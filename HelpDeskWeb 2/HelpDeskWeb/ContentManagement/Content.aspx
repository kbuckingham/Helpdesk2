<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Content.aspx.cs" Inherits="HelpDeskWeb.ContentManagement.Content" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">
    <form runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div class="row">
            <div class="col-lg-6 col-lg-offset-3">
                <%--BEGIN Editing View Div--%>
                    <div id="EditView" runat="server">

                    <%--Description Field Validator Here--%>
                   <asp:RequiredFieldValidator ID="RequiredDescription" runat="server" ControlToValidate="DescriptionTB" ErrorMessage="You must enter a description" style="float:left; clear:right;" />

                    <%--Button For switching to the Published view of the current content page--%>
                    <asp:Button ID="ViewPublished" runat="server" Text="View Published Page" OnClick="ViewPublished_Click" CssClass="btn" style= "float:right;" />
                    <br /><br />
                    <div style="float:left; clear:left;">

                        <%--Description TextBox Created Here--%>   
                        Content Description: <asp:TextBox ID="DescriptionTB" runat="server" Width="40%"></asp:TextBox>
    
                        <%--Check Box For HelpDesk only information--%>
                        Helpdesk Only? <asp:CheckBox runat="server" ID="HDCheck" value="value" />
    
                        <%--TextBox for entering a new Category--%>
                        <asp:TextBox ID="NewCatTB" runat="server" Width="200px"></asp:TextBox>

                        <%--Drop Down List of Curent Categories to choose from--%>
                        <asp:DropDownList ID="CatDDL" runat="server" Width="200px" AppendDataBoundItems="true" >
                        <asp:ListItem Value="4" Text="Select Category..."></asp:ListItem>
                        </asp:DropDownList>&nbsp;
    
                        <%--Button for adding a category to the Drop Down List--%>
                        <asp:Button ID="AddCat" runat="server" Text="+" Width="20px" OnClick="AddCat_Click" />
    
                        <%--Buttons for Publishing, Reverting & Deleting Content--%>
   
                        <asp:Button ID="Publish" runat="server" Text="Publish" OnClick="Publish_Click" />
                        <asp:Button ID="Revert" runat="server" Text="Revert" OnClick="Revert_Click" OnClientClick="return confirm('Pressing OK will Revert Content to the current Published version! \n All changes will be lost');" />
                        <asp:Button ID="Delete" runat="server" Text="Delete" OnClick="Delete_Click" />
                    </div>
                <div style="float:right;">

                     <%--Button for Saving--%> 
                     <asp:Button ID="Save" runat="server" Text="Save Progress" CssClass="btn" OnClick="Save_Click"/>
                </div>

                    <%--Editing/Formatting options are in this editor--%>
                    <cc1:Editor ID="Editor1" runat="server" Height="600px" />

                         <%--File upload box and upload button--%>
                        <asp:FileUpload ID="piclocation" runat="server" Height="24px"/>
                        <asp:Button ID="upload" runat="server" Text="Upload" CssClass="btn" OnClick="upload_Click"/>

                <%--END Editing View Div--%>
                    </div> 

                <%--BEGIN Published View Div--%>
                    <div id="PubView" runat="server"> 
                        <%--<input type="button" title='Print This Only' onclick='printDiv();' /> --%>
                        <%--Go Back Button--%>
                            <asp:Button ID="GoBack" runat="server" Text="Back to Index" OnClick="GoBack_Click" CssClass="btn"/> 
       
                        <%--Show Content Description, Content, and Button for switching to the Eding View--%>
                        <asp:Label Style="font-size:24px;" ID="Description" runat="server"></asp:Label>
                        <asp:Button ID="ViewEditor" runat="server" Text="Edit" OnClick="ViewEditor_Click" CssClass="btn" /><br />       
                        <div id="LiteralDiv"><asp:Literal ID="StaticContent" runat="server" ></asp:Literal></div>

                <%--END Published View Div--%>
                    </div>
            </div>
        </div>
    </form>
</asp:Content>

