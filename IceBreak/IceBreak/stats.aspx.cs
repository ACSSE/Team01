using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IceBreak
{
    public partial class stats : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string usr = (string)Session["USER"];
            //int lvl = (int)Session["LEVEL"];
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Level:"+lvl+"');", true);
            /*switch(lvl)
            {
                case 0://normal user
                    break;
                case 1://event admin
                    break;
            }*/
        }

        protected void btnEvents_Click(object sender, EventArgs e)
        {
            string usr = (string)Session["USER"];
            string lvl = (string)Session["LEVEL"];
            if (usr != null && lvl != null)
            {
                int iLvl = Convert.ToUInt16(lvl);
                if (iLvl > 0)
                {
                    string html =
                         "<div style=\"width: 500px; height: 500px; margin: auto; \">"
                            + "<canvas id=\"myChart\" width=\"600\" height=\"400\"></canvas>"
                        + "</div>"
                        + "<script>"
                        +    "alert('Rendered graphs!');"
                        + "</script>";
                    container.InnerHtml = html;
                    switch (iLvl)
                    {
                        case 0://normal user
                            break;
                        case 1://event admin
                            break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not authorized to view this page.');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not signed in.');", true);
                return;
            }
        }

        protected void btnIndividual_Click(object sender, EventArgs e)
        {
            string usr = (string)Session["USER"];
            string lvl = (string)Session["LEVEL"];
            if (usr != null && lvl != null)
            {
                int iLvl = Convert.ToUInt16(lvl);
                if (iLvl >= 0)
                {
                    switch (iLvl)
                    {
                        case 0://normal user
                            break;
                        case 1://event admin
                            break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not authorized to view this page.');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not signed in.');", true);
                return;
            }
        }
    }
}