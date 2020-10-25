using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SportsManagementSystem
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        protected void ButtonGames_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/GamesManagement.aspx");
        }

        protected void ButtonCompetitors_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CompetitorsManagement.aspx");
        }

        protected void ButtonEvents_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EventsManagement.aspx");
        }

        protected void ButtonReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ReportsManagement.aspx");
        }
    }
}