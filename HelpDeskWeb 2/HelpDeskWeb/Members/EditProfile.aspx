<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="HelpDeskWeb.Members.EditProfile" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<form class="form-horizontal" runat="server">
    <div class="row center-block">
        <div class="col-sm-offset-3 col-sm-3">
        <h4>Edit Profile</h4>

            <asp:Image ID="ProfilePic" Width="300px" runat="server"/>

            <div class="form-group">
                <asp:Label ID="FNameL" runat="server" Text="First Name:"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="FNameTB" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="LNameL" runat="server" Text="Last Name:"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="LNameTB" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="CellL" runat="server" Text="Cell Phone: "></asp:Label>
                <asp:TextBox CssClass="form-control" ID="CellTB" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="DormL" runat="server" Text="Dormitory: "></asp:Label>
                <asp:TextBox CssClass="form-control" ID="DormTB" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="RoomL" runat="server" Text="Room #: "></asp:Label>
                <asp:TextBox CssClass="form-control" ID="RoomTB" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="CityL" runat="server" Text="Home Town:"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="CityTB" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="StateL" runat="server" Text="State: "></asp:Label>
                <asp:TextBox CssClass="form-control" ID="StateTB" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="EmailL" runat="server" Text="E-Mail: "></asp:Label>
                <asp:TextBox CssClass="form-control" ID="EmailTB" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="DOBL" runat="server" Text="Birthday: "></asp:Label>
                <asp:TextBox CssClass="form-control" ID="DOBTB" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="CommentsL" runat="server" Text="Comments: "></asp:Label>
                <asp:TextBox CssClass="form-control" ID="CommentsTB" runat="server" TextMode="MultiLine"></asp:TextBox>
            </div> 
            <div class="form-group">
                <asp:Label ID="ErrorL" runat="server" ForeColor="Red"></asp:Label>
                <br />
                <asp:Button ID="Submit" runat="server" Text="Save" onclick="Submit_Click" Width="75px"/>
            </div>  
        </div>

    </div>
</form>



</asp:Content>