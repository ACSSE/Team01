using IcebreakServices;
using System;
using System.Collections.Generic;
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
            eventname.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
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
            string EventName = eventname.Value;
            string EventAddress = eventaddress.Value;
            string EventDescrip = eventdescrip.Value;
            string EventTime = time.Value;
            string EventEndTime = end_time.Value;
            string EventDate = date.Value;
            string EventGps = gps.Value;
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
            string filename = Path.GetFileName(FileUpload.FileName);
            if (String.IsNullOrEmpty(filename))
            {
                upload.InnerText = "Photo not chosen. Recommended size 700x300";
                upload.Style.Add("display", "normal");
             //   return;
            }
                   
            IcebreakServices.Event evnt = new IcebreakServices.Event();
            evnt.Title = EventName;
            evnt.Address = EventAddress;
            evnt.Gps_location = EventGps;
            evnt.Description = EventDescrip;
            evnt.Date = EventDate;
            evnt.Time = EventTime;
            evnt.EndTime = EventEndTime;
            evnt.Meeting_Places = meetingplace;

            DBServerTools dbs = new DBServerTools();
            string check = dbs.addEvent(evnt);
           
            if(check.Contains("Success"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You have successfully added an event');window.location ='Event.aspx';", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('You are not sucessful try again');", true);
            }
            
        }
      
     }
}