<%@ Page Title="Add Competitor to Event" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCompetitorToEvent.aspx.cs" Inherits="SportsManagementSystem.AddCompetitorToEvent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LoginView ID="CTELoginView" runat="server">
        <RoleGroups>
            <asp:RoleGroup Roles="Admin">
                <ContentTemplate>
                    <div style="text-align: center;">
                        <br />
                        <p>You do not have access to this page!</p>
                        <br />
                        <br />
                        <asp:LinkButton ID="CTEReturn" runat="server" Font-Bold="True" Font-Size="Large" OnClick="CTEReturn_Click">Return to Index</asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:RoleGroup>
            <asp:RoleGroup Roles="EventManager">
                <ContentTemplate>
                    <div style="text-align: center;">
                        <br />
                        <p>Event Name:</p>
                        <asp:TextBox ID="EventNameTB" runat="server"></asp:TextBox>
                        <br />
                        <p>Competitor Name:</p>
                        <asp:TextBox ID="CNTB" runat="server"></asp:TextBox>
                        <br />
                        <p>Competitor Position:</p>
                        <asp:TextBox ID="CompPosTB" runat="server"></asp:TextBox>
                        <br />
                        <p>Competitor Medal (G=Gold, S=Silver, B=Bronze, N=None):</p>
                        <asp:TextBox ID="CMTB" runat="server"></asp:TextBox>
                    </div>
                    <div>
                        <br />
                        <asp:Button ID="AddCTE" runat="server" OnClick="AddCTE_Click" Text="Add" Style="display: block; margin: 0 auto;" />
                    </div>
                    <p style="text-align: center;">
                        <asp:Label ID="CTEResultMessage" runat="server" ForeColor="Red"></asp:Label>
                    </p>
                    <div>
                        <br />
                        <asp:Button ID="ACTEReturn" runat="server" Text="Return" Style="display: block; margin: 0 auto;" OnClick="ACTEReturn_Click" />
                    </div>
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
</asp:Content>
