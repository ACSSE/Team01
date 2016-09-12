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
                string checkName = (string)Session["NAME"];
                string checkLastName = (string)Session["LASTNAME"];
                login.InnerHtml = "<a href='javascript:Logout()' runat='server'>Logout " + check + "</a>";
                DIV.InnerHtml = "<a href = '#'>" +
                        "<img class='image-circle' src='http://icebreak.azurewebsites.net/images/profile/" + check + ".png' alt=''/>" +
                    "</a>";
                name.InnerHtml = "<label class='Sidebarname'>" + checkName + " " + checkLastName + "</label>";

                
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
            String check =  dbs.signIn(user);

            IcebreakServices.User u = dbs.getUser(username);
            String firstname = u.Fname;
            String lastname = u.Lname;

            if (check.ToLower().Contains("isvaliduser=true"))
            {
               
                Session["USER"] = username;
                Session["NAME"] = firstname;
                Session["LASTNAME"] = lastname;
                user = dbs.getUser(username);
                int lvl  = user.Access_level;
                Session["LEVEL"] = lvl;
                login.InnerHtml = "<a href='javascript:Logout()'  runat='server' >Logout " + username + "</a>";
                DIV.InnerHtml = "<a href = '#'>" +
                        "<img class='image-circle' src='http://icebreak.azurewebsites.net/images/profile/" + username + ".png' alt=''/>" +
                    "</a>";
               name.InnerHtml = "<label class='Sidebarname'>" + firstname +" "+ lastname + "</label>";
            }
            else
            {
                txtInvalid.Style.Clear();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "loginModal", "$('#loginModal').modal();", true);
            }

        }
        protected void SignUp(object sender, EventArgs e)
        {
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

            DBServerTools dbs = new DBServerTools();
            dbs.registerUser(user);
            String check = dbs.registerUser(user);

            if (check.ToLower().Contains("Sucess"))
            {
                Session["USER"] = usrname;
                login.InnerHtml = "<a href='#' data-toggle='modal' data-target='#loginModal' >Logout " + usrname + "</a>";
            }
            else
            {
                txtInvalid.Style.Clear();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "loginModal", "$('#loginModal').modal();", true);
            }



        }
        //protected void profilePic()
        //{
        //    DBServerTools dbs = new DBServerTools();
        //    User user = new User();
        //    string username = txtUsername.Value;
        //    user.Username = username;
        //    String check = dbs.userExists(user);
        //    string usr = (string)Session["USER"];
        //    username = usr;

        //    dbs.getUser(username);
        //    pp.InnerHtml += "<a href = '#'>" +
        //                   "<img class='img-circle' src='http://icebreak.azurewebsites.net/images/profile/" + usr + ".png' alt=''/>" +
        //               "</a>";

        //}

    }
}