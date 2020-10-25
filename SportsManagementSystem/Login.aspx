<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SportsManagementSystem.WebForm5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <br />
        <strong>
            <asp:Login Style="margin: auto;" ID="LoginForm" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#333333" Height="277px" Width="552px" OnAuthenticate="Login1_Authenticate" PasswordLabelText="Password: &amp;nbsp;" RememberMeText="&amp;nbsp;Remember User?" UserNameLabelText="User Email: &amp;nbsp;" VisibleWhenLoggedIn="False">
                <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" Height="50px" Width="100px" />
                <TextBoxStyle Font-Size="0.8em" />
                <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
            </asp:Login>
        </strong>
    </div>
</asp:Content>

