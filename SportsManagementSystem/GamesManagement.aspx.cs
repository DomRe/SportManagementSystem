using Microsoft.Ajax.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace SportsManagementSystem
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                Label message_result = (Label)GamesLoginView.FindControl("GamesResultMessage");
                if (!(message_result is null))
                {
                    message_result.Text = "";
                    message_result.Visible = false;
                }
            }
        }

        protected void GamesReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }

        protected void GamesDropList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)GamesLoginView.FindControl("GamesDropList");

            TextBox gc = (TextBox)GamesLoginView.FindControl("GameCode");
            TextBox gn = (TextBox)GamesLoginView.FindControl("GameName");
            TextBox gt = (TextBox)GamesLoginView.FindControl("GameDuration");
            TextBox gd = (TextBox)GamesLoginView.FindControl("GameDescription");
            FileUpload gr = (FileUpload)GamesLoginView.FindControl("GameRulesBook");

            if (ddl.SelectedValue == "Title")
            {
                gc.Text = "";
                gn.Text = "";
                gt.Text = "";
                gd.Text = "";
                if (gr.HasFile)
                {
                    gr.Attributes.Clear();
                }
            }
            else if (ddl.SelectedValue == "Add")
            {
                gc.Text = "";
                gn.Text = "";
                gt.Text = "";
                gd.Text = "";
                if (gr.HasFile)
                {
                    gr.Attributes.Clear();
                }
            }
            else
            {
                String query = "SELECT game_code, game_name, game_duration_in_hours, game_description, game_rules_booklet " +
                "FROM games_data " +
                "WHERE game_id = " + ddl.SelectedValue;

                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader;

                connection.Open();
                reader = command.ExecuteReader();
                reader.Read();

                gc.Text = reader["game_code"].ToString();
                gn.Text = reader["game_name"].ToString();
                gt.Text = reader["game_duration_in_hours"].ToString();
                gd.Text = reader["game_description"].ToString();
                if (gr.HasFile)
                {
                    gr.Attributes.Clear();
                }

                reader.Close();
                command.Dispose();
                connection.Close();
            }
        }

        protected void GamesUpdate_Click(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)GamesLoginView.FindControl("GamesDropList");
            Label message_result = (Label)GamesLoginView.FindControl("GamesResultMessage");

            if (ddl.SelectedValue == "Title")
            {
                message_result.Text = "Nothing to add/update.";
                message_result.Visible = true;
            }
            else
            {
                TextBox gc = (TextBox)GamesLoginView.FindControl("GameCode");
                TextBox gn = (TextBox)GamesLoginView.FindControl("GameName");
                TextBox gt = (TextBox)GamesLoginView.FindControl("GameDuration");
                TextBox gd = (TextBox)GamesLoginView.FindControl("GameDescription");
                FileUpload gr = (FileUpload)GamesLoginView.FindControl("GameRulesBook");

                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                connection.Open();

                if (ddl.SelectedValue == "Add")
                {
                    String select_query = "SELECT game_code, game_name FROM games_data";

                    SqlDataReader reader;
                    SqlCommand select_command = new SqlCommand(select_query, connection);
                    reader = select_command.ExecuteReader();

                    bool success = true;
                    String err = "Error: Instance of game already exists. Duplicate:";
                    while (reader.Read())
                    {
                        if (gc.Text == reader["game_code"].ToString())
                        {
                            err += " GameCode";
                            success = false;
                        }

                        if (gn.Text == reader["game_name"].ToString())
                        {
                            err += " GameName";
                            success = false;
                        }

                        if (!success)
                        {
                            message_result.Text = err;
                            message_result.Visible = true;
                            break;
                        }
                    }

                    select_command.Dispose();
                    reader.Close();

                    if (gn.Text.IsNullOrWhiteSpace() || gc.Text.Length != 7)
                    {
                        message_result.Text = "Game code must be 7 characters long.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else
                    {
                        int digit_count = 0;
                        int char_count = 0;
                        for (int i = 0; i < gc.Text.Length; i++)
                        {
                            if (char.IsDigit(gc.Text[i]))
                            {
                                digit_count++;
                            }
                            else
                            {
                                char_count++;
                            }
                        }

                        if (digit_count != 3 && char_count != 4)
                        {
                            message_result.Text = "Game code must be 7 characters long.";
                            message_result.Visible = true;
                            success = false;
                        }
                    }

                    if (gn.Text.IsNullOrWhiteSpace() || gn.Text.Length == 0)
                    {
                        message_result.Text = "Game Name cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (gt.Text.IsNullOrWhiteSpace() || gt.Text.Length == 0)
                    {
                        message_result.Text = "Game Duration cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (!gr.HasFile)
                    {
                        message_result.Text = "Game Rules Booklet must be uploaded.";
                        message_result.Visible = true;
                        success = false;
                    }

                    if (success)
                    {
                        String insert_sql = "INSERT INTO games_data (game_code, game_name, game_duration_in_hours, game_description, game_rules_booklet) VALUES (@GameCode, @GameName, @GameDuration, @GameDesc, @GameRules)";
                        SqlCommand insert_command = new SqlCommand(insert_sql, connection);

                        insert_command.Parameters.Add("@GameCode", SqlDbType.Text).Value = gc.Text.ToUpper();
                        insert_command.Parameters.Add("@GameName", SqlDbType.Text).Value = gn.Text;
                        insert_command.Parameters.Add("@GameDuration", SqlDbType.Float).Value = float.Parse(gt.Text);
                        insert_command.Parameters.Add("@GameDesc", SqlDbType.Text).Value = gd.Text;

                        Stream ifs = gr.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(ifs);
                        byte[] data = br.ReadBytes((Int32)ifs.Length);
                        insert_command.Parameters.AddWithValue("@GameRules", data);

                        insert_command.ExecuteNonQuery();
                        insert_command.Dispose();
                        br.Dispose();
                        br.Close();
                        ifs.Dispose();
                        ifs.Close();
                        gr.Attributes.Clear();

                        message_result.Text = "Added successfully!";
                        message_result.Visible = true;
                    }
                }
                else if (ddl.SelectedValue != "Title") // So its not Add or Title, i.e. a game_id instead.
                {
                    bool success = true;
                    if (gn.Text.IsNullOrWhiteSpace() || gc.Text.Length != 7)
                    {
                        message_result.Text = "Game code must be 7 characters long.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else
                    {
                        int digit_count = 0;
                        int char_count = 0;
                        for (int i = 0; i < gc.Text.Length; i++)
                        {
                            if (char.IsDigit(gc.Text[i]))
                            {
                                digit_count++;
                            }
                            else
                            {
                                char_count++;
                            }
                        }

                        if (digit_count != 3 && char_count != 4)
                        {
                            message_result.Text = "Game code must be 7 characters long.";
                            message_result.Visible = true;
                            success = false;
                        }
                    }

                    if (gn.Text.IsNullOrWhiteSpace() || gn.Text.Length == 0)
                    {
                        message_result.Text = "Game Name cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (gt.Text.IsNullOrWhiteSpace() || gt.Text.Length == 0)
                    {
                        message_result.Text = "Game Duration cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (!gr.HasFile)
                    {
                        message_result.Text = "Game Rules Booklet must be uploaded.";
                        message_result.Visible = true;
                        success = false;
                    }

                    if (success)
                    {
                        String update_sql = "UPDATE games_data " +
                        "SET game_code = @GameCode, game_name = @GameName, game_duration_in_hours = @GameDuration, game_description = @GameDesc, game_rules_booklet = @GameRules " +
                        "WHERE game_id=@GameID";

                        SqlCommand update_command = new SqlCommand(update_sql, connection);

                        update_command.Parameters.Add("@GameID", SqlDbType.Int).Value = int.Parse(ddl.SelectedValue);
                        update_command.Parameters.Add("@GameCode", SqlDbType.Text).Value = gc.Text.ToUpper();
                        update_command.Parameters.Add("@GameName", SqlDbType.Text).Value = gn.Text;
                        update_command.Parameters.Add("@GameDuration", SqlDbType.Float).Value = float.Parse(gt.Text);
                        update_command.Parameters.Add("@GameDesc", SqlDbType.Text).Value = gd.Text;

                        Stream ifs = gr.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(ifs);
                        byte[] data = br.ReadBytes((Int32)ifs.Length);
                        update_command.Parameters.AddWithValue("@GameRules", data);

                        update_command.ExecuteNonQuery();
                        update_command.Dispose();
                        br.Dispose();
                        br.Close();
                        ifs.Dispose();
                        ifs.Close();
                        gr.Attributes.Clear();

                        message_result.Text = "Updated successfully!";
                        message_result.Visible = true;
                    }
                }

                connection.Close();
            }
        }

        protected void GamesDel_Click(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)GamesLoginView.FindControl("GamesDropList");
            Label message_result = (Label)GamesLoginView.FindControl("GamesResultMessage");

            if (ddl.SelectedValue == "Title" || ddl.SelectedValue == "Add")
            {
                message_result.Text = "Nothing to delete.";
                message_result.Visible = true;
            }
            else
            {
                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                connection.Open();

                String del_sql = "DELETE FROM games_data WHERE game_id = @GameID";
                SqlCommand del_command = new SqlCommand(del_sql, connection);

                del_command.Parameters.Add("@GameID", SqlDbType.Int).Value = int.Parse(ddl.SelectedValue);

                del_command.ExecuteNonQuery();
                del_command.Dispose();
                connection.Close();

                Response.Redirect(Request.RawUrl);
            }
        }

        protected void GamesRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}