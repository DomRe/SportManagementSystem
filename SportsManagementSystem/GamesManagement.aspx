<%@ Page Title="Games Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GamesManagement.aspx.cs" Inherits="SportsManagementSystem.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LoginView ID="GamesLoginView" runat="server">
        <RoleGroups>
            <asp:RoleGroup Roles="Admin">
                <ContentTemplate>
                    <asp:SqlDataSource ID="GamesSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [game_id], [game_name] FROM [games_data]"></asp:SqlDataSource>
                    <div style="text-align: center;">
                        <br />
                        <p>Use the Drop Down List to <strong>create a new entry</strong>, <strong>or select an entry for updating/deletion</strong>.</p>
                        <p>When creating a new entry or updating values, press <strong>Update</strong> for both.</p>
                        <p>No need to worry about casing for Game Code, converts to uppercase internally.</p>
                        <asp:DropDownList ID="GamesDropList" runat="server" DataSourceID="GamesSource" DataTextField="game_name" DataValueField="game_id" OnSelectedIndexChanged="GamesDropList_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true" Style="display: block; margin: 0 auto">
                            <asp:ListItem Text="[Select Option]" Value="Title"></asp:ListItem>
                            <asp:ListItem Text="[Add New]" Value="Add"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <br />
                        <p>
                            Game Code:
                            <asp:TextBox ID="GameCode" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Game Name:
                            <asp:TextBox ID="GameName" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Game Duration (Hours):
                            <asp:TextBox ID="GameDuration" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Game Description:
                            <asp:TextBox ID="GameDescription" runat="server"></asp:TextBox>
                        </p>
                        <br />
                        <p>
                            <strong>Upload Game Rulebook:</strong>
                        </p>
                        <asp:FileUpload ID="GameRulesBook" runat="server" Style="display: block; margin: 0 auto" accept=".doc, .docx, .pdf, .txt" />
                        <br />
                        <br />
                        <div>
                            <p>
                                <asp:Button ID="GamesUpdate" runat="server" Text="Update" OnClick="GamesUpdate_Click" Style="display: block; margin: 0 auto" />
                            </p>
                            <p>
                                <asp:Button ID="GamesDel" runat="server" Text="Delete" OnClick="GamesDel_Click" Style="display: block; margin: 0 auto" />
                            </p>
                            <p>
                                <asp:Label ID="GamesResultMessage" runat="server" ForeColor="Red"></asp:Label>
                            </p>
                            <p>
                                <asp:Button ID="GamesRefresh" runat="server" Text="Refresh List" OnClick="GamesRefresh_Click" Style="display: block; margin: 0 auto" />
                            </p>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:RoleGroup>
            <asp:RoleGroup Roles="EventManager">
                <ContentTemplate>
                    <div style="text-align: center;">
                        <br />
                        <p>You do not have access to this page!</p>
                        <br />
                        <br />
                        <asp:LinkButton ID="GamesReturn" runat="server" Font-Bold="True" Font-Size="Large" OnClick="GamesReturn_Click">Return to Index</asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
</asp:Content>
