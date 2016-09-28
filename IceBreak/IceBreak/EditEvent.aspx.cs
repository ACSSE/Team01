using IcebreakServices;
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
    public partial class EditEvent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LEVEL"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You must login');window.location ='index.aspx';", true);
            }
            else
            {
                int check = (int)Session["LEVEL"];
                if (check != 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You do not have access to this page');window.location ='index.aspx';", true);
                }
            }
            string eventid = Request.QueryString["evntid"];

            DBServerTools dbs = new DBServerTools();

            IcebreakServices.Event evnt = dbs.getEvent(eventid);

            eventname.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            eventdescrip.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            date.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            event_end_date.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            time.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            end_time.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            meeting_place_1.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            meeting_place_2.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            meeting_place_3.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            meeting_place_4.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            meeting_place_5.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

            eventname.Value = evnt.Title;
            eventaddress.Value = evnt.Address;
            gps.Value = evnt.Gps_location;
            eventdescrip.Value = evnt.Description;
            DateTime startdate = FromUnixTime(evnt.Date);
            DateTime enddate = FromUnixTime(evnt.End_Date);
            date.Value = String.Format("{0:yyyy-MM-dd}", startdate);
            event_end_date.Value = String.Format("{0:yyyy-MM-dd}", enddate);

            time.Value = String.Format("{0:HH:mm}", startdate);
            end_time.Value = String.Format("{0:HH:mm}", enddate);


            meeting_place_1.Style.Add("display", "none");
            meeting_place_2.Style.Add("display", "none");
            meeting_place_3.Style.Add("display", "none");
            meeting_place_4.Style.Add("display", "none");
            meeting_place_5.Style.Add("display", "none");

            if (!IsPostBack)
            {
                string[] mparray = evnt.Meeting_Places.Split(';');
                int num = mparray.Length - 1;
                NumEvents.SelectedIndex = num;
                switch (num)
                {
                    case 1:
                        meeting_place_1.Style.Add("display", "normal");
                        meeting_place_1.Value = mparray[0];
                        break;
                    case 2:
                        meeting_place_1.Style.Add("display", "normal");
                        meeting_place_2.Style.Add("display", "normal");

                        meeting_place_1.Value = mparray[0];
                        meeting_place_2.Value = mparray[1];
                        break;
                    case 3:
                        meeting_place_1.Style.Add("display", "normal");
                        meeting_place_2.Style.Add("display", "normal");
                        meeting_place_3.Style.Add("display", "normal");

                        meeting_place_1.Value = mparray[0];
                        meeting_place_2.Value = mparray[1];
                        meeting_place_3.Value = mparray[2];
                        break;
                    case 4:
                        meeting_place_1.Style.Add("display", "normal");
                        meeting_place_2.Style.Add("display", "normal");
                        meeting_place_3.Style.Add("display", "normal");
                        meeting_place_4.Style.Add("display", "normal");
                        meeting_place_1.Value = mparray[0];
                        meeting_place_2.Value = mparray[1];
                        meeting_place_3.Value = mparray[2];
                        meeting_place_4.Value = mparray[3];
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

                        meeting_place_1.Value = mparray[0];
                        meeting_place_2.Value = mparray[1];
                        meeting_place_3.Value = mparray[2];
                        meeting_place_1.Value = mparray[3];
                        meeting_place_1.Value = mparray[4];
                        break;
                }

            }
            else
            {
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

        }
        protected void btnUpdate_Event(object sender, EventArgs e)
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
            string mp1 = meeting_place_1.Value;
            string mp2 = meeting_place_2.Value;
            string mp3 = meeting_place_3.Value;
            string mp4 = meeting_place_4.Value;
            string mp5 = meeting_place_5.Value;
            string[] mpArray = { mp1, mp2, mp3, mp4, mp5 };

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
            else
            {
                gps_span.Style.Add("display", "none");
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
            int num = int.Parse(NumEvents.SelectedValue);
            string meetingplace = " ";
            for (int i = 0; i < num; i++)
            {
                if ((String.IsNullOrEmpty(mpArray[i])))
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
            //string filename = Path.GetFileName(FileUpload.FileName);
            //if (String.IsNullOrEmpty(filename))
            //{
            //    upload.InnerText = "Photo not chosen. Recommended size 700x300";
            //    upload.Style.Add("display", "normal");
            //    //   return;
            //}

            
            
            string str_start_date = EventDate + " " + EventTime;
            ulong start_date = (ulong)(DateTime.ParseExact(str_start_date, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) - new DateTime(1970, 1, 1)).TotalSeconds;

            string str_end_date = EventEndDate + " " + EventEndTime;
            ulong end_date = (ulong)(DateTime.ParseExact(str_end_date, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) - new DateTime(1970, 1, 1)).TotalSeconds;

           
            ulong now = (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            if (start_date <= 0 || end_date <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Start date and/or end date is invalid. Date must be in the future.');window.location ='YourEvents.aspx';", true);
                return;
            }
            if (start_date <= now || end_date <= now)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Start date and/or end date is invalid. Date must be in the future.');window.location ='YourEvents.aspx';", true);
                return;
            }
            if (start_date > end_date)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Start date and/or end date is invalid. End date must be after start date.');window.location ='YourEvents.aspx';", true);
                return;
            }

            string eventid = Request.QueryString["evntid"];

            DBServerTools dbs = new DBServerTools();

            IcebreakServices.Event evnt = dbs.getEvent(eventid);
            evnt.Title = EventName;
            evnt.Address = EventAddress;
            evnt.Gps_location = EventGps;
            evnt.Date = Convert.ToUInt32(start_date);
            evnt.Description = EventDescrip;
            evnt.End_Date = Convert.ToUInt32(end_date);
            evnt.Meeting_Places = meetingplace;
            
            string check = dbs.updateEvent(evnt, lvl);

            if (check.ToLower().Contains("success"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "window.location ='index.aspx';", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Event editing unsuccessful. Try again.');", true);
            }
        }
        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }

    }
}