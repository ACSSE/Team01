using IcebreakServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IceBreak
{
    public partial class YourEvents : System.Web.UI.Page
    {
        protected string EventID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            String username = (string)Session["User"];
            DBServerTools dbs = new DBServerTools();


            List<IcebreakServices.Event> events = dbs.getEventsforUser(username);
            foreach (IcebreakServices.Event evnt in events)
            {
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
                    pageExists = false;
                }

                if (!pageExists)
                {
                    picUrl = "http://icebreak.azurewebsites.net/images/events/default.png";
                }

                EventView.InnerHtml += "<div class='row'>" +
                           "<div class='col-md-7'>" +
                        "<a href = '#'>" +
                            "<img class='img-responsive' src='" + picUrl + "' alt=''/>" +
                        "</a>" +
                    "</div>" +
                    "<div class='col-md-5'>" +
                        "<h3>" + evnt.Title + "</h3>" +
                        "<h4>" + evnt.Address + "</h4>" +
                        "<p>" + evnt.Description + "</p>" +
                        "<a class='btn btn-primary' href='EditEvent.aspx?evntid=" + evnt.Id + "'>Edit Event <span class='glyphicon glyphicon-chevron-right'><a href='javascript:Delete("+evnt.Id +")' class='btn remove'><span class='glyphicon glyphicon-remove'></span></a>" +
                   " </div>" +
                "</div>" +
                "<hr>";
            }
        }
       
        [ScriptMethod,WebMethod]
        protected void Delete(object sender, CommandEventArgs e)
        {
            this.EventID = Request.Form["EventId"];
           

            DBServerTools dst = new DBServerTools();

            String check = dst.deleteEvent(this.EventID);
            if (check.ToLower().Contains("success"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "window.location ='YourEvents.aspx';", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(\"Event deletion unsuccessful: " + check + "\");", true);
            }


        }
    }
}