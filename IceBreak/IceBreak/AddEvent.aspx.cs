using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IceBreak
{
    public partial class AddEvent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LEVEL"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),"alert","alert('You must login');window.location ='index.aspx';", true);
            }
            else
            {
                int check = (int)Session["LEVEL"];
                if (check != 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You do not have access to this page');window.location ='index.aspx';", true);
                }
            }

        }
    }
}