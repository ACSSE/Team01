using IcebreakServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IceBreak
{
    public partial class stats : System.Web.UI.Page
    {
        public List<IcebreakServices.Event> usr_events;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string usr = (string)Session["USER"];
                string lvl = Convert.ToString(Session["LEVEL"]);
                if (usr != null && lvl != null)
                {
                    int iLvl = Convert.ToUInt16(lvl);
                    if (iLvl > 0)
                    {
                        welcome_msg.InnerText = "Welcome back " + Session["NAME"] + ".";

                        DBServerTools dbTools = new DBServerTools();
                        usr_events = dbTools.getEventsforUser(usr);
                        //dd_events.DataSource = usr_events;
                        //dd_events.DataBind();
                        dd_events.Items.Clear();
                        foreach (IcebreakServices.Event ev in usr_events)
                            dd_events.Items.Add(ev.Title);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not authorized to view this page.');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not signed in.');", true);
                    return;
                }
            }
        }

        protected void btnViewStatsForEvent_Click(object sender, EventArgs e)
        {
            string usr = (string)Session["USER"];
            string lvl = Convert.ToString(Session["LEVEL"]);
            if (usr != null && lvl != null)
            {
                int iLvl = Convert.ToUInt16(lvl);
                if (iLvl > 0)
                {
                    DBServerTools dbTools = new DBServerTools();
                    List<IcebreakServices.Event> usr_events = dbTools.getEventsforUser(usr);
                    int selected = dd_events.SelectedIndex;
                    if (usr_events != null)
                    {
                        if (usr_events.Count > selected)
                        {
                            IcebreakServices.Event selected_event = usr_events.ElementAt(selected);

                            //Properties
                            int vpad = 0;//px
                            string graph_type = "line";//bar|line,etc. refer to Chart.js website.
                            int graph_w = 700;//px
                            int graph_h = 400;//px

                            //Get Icebreak count graph
                            string count_canvas_js = getUserIcebreakCountGraph(selected_event, 
                                                                                graph_w, 
                                                                                graph_h, 
                                                                                vpad, 
                                                                                graph_type,
                                                                                "# of user Icebreaks",
                                                                                dbTools);

                            //Get successful Icebreak count graph
                            string scount_canvas_js = getSuccessfulUserIcebreakCountGraph(selected_event, 
                                                                                            graph_w, 
                                                                                            graph_h, 
                                                                                            vpad, 
                                                                                            graph_type,
                                                                                            "# of successful user Icebreaks",
                                                                                            dbTools);

                            //Get unsuccessful Icebreak count graph
                            string unscount_canvas_js = getUnsuccessfulUserIcebreakCountGraph(selected_event,
                                                                                            graph_w,
                                                                                            graph_h,
                                                                                            vpad,
                                                                                            graph_type,
                                                                                            "# of unsuccessful user Icebreaks",
                                                                                            dbTools);

                            int usr_img_size = 90;//px
                            List<User> users_at_event = dbTools.getUsersAtEvent(selected_event.Id);
                            string users_at_event_html = "<div style='width:auto;max-height:900px;y-overflow:scroll;'>";
                            foreach (User u in users_at_event)
                            {
                                string img_url = "./images/profile/" + u.Username + ".png";
                                string name = String.IsNullOrEmpty(u.Fname) ? "Anonymous" : u.Fname + " " + u.Lname;
                                users_at_event_html += "<div style='width:500px;height:100px;margin:auto;background-color:#e2e2e2;border:1px solid #343434;'>"
                                                            + "<div style='border-radius:100px;width:" + usr_img_size + "px;height:"+ usr_img_size + "px;float:left;'>"
                                                                + "<img style='border-radius:100px;border:1px solid #343434;' src='" + img_url+"' height='"+ usr_img_size + "px' width='"+ usr_img_size + "px' alt='"+name+" profile image'/>"
                                                            +"</div>"
                                                            +"<h3 align='center' style='margin-top:30px;'>"+name+"</h3>"
                                                        +"</div>";
                            }
                            users_at_event_html += "</div>";

                            int NUM_GRAPHS = 2;
                            int GRAPH_VPOS_OFFSET = 10;//px;
                            //" + (graph_h*NUM_GRAPHS+GRAPH_VPOS_OFFSET) + "

                            string overview_html = "<div style='width:400px;height:auto;margin:auto;background-color:#e2e2e2;border-radius:5px;border:1px solid #494949;'>"
                                                        + "<p># users at " + selected_event.Title + ": " + users_at_event.Count + "</p>"
                                                        + "<p># Icebreak count at " + selected_event.Title + ": " + dbTools.getEventIcebreakCountBetweenTime(selected_event.Id, selected_event.Date, selected_event.End_Date) + "</p>"
                                                        + "<p># successful Icebreaks at " + selected_event.Title + ": " + dbTools.getEventSuccessfulIcebreakCountBetweenTime(selected_event.Id,selected_event.Date,selected_event.End_Date) + "</p>"
                                                        + "<p># unsuccessful Icebreaks at " + selected_event.Title + ": " + dbTools.getEventUnsuccessfulIcebreakCountBetweenTime(selected_event.Id, selected_event.Date, selected_event.End_Date) + "</p>"
                                                    +"</div>";
                            string canvas_html =
                                 "<div style='width:" + graph_w + "px;height:auto;margin-left:auto;margin-right:auto;margin-top:" + vpad + "px;'>"
                                    + "<h2 align='center'>Overview of " + selected_event.Title + "</h2>"
                                    + overview_html
                                    + "<canvas id=\"graph_ib_count_" + selected_event.Id + "\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_succ_ib_count_" + selected_event.Id + "\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_unsucc_ib_count_" + selected_event.Id + "\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<h2 align='center'>Users currently at "+selected_event.Title+"</h2>"
                                    + users_at_event_html
                                + "</div>"
                                +count_canvas_js
                                +scount_canvas_js
                                +unscount_canvas_js
                                + "<script>window.location='./stats.aspx#tab_events'</script>";

                            canvas_container.InnerHtml = canvas_html;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You have no events.');", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You have no events.');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not authorized to view this page.');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not signed in.');", true);
                return;
            }
        }

        public string getUserIcebreakCountGraph(IcebreakServices.Event selected_event, int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";
            long start_date = selected_event.Date;//in seconds
            long end_date = selected_event.End_Date;//in seconds


            long interval = 60 * 60;//increase by 1 hour every time by default
            long diff = (end_date - start_date) / (interval * 24); // event length in days

            if (diff > 12)//if event is > 12 days then interval must go in 1 week increments
                interval *= 24 * 7;
            else if (diff > 10)//if event is > 10 days then interval must go in 4 day increments
                interval *= 24 * 4;
            else if (diff > 8)//if event is > 8 days then interval must go in 2 day increments
                interval *= 24 * 2;
            else if (diff > 6)//if event is >= 1 week then interval must go in 1 day increments
                interval *= 24 * 1;

            //container.InnerHtml += "[start:" + start_date + "],[end:" + end_date + "]; interval="+interval +"<hr/>";
            for (long t = start_date; t <= end_date; t += interval)
            {
                int count = dbTools.getEventIcebreakCountBetweenTime(selected_event.Id, t, t + interval);
                y_axis += count + ",";
                x_axis += "'" + FromUnixTime(t) + "',";
            }

            x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
            y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

            string jsGraph =
                  "<script>"
                    + getChartJS("graph_ib_count_" + Convert.ToString(selected_event.Id), 
                                    selected_event.Title, 
                                    label,
                                    graph_type, 
                                    x_axis, 
                                    y_axis)
                + "</script>";

            return jsGraph;
        }

        public string getSuccessfulUserIcebreakCountGraph(IcebreakServices.Event selected_event, int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";
            long start_date = selected_event.Date;//in seconds
            long end_date = selected_event.End_Date;//in seconds


            long interval = 60 * 60;//increase by 1 hour every time by default
            long diff = (end_date - start_date) / (interval * 24); // event length in days

            if (diff > 12)//if event is > 12 days then interval must go in 1 week increments
                interval *= 24 * 7;
            else if (diff > 10)//if event is > 10 days then interval must go in 4 day increments
                interval *= 24 * 4;
            else if (diff > 8)//if event is > 8 days then interval must go in 2 day increments
                interval *= 24 * 2;
            else if (diff > 6)//if event is >= 1 week then interval must go in 1 day increments
                interval *= 24 * 1;

            //container.InnerHtml += "[start:" + start_date + "],[end:" + end_date + "]; interval="+interval +"<hr/>";
            for (long t = start_date; t <= end_date; t += interval)
            {
                int count = dbTools.getEventSuccessfulIcebreakCountBetweenTime(selected_event.Id, t, t + interval);
                y_axis += count + ",";
                x_axis += "'" + FromUnixTime(t) + "',";
            }

            x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
            y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

            string jsGraph =
                  "<script>"
                    + getChartJS("graph_succ_ib_count_" + Convert.ToString(selected_event.Id),
                                    selected_event.Title,
                                    label,
                                    graph_type, 
                                    x_axis, 
                                    y_axis)
                + "</script>";

            return jsGraph;
        }

        public string getUnsuccessfulUserIcebreakCountGraph(IcebreakServices.Event selected_event, int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";
            long start_date = selected_event.Date;//in seconds
            long end_date = selected_event.End_Date;//in seconds


            long interval = 60 * 60;//increase by 1 hour every time by default
            long diff = (end_date - start_date) / (interval * 24); // event length in days

            if (diff > 12)//if event is > 12 days then interval must go in 1 week increments
                interval *= 24 * 7;
            else if (diff > 10)//if event is > 10 days then interval must go in 4 day increments
                interval *= 24 * 4;
            else if (diff > 8)//if event is > 8 days then interval must go in 2 day increments
                interval *= 24 * 2;
            else if (diff > 6)//if event is >= 1 week then interval must go in 1 day increments
                interval *= 24 * 1;

            //container.InnerHtml += "[start:" + start_date + "],[end:" + end_date + "]; interval="+interval +"<hr/>";
            for (long t = start_date; t <= end_date; t += interval)
            {
                int count = dbTools.getEventUnsuccessfulIcebreakCountBetweenTime(selected_event.Id, t, t + interval);
                y_axis += count + ",";
                x_axis += "'" + FromUnixTime(t) + "',";
            }

            x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
            y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

            string jsGraph =
                  "<script>"
                    + getChartJS("graph_unsucc_ib_count_" + Convert.ToString(selected_event.Id),
                                    selected_event.Title,
                                    label,
                                    graph_type,
                                    x_axis,
                                    y_axis)
                + "</script>";

            return jsGraph;
        }

        public string getChartJS(string id, string title, string label, string graph_type, string x_axis, string y_axis)
        {
            string chart = "var ctx = document.getElementById('" + id + "');"
                                            + "var myChart = new Chart(ctx, "
                                            + "{"
                                                + "type: '" + graph_type + "',"
                                                + "data:"
                                                + "{"
                                                + "labels: [" + x_axis + "],"
                                                + "datasets: "
                                                + "["
                                                    + "{"
                                                        + "label: '"+label+"',"
                                                        + "data: [" + y_axis + "],"
                                                        + "backgroundColor: ["
                                                            + "'rgba(255, 99, 132, 0.2)',"
                                                            + "'rgba(54, 162, 235, 0.2)',"
                                                            + "'rgba(255, 206, 86, 0.2)',"
                                                            + "'rgba(75, 192, 192, 0.2)',"
                                                            + "'rgba(153, 102, 255, 0.2)',"
                                                            + "'rgba(255, 159, 64, 0.2)'"
                                                        + "],"
                                                        + "borderColor: ["
                                                            + "'rgba(255,99,132,1)',"
                                                            + "'rgba(54, 162, 235, 1)',"
                                                            + "'rgba(255, 206, 86, 1)',"
                                                            + "'rgba(75, 192, 192, 1)',"
                                                            + "'rgba(153, 102, 255, 1)',"
                                                            + "'rgba(255, 159, 64, 1)'"
                                                        + "],"
                                                        + "borderWidth:1"
                                                    + "}"
                                                + "]"
                                            + "},"
                                            + "options:"
                                            + "{"
                                                + "title:"
                                                + "{"
                                                    + "display:true,"
                                                    + "text:'" + title + "'"
                                                + "},"
                                                + "scales:"
                                                + "{"
                                                    + "yAxes: [{"
                                                        + "ticks:"
                                                        + "{"
                                                            + "beginAtZero:"
                                                            + "true"
                                                        + "}"
                                                    + "}]"
                                                + "}"
                                            + "}"
                                        + "});";

            return chart;
        }

        protected void btnIndividual_Click(object sender, EventArgs e)
        {
            string usr = (string)Session["USER"];
            string lvl = (string)Session["LEVEL"];
            if (usr != null && lvl != null)
            {
                int iLvl = Convert.ToUInt16(lvl);
                if (iLvl >= 0)
                {
                    switch (iLvl)
                    {
                        case 0://normal user
                            break;
                        case 1://event admin
                            break;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not authorized to view this page.');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not signed in.');", true);
                return;
            }
        }

        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}