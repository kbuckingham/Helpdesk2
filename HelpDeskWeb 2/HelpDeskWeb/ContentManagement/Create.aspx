<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="HelpDeskWeb.ContentManagement.Create" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">
    <form runat="server">
        <div class="col-lg-6 col-lg-offset-3">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

            <div class="form-group">
                <%--Description Field Validator Here--%>
                <asp:RequiredFieldValidator ID="RequiredDescription" runat="server" ControlToValidate="DescriptionTB" ErrorMessage="You must enter a description" />
            </div>
            <div class="form-group">
                <%--Description TextBox Created Here--%>
                Content Description: <asp:TextBox CssClass="form-control" ID="DescriptionTB" runat="server" Width="50%"></asp:TextBox>
            </div>
            <div class="form-group">
                <%--Check Box For HelpDesk only information--%>
                Helpdesk Only?: <asp:CheckBox runat="server" name="HelpdeskCheck" ID="HDCheck" value="value" />
            </div>
            <div class="form-group">
                <%--Drop Down List of Curent Categories to choose from--%>
                <asp:DropDownList ID="CatDDL" runat="server" cssclass="dropdown" AppendDataBoundItems="true" >
                    <asp:ListItem Value="4" Text="Select Category..."></asp:ListItem>
                </asp:DropDownList>
            </div>                            
            <div class="form-group">
                Add New Category: <asp:TextBox ID="NewCatTB" runat="server" cssclass="form-control" Width="50%"></asp:TextBox>
                <asp:Button ID="AddCat" runat="server" Text="+" cssclass="btn" OnClick="AddCat_Click" />
            </div>
            <div class="form-group">
                <%--Editing/Formatting options are in this editor--%>
                <cc1:Editor ID="Editor1" runat="server" />
            </div>           
           <div class="form-group">
                <%--File upload box and upload button--%>
                <asp:FileUpload ID="piclocation" runat="server" cssclass="btn" Width="25%"/>
           </div>
            <div class="form-group">           
                <asp:Button ID="upload" runat="server" Text="Upload" CssClass="btn" OnClick="upload_Click"/>
           </div>
            <div class="form-group">
                <asp:Button ID="SaveNew" runat="server" cssclass="btn" OnClick="SaveNew_Click" Text="Save" />
            </div>          
        </div>
    </form>
</asp:Content>
