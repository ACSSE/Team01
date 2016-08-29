using IcebreakServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IceBreak
{
    public partial class master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                if (Session["USER"] != null)
                {
                string check = (string)Session["USER"];
                login.InnerHtml = "<a href='javascript:Logout()' runat='server'>Logout " + check + "</a>";

                }
        }

        [ScriptMethod, WebMethod]
        protected void Logout(object sender, EventArgs e)
        {
            if (Session["USER"] != null)
            {
                Session.Clear();
                Response.Redirect("index.aspx");
            }
        }


        protected void Login(object sender, EventArgs e)
        {
           

            string username = txtUsername.Value;
            string password = txtPassword.Value;
            if(String.IsNullOrEmpty(username))
            {
                usernameBox.Attributes.Add("class", "form-inline has-error has-feedback");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "loginModal", "$('#loginModal').modal();", true);
                return;
            }
            if (String.IsNullOrEmpty(password))
            {
                passwordBox.Attributes.Add("class", "form-inline has-error has-feedback");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "loginModal", "$('#loginModal').modal();", true);
                return;
            }
            User user = new User();
            user.Username = username;
            user.Password = password;

            DBServerTools dbs = new DBServerTools();
            String check =  dbs.signIn(user);

            if (check.ToLower().Contains("isvaliduser=true"))
            {
                Session["USER"] = username;
                user = dbs.getUser(username);
                int lvl  = user.Access_level;
                Session["LEVEL"] = lvl;
                login.InnerHtml = "<a href='javascript:Logout()'  runat='server' >Logout " + username + "</a>";
            }
            else
            {
                txtInvalid.Style.Clear();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "loginModal", "$('#loginModal').modal();", true);
            }

        }
       
    }
}