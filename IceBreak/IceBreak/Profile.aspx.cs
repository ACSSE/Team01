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
            DBServerTools dbs = new DBServerTools();
            master m = new master();
                 
           
            //String username;
            //TextBox txtUsername = (TextBox)this.Master.FindControl("txtUsername");
            //username = txtUsername.Text;


            //User user = dbs.getUser(username);

        }

       
        
    }
}