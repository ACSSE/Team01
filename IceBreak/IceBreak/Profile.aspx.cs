using System;
using IcebreakServices;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IceBreak
{
    
    public partial class Profile : System.Web.UI.Page 
    {

       protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LEVEL"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You must login');window.location ='index.aspx';", true);

            }
           else if (!IsPostBack)
            {
                getUser();

                update.Click += new EventHandler(this.Updatebutton_click);
            }

        }

        protected void getUser()
        {
            DBServerTools dbs = new DBServerTools();
            User user = new User();

            string name = txtName.Text;
            string lastname = txtSurname.Text;
            string email = txtEmail.Text;
            string usrname = txtUser.Text;
            string Bio = txtBio.Text;
            int Age = user.Age;
            string catchphrase = txtCatch.Text;


            //string username = m.txtUsername.Value;

            user.Username = usrname;
            user.Fname = name;
            user.Lname = lastname;
            user.Email = email;
            user.Bio = Bio;
            user.Age = Age;
            user.Catchphrase = catchphrase;


            String check = dbs.userExists(user);
            string usr = (string)Session["USER"];
            usrname = usr;


            IcebreakServices.User u = dbs.getUser(usrname);
            
            txtName.Text = u.Fname;
            txtSurname.Text = u.Lname;
            txtEmail.Text = u.Email;
            txtUser.Text = u.Username;
            txtBio.Text = u.Bio;
            txtCatch.Text = u.Catchphrase;
            txtAge.Text = u.Age.ToString();
            lblName.Text = u.Fname;
            lblSurname.Text = u.Lname;
            lblOccupation.Text = u.Occupation;
            lblAge.Text = u.Age.ToString();
            lblBio.Text = u.Bio;
            DIV.InnerHtml += "<a href = '#'>" +
                            "<img class='img-circle' src='http://icebreak.azurewebsites.net/images/profile/" + usr + ".png' alt=''/>" +
                        "</a>";
            

        }

        protected void Updatebutton_click(Object sender,EventArgs e)
        {
            
            Button clickedButton = (Button)sender;
            DBServerTools dbs = new DBServerTools();
            User user = new User();

            string name = txtName.Text;
            string lastname = txtSurname.Text;
            string email = txtEmail.Text;
            string usrname = txtUser.Text;
            string Bio = txtBio.Text;
            UInt16 age;
            try
            {
                 age = (UInt16)(Convert.ToInt16(txtAge.Text));
            }
            catch(FormatException ex)
            {
                return;//TODO: return message
            }
            catch (OverflowException ex)
            {
                return;//TODO: return message
            }
            string catchphrase = txtCatch.Text;

           
            user.Fname = name;
            user.Lname = lastname;
            user.Email = email;
            user.Username = usrname;
            user.Bio = Bio;
            user.Catchphrase = catchphrase;
            user.Age = (int)age;


            String check = dbs.userExists(user);
            string usr = (string)Session["USER"];
            dbs.updateUserDetails(user);

            usrname = usr;
            IcebreakServices.User u = dbs.getUser(usrname);
            txtName.Text = u.Fname;
            txtSurname.Text = u.Lname;
            txtEmail.Text = u.Email;
            txtUser.Text = u.Username;
            txtBio.Text = u.Bio;
            txtCatch.Text = u.Catchphrase;
            txtAge.Text = u.Age.ToString();
            lblName.Text = u.Fname;
            lblSurname.Text = u.Lname;

           
        }

    }
   
}