<%@ Page Title="Events Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EventsManagement.aspx.cs" Inherits="SportsManagementSystem.WebForm3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LoginView ID="EventsLoginView" runat="server">
        <RoleGroups>
            <asp:RoleGroup Roles="Admin">
                <ContentTemplate>
                    <div style="text-align: center;">
                        <br />
                        <p>You do not have access to this page!</p>
                        <br />
                        <br />
                        <asp:LinkButton ID="EventsReturn" runat="server" Font-Bold="True" Font-Size="Large" OnClick="EventsReturn_Click">Return to Index</asp:LinkButton>
                    </div>
                </ContentTemplate>
            </asp:RoleGroup>
            <asp:RoleGroup Roles="EventManager">
                <ContentTemplate>
                    <asp:SqlDataSource ID="EventSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [event_id], [feature_event] FROM [event_data]"></asp:SqlDataSource>
                    <div style="width: 700px;">
                        <div style="position: absolute; top: 10%; right: 34%; border: solid; border-color: red;">
                            <asp:Button ID="EnterComp" runat="server" Text="Add Competitor Result to Event." OnClick="EnterComp_Click" />
                        </div>
                        <div style="text-align: center;">
                            <br />
                            <p>Use the Drop Down List to <strong>view an entry</strong>, <strong>create a new entry, or delete an entry</strong>.</p>
                            <asp:DropDownList ID="EventDropList" runat="server" DataSourceID="EventSource" DataTextField="feature_event" DataValueField="event_id" OnSelectedIndexChanged="EventDropList_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true" Style="display: block; margin: 0 auto">
                                <asp:ListItem Text="[View Event]" Value="Title"></asp:ListItem>
                                <asp:ListItem Text="[Add New]" Value="Add"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <p>Feature Event:</p>
                            <asp:TextBox ID="FeatureTextBox" runat="server"></asp:TextBox>

                            <p>Event Venue:</p>
                            <asp:TextBox ID="VenueTextBox" runat="server"></asp:TextBox>

                            <p>Event Date:</p>
                            <asp:TextBox ID="DateTextBox" runat="server"></asp:TextBox>

                            <p>Event Start Time:</p>
                            <asp:TextBox ID="StartTextBox" runat="server"></asp:TextBox>

                            <p>Event End Time:</p>
                            <asp:TextBox ID="EndTextBox" runat="server"></asp:TextBox>

                            <p>Event Description:</p>
                            <asp:TextBox ID="DescTextBox" runat="server"></asp:TextBox>

                            <p>World Record (Athlete Name):</p>
                            <asp:TextBox ID="WRTextBox" runat="server"></asp:TextBox>
                        </div>
                        <div style="position: absolute; top: 15%; right: 30%;">
                            <p>Upload Photo:</p>
                            <asp:FileUpload ID="EventPhotoUpload" runat="server" accept=".png, .PNG" />
                            <br />
                            <p>Add Photo Tags (required) (comma seperated):</p>
                            <asp:TextBox ID="EventPhotoTags" runat="server"></asp:TextBox>
                        </div>
                        <div>
                            <br />
                            <asp:Button ID="Add" runat="server" OnClick="Add_Click" Text="Add" Style="display: block; margin: 0 auto;" />
                            <asp:Button ID="Delete" runat="server" OnClick="Delete_Click" Text="Delete" Style="display: block; margin: 0 auto;" />
                        </div>
                        <p style="text-align: center;">
                            <asp:Label ID="EventsResultMessage" runat="server" ForeColor="Red"></asp:Label>
                        </p>
                        <div>
                            <br />
                            <asp:Button ID="Refresh" runat="server" OnClick="Refresh_Click" Text="Refresh Page" Style="display: block; margin: auto;" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
</asp:Content>
