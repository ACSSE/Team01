using IcebreakServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IceBreak
{
    public partial class SubscriptionFormDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LEVEL"] == null || Session["USER"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You must login');window.location ='index.aspx';", true);
                return;
            }
        }
        protected void ChangeAccess(object sender, EventArgs e)
        {
            String username = (string)Session["User"];
            DBServerTools dbs = new DBServerTools();
            User usr = dbs.getUser(username);
            usr.Access_level = 1;
            dbs.updateUserDetails(usr);
        }
    }
}