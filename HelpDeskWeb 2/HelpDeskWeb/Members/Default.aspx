<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeBehind="/Members/Default.aspx.cs" Inherits="HelpDeskWeb.Members.Default" %>


    <asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<form runat="server">
    <div class="row">
        <div class="rnd1">
            <h4 style="text-align: center">Computer Center Workers</h4>
                <asp:GridView ID="MemberGrid" runat="server" AutoGenerateColumns="false" cssclass="GridView" 
                    gridlines="Horizontal"  AllowPaging="false" horizontalalign="Center" >
                    <RowStyle cssclass="GridRow"/>
                    <AlternatingRowStyle cssclass="GridAltRow" />
                    <EditRowStyle cssclass="table" />
                    <FooterStyle cssclass="GridRow" />
                    <HeaderStyle cssclass="GridHeader" />
                    <SelectedRowStyle cssclass="table-hover" />
                    <EmptyDataTemplate><center>No Data :(</center></EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="Picture" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="PicLink"  runat="server" Text='<%# Bind("PicLink") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="First Name" >
                            <ItemTemplate>
                                <asp:Label ID="FirstName"  runat="server" Text='<%# Bind("FirstName") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Name">
                            <ItemTemplate>
                                <asp:Label ID="LastName"  runat="server" Text='<%# Bind("LastName") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cell Number">
                            <ItemTemplate>
                                <asp:Label ID="CellPhone"  runat="server" Text='<%# Bind("CellPhone") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dorm">
                            <ItemTemplate>
                                <asp:Label ID="Dorm"  runat="server" Text='<%# Bind("Dorm") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Room Number">
                            <ItemTemplate>
                                <asp:Label ID="RoomNumber"  runat="server" Text='<%# Bind("RoomNumber") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Home Town">
                            <ItemTemplate>
                                <asp:Label ID="HomeTown"  runat="server" Text='<%# Bind("HomeTown") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="State">
                            <ItemTemplate>
                                <asp:Label ID="State"  runat="server" Text='<%# Bind("State") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
        </div>
        
     
    </div>       
</form>
    </asp:Content>



