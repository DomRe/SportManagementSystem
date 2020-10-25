using Microsoft.Ajax.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SportsManagementSystem
{
    public partial class AddCompetitorToEvent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                Label message_result = (Label)CTELoginView.FindControl("CTEResultMessage");
                if (!(message_result is null))
                {
                    message_result.Text = "";
                    message_result.Visible = false;
                }
            }
        }

        protected void CTEReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }

        protected void AddCTE_Click(object sender, EventArgs e)
        {
            Label message_result = (Label)CTELoginView.FindControl("CTEResultMessage");

            TextBox en = (TextBox)CTELoginView.FindControl("EventNameTB");
            TextBox cn = (TextBox)CTELoginView.FindControl("CNTB");
            TextBox cpos = (TextBox)CTELoginView.FindControl("CompPosTB");
            TextBox cmedal = (TextBox)CTELoginView.FindControl("CMTB");

            bool success = true;
            if (en.Text.IsNullOrWhiteSpace() || en.Text.Length < 1)
            {
                message_result.Text = "Game Name cannot be blank.";
                message_result.Visible = true;
                success = false;
            }
            else if (cn.Text.IsNullOrWhiteSpace() || cn.Text.Length < 1)
            {
                message_result.Text = "Competitor name cannot be blank.";
                message_result.Visible = true;
                success = false;
            }
            else if (cpos.Text.IsNullOrWhiteSpace() || cpos.Text.Length < 1)
            {
                message_result.Text = "Competitor position cannot be blank.";
                message_result.Visible = true;
                success = false;
            }
            else if (cmedal.Text.IsNullOrWhiteSpace() || cmedal.Text.Length < 1)
            {
                message_result.Text = "Use \"N\" for no medals.";
                message_result.Visible = true;
                success = false;
            }

            if (success)
            {
                if (!(cmedal.Text == "G" || cmedal.Text == "S" || cmedal.Text == "B" || cmedal.Text == "N"))
                {
                    message_result.Text = "Medal must be G (gold), S (silver) or B (bronze).";
                    success = false;
                }
            }

            if (success)
            {
                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                connection.Open();

                SqlCommand eid_command = new SqlCommand("SELECT event_id FROM event_data WHERE feature_event LIKE @EventName", connection);
                eid_command.Parameters.Add("@EventName", SqlDbType.Text).Value = en.Text;

                SqlDataReader eid_reader = eid_command.ExecuteReader();
                eid_reader.Read();

                int event_id = int.Parse(eid_reader["event_id"].ToString());

                eid_reader.Close();
                eid_reader.Dispose();

                SqlCommand cid_command = new SqlCommand("SELECT comp_id FROM comp_data WHERE comp_name LIKE @CompName", connection);
                cid_command.Parameters.Add("@CompName", SqlDbType.Text).Value = cn.Text;

                SqlDataReader cid_reader = cid_command.ExecuteReader();
                cid_reader.Read();

                int comp_id = int.Parse(cid_reader["comp_id"].ToString());

                cid_reader.Close();
                cid_reader.Dispose();

                String insert_sql = "INSERT INTO event_results (event_id, comp_id, comp_position, comp_medal) VALUES (@EventID, @CompID, @CompPos, @CompMedal)";
                SqlCommand insert_command = new SqlCommand(insert_sql, connection);

                insert_command.Parameters.Add("@EventID", SqlDbType.Int).Value = event_id;
                insert_command.Parameters.Add("@CompID", SqlDbType.Int).Value = comp_id;
                insert_command.Parameters.Add("@CompPos", SqlDbType.Text).Value = cpos.Text;
                insert_command.Parameters.Add("@CompMedal", SqlDbType.Text).Value = cmedal.Text;

                insert_command.ExecuteNonQuery();
                insert_command.Dispose();

                message_result.Text = "Added successfully!";
                message_result.Visible = true;

                connection.Close();
            }
        }

        protected void ACTEReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EventsManagement");
        }
    }
}