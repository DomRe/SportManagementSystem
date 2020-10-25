using Microsoft.Ajax.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Web.UI.WebControls;

namespace SportsManagementSystem
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                Label message_result = (Label)CompetitorsLoginView.FindControl("CompResultMessage");
                if (!(message_result is null))
                {
                    message_result.Text = "";
                    message_result.Visible = false;
                }
            }
        }

        protected void CompetitorsReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }

        protected void CompDropList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)CompetitorsLoginView.FindControl("CompDropList");

            TextBox cs = (TextBox)CompetitorsLoginView.FindControl("CompSal");
            TextBox cn = (TextBox)CompetitorsLoginView.FindControl("CompName");
            TextBox cdob = (TextBox)CompetitorsLoginView.FindControl("CompDOB");
            TextBox ce = (TextBox)CompetitorsLoginView.FindControl("CompEmail");
            TextBox cd = (TextBox)CompetitorsLoginView.FindControl("CompDesc");
            TextBox cc = (TextBox)CompetitorsLoginView.FindControl("CompCountry");
            TextBox cg = (TextBox)CompetitorsLoginView.FindControl("CompGender");
            TextBox cnum = (TextBox)CompetitorsLoginView.FindControl("CompNum");
            TextBox cw = (TextBox)CompetitorsLoginView.FindControl("CompWeb");
            Image photo = (Image)CompetitorsLoginView.FindControl("CompPhoto");
            FileUpload cu = (FileUpload)CompetitorsLoginView.FindControl("CompUpload");
            CheckBoxList cbl = (CheckBoxList)CompetitorsLoginView.FindControl("CompGameCheckBoxList");

            cs.Text = "";
            cn.Text = "";
            cdob.Text = "";
            ce.Text = "";
            cd.Text = "";
            cc.Text = "";
            cg.Text = "";
            cnum.Text = "";
            cw.Text = "";
            photo.ImageUrl = "";
            if (cu.HasFile)
            {
                cu.Attributes.Clear();
            }
            cbl.ClearSelection();

            if (ddl.SelectedValue != "Title" && ddl.SelectedValue != "Add")
            {
                String query = "SELECT comp_salutation, comp_name, comp_dob, comp_email, comp_desc, comp_country, comp_gender, comp_contact_num, comp_website, comp_photo " +
                "FROM comp_data " +
                "WHERE comp_id = " + ddl.SelectedValue;

                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader;

                connection.Open();
                reader = command.ExecuteReader();
                reader.Read();

                cs.Text = reader["comp_salutation"].ToString();
                cn.Text = reader["comp_name"].ToString();
                cdob.Text = reader["comp_dob"].ToString();
                ce.Text = reader["comp_email"].ToString();
                cd.Text = reader["comp_desc"].ToString();
                cc.Text = reader["comp_country"].ToString();
                cg.Text = reader["comp_gender"].ToString();
                cnum.Text = reader["comp_contact_num"].ToString();
                cw.Text = reader["comp_website"].ToString();

                byte[] pdata = (byte[])reader["comp_photo"];
                string pimg = Convert.ToBase64String(pdata);
                photo.ImageUrl = String.Format("data:image/png;base64,{0}", pimg);

                if (cu.HasFile)
                {
                    cu.Attributes.Clear();
                }

                reader.Close();
                command.Dispose();

                String comp_games_query = "SELECT game_id_array FROM comp_list_of_games WHERE comp_id = " + ddl.SelectedValue;
                SqlCommand cg_command = new SqlCommand(comp_games_query, connection);
                reader = cg_command.ExecuteReader();

                if (reader.Read())
                {
                    String to_select_str = reader["game_id_array"].ToString();
                    if (!(to_select_str.IsNullOrWhiteSpace() || to_select_str.Length < 1))
                    {
                        var to_select = to_select_str.Split(',');
                        foreach (String i in to_select)
                        {
                            for (int j = 0; j < cbl.Items.Count; j++)
                            {
                                if (cbl.Items[j].Value == i)
                                {
                                    cbl.Items[j].Selected = true;
                                    j = cbl.Items.Count;
                                }
                            }
                        }
                    }
                }

                reader.Close();
                cg_command.Dispose();
                connection.Close();
            }
        }

        protected void CompUpdate_Click(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)CompetitorsLoginView.FindControl("CompDropList");
            Label message_result = (Label)CompetitorsLoginView.FindControl("CompResultMessage");

            if (ddl.SelectedValue == "Title")
            {
                message_result.Text = "Nothing to add/update.";
                message_result.Visible = true;
            }
            else
            {
                TextBox cs = (TextBox)CompetitorsLoginView.FindControl("CompSal");
                TextBox cn = (TextBox)CompetitorsLoginView.FindControl("CompName");
                TextBox cdob = (TextBox)CompetitorsLoginView.FindControl("CompDOB");
                TextBox ce = (TextBox)CompetitorsLoginView.FindControl("CompEmail");
                TextBox cd = (TextBox)CompetitorsLoginView.FindControl("CompDesc");
                TextBox cc = (TextBox)CompetitorsLoginView.FindControl("CompCountry");
                TextBox cg = (TextBox)CompetitorsLoginView.FindControl("CompGender");
                TextBox cnum = (TextBox)CompetitorsLoginView.FindControl("CompNum");
                TextBox cw = (TextBox)CompetitorsLoginView.FindControl("CompWeb");
                FileUpload cu = (FileUpload)CompetitorsLoginView.FindControl("CompUpload");
                CheckBoxList cbl = (CheckBoxList)CompetitorsLoginView.FindControl("CompGameCheckBoxList");

                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                connection.Open();

                if (ddl.SelectedValue == "Add")
                {
                    String select_query = "SELECT comp_email FROM comp_data";

                    SqlDataReader reader;
                    SqlCommand select_command = new SqlCommand(select_query, connection);
                    reader = select_command.ExecuteReader();

                    bool success = true;
                    while (reader.Read())
                    {
                        if (ce.Text == reader["comp_email"].ToString())
                        {
                            message_result.Text = "Error: Competitor already exists. Make sure email is not duplicate.";
                            message_result.Visible = true;
                            success = false;
                            break;
                        }
                    }

                    select_command.Dispose();
                    reader.Close();

                    int count = 0;
                    foreach (ListItem item in cbl.Items)
                    {
                        if (item.Selected)
                        {
                            count++;
                        }
                    }

                    if (cs.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Salutation cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cn.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Name cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cdob.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Date of Birth cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (ce.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Email cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cd.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Description cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cc.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Country cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cg.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Gender cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cnum.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Contact Number cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (count == 0)
                    {
                        message_result.Text = "Competitor must be in an event.";
                        message_result.Visible = true;
                        success = false;
                    }

                    // Only validate if email isnt blank.
                    if (success)
                    {
                        try
                        {
                            String parsed = new MailAddress(ce.Text).Address;
                        }
                        catch (FormatException format_excep)
                        {
                            message_result.Text = "Email format is invalid: " + format_excep.Message;
                            message_result.Visible = true;
                            success = false;
                        }
                    }

                    if (success)
                    {
                        String insert_sql = "INSERT INTO comp_data (comp_salutation, comp_name, comp_dob, comp_email, comp_desc, comp_country, comp_gender, comp_contact_num, comp_website, comp_photo) VALUES (@CompSal, @CompName, @CompDOB, @CompEmail, @CompDesc, @CompCountry, @CompGender, @CompContactNum, @CompWeb, @CompPhoto)";
                        SqlCommand insert_command = new SqlCommand(insert_sql, connection);

                        insert_command.Parameters.Add("@CompSal", SqlDbType.Text).Value = cs.Text;
                        insert_command.Parameters.Add("@CompName", SqlDbType.Text).Value = cn.Text;
                        insert_command.Parameters.Add("@CompDOB", SqlDbType.Text).Value = cdob.Text;
                        insert_command.Parameters.Add("@CompEmail", SqlDbType.Text).Value = ce.Text;
                        insert_command.Parameters.Add("@CompDesc", SqlDbType.Text).Value = cd.Text;
                        insert_command.Parameters.Add("@CompCountry", SqlDbType.Text).Value = cc.Text;
                        insert_command.Parameters.Add("@CompGender", SqlDbType.Text).Value = cg.Text;
                        insert_command.Parameters.Add("@CompContactNum", SqlDbType.Int).Value = int.Parse(cnum.Text);
                        insert_command.Parameters.Add("@CompWeb", SqlDbType.Text).Value = cw.Text;

                        Stream ifs = cu.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(ifs);
                        byte[] data = br.ReadBytes((Int32)ifs.Length);
                        insert_command.Parameters.AddWithValue("@CompPhoto", data);

                        insert_command.ExecuteNonQuery();
                        insert_command.Dispose();
                        br.Dispose();
                        br.Close();
                        ifs.Dispose();
                        ifs.Close();
                        cu.Attributes.Clear();

                        SqlCommand cg_command = new SqlCommand("INSERT INTO comp_list_of_games (game_id_array) VALUES (@CompGamesArray)", connection);

                        String games = "";
                        foreach (ListItem item in cbl.Items)
                        {
                            if (item.Selected)
                            {
                                games += (item.Value + ",");
                            }
                        }
                        games = games.Substring(0, games.Length - 1); // Remove extra comma.
                        cg_command.Parameters.Add("@CompGamesArray", SqlDbType.Text).Value = games;
                        cg_command.Dispose();

                        message_result.Text = "Added successfully!";
                        message_result.Visible = true;
                    }
                }
                else if (ddl.SelectedValue != "Title") // So its not Add or Title, i.e. a comp_id instead.
                {
                    bool success = true;

                    int count = 0;
                    foreach (ListItem item in cbl.Items)
                    {
                        if (item.Selected)
                        {
                            count++;
                        }
                    }

                    if (cs.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Salutation cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cn.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Name cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cdob.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Date of Birth cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (ce.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Email cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cd.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Description cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cc.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Country cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cg.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Gender cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (cnum.Text.IsNullOrWhiteSpace() || cs.Text.Length < 1)
                    {
                        message_result.Text = "Competitor Contact Number cannot be blank.";
                        message_result.Visible = true;
                        success = false;
                    }
                    else if (count == 0)
                    {
                        message_result.Text = "Competitor must be in an event.";
                        message_result.Visible = true;
                        success = false;
                    }

                    // Only validate if email isnt blank.
                    if (success)
                    {
                        try
                        {
                            String parsed = new MailAddress(ce.Text).Address;
                        }
                        catch (FormatException format_excep)
                        {
                            message_result.Text = "Email format is invalid: " + format_excep.Message;
                            message_result.Visible = true;
                            success = false;
                        }
                    }

                    if (success)
                    {
                        String update_sql = "UPDATE comp_data " +
                        "SET comp_salutation = @CompSal, comp_name = @CompName, comp_dob = @CompDOB, comp_email = @CompEmail, comp_desc = @CompDesc, comp_country = @CompCountry, comp_gender = @CompGender, comp_contact_num = @CompContactNum, comp_website = @CompWeb, comp_photo = @CompPhoto " +
                        "WHERE comp_id = @CompID";

                        SqlCommand update_command = new SqlCommand(update_sql, connection);

                        update_command.Parameters.Add("@CompID", SqlDbType.Int).Value = int.Parse(ddl.SelectedValue);
                        update_command.Parameters.Add("@CompSal", SqlDbType.Text).Value = cs.Text;
                        update_command.Parameters.Add("@CompName", SqlDbType.Text).Value = cn.Text;
                        update_command.Parameters.Add("@CompDOB", SqlDbType.Text).Value = cdob.Text;
                        update_command.Parameters.Add("@CompEmail", SqlDbType.Text).Value = ce.Text;
                        update_command.Parameters.Add("@CompDesc", SqlDbType.Text).Value = cd.Text;
                        update_command.Parameters.Add("@CompCountry", SqlDbType.Text).Value = cc.Text;
                        update_command.Parameters.Add("@CompGender", SqlDbType.Text).Value = cg.Text;
                        update_command.Parameters.Add("@CompContactNum", SqlDbType.Int).Value = int.Parse(cnum.Text);
                        update_command.Parameters.Add("@CompWeb", SqlDbType.Text).Value = cw.Text;

                        Stream ifs = cu.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(ifs);
                        byte[] data = br.ReadBytes((Int32)ifs.Length);
                        update_command.Parameters.AddWithValue("@CompPhoto", data);

                        update_command.ExecuteNonQuery();
                        update_command.Dispose();
                        br.Dispose();
                        br.Close();
                        ifs.Dispose();
                        ifs.Close();
                        cu.Attributes.Clear();

                        String cg_u_sql = "UPDATE comp_list_of_games " +
                            "SET game_id_array = @CompGamesArray " +
                            "WHERE comp_id = @CompID";
                        SqlCommand cg_command = new SqlCommand(cg_u_sql, connection);

                        String games = "";
                        foreach (ListItem item in cbl.Items)
                        {
                            if (item.Selected)
                            {
                                games += (item.Value + ",");
                            }
                        }
                        games = games.Substring(0, games.Length - 1); // Remove extra comma.
                        cg_command.Parameters.Add("@CompID", SqlDbType.Int).Value = int.Parse(ddl.SelectedValue);
                        cg_command.Parameters.Add("@CompGamesArray", SqlDbType.Text).Value = games;
                        cg_command.ExecuteNonQuery();
                        cg_command.Dispose();

                        message_result.Text = "Updated successfully!";
                        message_result.Visible = true;
                    }
                }

                connection.Close();
            }
        }

        protected void CompDel_Click(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)CompetitorsLoginView.FindControl("CompDropList");
            Label message_result = (Label)CompetitorsLoginView.FindControl("CompResultMessage");

            if (ddl.SelectedValue == "Title" || ddl.SelectedValue == "Add")
            {
                message_result.Text = "Nothing to delete.";
                message_result.Visible = true;
            }
            else
            {
                SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");
                connection.Open();

                SqlCommand del_command = new SqlCommand("DELETE FROM comp_data WHERE comp_id = @CompID", connection);
                del_command.Parameters.Add("@CompID", SqlDbType.Int).Value = int.Parse(ddl.SelectedValue);

                del_command.ExecuteNonQuery();
                del_command.Dispose();

                SqlCommand cg_del_command = new SqlCommand("DELETE FROM comp_list_of_games WHERE comp_id = @CompID", connection);
                cg_del_command.Parameters.Add("@CompID", SqlDbType.Int).Value = int.Parse(ddl.SelectedValue);

                cg_del_command.ExecuteNonQuery();
                cg_del_command.Dispose();
                connection.Close();

                Response.Redirect(Request.RawUrl);
            }
        }

        protected void CompRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}