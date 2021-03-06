﻿using IcebreakServices;
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
            searchtext.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            if (Session["USER"] != null)
            {
                string check = (string)Session["USER"];
                string checkName = (string)Session["NAME"];
                string checkLastName = (string)Session["LASTNAME"];
                login.InnerHtml = "<a href='javascript:Logout()' runat='server'>Logout " + check + "</a>";
                logout2.InnerHtml = "<a href='javascript:Logout()' runat='server'>Logout " + check + "</a>";

                YourEvents.InnerHtml = "<a href='YourEvents.aspx'>Your Events</a>";
                DIV.InnerHtml = "<a href = '#'>" +
                        "<img class='image-circle' src='http://icebreak.azurewebsites.net/images/profile/" + check + ".png' alt=''/>" +
                    "</a>" + "<label class='Sidebarname'>" + checkName + " " + checkLastName + "</label>";
                //DIV.InnerHtml = "<label class='Sidebarname'>" + checkName + " " + checkLastName + "</label>";
            }
            
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "NotificationModalMessage", "$('#notifMsg').data('value');", true);
            string msg = Request.QueryString["notif_msg"];
            if (msg != null)
            {
                if (msg.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NotificationModal", "$('#NotificationModal').modal();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NotificationModalMessage", "$('#notif_msg').text('" + msg + "');", true);
                }
            }
        }

        [ScriptMethod, WebMethod]
        protected void Logout(object sender, EventArgs e)
        {
            if (Session["USER"] != null)
            {
                Session["LEVEL"] = 0;
                Session.Clear();
                Response.Redirect("index.aspx");
            }
        }
        [ScriptMethod, WebMethod]
        protected void SearchEvent(object sender, EventArgs e)
        {
            Response.Redirect("SearchResults.aspx?search=" + searchtext.Text);

        }
        protected void Login(object sender, EventArgs e)
        {

            string username = txtUsername.Value;
            string password = txtPassword.Value;

            if (String.IsNullOrEmpty(username))
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

            IcebreakServices.User usr = dbs.getUser(username);

            String check = dbs.signIn(user);

            if (check.ToLower().Contains("isvaliduser=true"))
            {
                String firstname = usr.Fname;
                String lastname = usr.Lname;

                Session["USER"] = username;
                Session["NAME"] = firstname;
                Session["LASTNAME"] = lastname;
                user = dbs.getUser(username);
                int lvl = user.Access_level;
                Session["LEVEL"] = lvl;
                login.InnerHtml = "<a href='javascript:Logout()'  runat='server' >Logout " + username + "</a>";
                logout2.InnerHtml = "<a href='javascript:Logout()' runat='server'>Logout " + check + "</a>";
                YourEvents.InnerHtml = "<a href='YourEvents.aspx'>Your Events</a>";
                DIV.InnerHtml = "<a href = '#'>" +
                        "<img class='image-circle' src='http://icebreak.azurewebsites.net/images/profile/" + username + ".png' alt=''/>" +
                    "</a>" + "<label class='Sidebarname'>" + firstname + " " + lastname + "</label>";
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                txtInvalid.Style.Clear();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "loginModal", "$('#loginModal').modal();", true);
            }

        }
        protected void SignUp(object sender, EventArgs e)
        {
            DBServerTools dbs = new DBServerTools();
            string name = txtName.Value;
            string lastname = txtLastName.Value;
            string email = txtemail.Value;
            string usrname = txtUsrname.Value;
            string pass = txtPass.Value;
            if (String.IsNullOrEmpty(name))
            {
                NameBox.Attributes.Add("class", "form-inline has-error has-feedback");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "signUpModal", "$('#signUpModal').modal();", true);
                return;
            }
            if (String.IsNullOrEmpty(lastname))
            {
                lastNameBox.Attributes.Add("class", "form-inline has-error has-feedback");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "signUpModal", "$('#signUpModal').modal();", true);
                return;
            }
            if (String.IsNullOrEmpty(email))
            {
                EmailBox.Attributes.Add("class", "form-inline has-error has-feedback");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "signUpModal", "$('#signUpModal').modal();", true);
                return;
            }
            if (String.IsNullOrEmpty(usrname))
            {
                UsernameSignupBox.Attributes.Add("class", "form-inline has-error has-feedback");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "signUpModal", "$('#signUpModal').modal();", true);
                return;
            }
            if (String.IsNullOrEmpty(pass))
            {
                PasswordSignupBox.Attributes.Add("class", "form-inline has-error has-feedback");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "signUpModal", "$('#signUpModal').modal();", true);
                return;
            }


            User user = new User();
            user.Fname = name;
            user.Lname = lastname;
            user.Email = email;
            user.Username = usrname;
            user.Password = pass;

            String check = dbs.registerUser(user);

            if (check.ToLower().Contains("success"))
            {
                Session["USER"] = usrname;
                login.InnerHtml = "<a href='#' data-toggle='modal' data-target='#loginModal' >Logout " + usrname + "</a>";

                logout2.InnerHtml = "<a href='javascript:Logout()' runat='server'>Logout " + check + "</a>";
            }
            else
            {
                txtInvalid.Style.Clear();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "loginModal", "$('#loginModal').modal();", true);
            }

        }


    }
}
