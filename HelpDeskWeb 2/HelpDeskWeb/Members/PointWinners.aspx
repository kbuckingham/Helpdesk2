<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PointWinners.aspx.cs" Inherits="HelpDeskWeb.Members.PointWinners" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <form runat="server">
        <div class="row">
            <div class="col-lg-6 col-lg-offset-3">
                <div class="form-horizontal">
                <asp:Label ID="before" runat="server"></asp:Label>
                <asp:DropDownList ID="SemesterDD" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="SemesterDD_SelectedIndexChanged">
                            <asp:ListItem Value="-1" Text="Select Semester..."></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="after" runat="server"></asp:Label>
                    
            </div>
                <br />
             <asp:GridView ID="PointsGV" CssClass="GridView" runat="server" 
                AutoGenerateColumns="false"
                Width="100%">  
                <RowStyle cssclass="GridRow" />
                <HeaderStyle cssclass="GridHeader" />
                <EmptyDataTemplate><center>No Point data found for given semester.</center></EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="User Name">
                        <ItemTemplate>
                            <asp:Label ID="NameL" runat="server" Text='<%# Bind("AssignedTechnician") %>'  /> <br />
                            <asp:Literal ID="Pic" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Points">
                        <ItemTemplate>
                            <asp:Label ID="PointsL" runat="server" Text='<%# Bind("Points") %>' />
                        </ItemTemplate>
                    </asp:TemplateField> 
                </Columns>
            </asp:GridView>
            </div>
            
        </div>
    </form>

</asp:Content>
