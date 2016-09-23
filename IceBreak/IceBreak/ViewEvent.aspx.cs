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
            string eventid = Request.QueryString["evntid"];

            DBServerTools dbs = new DBServerTools();

            IcebreakServices.Event evnt = dbs.getEvent(eventid);

            String picUrl = "http://icebreak.azurewebsites.net/images/events/event_icons-" + evnt.Id + ".png";
            String server = "http://icebreak.azurewebsites.net";
            String relativePath = "/images/events/event_icons-" + evnt.Id + ".png";
            Uri serverUri = new Uri(server);
            Uri relativeUri = new Uri(relativePath, UriKind.Relative);
            Uri fullUri = new Uri(serverUri, relativeUri);

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
                dbs.addError(ErrorCodes.EEVENT, ex.Message, "ViewEvent.aspx[Page_Load]");
                pageExists = false;
            }

            if (!pageExists)
            {
                picUrl = "http://icebreak.azurewebsites.net/images/events/default.png";
            }

            EventImage.InnerHtml = "<img class='img-responsive img-center' src='" +picUrl+ "'/>";

            EventName.InnerHtml = evnt.Title;

            EventDate.InnerHtml = "Date: "+evnt.Date;

            EventEnd.InnerHtml = "End Date: " + evnt.End_Date;

            EventAddress.InnerHtml = "Address: " + evnt.Address;

            EVentDescription.InnerHtml = evnt.Description;
        }
    }
}