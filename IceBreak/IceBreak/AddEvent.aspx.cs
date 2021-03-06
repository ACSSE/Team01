﻿using IcebreakServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
            if (Session["LEVEL"] == null || Session["USER"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You must login');window.location ='index.aspx';", true);
                return;
            }
            else
            {
                int lvl = (int)Session["LEVEL"];
                if (lvl < DBServerTools.CAN_EDIT_EVENTS)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You do not have access to this page');window.location ='index.aspx';", true);
                    return;
                }
            }
            eventname.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            eventdescrip.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            time.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            date.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            event_end_date.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            end_time.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            rewardname.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            rewarddescrip.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            meeting_place_1.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            meeting_place_2.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            meeting_place_3.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            meeting_place_4.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            meeting_place_5.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

            meeting_place_1.Style.Add("display", "none");
            meeting_place_2.Style.Add("display", "none");
            meeting_place_3.Style.Add("display", "none");
            meeting_place_4.Style.Add("display", "none");
            meeting_place_5.Style.Add("display", "none");

            if (NumEvents.SelectedIndex > 0)
            {
                int num = int.Parse(NumEvents.SelectedValue);
                switch (num)
                {
                    case 1:
                        meeting_place_1.Style.Add("display", "normal");
                    break;
                    case 2:
                        meeting_place_1.Style.Add("display", "normal");
                        meeting_place_2.Style.Add("display", "normal");
                        break;
                    case 3:
                        meeting_place_1.Style.Add("display", "normal");
                        meeting_place_2.Style.Add("display", "normal");
                        meeting_place_3.Style.Add("display", "normal");
                        break;
                    case 4:
                        meeting_place_1.Style.Add("display", "normal");
                        meeting_place_2.Style.Add("display", "normal");
                        meeting_place_3.Style.Add("display", "normal");
                        meeting_place_4.Style.Add("display", "normal");
                        break;
                    case 5:
                        meeting_place_1.Style.Add("display", "normal");
                        meeting_place_2.Style.Add("display", "normal");
                        meeting_place_3.Style.Add("display", "normal");
                        meeting_place_4.Style.Add("display", "normal");
                        meeting_place_5.Style.Add("display", "normal");
                        break;
                    default:
                        meeting_place_1.Style.Add("display", "none");
                        meeting_place_2.Style.Add("display", "none");
                        meeting_place_3.Style.Add("display", "none");
                        meeting_place_4.Style.Add("display", "none");
                        meeting_place_5.Style.Add("display", "none");
                        break;
                }

            }

        }
        protected void btnAdd_Event(object sender, EventArgs e)
        {
           
            int lvl = (int)Session["LEVEL"];
            if (lvl < DBServerTools.CAN_EDIT_EVENTS)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You do not have the necessary permissions to view this page.');window.location ='index.aspx';", true);
                return;
            }

            string EventName = eventname.Value;
            string EventAddress = eventaddress.Value;
            string EventDescrip = eventdescrip.Value;
            string EventTime = time.Value;
            string EventEndTime = end_time.Value;
            string EventEndDate = event_end_date.Value;
            string EventDate = date.Value;
            string EventGps = gps.Value;
            string EventRadius = txRadius.Value;
            string RewardName = rewardname.Value;
            string RewardDescrip = rewarddescrip.Value;
            string mp1 = meeting_place_1.Value;
            string mp2 = meeting_place_2.Value;
            string mp3 = meeting_place_3.Value;
            string mp4 = meeting_place_4.Value;
            string mp5 = meeting_place_5.Value;
            string[] mpArray = {mp1,mp2,mp3,mp4,mp5};

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
            if (String.IsNullOrEmpty(EventGps))
            {
                gps_span.Style.Add("display", "normal");
                return;
            }
            if (String.IsNullOrEmpty(EventRadius))
            {
                radius_span.Style.Add("display", "normal");
                return;
            }
            else
            {
                radius_span.Style.Add("display", "none");
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
            if (String.IsNullOrEmpty(EventEndDate))
            {
                end_date_span.Style.Add("display", "normal");
                return;
            }
            else
            {
                end_date_span.Style.Add("display", "none");
            }
            if (String.IsNullOrEmpty(EventEndTime))
            {
                end_time_span.Style.Add("display", "normal");
                return;
            }
            else
            {
                end_time_span.Style.Add("display", "none");
            }
            if (int.Parse(NumEvents.SelectedValue) < 0)
            {
                meeting_span.Style.Add("display", "normal");
                return;
            }
            else
            {
                meeting_span.Style.Add("display", "none");
            }
            if (String.IsNullOrEmpty(RewardName))
            {
               reward_name_span.Style.Add("display", "normal");
                return;
            }
            else
            {
                reward_name_span.Style.Add("display", "none");
            }
            if (String.IsNullOrEmpty(cost.Value))
            {
                cost_span.Style.Add("display", "normal");
                return;
            }
            else
            {
                int cst;
                if(int.TryParse(cost.Value,out cst))
                    cost_span.Style.Add("display", "none");
                else cost_span.Style.Add("display", "normal");
            }
            if (String.IsNullOrEmpty(RewardDescrip))
            {
                rdescrip_span.Style.Add("display", "normal");
                return;
            }
            else
            {
                rdescrip_span.Style.Add("display", "none");
            }
            //Validate number of meeting places
            int num=0;
            if(!int.TryParse(NumEvents.SelectedValue, out num))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid number of meeting places');", true);
                meeting_span.Style.Add("display", "normal");
                return;
            }
            if (num <= 0)
                return;
            //Validate event radius
            double radius = 0.0;
            if (!double.TryParse(EventRadius, out radius))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid event radius.');", true);
                radius_span.Style.Add("display", "normal");
                return;
            }
            if (radius <= 0)
                return;

            string meetingplace = " ";
            for(int i =0;i<num;i++)
            {
                if((String.IsNullOrEmpty(mpArray[i])))
                {
                    meeting_span.Style.Add("display", "normal");
                    return;
                }
                else
                {
                    meeting_span.Style.Add("display", "none");
                    meetingplace += mpArray[i] + ";";
                }
            }
            
            string str_start_date = EventDate + " " + EventTime;
            if (int.Parse(EventTime.Split(':')[0]) >= 12)
                str_start_date += ":01 PM";
            else str_start_date += ":01 AM";

            ulong start_date = (ulong)(DateTime.ParseExact(str_start_date, "yyyy-MM-dd HH:mm:ss tt", CultureInfo.CurrentCulture) - new DateTime(1970, 1, 1)).TotalSeconds;

            string str_end_date = EventEndDate + " " + EventEndTime;
            if (int.Parse(EventEndTime.Split(':')[0]) >= 12)
                str_end_date += ":01 PM";
            else str_end_date += ":01 AM";
            ulong end_date = (ulong)(DateTime.ParseExact(str_end_date, "yyyy-MM-dd HH:mm:ss tt", CultureInfo.CurrentCulture) - new DateTime(1970, 1, 1)).TotalSeconds;

            ulong now = (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

            if (start_date <= 0 || end_date<=0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Start date and/or end date is invalid. Date must be in the future.');window.location ='AddEvent.aspx';", true);
                return;
            }
            if (start_date <= now || end_date <= now)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Start date and/or end date is invalid. Date must be in the future.');window.location ='AddEvent.aspx';", true);
                return;
            }
            if (start_date > end_date)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Start date and/or end date is invalid. End date must be after start date.');window.location ='AddEvent.aspx';", true);
                return;
            }
            //Get passcode
            int pcode = DBServerTools.getRandomNumber(9999);
            pcode = pcode >= 1000 ? pcode : pcode + 1000;//make sure passcode is always >= 4 digits

            IcebreakServices.Event evnt = new IcebreakServices.Event();
            evnt.Title = EventName;
            evnt.Address = EventAddress;

            EventGps = EventGps.Replace(" ", "");
            string loc=EventGps;

            if (loc.Length > 0)
            {
                if (loc.ElementAt(0).Equals('('))
                    loc = loc.Substring(1);
                if (loc.ElementAt(loc.Length - 1).Equals(')'))
                    loc = loc.Substring(0, loc.Length - 1);

                if (!loc.Contains(";"))//If there's not already and array of coordinates
                {
                    //double radius = 0.003555;
                    double lat = double.Parse(loc.Split(',')[0]);
                    double lng = double.Parse(loc.Split(',')[1]);

                    double[] top_left = { lat - radius, lng - radius };
                    double[] top_right = { lat - radius, lng + radius };
                    double[] bottom_left = { lat + radius, lng - radius };
                    double[] bottom_right = { lat + radius, lng + radius };

                    loc = top_left[0] + "," + top_left[1] + ";" + top_right[0] + "," + top_right[1] + ";" + bottom_right[0] + "," + bottom_right[1] + ";" + bottom_left[0] + "," + bottom_left[1] + ";" + top_left[0] + "," + top_left[1];
                }
            }
            else return;//Invalid GPS coordinates

            evnt.Gps_location = loc;
            evnt.Description = EventDescrip;
            evnt.Date = Convert.ToUInt32(start_date);
            evnt.AccessCode = pcode;
            evnt.End_Date = Convert.ToUInt32(end_date);
            evnt.Meeting_Places = meetingplace;
            evnt.Manager = (string)Session["USER"];

            DBServerTools dbs = new DBServerTools();
            string check = dbs.addEvent(evnt,lvl);
                        
            if (check.ToLower().Contains("success"))
            {
                IcebreakServices.Event lastevent = dbs.getLastEvent();

                if (lastevent != null)
                {
                    string filename = Path.GetFileName(FileUpload.FileName);
                    if (!String.IsNullOrEmpty(filename))
                    {
                        string event_icon_title = "event_icons-" + lastevent.Id;
                        byte[] file_bytes = FileUpload.FileBytes;
                        string response = dbs.imageUpload("events;" + event_icon_title + ".png", file_bytes);
                    }

                    Reward rwd = new Reward();
                    rwd.Name = RewardName;
                    rwd.Description = RewardDescrip;
                    rwd.Owner = (string)Session["USER"];
                    rwd.Event_ID = (int)lastevent.Id;
                    rwd.Value = int.Parse((string)cost.Value);

                    string check1 = dbs.addReward(rwd, lvl);

                    if (check1.ToLower().Contains("success"))
                    {
                        
                    }
                    else
                    {
                        Console.WriteLine(check);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(\"Event creation unsuccessful: " + check + "\");", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(\"Event creation unsuccessful: " + check + "\");", true);
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(\"Event creation unsuccessful: " + check + "\");", true);
                    Console.WriteLine(check);
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('File size: " + FileUpload.FileBytes.Length + "');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "window.location ='Event.aspx';", true);
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(\"Event creation unsuccessful: "+check+"\");", true);
            }
            
        }
      
     }
}