using IcebreakServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IceBreak
{
    public partial class ViewEvent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            loading_qr_ico.Visible = false;
            if (Session["LEVEL"] != null && Session["USER"] != null)
            {
                int check = (int)Session["LEVEL"];
                if (check >= 0)
                {
                    //divQR.Style.Add("display", "normal");
                    loading_qr_ico.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You do not have access to this page');window.location ='index.aspx';", true);
                    Response.Redirect("http://icebreak.azurewebsites.net/index.aspx");
                    return;
                }
            }
           
            string eventid = Request.QueryString["evntid"];

            DBServerTools dbs = new DBServerTools();

            IcebreakServices.Event evnt = dbs.getEvent(eventid);

            if (evnt == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Event object is null.');window.location ='index.aspx';", true);
                return;
            }
            qr_code.Src = "http://chart.googleapis.com/chart?cht=qr&chl=" + evnt.AccessCode + "&chs=400x400";
            access_code.InnerText = "The access code is " + Convert.ToString(evnt.AccessCode);
            loading_qr_ico.Visible = true;

            String picUrl = "http://icebreak.azurewebsites.net/images/events/event_icons-" + evnt.Id + ".png";
            //String server = "http://icebreak.azurewebsites.net";
            //String relativePath = "/images/events/event_icons-" + evnt.Id + ".png";
            //Uri serverUri = new Uri(server);
            //Uri relativeUri = new Uri(relativePath, UriKind.Relative);
            //Uri fullUri = new Uri(serverUri, relativeUri);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);
            request.Method = WebRequestMethods.Http.Head;
            bool pageExists = true;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                pageExists = response.StatusCode == HttpStatusCode.OK;
            }
            catch (WebException ex)
            {
                //404 error - dbs.addError(ErrorCodes.EEVENT, ex.Message, "ViewEvent.aspx[Page_Load]");
                pageExists = false;
            }

            if (!pageExists)
            {
                picUrl = "http://icebreak.azurewebsites.net/images/events/default.png";
            }

            DateTime date = FromUnixTime(evnt.Date);
            DateTime enddate = FromUnixTime(evnt.End_Date);



            EventImage.InnerHtml = "<img class='img-responsive img-center' src='" +picUrl+ "'/>";

            EventName.InnerHtml = evnt.Title;


            EventDate.InnerHtml = "Date: " + date.ToShortDateString();
            EventStart.InnerHtml = "Start Time: "+date.ToShortTimeString();
            
            EventEnd.InnerHtml = "End Date: " + enddate.ToShortDateString();

            EndTime.InnerHtml = "End Time: " + enddate.ToShortTimeString();

            EventAddress.InnerHtml = "Address: " + evnt.Address;

            EVentDescription.InnerHtml = evnt.Description;
        }

        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}