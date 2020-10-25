<%@ Page Title="Competitors Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CompetitorsManagement.aspx.cs" Inherits="SportsManagementSystem.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LoginView ID="CompetitorsLoginView" runat="server">
        <RoleGroups>
            <asp:RoleGroup Roles="Admin">
                <ContentTemplate>
                    <asp:SqlDataSource ID="CompSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [comp_id], [comp_name] FROM [comp_data]"></asp:SqlDataSource>
                    <asp:SqlDataSource ID="CompGamesSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [game_id], [game_name] FROM [games_data]"></asp:SqlDataSource>
                    <div style="text-align: center;">
                        <br />
                        <p>Use the Drop Down List to <strong>create a new entry</strong>, <strong>or select an entry for updating/deletion</strong>.</p>
                        <p>When creating a new entry or updating values, press <strong>Update</strong> for both.</p>
                        <asp:DropDownList ID="CompDropList" runat="server" DataSourceID="CompSource" DataTextField="comp_name" DataValueField="comp_id" OnSelectedIndexChanged="CompDropList_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true" Style="display: block; margin: 0 auto">
                            <asp:ListItem Text="[Select Option]" Value="Title"></asp:ListItem>
                            <asp:ListItem Text="[Add New]" Value="Add"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <br />
                        <p>
                            Competitor Salutation:
                            <asp:TextBox ID="CompSal" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Competitor Name:
                            <asp:TextBox ID="CompName" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Competitor Date of Birth:
                            <asp:TextBox ID="CompDOB" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Competitor Email:
                            <asp:TextBox ID="CompEmail" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Competitor Description:
                            <asp:TextBox ID="CompDesc" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Competitor Country:
                            <asp:TextBox ID="CompCountry" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Competitor Gender:
                            <asp:TextBox ID="CompGender" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Competitor Contact Number:
                            <asp:TextBox ID="CompNum" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Competitor Website:
                            <asp:TextBox ID="CompWeb" runat="server"></asp:TextBox>
                        </p>
                        <p>
                            Competitor Photo:
                            <asp:Image ID="CompPhoto" runat="server" />
                        </p>
                        <p>
                            <strong>Upload Image (.png):</strong>
                        </p>
                        <asp:FileUpload ID="CompUpload" runat="server" Style="display: block; margin: 0 auto;" accept=".png, .PNG" />
                        <br />
                        <br />
                        <div>
                            <p>
                                <asp:Button ID="CompUpdate" runat="server" Text="Update" OnClick="CompUpdate_Click" Style="display: block; margin: 0 auto" />
                            </p>
                            <p>
                                <asp:Button ID="CompDel" runat="server" Text="Delete" OnClick="CompDel_Click" Style="display: block; margin: 0 auto" />
                            </p>
                            <p>
                                <asp:Button ID="CompRefresh" runat="server" Text="Refresh List" OnClick="CompRefresh_Click" Style="display: block; margin: 0 auto" />
                            </p>
                            <p>
                                <asp:Label ID="CompResultMessage" runat="server" ForeColor="Red"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div style="position: absolute; top: 14%; right: 25%; border: solid; border-color: red;">
                        <p style="font-size: 18px;">
                            <strong>Competitor&nbsp;Events:</strong>
                        </p>
                        <asp:CheckBoxList ID="CompGameCheckBoxList" runat="server" DataSourceID="CompGamesSource" DataTextField="game_name" DataValueField="game_id" AutoPostBack="true"></asp:CheckBoxList>
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
                        <asp:LinkButton ID="CompetitorsReturn" runat="server" Font-Bold="True" Font-Size="Large" OnClick="CompetitorsReturn_Click">Return to Index</asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
</asp:Content>
