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
            //if (Session["LEVEL"] == null)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(),"alert","alert('You must login');window.location ='index.aspx';", true);
            //}
            //else
            //{
            //    int check = (int)Session["LEVEL"];
            //    if (check != 1)
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You do not have access to this page');window.location ='index.aspx';", true);
            //    }
            //}
        }
        protected void btnAdd_Event(object sender, EventArgs e)
        {
            string EventName = eventname.Value;
            string EventAddress = eventaddress.Value;
            string EventDescrip = eventdescrip.Value;
            string EventTime = time.Value;
            string EventDate = date.Value;
            if (String.IsNullOrEmpty(EventName))
            {
                lbl_name.Style.Add("display", "normal");
                return;
            }
            else
            {
                lbl_name.Style.Add("display", "none");
            }           
            if (String.IsNullOrEmpty(EventAddress))
            {
                address_span.Style.Add("display", "normal");
                return;
            }
            else
            {
                address_span.Style.Add("display", "none");
            }
            if (String.IsNullOrEmpty(EventDescrip))
            {
                descrip_span.Style.Add("display", "normal");
                return;
            }
            else
            {
                descrip_span.Style.Add("display", "none");
            }
            if (String.IsNullOrEmpty(EventDate))
            {
                date_span.Style.Add("display", "normal");
                return;
            }
            else
            {
                date_span.Style.Add("display", "none");
            }
            if (String.IsNullOrEmpty(EventTime))
            {
                time_span.Style.Add("display", "normal");
                return;
            }
            else
            {
                time_span.Style.Add("display", "none");
            }
            
        }
    }
}