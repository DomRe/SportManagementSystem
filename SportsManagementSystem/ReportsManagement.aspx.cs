using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace SportsManagementSystem
{
    public class Country
    {
        public Country(string Name)
        {
            this.Name = Name;
        }

        public string Name = "";
        public uint GoldMedals = 0;
        public uint SilverMedals = 0;
        public uint BronzeMedals = 0;
        public int TotalMedals = 0;

        public void TallyMedal(string Medal)
        {
            if (Medal == "G")
            {
                GoldMedals++;
            }
            else if (Medal == "S")
            {
                SilverMedals++;
            }
            else if (Medal == "B")
            {
                BronzeMedals++;
            }

            TotalMedals++;
        }
    }

    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                Label search_error = (Label)ReportsLoginView.FindControl("SearchError");
                if (!(search_error is null))
                {
                    search_error.Text = "";
                    search_error.Visible = false;
                }

                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=true");
                connection.Open();

                SqlCommand get_competitors = new SqlCommand("SELECT comp_id, comp_name, comp_country FROM comp_data", connection);
                SqlDataReader competitors_reader = get_competitors.ExecuteReader();

                List<Country> countries = new List<Country>();
                while (competitors_reader.Read())
                {
                    String country = competitors_reader["comp_country"].ToString();
                    if (countries.Find(x => x.Name == country) == null)
                    {
                        countries.Add(new Country(country));
                    }

                    Country c = countries.Find(x => x.Name == country);
                    SqlCommand get_results = new SqlCommand("SELECT comp_medal FROM event_results WHERE comp_id = " + competitors_reader["comp_id"].ToString(), connection);
                    SqlDataReader results_reader = get_results.ExecuteReader();

                    if (results_reader.Read())
                    {
                        c.TallyMedal(results_reader["comp_medal"].ToString());
                    }

                    results_reader.Close();
                    get_results.Dispose();
                }

                countries.Sort(delegate (Country A, Country B)
                {
                    if (A.TotalMedals == B.TotalMedals)
                    {
                        return string.Compare(A.Name, B.Name);
                    }

                    // for reverse ordering
                    return A.TotalMedals.CompareTo(B.TotalMedals) * -1;
                });

                uint place = 1;
                int prev_medals = -1;

                HtmlContainerControl control = (HtmlContainerControl)ReportsLoginView.FindControl("ReportsDiv");
                String html = "<br />";

                foreach (Country c in countries)
                {
                    if (!(c.TotalMedals < 1))
                    {
                        if (c.TotalMedals == prev_medals)
                        {
                            place--;
                            html += ("<p>" + place.ToString() + "). " + c.Name + ". Total Medals: " + c.TotalMedals.ToString() + "</p>");
                            html += ("<p>" + "Bronze Medals: " + c.BronzeMedals.ToString() + ". Siver Medals: " + c.SilverMedals.ToString() + ". Gold Medals:" + c.GoldMedals.ToString() + ".</p>");
                            html += "<p>&nbsp;</p>";
                            place++;
                        }

                        else
                        {
                            html += ("<p>" + place.ToString() + "). " + c.Name + ". Total Medals: " + c.TotalMedals.ToString() + "</p>");
                            html += ("<p>" + "Bronze Medals: " + c.BronzeMedals.ToString() + ". Siver Medals: " + c.SilverMedals.ToString() + ". Gold Medals:" + c.GoldMedals.ToString() + ".</p>");
                            html += "<p>&nbsp;</p>";
                            place++;
                        }

                        prev_medals = c.TotalMedals;
                    }
                }

                html += "<br /><br />";

                SqlCommand awr = new SqlCommand("SELECT world_record, feature_event FROM event_data", connection);
                SqlDataReader wr_data = awr.ExecuteReader();
                while (wr_data.Read())
                {
                    html += ("<p>" + wr_data["world_record"] + " set a world record for: " + wr_data["feature_event"] + ". </p>");
                }

                control.InnerHtml = html;
                connection.Close();
            }
        }

        protected void ReportsReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }

        protected void REventDropList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)ReportsLoginView.FindControl("REventDropList");
            if (ddl.SelectedValue == "Title")
            {
                HtmlContainerControl EventViewDiv = (HtmlContainerControl)ReportsLoginView.FindControl("EventViewDiv");
                EventViewDiv.Visible = false;
            }
            else
            {
                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                connection.Open();

                String query = "SELECT feature_event, event_venue, event_data, event_start, event_end, event_desc, world_record FROM event_data WHERE event_id = @EventID";
                SqlCommand get_event = new SqlCommand(query, connection);
                get_event.Parameters.Add("@EventID", SqlDbType.Int).Value = int.Parse(ddl.SelectedValue);
                SqlDataReader event_info = get_event.ExecuteReader();
                event_info.Read();

                Label fe = (Label)ReportsLoginView.FindControl("RFeatureTB");
                Label ev = (Label)ReportsLoginView.FindControl("RVenueTB");
                Label ed = (Label)ReportsLoginView.FindControl("RDateTB");
                Label es = (Label)ReportsLoginView.FindControl("RStartTB");
                Label ee = (Label)ReportsLoginView.FindControl("REndTB");
                Label edesc = (Label)ReportsLoginView.FindControl("RDescTB");
                Label wr = (Label)ReportsLoginView.FindControl("RWRTB");

                fe.Text = event_info["feature_event"].ToString();
                ev.Text = event_info["event_venue"].ToString();
                ed.Text = event_info["event_data"].ToString();
                es.Text = event_info["event_start"].ToString();
                ee.Text = event_info["event_end"].ToString();
                edesc.Text = event_info["event_desc"].ToString();
                wr.Text = event_info["world_record"].ToString();

                HtmlContainerControl EventViewDiv = (HtmlContainerControl)ReportsLoginView.FindControl("EventViewDiv");
                EventViewDiv.Visible = true;

                event_info.Close();
                get_event.Dispose();
                connection.Close();
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            TextBox search = (TextBox)ReportsLoginView.FindControl("SearchBox");
            Label search_error = (Label)ReportsLoginView.FindControl("SearchError");

            if (!(search.Text.IsNullOrWhiteSpace() || search.Text.Length < 1))
            {
                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                connection.Open();

                String query = "SELECT event_id, feature_event, event_venue, event_data, event_start, event_end, event_desc, world_record FROM event_data WHERE feature_event LIKE @EventName";
                SqlCommand get_event = new SqlCommand(query, connection);
                get_event.Parameters.Add("@EventName", SqlDbType.Text).Value = search.Text;
                SqlDataReader event_info = get_event.ExecuteReader();
                if (event_info.Read())
                {
                    int event_id = int.Parse(event_info["event_id"].ToString());

                    Label fe = (Label)ReportsLoginView.FindControl("RFeatureTB");
                    Label ev = (Label)ReportsLoginView.FindControl("RVenueTB");
                    Label ed = (Label)ReportsLoginView.FindControl("RDateTB");
                    Label es = (Label)ReportsLoginView.FindControl("RStartTB");
                    Label ee = (Label)ReportsLoginView.FindControl("REndTB");
                    Label edesc = (Label)ReportsLoginView.FindControl("RDescTB");
                    Label wr = (Label)ReportsLoginView.FindControl("RWRTB");

                    fe.Text = event_info["feature_event"].ToString();
                    ev.Text = event_info["event_venue"].ToString();
                    ed.Text = event_info["event_data"].ToString();
                    es.Text = event_info["event_start"].ToString();
                    ee.Text = event_info["event_end"].ToString();
                    edesc.Text = event_info["event_desc"].ToString();
                    wr.Text = event_info["world_record"].ToString();

                    HtmlContainerControl EventViewDiv = (HtmlContainerControl)ReportsLoginView.FindControl("EventViewDiv");
                    EventViewDiv.Visible = true;

                    event_info.Close();
                    get_event.Dispose();
                    connection.Close();
                }
                else
                {
                    search_error.Text = "No event found.";
                    search_error.Visible = true;
                }
            }
            else
            {
                search_error.Text = "Search cannot be blank, empty or null.";
                search_error.Visible = true;
            }
        }

        protected void ExportEvent_Click(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)ReportsLoginView.FindControl("REventDropList");
            if (ddl.SelectedValue == "Title")
            {
                Label search_error = (Label)ReportsLoginView.FindControl("SearchError");

                search_error.Text = "Cannot export an empty event.";
                search_error.Visible = true;
            }
            else
            {
                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                connection.Open();

                String query = "SELECT * FROM event_data WHERE event_id = " + ddl.SelectedValue;
                SqlDataAdapter db = new SqlDataAdapter(query, connection);
                DataSet ds = new DataSet("SQL_DATABASE_EXPORT");
                db.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                db.Fill(ds);

                foreach (DataTable table in ds.Tables)
                {
                    if (table.Columns.Contains("game_rules_booklet"))
                    {
                        table.Columns.Remove("game_rules_booklet");
                    }

                    if (table.Columns.Contains("event_photo"))
                    {
                        table.Columns.Remove("event_photo");
                    }

                    if (table.Columns.Contains("comp_photo"))
                    {
                        table.Columns.Remove("comp_photo");
                    }
                }

                StringWriter writer = new StringWriter();
                ds.WriteXml(writer);

                MemoryStream ms = new MemoryStream();
                DocX doc = DocX.Create(ms, DocumentTypes.Document);
                Paragraph p = doc.InsertParagraph();
                p.Append(writer.ToString()).Font("Times New Roman").FontSize(8);
                doc.Save();

                ms.Seek(0, SeekOrigin.Begin);
                byte[] binary = ms.ToArray();
                ms.Close();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                Response.AddHeader("content-disposition", "attachment; filename=event.docx");
                Response.BinaryWrite(binary);
                Response.End();

                ms.Dispose();
                doc.Dispose();
                writer.Close();
                writer.Dispose();
                ds.Dispose();
                db.Dispose();
                connection.Close();
            }
        }

        protected void SearchPhotoButton_Click(object sender, EventArgs e)
        {
            TextBox search = (TextBox)ReportsLoginView.FindControl("SearchBox");
            Label search_error = (Label)ReportsLoginView.FindControl("SearchError");
            Literal cont = (Literal)ReportsLoginView.FindControl("ReportsPhotoLiteral");
            cont.Text = "";

            if (!(search.Text.IsNullOrWhiteSpace() || search.Text.Length < 1))
            {
                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT event_id, event_photo, event_photo_tags FROM event_photos", connection);
                SqlDataReader reader = command.ExecuteReader();
                String search_text = string.Join("", search.Text.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                search_text = search_text.ToUpper();
                String[] st_arr = search_text.Split(',');

                int count = 0;
                while (reader.Read())
                {
                    String[] pt_arr = reader["event_photo_tags"].ToString().Split(',');
                    if (pt_arr.Intersect(st_arr).Any())
                    {
                        byte[] pdata = (byte[])reader["event_photo"];
                        string pimg = Convert.ToBase64String(pdata);

                        cont.Text += String.Format("<img src=\"{0}\" />", String.Format("data:image/png;base64,{0}", pimg));
                        count++;
                    }
                }

                if (cont.Text.Length < 1)
                {
                    search_error.Text = "No images matched to tags.";
                    search_error.Visible = true;
                }
            }
            else
            {
                search_error.Text = "Cannot have an empty or blank search.";
                search_error.Visible = true;
            }
        }

        protected void ExportResults_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=true");
            connection.Open();

            SqlCommand get_competitors = new SqlCommand("SELECT comp_id, comp_name, comp_country FROM comp_data", connection);
            SqlDataReader competitors_reader = get_competitors.ExecuteReader();

            List<Country> countries = new List<Country>();
            while (competitors_reader.Read())
            {
                String country = competitors_reader["comp_country"].ToString();
                if (countries.Find(x => x.Name == country) == null)
                {
                    countries.Add(new Country(country));
                }

                Country c = countries.Find(x => x.Name == country);
                SqlCommand get_results = new SqlCommand("SELECT comp_medal FROM event_results WHERE comp_id = " + competitors_reader["comp_id"].ToString(), connection);
                SqlDataReader results_reader = get_results.ExecuteReader();

                if (results_reader.Read())
                {
                    c.TallyMedal(results_reader["comp_medal"].ToString());
                }

                results_reader.Close();
                get_results.Dispose();
            }

            countries.Sort(delegate (Country A, Country B)
            {
                if (A.TotalMedals == B.TotalMedals)
                {
                    return string.Compare(A.Name, B.Name);
                }

                // for reverse ordering
                return A.TotalMedals.CompareTo(B.TotalMedals) * -1;
            });

            uint place = 1;
            int prev_medals = -1;

            MemoryStream ms = new MemoryStream();
            DocX doc = DocX.Create(ms, DocumentTypes.Document);
            foreach (Country c in countries)
            {
                if (!(c.TotalMedals < 1))
                {
                    if (c.TotalMedals == prev_medals)
                    {
                        Paragraph p = doc.InsertParagraph();
                        place--;
                        p.Append(place.ToString() + "). " + c.Name + ". Total Medals: " + c.TotalMedals.ToString()).Font("Times New Roman").FontSize(8);
                        p.Append("Bronze Medals: " + c.BronzeMedals.ToString() + ". Siver Medals: " + c.SilverMedals.ToString() + ". Gold Medals:" + c.GoldMedals.ToString()).Font("Times New Roman").FontSize(8);
                        place++;
                    }
                    else
                    {
                        Paragraph p = doc.InsertParagraph();
                        p.Append(place.ToString() + "). " + c.Name + ". Total Medals: " + c.TotalMedals.ToString()).Font("Times New Roman").FontSize(8);
                        p.Append("Bronze Medals: " + c.BronzeMedals.ToString() + ". Siver Medals: " + c.SilverMedals.ToString() + ". Gold Medals:" + c.GoldMedals.ToString()).Font("Times New Roman").FontSize(8);
                        place++;
                    }

                    prev_medals = c.TotalMedals;
                }
            }

            Paragraph p2 = doc.InsertParagraph();
            SqlCommand awr = new SqlCommand("SELECT world_record, feature_event FROM event_data", connection);
            SqlDataReader wr_data = awr.ExecuteReader();
            while (wr_data.Read())
            {
                p2.Append(wr_data["world_record"] + " set a world record for: " + wr_data["feature_event"]).Font("Times New Roman").FontSize(8);
            }

            doc.Save();

            ms.Seek(0, SeekOrigin.Begin);
            byte[] binary = ms.ToArray();
            ms.Close();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            Response.AddHeader("content-disposition", "attachment; filename=event.docx");
            Response.BinaryWrite(binary);
            Response.End();

            connection.Close();
        }
    }
}