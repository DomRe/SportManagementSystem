using Microsoft.Ajax.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace SportsManagementSystem
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                Label message_result = (Label)EventsLoginView.FindControl("EventsResultMessage");
                if (!(message_result is null))
                {
                    message_result.Text = "";
                    message_result.Visible = false;
                }
            }
        }

        protected void EventsReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }
        protected void EventDropList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)EventsLoginView.FindControl("EventDropList");

            TextBox fe = (TextBox)EventsLoginView.FindControl("FeatureTextBox");
            TextBox ev = (TextBox)EventsLoginView.FindControl("VenueTextBox");
            TextBox ed = (TextBox)EventsLoginView.FindControl("DateTextBox");
            TextBox es = (TextBox)EventsLoginView.FindControl("StartTextBox");
            TextBox ee = (TextBox)EventsLoginView.FindControl("EndTextBox");
            TextBox edesc = (TextBox)EventsLoginView.FindControl("DescTextBox");
            TextBox wr = (TextBox)EventsLoginView.FindControl("WRTextBox");
            FileUpload pu = (FileUpload)EventsLoginView.FindControl("EventPhotoUpload");
            TextBox ept = (TextBox)EventsLoginView.FindControl("EventPhotoTags");

            fe.Text = "";
            ev.Text = "";
            ed.Text = "";
            es.Text = DateTime.Now.ToString();
            ee.Text = "Will be pre-filled.";
            edesc.Text = "";
            wr.Text = "";
            if (pu.HasFile)
            {
                pu.Attributes.Clear();
            }
            ept.Text = "";

            if ((ddl.SelectedValue != "Title") && (ddl.SelectedValue != "Add"))
            {
                String query = "SELECT event_venue, event_data, event_start, event_end, event_desc, world_record, feature_event " +
                "FROM event_data " +
                "WHERE event_id = " + ddl.SelectedValue;

                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader;

                connection.Open();
                reader = command.ExecuteReader();
                reader.Read();

                fe.Text = reader["feature_event"].ToString();
                ev.Text = reader["event_venue"].ToString();
                ed.Text = reader["event_data"].ToString();
                es.Text = reader["event_start"].ToString();
                ee.Text = reader["event_end"].ToString();
                edesc.Text = reader["event_desc"].ToString();
                wr.Text = reader["world_record"].ToString();

                reader.Close();
                command.Dispose();

                String tag_query = "SELECT event_photo_tags FROM event_photos WHERE event_id = " + ddl.SelectedValue;
                SqlCommand get_tags = new SqlCommand(tag_query, connection);
                SqlDataReader tag_reader = get_tags.ExecuteReader();
                tag_reader.Read();
                ept.Text = tag_reader["event_photo_tags"].ToString();

                tag_reader.Close();
                get_tags.Dispose();
                connection.Close();
            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)EventsLoginView.FindControl("EventDropList");
            Label message_result = (Label)EventsLoginView.FindControl("EventsResultMessage");

            if (ddl.SelectedValue == "Title")
            {
                message_result.Text = "Nothing to add.";
                message_result.Visible = true;
            }
            else
            {
                TextBox fe = (TextBox)EventsLoginView.FindControl("FeatureTextBox");
                TextBox ev = (TextBox)EventsLoginView.FindControl("VenueTextBox");
                TextBox ed = (TextBox)EventsLoginView.FindControl("DateTextBox");
                TextBox es = (TextBox)EventsLoginView.FindControl("StartTextBox");
                TextBox ee = (TextBox)EventsLoginView.FindControl("EndTextBox");
                TextBox edesc = (TextBox)EventsLoginView.FindControl("DescTextBox");
                TextBox wr = (TextBox)EventsLoginView.FindControl("WRTextBox");
                FileUpload pu = (FileUpload)EventsLoginView.FindControl("EventPhotoUpload");
                TextBox ept = (TextBox)EventsLoginView.FindControl("EventPhotoTags");

                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                connection.Open();

                if (ddl.SelectedValue == "Add")
                {
                    String select_query = "SELECT feature_event FROM event_data";

                    SqlDataReader reader;
                    SqlCommand select_command = new SqlCommand(select_query, connection);
                    reader = select_command.ExecuteReader();

                    bool success = true;
                    while (reader.Read())
                    {
                        if (fe.Text == reader["feature_event"].ToString())
                        {
                            message_result.Text = "Error: Event already exists.";
                            message_result.Visible = true;
                            success = false;
                            break;
                        }
                    }

                    select_command.Dispose();
                    reader.Close();

                    if (fe.Text.IsNullOrWhiteSpace() || fe.Text.Length < 1)
                    {
                        message_result.Text = "Event cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (ev.Text.IsNullOrWhiteSpace() || ev.Text.Length < 1)
                    {
                        message_result.Text = "Venue cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (ed.Text.IsNullOrWhiteSpace() || ed.Text.Length < 1)
                    {
                        message_result.Text = "Date cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (edesc.Text.IsNullOrWhiteSpace() || edesc.Text.Length < 1)
                    {
                        message_result.Text = "Description cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (wr.Text.IsNullOrWhiteSpace() || wr.Text.Length < 1)
                    {
                        message_result.Text = "World Record cannot be blank. Use no if blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (!pu.HasFile)
                    {
                        message_result.Text = "You need to upload a photo of the event.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (ept.Text.IsNullOrWhiteSpace() || ept.Text.Length < 1)
                    {
                        message_result.Text = "Event Photo must be tagged.";
                        message_result.Visible = true;
                        success = false;
                    }

                    if (success)
                    {
                        DateTime dt;
                        if (!DateTime.TryParse(es.Text, out dt))
                        {
                            message_result.Text = "Date/Time is in an invalid format.";
                            message_result.Visible = true;
                            success = false;
                        }
                    }

                    if (success)
                    {
                        String insert_sql = "INSERT INTO event_data (feature_event, event_venue, event_data, event_start, event_end, event_desc, world_record) VALUES (@FeatureEvent, @EventVenue, @EventData, @EventStart, @EventEnd, @EventDesc, @WorldRecord)";

                        SqlCommand event_dur = new SqlCommand("SELECT game_duration_in_hours FROM games_data WHERE game_name LIKE @FeatureEvent", connection);
                        event_dur.Parameters.Add("@FeatureEvent", SqlDbType.Text).Value = fe.Text;
                        SqlDataReader ed_reader = event_dur.ExecuteReader();
                        ed_reader.Read();

                        String hours = ed_reader["game_duration_in_hours"].ToString();
                        DateTime dt = DateTime.Parse(es.Text).AddHours(float.Parse(hours));
                        ed_reader.Close();
                        event_dur.Dispose();

                        SqlCommand insert_command = new SqlCommand(insert_sql, connection);
                        insert_command.Parameters.Add("@FeatureEvent", SqlDbType.Text).Value = fe.Text;
                        insert_command.Parameters.Add("@EventVenue", SqlDbType.Text).Value = ev.Text;
                        insert_command.Parameters.Add("@EventData", SqlDbType.Text).Value = ed.Text;
                        insert_command.Parameters.Add("@EventStart", SqlDbType.Text).Value = es.Text;
                        insert_command.Parameters.Add("@EventEnd", SqlDbType.Text).Value = dt.ToString();
                        insert_command.Parameters.Add("@EventDesc", SqlDbType.Text).Value = edesc.Text;
                        insert_command.Parameters.Add("@WorldRecord", SqlDbType.Text).Value = wr.Text;

                        insert_command.ExecuteNonQuery();
                        insert_command.Dispose();

                        SqlCommand photo_command = new SqlCommand("INSERT INTO event_photos (event_id, event_photo, event_photo_tags) VALUES (@EventID, @EventPhoto, @EventPhotoTags)", connection);
                        SqlCommand get_e_id = new SqlCommand("SELECT event_id FROM event_data WHERE feature_event LIKE @FeatureEvent", connection);

                        get_e_id.Parameters.Add("@FeatureEvent", SqlDbType.Text).Value = fe.Text;
                        SqlDataReader get_e_id_reader = get_e_id.ExecuteReader();
                        get_e_id_reader.Read();

                        photo_command.Parameters.Add("@EventID", SqlDbType.Int).Value = get_e_id_reader["event_id"];
                        String tags = string.Join("", ept.Text.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                        tags = tags.ToUpper();
                        photo_command.Parameters.Add("@EventPhotoTags", SqlDbType.Text).Value = tags;

                        get_e_id_reader.Close();
                        get_e_id.Dispose();

                        Stream ifs = pu.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(ifs);
                        byte[] data = br.ReadBytes((Int32)ifs.Length);
                        photo_command.Parameters.AddWithValue("@EventPhoto", data);

                        photo_command.ExecuteNonQuery();

                        ifs.Close();
                        ifs.Dispose();
                        br.Close();
                        br.Dispose();
                        get_e_id_reader.Close();
                        get_e_id_reader.Dispose();
                        photo_command.Dispose();
                        get_e_id.Dispose();

                        message_result.Text = "Added successfully!";
                        message_result.Visible = true;
                    }
                }

                connection.Close();
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)EventsLoginView.FindControl("EventsDropList");
            Label message_result = (Label)EventsLoginView.FindControl("EventsResultMessage");

            if (ddl.SelectedValue == "Title" || ddl.SelectedValue == "Add")
            {
                message_result.Text = "Nothing to delete.";
                message_result.Visible = true;
            }
            else
            {
                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                connection.Open();

                SqlCommand del_command = new SqlCommand("DELETE FROM event_data WHERE event_id = @EventID", connection);
                del_command.Parameters.Add("@EventID", SqlDbType.Int).Value = int.Parse(ddl.SelectedValue);

                del_command.ExecuteNonQuery();
                del_command.Dispose();

                SqlCommand ep_del_command = new SqlCommand("DELETE FROM event_photos WHERE event_id = @EventID", connection);
                ep_del_command.Parameters.Add("@EventID", SqlDbType.Int).Value = int.Parse(ddl.SelectedValue);

                ep_del_command.ExecuteNonQuery();
                ep_del_command.Dispose();

                SqlCommand er_del_command = new SqlCommand("DELETE FROM event_results WHERE event_id = @EventID", connection);
                er_del_command.Parameters.Add("@EventID", SqlDbType.Int).Value = int.Parse(ddl.SelectedValue);

                er_del_command.ExecuteNonQuery();
                er_del_command.Dispose();

                connection.Close();

                Response.Redirect(Request.RawUrl);
            }
        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void EnterComp_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AddCompetitorToEvent.aspx");
        }
    }
}