<%@ Page Title="Reports Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportsManagement.aspx.cs" Inherits="SportsManagementSystem.WebForm4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LoginView ID="ReportsLoginView" runat="server">
        <RoleGroups>
            <asp:RoleGroup Roles="Admin">
                <ContentTemplate>
                    <div style="text-align: center;">
                        <br />
                        <p>You do not have access to this page!</p>
                        <br />
                        <br />
                        <asp:LinkButton ID="ReportsReturn" runat="server" Font-Bold="True" Font-Size="Large" OnClick="ReportsReturn_Click">Return to Index</asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:RoleGroup>
            <asp:RoleGroup Roles="EventManager">
                <ContentTemplate>
                    <div style="width: 800px;">
                        <div style="text-align: center;">
                            <asp:SqlDataSource ID="REventSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [event_id], [feature_event] FROM [event_data]"></asp:SqlDataSource>
                            <div style="text-align: center;">
                                <br />
                                <p>Use the Drop Down List to <strong>view an entry</strong>, or enter an event name to <strong>search for an event</strong>.</p>
                                <p>Event name is (case sensitive). Results will be displayed below.</p>
                                <p>You can also search for photos by tag (comma seperated) by pressing "Search for Photos" instead.</p>
                                <asp:DropDownList ID="REventDropList" runat="server" DataSourceID="REventSource" DataTextField="feature_event" DataValueField="event_id" OnSelectedIndexChanged="REventDropList_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true" Style="display: block; margin: 0 auto">
                                    <asp:ListItem Text="[View Event]" Value="Title"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div style="position: absolute; top: 20%; right: 35%;">
                                <asp:TextBox ID="SearchBox" runat="server"></asp:TextBox>
                                <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" />
                                <asp:Button ID="SearchPhotoButton" runat="server" Text="Search for Photo" OnClick="SearchPhotoButton_Click" />
                            </div>
                            <div style="text-align: center;">
                                <br />
                                <asp:Label ID="SearchError" runat="server" ForeColor="Red" />
                            </div>
                            <div style="text-align: center;" id="EventViewDiv" runat="server" visible="false">
                                <strong>Feature Event:</strong>
                                <asp:Label ID="RFeatureTB" runat="server"></asp:Label>
                                <br />
                                <strong>Event Venue:</strong>
                                <asp:Label ID="RVenueTB" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                                <strong>Event Date:</strong>
                                <asp:Label ID="RDateTB" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                                <strong>Event Start Time:</strong>
                                <asp:Label ID="RStartTB" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                                <strong>Event End Time:</strong>
                                <asp:Label ID="REndTB" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                                <strong>Event Description:</strong>
                                <asp:Label ID="RDescTB" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                                <strong>World Record:</strong>
                                <asp:Label ID="RWRTB" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                            </div>
                            <div id="ReportsDiv" runat="server" style="text-align: center;">
                            </div>
                            <div style="display: block; margin: 0 auto;">
                                <br />
                                <asp:Literal ID="ReportsPhotoLiteral" runat="server">
                                </asp:Literal>
                                <br />
                            </div>
                            <div style="position: absolute; top: 24%; right: 40%;">
                                <asp:Button ID="ExportEvent" runat="server" Text="Export Event to MS Word" Style="display: block; margin: 0 auto; border: solid; border-color: red;" OnClick="ExportEvent_Click" />
                                <br />
                                <asp:Button ID="ExportResults" runat="server" Text="Export Results to MS Word" Style="display: block; margin: 0 auto; border: solid; border-color: red;" OnClick="ExportResults_Click" />
                            </div>
                        </div>
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
</asp:Content>
