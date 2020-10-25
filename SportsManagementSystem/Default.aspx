<%@ Page Title="Index" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SportsManagementSystem._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center;">
        <br />
        <p>
            <strong>Welcome to Sports Management System, </strong>
            <asp:LoginName ID="LoginName" runat="server" BorderStyle="None" Font-Italic="True" />
            .
        </p>
    </div>
    <div style="text-align: center;">
        <br />
        <asp:LoginStatus ID="LoginStatus" runat="server" BorderStyle="Dotted" Font-Bold="True" Width="265px" BorderColor="Black" />
    </div>
    <div style="text-align: center;">
        <br />
        <asp:Button ID="ButtonGames" runat="server" Text="Games Management" OnClick="ButtonGames_Click" BorderStyle="Solid" Font-Bold="True" Width="265px" />
    </div>
    <div style="text-align: center;">
        <br />
        <asp:Button ID="ButtonCompetitors" runat="server" Text="Competitors Management" OnClick="ButtonCompetitors_Click" BorderStyle="Solid" Font-Bold="True" Width="265px" />
    </div>
    <div style="text-align: center;">
        <br />
        <asp:Button ID="ButtonEvents" runat="server" Text="Events Management" OnClick="ButtonEvents_Click" BorderStyle="Solid" Font-Bold="True" Width="265px" />
    </div>
    <div style="text-align: center;">
        <br />
        <asp:Button ID="ButtonReports" runat="server" Text="Reports Management" OnClick="ButtonReports_Click" BorderStyle="Solid" Font-Bold="True" Width="265px" />
    </div>
    <br />
</asp:Content>
