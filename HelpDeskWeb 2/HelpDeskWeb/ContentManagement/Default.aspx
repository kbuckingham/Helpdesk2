<%@ Page Title="Help Topics" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HelpDeskWeb.ContentManagement.Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    
    <script lang="javascript" type="text/javascript">
        function doClick(buttonName, e) {
            var key;

            if (window.event)
                key = window.event.keyCode; //IE
            else
                key = e.which;              //Firefox/Chrome

            if (key == 13) {
                var btn = document.getElementById(buttonName);
                if (btn != null) {
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
    </script>

    <form runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div class="row">
            <div class="col-lg-6 col-lg-offset-3">
                <p><asp:Button CssClass="btn" ID="CreateNew" runat="server" Text="Create New" OnClick="Create_Click"/></p>
                
<%--                <asp:textbox ID="Searchbox" runat="server" />
                <asp:Button ID="Searchbutton" runat="server" Text="Search" OnClick="Searchbutton_Click" />--%>
                <asp:GridView ID="TopicGrid" runat="server" AutoGenerateColumns="false" cssclass="table" 
                    gridlines="Horizontal"  AllowPaging="false" horizontalalign="center" ondatabound="GridScan">
                    <RowStyle cssclass="GridRowLeft"/>
                    <AlternatingRowStyle cssclass="GridAltRowLeft" />
                    <EditRowStyle cssclass="table" />
                    <FooterStyle cssclass="GridRow" />
                    <HeaderStyle cssclass="GridHeader" />
                    <SelectedRowStyle cssclass="table-hover" />
                    <EmptyDataTemplate><center>No Data :(</center></EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="CMS_ID"  runat="server" Text='<%# Bind("CMS_ID") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Topic" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="Topic"  runat="server" Text='<%# Bind("Description") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Category" >
                            <ItemTemplate>
                                <asp:Label ID="Category"  runat="server" Text='<%# Bind("CategoryName") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                    </Columns>
                    </asp:GridView>
            </div>
        </div>
    </form>
</asp:Content>

