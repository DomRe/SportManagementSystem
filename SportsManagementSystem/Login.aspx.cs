using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SportsManagementSystem
{
    public partial class WebForm5 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/");
            }
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            if (log_in(LoginForm.UserName, LoginForm.Password))
            {
                e.Authenticated = true;
                FormsAuthentication.RedirectFromLoginPage(LoginForm.UserName, LoginForm.RememberMeSet);
            }
            else
            {
                e.Authenticated = false;
            }
        }

        protected bool log_in(String username, String password)
        {
            const String query = "SELECT user_password, user_email, user_type FROM login_data";
            const String connection_string = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True";

            SqlConnection connection = new SqlConnection(connection_string);
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader;

            connection.Open();
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (username == reader["user_email"].ToString() && password == reader["user_password"].ToString())
                {
                    int role = (int)reader["user_type"];
                    if (role == 0)
                    {
                        if (!Roles.RoleExists("Admin"))
                        {
                            Roles.CreateRole("Admin");
                        }

                        if (!Roles.IsUserInRole(username, "Admin"))
                        {
                            Roles.AddUserToRole(username, "Admin");
                        }
                    }
                    else if (role == 1)
                    {
                        if (!Roles.RoleExists("EventManager"))
                        {
                            Roles.CreateRole("EventManager");
                        }

                        if (!Roles.IsUserInRole(username, "EventManager"))
                        {
                            Roles.AddUserToRole(username, "EventManager");
                        }
                    }

                    reader.Close();
                    command.Dispose();
                    connection.Close();
                    return true;
                }
            }

            reader.Close();
            command.Dispose();
            connection.Close();
            return false;
        }
    }
}