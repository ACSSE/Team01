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
            string usrname = txtName.Text;
            string Bio = txtBio.Text;
            string catchphrase = txtCatch.Text;

            //string username = m.txtUsername.Value;
            
            user.Username = usrname;
            user.Fname = name;
            user.Lname = lastname;
            user.Email = email;
            user.Bio = Bio;
            user.Catchphrase = catchphrase;

            
           String check = dbs.userExists(user);
            usrname = "tevmoodley";
            //User obj = (User)Session["USER"];
            //obj.Username = usrname;
            //usrname = obj.Username;
            

            IcebreakServices.User u = dbs.getUser(usrname);
            txtName.Text = u.Fname;
            txtSurname.Text = u.Lname;
            txtEmail.Text = u.Email;
            txtBio.Text = u.Bio;
            //if (check.ToLower().Contains("isvaliduser=true"))
            //{
            //    Session["USER"] = usrname;
            //}
            //else
            //{
            //}




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