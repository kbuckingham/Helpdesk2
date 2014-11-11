<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="StudentSchedule.aspx.cs" Inherits="HelpDeskWeb.Members.StudentSchedule" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Contents" ContentPlaceHolderID="Content" Runat="Server">

<form runat="server">
<asp:ToolkitScriptManager ID="Toolkit2" runat="server"></asp:ToolkitScriptManager>
    <div class="row">
        <div class="col-lg-6 col-lg-offset-3">
            
            <div class="form-horizontal">
                <asp:Button ID="MasterB" runat="server" Text="Master" Visible="false" OnClick="MasterB_Click" />&nbsp;
                <asp:Button ID="EditB" runat="server" Text="Edit" Visible="false" OnClick="EditB_Click" />&nbsp;
                <asp:DropDownList ID="StudentDD" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="StudentDD_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label ID="StudentName" runat="server"></asp:Label>
                <asp:DropDownList ID="SemesterDD" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="SemesterDD_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label ID="afterName" runat="server"></asp:Label>
            </div>        
            <asp:Literal ID="schedulelit" runat="server"></asp:Literal>  
        </div>
    </div>        
       

</form>
</asp:Content>
