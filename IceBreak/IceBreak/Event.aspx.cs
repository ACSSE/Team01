using IcebreakServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IceBreak
{
    public partial class Event : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // Page.ClientScript.RegisterStartupScript(this.GetType(),"onLoad", "<script type='text/javascript'> startTime();</script>", true);

            DBServerTools dbs = new DBServerTools();

            List<IcebreakServices.Event> events = dbs.getEvents();

            foreach(IcebreakServices.Event evnt in events)
            {
                        EventView.InnerHtml += "<div class='row'>"+
                           "<div class='col-md-7'>"+
                        "<a href = '#'>"+
                            "<img class='img-responsive' src='http://icebreak.azurewebsites.net/images/event_icons-"+evnt.Id+".png' alt=''/>"+
                        "</a>"+
                    "</div>"+
                    "<div class='col-md-5'>"+
                        "<h3>"+evnt.Title+"</h3>"+
                        "<h4>"+evnt.Address+"</h4>"+
                        "<p>"+evnt.Description+"</p>"+
                        "<a class='btn btn-primary' href='#'>View Event <span class='glyphicon glyphicon-chevron-right'></span></a>"+
                   " </div>" +
                "</div>"+
                "<hr>";
            }

        }
    }
}