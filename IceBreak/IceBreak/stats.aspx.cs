﻿using IcebreakServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            loading_user_ico.Visible = false;

            if (!IsPostBack)
            {
                string usr = (string)Session["USER"];
                string lvl = Convert.ToString(Session["LEVEL"]);
                if (usr != null && lvl != null)
                {
                    int iLvl = Convert.ToUInt16(lvl);
                    if (iLvl >= 0)
                    {
                        welcome_msg.InnerText = "Welcome back " + Session["NAME"] + ".";
                        DBServerTools dbTools = new DBServerTools();
                        usr_events = dbTools.getEventsforUser(usr);
                        if (usr_events == null)
                            usr_events = new List<IcebreakServices.Event>();
                        if (usr_events.Count > 0)
                        {
                            dd_events.Items.Clear();
                            foreach (IcebreakServices.Event ev in usr_events)
                                dd_events.Items.Add(ev.Title);

                            dd_events.Items.Add("General");

                            int vpad = 0;//px
                            string graph_type = "line";//bar|line,etc. refer to Chart.js website.
                            int graph_w = 700;//px
                            int graph_h = 400;//px

                            //Get Icebreak count graph
                            string count_canvas_js = getUserIcebreakCountBetweenTimeGraph(usr,
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Number of Icebreaks for each mounth",
                                                                                dbTools);

                            string usr_ib_count_canvas_js = getUserIcebreakCountAtEventsGraph(usr,
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Number of Icebreaks at events",
                                                                                dbTools);
                            string usr_succ_ib_count_canvas_js = getUserSuccessfulIcebreakCountAtEventsGraph(usr,
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Total number of Successful Icebreaks at events",
                                                                                dbTools);
                            string usr_unsucc_ib_count_canvas_js = getUserUnsuccessfulIcebreakCountAtEventsGraph(usr,
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Total number of Unsuccessful Icebreakd Count st events",
                                                                                dbTools);
                            long id = dbTools.getUserEventId(usr);

                            IcebreakServices.Event curr_event = null;
                            if (id > 0)
                                curr_event = dbTools.getEvent(Convert.ToString(id));
                            string overview_html = "<div style='width:400px;height:auto;margin:auto;background-color:#ffff;border-radius:3px;border:1px solid #494949;'>"
                                                            + "<p>Number of total Icebreaks: " + dbTools.getUserIcebreakCount(usr) + "</p>"
                                                            + "<p>Number successful Icebreaks: " + dbTools.getUserSuccessfulIcebreakCount(usr) + "</p>";
                            if (curr_event != null)
                            {
                                overview_html += "<p>Number of Icebreaks at '" + curr_event.Title + "': " + dbTools.getUserIcebreakCountAtEvent(usr, id) + "</p>";
                                overview_html += "<p>Number of successful Icebreaks at '" + curr_event.Title + "': " + dbTools.getUserSuccessfulIcebreakCountAtEvent(usr, id) + "</p>";
                            }
                            overview_html += "</div>";

                            string canvas_html =
                                    "<div style='width:" + graph_w + "px;height:auto;margin-left:auto;margin-right:auto;margin-top:" + vpad + "px;'>"
                                    + "<h2 align='center'>Overview of your account.</h2>"
                                    + overview_html
                                    + "<canvas id=\"graph_ib_count_" + usr + "\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_usr_ib_count_event\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_usr_succ_ib_count_event\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_usr_unsucc_ib_count_event\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                + "</div>"
                                + count_canvas_js
                                + usr_ib_count_canvas_js
                                + usr_succ_ib_count_canvas_js
                                + usr_unsucc_ib_count_canvas_js
                                + "<script>window.location='./stats.aspx#tab_personal'</script>";

                            personal_canvas_container.InnerHtml = canvas_html;
                        }
                        else
                        {
                            dd_events.Items.Add("General");

                            int vpad = 0;//px
                            string graph_type = "line";//bar|line,etc. refer to Chart.js website.
                            int graph_w = 700;//px
                            int graph_h = 400;//px

                            //Get Icebreak count graph
                            string count_canvas_js = getUserIcebreakCountBetweenTimeGraph(usr,
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Number of Icebreaks over the year",
                                                                                dbTools);

                            long id = dbTools.getUserEventId(usr);

                            IcebreakServices.Event curr_event = null;
                            if (id > 0)
                                curr_event = dbTools.getEvent(Convert.ToString(id));
                            string overview_html = "<div style='width:500px;height:auto;margin:auto;background-color:#ffff;border-radius:3px;border:1px solid #494949;font-family:Lucida Sans Unicode'>"
                                                            + "<li><p style='margin-left:3%' >Total Icebreaks: " + dbTools.getUserIcebreakCount(usr) + "</p></li>"
                                                            + "<li><p style='margin-left:3%' > Successful Icebreaks: " + dbTools.getUserSuccessfulIcebreakCount(usr) + "</p></li>";
                            if (curr_event != null)
                            {
                                overview_html += "<li><p style='margin-left:3%' >Number of Icebreaks at '" + curr_event.Title + "': " + dbTools.getUserIcebreakCountAtEvent(usr, id) + "</p></li>";
                                overview_html += "<li><p style='margin-left:3%' >Number of successful Icebreaks at '" + curr_event.Title + "': " + dbTools.getUserSuccessfulIcebreakCountAtEvent(usr, id) + "</p></li>";
                            }
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
                                                                                "Number of Icebreaks users made",
                                                                                dbTools);

                            //Get successful Icebreak count graph
                            string scount_canvas_js = getSuccessfulUserIcebreakCountGraph(selected_event, 
                                                                                            graph_w, 
                                                                                            graph_h, 
                                                                                            vpad, 
                                                                                            graph_type,
                                                                                            "Number of succesful Icebreaks user made",
                                                                                            dbTools);

                            //Get unsuccessful Icebreak count graph
                            string unscount_canvas_js = getUnsuccessfulUserIcebreakCountGraph(selected_event,
                                                                                            graph_w,
                                                                                            graph_h,
                                                                                            vpad,
                                                                                            graph_type,
                                                                                            "Number of unsuccesful Icebreaks user made",
                                                                                            dbTools);

                            string age_count_canvas_js = getPopularUserAgeGroupGraph(selected_event,
                                                                                            graph_w,
                                                                                            graph_h,
                                                                                            vpad,
                                                                                            graph_type,
                                                                                            "Total user age group count",
                                                                                            dbTools);

                            int usr_img_size = 90;//px
                            List<User> users_at_event = dbTools.getUsersAtEvent(selected_event.Id);
                            string users_at_event_html = "<div style='width:auto;max-height:900px;y-overflow:scroll;'>";
                            foreach (User u in users_at_event)
                            {
                                string img_url = "./images/profile/" + u.Username + ".png";
                                string name = String.IsNullOrEmpty(u.Fname) ? "Anonymous" : u.Fname + " " + u.Lname;
                                users_at_event_html += "<div style='width:500px;height:100px;margin:auto;background-color:#ffff;border:1px solid #343434;'>"
                                                            + "<div style='border-radius:100px;width:" + usr_img_size + "px;height:"+ usr_img_size + "px;float:left;'>"
                                                                + "<img style='border-radius:100px;border:1px solid #343434;' src='" + img_url+"' height='"+ usr_img_size + "' width='"+ usr_img_size + "' alt='"+name+" profile image'/>"
                                                            +"</div>"
                                                            + "<h3 style='margin-top:30px;text-align:center;'>" + name+"</h3>"
                                                        +"</div>";
                            }
                            users_at_event_html += "</div>";

                            int NUM_GRAPHS = 2;
                            int GRAPH_VPOS_OFFSET = 10;//px;
                            //" + (graph_h*NUM_GRAPHS+GRAPH_VPOS_OFFSET) + "

                            string overview_html = "<div style='width:400px;height:auto;margin:auto;background-color:#ffff;border-radius:5px;border:1px solid #494949;'>"
                                                        + "<p>Number of users at " + selected_event.Title + ": " + users_at_event.Count + "</p>"
                                                        + "<p>Number of Icebreak count at " + selected_event.Title + ": " + dbTools.getEventIcebreakCountBetweenTime(selected_event.Id, selected_event.Date, selected_event.End_Date) + "</p>"
                                                        + "<p>Number of successful Icebreaks at " + selected_event.Title + ": " + dbTools.getEventSuccessfulIcebreakCountBetweenTime(selected_event.Id,selected_event.Date,selected_event.End_Date) + "</p>"
                                                        + "<p>Number of unsuccessful Icebreaks at " + selected_event.Title + ": " + dbTools.getEventUnsuccessfulIcebreakCountBetweenTime(selected_event.Id, selected_event.Date, selected_event.End_Date) + "</p>"
                                                    +"</div>";
                            string canvas_html =
                                 "<div style='width:" + graph_w + "px;height:auto;margin-left:auto;margin-right:auto;margin-top:" + vpad + "px;'>"
                                    + "<h2 align='center'>Overview of " + selected_event.Title + "</h2>"
                                    + overview_html
                                    + "<canvas id=\"graph_ib_count_" + selected_event.Id + "\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_succ_ib_count_" + selected_event.Id + "\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_unsucc_ib_count_" + selected_event.Id + "\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_usr_age_count_" + selected_event.Id + "\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<h2 align='center'>Users currently at "+selected_event.Title+"</h2>"
                                    + users_at_event_html
                                + "</div>"
                                + count_canvas_js
                                + scount_canvas_js
                                + unscount_canvas_js
                                + age_count_canvas_js
                                + "<script>window.location='./stats.aspx#tab_events'</script>";

                            canvas_container.InnerHtml = canvas_html;
                        }
                        else //Load general stats
                        {
                            //Properties
                            int vpad = 0;//px
                            string graph_type = "line";//bar|line,etc. refer to Chart.js website.
                            int graph_w = 700;//px
                            int graph_h = 400;//px

                            

                            //Get Icebreak count graph
                            string count_canvas_js = getAllEventsIcebreakCountGraph(
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Total Icebreaks",
                                                                                dbTools);

                            string succ_count_canvas_js = getAllEventsSuccessfulUserIcebreakCountGraph(
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Total successful Icebreaks",
                                                                                dbTools);

                            string unsucc_count_canvas_js = getAllEventsUnsuccessfulUserIcebreakCountGraph(
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Total unsuccessful Icebreaks",
                                                                                dbTools);

                            string usr_count_canvas_js = getAllEventsUserCountGraph(
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Total user count",
                                                                                dbTools);

                            string usr_count_hour_canvas_js = getAllEventsIcebreakCountLastHourGraph(
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Total Icebreak count in the last hour",
                                                                                dbTools);

                            string male_usr_count_canvas_js = getAllEventsMaleUserCountGraph(
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Total male user count",
                                                                                dbTools);

                            string female_usr_count_canvas_js = getAllEventsFemaleUserCountGraph(
                                                                                graph_w,
                                                                                graph_h,
                                                                                vpad,
                                                                                graph_type,
                                                                                "Total female user count",
                                                                                dbTools);
                            string canvas_html =
                                 "<div style='width:" + graph_w + "px;height:auto;margin-left:auto;margin-right:auto;margin-top:" + vpad + "px;'>"
                                    + "<canvas id=\"graph_usr_count_all\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_ib_count_all\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_succ_ib_count_all\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_unsucc_ib_count_all\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_ib_count_hour_all\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_male_usr_count_all\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                    + "<canvas id=\"graph_female_usr_count_all\" width='" + graph_w + "' height='" + graph_h + "'></canvas>"
                                + "</div>"
                                + count_canvas_js
                                + succ_count_canvas_js
                                + unsucc_count_canvas_js
                                + usr_count_canvas_js
                                + usr_count_hour_canvas_js
                                + male_usr_count_canvas_js
                                + female_usr_count_canvas_js
                                + "<script>window.location='./stats.aspx#tab_events'</script>";

                            canvas_container.InnerHtml = canvas_html;
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You have no events.');", true);
                            //return;
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

        public string getAllEventsIcebreakCountGraph(int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";

            List<IcebreakServices.Event> all_events = dbTools.getAllEvents();

            foreach (IcebreakServices.Event evnt in all_events)
            {
                int count = dbTools.getEventIcebreakCount(evnt.Id);
                y_axis += count + ",";
                x_axis += "'" + evnt.Title + "',";
            }
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_ib_count_all",
                                        "All Events",
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }
            else return "";
        }

        public string getAllEventsSuccessfulUserIcebreakCountGraph(int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";

            List<IcebreakServices.Event> all_events = dbTools.getAllEvents();

            foreach (IcebreakServices.Event evnt in all_events)
            {
                int count = dbTools.getEventSuccessfulIcebreakCountBetweenTime(evnt.Id, evnt.Date, evnt.End_Date);
                y_axis += count + ",";
                x_axis += "'" + evnt.Title + "',";
            }
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_succ_ib_count_all",
                                        "All Events",
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }
            else return "";
        }

        public string getAllEventsUnsuccessfulUserIcebreakCountGraph(int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";

            List<IcebreakServices.Event> all_events = dbTools.getAllEvents();

            foreach (IcebreakServices.Event evnt in all_events)
            {
                int count = dbTools.getEventUnsuccessfulIcebreakCountBetweenTime(evnt.Id, evnt.Date, evnt.End_Date);
                y_axis += count + ",";
                x_axis += "'" + evnt.Title + "',";
            }
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_unsucc_ib_count_all",
                                        "All Events",
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }
            else return "";
        }

        public string getAllEventsUserCountGraph(int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";

            List<IcebreakServices.Event> all_events = dbTools.getAllEvents();

            foreach (IcebreakServices.Event evnt in all_events)
            {
                List<User> usrs = dbTools.getUsersAtEvent(evnt.Id);
                int count = usrs.Count;
                y_axis += count + ",";
                x_axis += "'" + evnt.Title + "',";
            }
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_usr_count_all",
                                        "All Events",
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }
            else return "";
        }

        public string getAllEventsIcebreakCountLastHourGraph(int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";

            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            long since_epoch = (long)t.TotalSeconds;

            List<IcebreakServices.Event> all_events = dbTools.getAllEvents();

            foreach (IcebreakServices.Event evnt in all_events)
            {
                int count = dbTools.getEventIcebreakCountBetweenTime(evnt.Id, since_epoch, (long)(since_epoch + (1 * 60 * 60)));
                y_axis += count + ",";
                x_axis += "'" + evnt.Title + "',";
            }
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_ib_count_hour_all",
                                        "All Events",
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }
            else return "";
        }

        public string getAllEventsMaleUserCountGraph(int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";

            List<IcebreakServices.Event> all_events = dbTools.getAllEvents();

            foreach (IcebreakServices.Event evnt in all_events)
            {
                List<User> usrs = dbTools.getUsersAtEvent(evnt.Id);
                int count = 0;
                foreach (User usr in usrs)
                    if (usr.Gender.ToLower().Equals("male"))
                        count++;
                y_axis += count + ",";
                x_axis += "'" + evnt.Title + "',";
            }
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_male_usr_count_all",
                                        "All Events",
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }
            else return "";
        }

        public string getAllEventsFemaleUserCountGraph(int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";

            List<IcebreakServices.Event> all_events = dbTools.getAllEvents();

            foreach (IcebreakServices.Event evnt in all_events)
            {
                List<User> usrs = dbTools.getUsersAtEvent(evnt.Id);
                int count = 0;
                foreach (User usr in usrs)
                    if (usr.Gender.ToLower().Equals("female"))
                        count++;
                y_axis += count + ",";
                x_axis += "'" + evnt.Title + "',";
            }
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_female_usr_count_all",
                                        "All Events",
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }
            else return "";
        }

        public string getPopularUserAgeGroupGraph(IcebreakServices.Event selected_event, int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";

            List<User> usrs = dbTools.getUsersAtEvent(selected_event.Id);
            List<KeyValuePair<int, int>> age_to_count = new List<KeyValuePair<int, int>>();
            foreach (User usr in usrs)
            {
                int count = 0;
                bool found = false;
                //check if user age has been counted already
                foreach (KeyValuePair<int,int> kv in age_to_count)
                {
                    if (kv.Key == usr.Age)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    //It's not in the list
                    foreach (User u in usrs)
                    {
                        if (usr.Age == u.Age)
                            count++;
                    }
                    //Add to list
                    age_to_count.Add(new KeyValuePair<int, int>(usr.Age, count));
                    y_axis += count + ",";
                    x_axis += "'" + usr.Age + "',";
                }
            }
            if (x_axis.Length > 0 && y_axis.Length>0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_usr_age_count_" + Convert.ToString(selected_event.Id),
                                        selected_event.Title,
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }else return "";
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
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
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
            }else return "";
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
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
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
            }else return "";
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
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
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
            }else return "";
        }

        public string getUserIcebreakCountAtEventsGraph(string username, int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";

            List<IcebreakServices.Event> evts = dbTools.getUserEventHistory(username);
            foreach (IcebreakServices.Event evt in evts)
            {
                int count = dbTools.getUserIcebreakCountAtEvent(username, evt.Id);
                y_axis += count + ",";
                x_axis += "'" + evt.Title + "',";
            }

            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_usr_ib_count_event",
                                        (string)Session["NAME"] + " Icebreak event history",
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }else return "";
        }

        public string getUserSuccessfulIcebreakCountAtEventsGraph(string username, int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";

            List<IcebreakServices.Event> evts = dbTools.getUserEventHistory(username);
            foreach (IcebreakServices.Event evt in evts)
            {
                int count = dbTools.getUserSuccessfulIcebreakCountAtEvent(username, evt.Id);
                y_axis += count + ",";
                x_axis += "'" + evt.Title + "',";
            }
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_usr_succ_ib_count_event",
                                        (string)Session["NAME"] + " Icebreak event history",
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }else return "";
        }

        public string getUserUnsuccessfulIcebreakCountAtEventsGraph(string username, int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";

            List<IcebreakServices.Event> evts = dbTools.getUserEventHistory(username);
            foreach (IcebreakServices.Event evt in evts)
            {
                int ib_count = dbTools.getUserIcebreakCountAtEvent(username, evt.Id);
                int succ_ib_count = dbTools.getUserSuccessfulIcebreakCountAtEvent(username, evt.Id);
                int count = ib_count - succ_ib_count;
                y_axis += count + ",";
                x_axis += "'" + evt.Title + "',";
            }
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_usr_unsucc_ib_count_event",
                                        (string)Session["NAME"] + " Icebreak event history",
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }else return "";
        }

        public string getUserIcebreakCountBetweenTimeGraph(string username, int graph_w, int graph_h, int vpad, string graph_type, string label, DBServerTools dbTools)
        {
            string x_axis = "";
            string y_axis = "";
            long start_date = (long)(new DateTime(DateTime.Now.Year,1,1) - new DateTime(1970, 1, 1)).TotalSeconds;
            //long end_date = (long)(new DateTime(DateTime.Now.Year, 12, 31) - new DateTime(1970, 1, 1)).TotalSeconds;

            int[] days_int_months;
            if (DateTime.IsLeapYear(DateTime.Now.Year))
                days_int_months = new int[]{ 30, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};//jan is 30 because the 'days' var starts at 1
            else days_int_months = new int[] { 30, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            string[] months = {"Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec" };

            //container.InnerHtml += "[start:" + start_date + "],[end:" + end_date + "]; interval="+interval +"<hr/>";
            int days = 1;
            for (long m = 0; m<12; m++)
            {
                long prev_seconds = 60 * 60 * 24 * days;//seconds up to end of prev month
                days += days_int_months[m];
                long seconds = 60 * 60 * 24 * days;//seconds up to end of current month

                int count = dbTools.getUserIcebreakCountBetweenTime(username, start_date+ prev_seconds, start_date + seconds);
                y_axis += count + ",";
                x_axis += "'" + months[m] + "',";//FromUnixTime(start_date + prev_seconds)
            }
            if (x_axis.Length > 0 && y_axis.Length > 0)
            {
                x_axis = x_axis.Substring(0, x_axis.Length - 1); //Remove last comma
                y_axis = y_axis.Substring(0, y_axis.Length - 1); //Remove last comma

                string jsGraph =
                      "<script>"
                        + getChartJS("graph_ib_count_" + username,
                                        (string)Session["NAME"]+" Icebreak History",
                                        label,
                                        graph_type,
                                        x_axis,
                                        y_axis)
                    + "</script>";

                return jsGraph;
            }else return "";
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
                                                            + "'rgba(70 , 189, 240, 0.5)',"
                                                            + "'rgba(70 , 189, 240, 0.5)',"
                                                            + "'rgba(70 , 189, 240, 0.5)',"
                                                            + "'rgba(70 , 189, 240, 0.5)',"
                                                            + "'rgba(70 , 189, 240, 0.5)',"
                                                            + "'rgba(70 , 189, 240, 0.5)'"
                                                        + "],"
                                                        + "borderColor: ["
                                                            + "'rgba(70 , 189, 240, 1)',"
                                                            + "'rgba(70 , 189, 240, 1)',"
                                                            + "'rgba(70 , 189, 240, 1)',"
                                                            + "'rgba(70 , 189, 240, 1)',"
                                                            + "'rgba(70 , 189, 240, 1)',"
                                                            + "'rgba(70 , 189, 240, 1)'"
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

        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        protected void btnSearchUser_Click(object sender, EventArgs e)
        {
            loading_user_ico.Visible = true;
            var usr = search_box.Text;
            if (!String.IsNullOrEmpty(usr))
            {
                string users_html = "<div>";
                DBServerTools dbTools = new DBServerTools();
                List<User> users = dbTools.searchUser(usr);
                if (users == null)
                {
                    redeem_container.InnerHtml = "<h2 style='text-align:center;'>No users found</h2>";
                    loading_user_ico.Visible = false;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not authorized to view this page.');", true);
                    return;
                }
                if (users.Count <= 0)
                {
                    redeem_container.InnerHtml = "<h2 style='text-align:center;'>No users found</h2>";
                    loading_user_ico.Visible = false;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not authorized to view this page.');", true);
                    return;
                }

                foreach (User u in users)
                {
                    string img_url = "http://icebreak.azurewebsites.net/images/profile/" + u.Username + ".png";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(img_url);
                    request.Method = WebRequestMethods.Http.Head;
                    bool imgExists = true;
                    
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        imgExists = response.StatusCode == HttpStatusCode.OK;
                    }
                    catch (WebException ex)
                    {
                        //404 error - dbs.addError(ErrorCodes.EEVENT, ex.Message, "ViewEvent.aspx[Page_Load]");
                        imgExists = false;
                    }

                    if (!imgExists)
                    {
                        img_url = "http://icebreak.azurewebsites.net/images/profile/default.png";
                    }
                    
                    string name;
                    if (String.IsNullOrEmpty(u.Fname))
                        name = "Anonymous";
                    else name = u.Fname + " " + u.Lname;

                    if (u.Fname.ToLower().Equals("x"))
                        name = "Anonymous";
                    else name = u.Fname + " " + u.Lname;
                    int usr_img_size = 135;//px;

                    long id = dbTools.getUserEventId(u.Username);
                    u.Event_id = id;
                    List<Reward> user_rewards = dbTools.getUserRewardsAtEvent(u.Username, Convert.ToString(u.Event_id));

                    users_html += "<div style='width:600px;height:140px;margin:auto;background-color:#ffff;border:1px solid #343434;border-radius:3px;'>"
                                    + "<div style='border-radius:" + usr_img_size + "px;width:" + usr_img_size + "px;height:" + usr_img_size + "px;float:left;'>"
                                        + "<img style='border-radius:" + usr_img_size + "px;border:1px solid #343434;' src='" + img_url + "' height='" + usr_img_size + "' width='" + usr_img_size + "' alt='" + name + " profile image'/>"
                                    + "</div>"
                                    + "<h3 style='margin-top:20px;text-align:center;'>" + name + "</h3>"
                                    + "<select id='dd_usr_"+u.Username+ "' style='margin-left:15px;margin-right:5px;'>";
                    foreach (Reward r in user_rewards)
                        users_html += "<option value='"+r.Id+"'>"+r.Name+"</option>";
                    users_html += "</select>"
                                    + "<input type='text' id='usr_"+u.Username+ "_redeem_code' value='' style='margin-top:5px;margin-left:auto;margin-right:auto;'/>"
                                    + "<button style='margin-top:5px;margin-left:20px;' id='redeem_usr_" + u.Username + "'>Redeem</button>"
                                + "</div>";
                    users_html += getUserRedeemHandler(u.Username, Convert.ToString(u.Event_id));
                }
                users_html += "</div>";
                users_html += "<script>window.location='./stats.aspx#tab_redeem'</script>";

                loading_user_ico.Visible = false;
                redeem_container.InnerHtml = users_html;

            }
            else
            {
                redeem_container.InnerHtml = "<h2 style='text-align:center;'>No users found</h2>";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not authorized to view this page.');", true);
                return;
            }
        }

        /*public string getUserRedeemHandler(string username, string ev_id)
        {
            string script = 
                "<script>"
                + "$('#redeem_usr_" + username + "').click(function()"
                + "{"
                    + "var code=document.getElementById('usr_" + username + "_redeem_code').value;"
                    + "var rews=document.getElementById('dd_usr_" + username + "');"
                    + "var rew_id = rews.options[rews.selectedIndex].value;"
                    + "$.ajax("
                    +"{"
                    + "url: 'http://icebreak.azurewebsites.net/IBUserRequestService.svc/redeemReward/" + username + "/'+rew_id+'/" + ev_id + "/1234/" + DBServerTools.RW_CLAIMED + "',"
                    //+ "url: 'http://icebreak.azurewebsites.net/IBUserRequestService.svc/redeemReward/" + username + "/'+rew_id+'/" + ev_id + "/'+code+'/" + DBServerTools.RW_CLAIMED + "',"
                    //+ "url: 'http://icebreak.azurewebsites.net/IBUserRequestService.svc/redeemReward/" + username + "/7/39/1234/" + DBServerTools.RW_CLAIMED + "',"
                    + "async: true,"
                    + "success: function(result){alert(result);}"
                    //+ "error: function(xhr){alert('An error occured, please try again. ['+xhr.statusText+']');}"
                    + "});"
                + "});"
                +"</script>";
            return script;
        }*/

        public string getUserRedeemHandler(string username, string ev_id)
        {
            string script = 
                "<script>"
                    + "$('#redeem_usr_" + username + "').click(function()"
                    + "{"
                        + "var code=document.getElementById('usr_" + username + "_redeem_code').value;"
                        + "var rews=document.getElementById('dd_usr_" + username + "');"
                        + "var rew_id = rews.options[rews.selectedIndex].value;"
                        + "$.ajax({"
                            + "type: 'GET',"
                            + "url: 'http://icebreak.azurewebsites.net/IBUserRequestService.svc/redeemReward/" + username + "/'+rew_id+'/" + ev_id + "/'+code+'/" + DBServerTools.RW_CLAIMED + "',"
                            //+ "contentType: 'text/plain; charset=utf-8',"
                            //+ "dataType: 'json',"
                            + "async: false"
                    + "}).done(function(result){alert(result);});"
                    /*+ "$.get('http://icebreak.azurewebsites.net/IBUserRequestService.svc/redeemReward/" + username + "/'+rew_id+'/" + ev_id + "/'+code+'/" + DBServerTools.RW_CLAIMED + "',"
                        +"function(data, status)"
                        +"{"
                            +"alert(data);"
                        +"});"*/
                    + "});"
                +"</script>";
            return script;
        }
    }
}