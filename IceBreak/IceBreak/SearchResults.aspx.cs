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
    public partial class SearchResults : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string res = Request.QueryString["search"];

            DBServerTools dbs = new DBServerTools();

            if (!String.IsNullOrWhiteSpace(res))
            {
                List<IcebreakServices.Event> events = dbs.getAllSearchedEvents(res);

                if (events.Any())
                {
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
                            //dbs.addError(ErrorCodes.EEVENT, ex.Message, "Event.aspx[Page_Load]");
                        }

                        if (!pageExists)
                        {
                            picUrl = "http://icebreak.azurewebsites.net/images/events/default.png";
                        }

                        DateTime date = FromUnixTime(evnt.Date);

                        SearchView.InnerHtml += "<div class='row'>" +
                                   "<div class='col-md-7'>" +
                                "<a href = '#'>" +
                                    "<img class='img-responsive' src='" + picUrl + "' alt=''/>" +
                                "</a>" +
                            "</div>" +
                            "<div class='col-md-5'>" +
                                "<h3>" + evnt.Title + "</h3>" +
                                "<h4>" + evnt.Address + "</h4>" +
                            "<p>" + date.ToShortDateString() + " " + date.ToShortTimeString() + "</p>" +
                                "<a class='btn btn-primary' href='ViewEvent.aspx?evntid=" + evnt.Id + "'>View Event <span class='glyphicon glyphicon-chevron-right'></span></a>" +
                           " </div>" +
                        "</div>" +
                        "<hr>";
                    }
                }
                else
                {
                    SearchView.InnerHtml += "No search results";
                }
            }
            else
            {
                SearchView.InnerHtml += "No search results";
            }
        }
        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}