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

        //    if (Session["USER"] != null)
        //    {
        //        string checking = (string)Session["USER"];
        //    }


            DBServerTools dbs = new DBServerTools();
            master m = new master();
            User user = new User();
           

            

            string name = txtName.Text;
            string lastname = txtSurname.Text;
            string email = txtEmail.Text;
            string usrname = txtUser.Text;
            string Bio = txtBio.Text;
            int Age = user.Age;
            string agestring = txtAge.Text.ToString();
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
            txtUser.Text = u.Username;
            txtCatch.Text = u.Catchphrase;
            txtAge.Text = u.Age.ToString();




        }

        class Helpers
        {
            public static void getUsername(master page)
            {
                //You can access your controls here like:
                String username = page.txtUsername.Value;

            }
        }

    

        

    }
   
}