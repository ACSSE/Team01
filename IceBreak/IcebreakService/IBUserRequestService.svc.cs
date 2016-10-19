using System;
using System.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.Net;
using System.Web;
using System.ServiceModel.Activation;
using System.IO;
using System.Web.Hosting;

namespace IcebreakServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TestService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TestService.svc or TestService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class IBUserRequestService : IIBUserRequestService
    {
        private DBServerTools db;
        private static double _eQuatorialEarthRadius = 6378.1370D;
        private static double _d2r = (Math.PI / 180D);

        public IBUserRequestService()
        {
            db = new DBServerTools();
        }

        public string addEvent(Stream streamdata)
        {
            Event ev = new Event();
            StreamReader reader = new StreamReader(streamdata);
            string inbound_payload = reader.ReadToEnd();
            string response = "";
            reader.Close();
            reader.Dispose();

            inbound_payload = HttpContext.Current.Server.UrlDecode(inbound_payload);
            string[] usr_details = inbound_payload.Split('&');
            if (usr_details.Length == 5)
            {
                foreach (string kv_pair in usr_details)
                {
                    if (kv_pair.Contains('='))
                    {
                        string var = kv_pair.Split('=')[0];
                        string val = kv_pair.Split('=')[1];

                        switch (var.ToLower())
                        {
                            case "access_code":
                                int code;
                                if (int.TryParse(val, out code))
                                {
                                    ev.AccessCode = int.Parse(val);
                                    break;
                                }
                                else
                                {
                                    response = "Error: Cannot convert " + val + " to integer.";
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                                    return response;
                                }
                            case "date":
                                long date;
                                if (long.TryParse(val, out date))
                                {
                                    ev.Date = long.Parse(val);
                                    break;
                                }
                                else
                                {
                                    response = "Error: Cannot convert " + val + " to long.";
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                                    return response;
                                }
                            case "end_date":
                                long end_date;
                                if (long.TryParse(val, out end_date))
                                {
                                    ev.End_Date = long.Parse(val);
                                    break;
                                }
                                else
                                {
                                    response = "Error: Cannot convert " + val + " to long.";
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                                    return response;
                                }
                            case "meeting_places":
                                ev.Meeting_Places = val;
                                break;
                            case "title":
                                ev.Title = val;
                                break;
                            case "description":
                                ev.Description = val;
                                break;
                            case "address":
                                ev.Address = val;
                                break;
                            case "gps":
                                ev.Gps_location = val;
                                break;
                            /*case "radius":
                                int radius;
                                if (int.TryParse(val, out radius))
                                {
                                    ev.Radius = int.Parse(val);
                                    break;
                                }
                                else
                                {
                                    response = "Error: Cannot convert " + val + " to integer.";
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                                    return response;
                                }*/
                            case "manager":
                                ev.Manager = val;
                                break;
                        }
                    }
                    else
                    {
                        response = "Error: Broken key-value pair";
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                        break;
                    }
                }
                //Attempt to add Event to DB
                if (!String.IsNullOrEmpty(ev.Manager))
                {
                    User mgr = db.getUser(ev.Manager);
                    string exec_res = db.addEvent(ev,mgr.Access_level);

                    if (exec_res.ToLower().Contains("success"))
                    {
                        List<Event> events = db.getAllEvents();
                        string eventId = "";
                        foreach (Event e in events)
                        {
                            //if(e.Title.Equals(ev.Title) && e.Gps_location.Equals(ev.Gps_location) && e.Radius==ev.Radius)
                            if (e.isEqualTo(ev))
                            {
                                eventId = Convert.ToString(e.Id);
                            }
                        }

                        response = exec_res;
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("event_id", eventId);
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = "Query Execution Error: " + exec_res;
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                    }
                }
            }
            else
            {
                response = "Error: Invalid token count";
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
            }
            return response;
        }

        public string updateEvent(Stream streamdata)
        {
            Event ev = new Event();
            StreamReader reader = new StreamReader(streamdata);
            string inbound_payload = reader.ReadToEnd();
            string response = "";
            reader.Close();
            reader.Dispose();

            inbound_payload = HttpContext.Current.Server.UrlDecode(inbound_payload);
            string[] usr_details = inbound_payload.Split('&');
            if (usr_details.Length == 5)
            {
                foreach (string kv_pair in usr_details)
                {
                    if (kv_pair.Contains('='))
                    {
                        string var = kv_pair.Split('=')[0];
                        string val = kv_pair.Split('=')[1];

                        switch (var.ToLower())
                        {
                            case "id":
                                long id;
                                if (long.TryParse(val, out id))
                                {
                                    ev.Id = long.Parse(val);
                                    break;
                                }
                                else
                                {
                                    response = "Error: Cannot convert " + val + " to UInt16.";
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                                    return response;
                                }
                            case "access_code":
                                int code;
                                if (int.TryParse(val, out code))
                                {
                                    ev.AccessCode = int.Parse(val);
                                    break;
                                }
                                else
                                {
                                    response = "Error: Cannot convert " + val + " to integer.";
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                                    return response;
                                }
                            case "date":
                                long date;
                                if (long.TryParse(val, out date))
                                {
                                    ev.Date = long.Parse(val);
                                    break;
                                }
                                else
                                {
                                    response = "Error: Cannot convert " + val + " to long.";
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                                    return response;
                                }
                            case "end_date":
                                long end_date;
                                if (long.TryParse(val, out end_date))
                                {
                                    ev.End_Date = long.Parse(val);
                                    break;
                                }
                                else
                                {
                                    response = "Error: Cannot convert " + val + " to long.";
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                                    return response;
                                }
                            case "meeting_places":
                                ev.Meeting_Places = val;
                                break;
                            case "title":
                                ev.Title = val;
                                break;
                            case "description":
                                ev.Description = val;
                                break;
                            case "address":
                                ev.Address = val;
                                break;
                            case "gps":
                                ev.Gps_location = val;
                                break;
                            /*case "radius":
                                int radius;
                                if (int.TryParse(val, out radius))
                                {
                                    ev.Radius = int.Parse(val);
                                    break;
                                }
                                else
                                {
                                    response = "Error: Cannot convert " + val + " to integer.";
                                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                                    return response;
                                }*/
                            case "manager":
                                ev.Manager = val;
                                break;
                        }
                    }
                    else
                    {
                        response = "Error: Broken key-value pair";
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                        break;
                    }
                }
                //Update Event on DB
                if(!String.IsNullOrEmpty(ev.Manager))
                {
                    User mgr = db.getUser(ev.Manager);
                    string exec_res = db.updateEvent(ev, mgr.Access_level);
                    if (exec_res.ToLower().Contains("success"))
                    {
                        response = exec_res;
                        //WebOperationContext.Current.OutgoingResponse.Headers.Add("req_event_icon", eventId);
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response = "Query Execution Error: " + exec_res;
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                    }
                }else
                {
                    response = "Error:Invalid Manager.";
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                }
            }
            else
            {
                response = "Error: Invalid token count";
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
            }
            return response;
        }

        public string imageUpload(string name, Stream fileStream)
        {
            name = name.ToLower();
            StreamReader reader = new StreamReader(fileStream);
            string inbound_payload = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            char delim = ';';
            byte[] bytes = Convert.FromBase64String(inbound_payload);
            if (name.Contains(delim))
            {
                if (name[0] == delim)
                    name = name.Substring(1);//Remove first slash if it exists

                //Write file data
                string[] dirs = name.Split(delim);//get directory structure
                string dir = "";
                for (int i = 0; i < dirs.Length - 1; i++)//last element would be the filename
                    dir += dirs[i] + '/';
                var path = Path.Combine(HostingEnvironment.MapPath("~/images/" + dir), dirs[dirs.Length - 1]);//Path.Combine(@"C:\UploadedImages\" + name);
                File.WriteAllBytes(path, bytes);

                //Write Metadata
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                long since_epoch = (long)t.TotalSeconds;
                Metadata meta = new Metadata()
                {
                    Entry=(dir+dirs[dirs.Length-1]).Replace("/","|"),
                    Meta ="dmd="+Convert.ToString(since_epoch)
                };
                db.addMeta(meta);

                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";

                return "Success";
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
                return "Error: Cannot write to root";
            }
        }

        /*[OperationContract]
        [WebGet(UriTemplate = "foo")]*/
        public string imageDownload(string fileName)
        {
            fileName = fileName.ToLower();

            if (fileName.Contains("|"))
            {
                    if (fileName[0] == '|')
                        fileName = fileName.Substring(1);//Remove first slash if it exists

                string[] dirs = fileName.Split('|');//get directory structure
                string dir = "";
                for (int i = 0; i < dirs.Length - 1; i++)//last element would be the filename
                    dir += dirs[i] + '/';

                var path = Path.Combine(HostingEnvironment.MapPath("~/images/" + dir), dirs[dirs.Length - 1]);//Path.Combine(@"C:\UploadedImages\" + name);
                byte[] binFileArr;

                if (File.Exists(path))
                {
                    binFileArr = File.ReadAllBytes(path);
                }
                else
                {
                    path = Path.Combine(HostingEnvironment.MapPath("~/images/" + dir), "default.png");//Path.Combine(@"C:\UploadedImages\" + name);
                    binFileArr = File.ReadAllBytes(path);
                }

                //byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

                string base64bin = Convert.ToBase64String(binFileArr);

                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain;charset=utf-8";//"multi-part/form-data"
                return base64bin;
            }
            else
            {
                return "FNE";
            }
        }

        public string registerUser(Stream streamdata)
        {
            StreamReader reader = new StreamReader(streamdata);
            string inbound_payload = reader.ReadToEnd();
            string response="";
            reader.Close();
            reader.Dispose();

            //Process form submission

            User new_user = new User();
            //Set user defaults
            /*new_user.Fb_id = "NONE";
            new_user.Fb_token = "NONE";*/
            new_user.Access_level = 0;
            new_user.Event_id = 0;

            inbound_payload = HttpContext.Current.Server.UrlDecode(inbound_payload);
            string[] usr_details = inbound_payload.Split('&');
            if (usr_details.Length >= 3)
            {
                foreach (string kv_pair in usr_details)
                {
                    if (kv_pair.Contains('='))
                    {
                        string var = kv_pair.Split('=')[0];
                        string val = kv_pair.Split('=')[1];

                        switch (var)
                        {
                            case "fname":
                                new_user.Fname = val;
                                break;
                            case "lname":
                                new_user.Lname = val;
                                break;
                            case "age":
                                new_user.Age = Convert.ToUInt16(val);
                                break;
                            case "access_level":
                                new_user.Access_level = Convert.ToUInt16(val);
                                break;
                            case "username":
                                new_user.Username = val;
                                break;
                            case "email":
                                new_user.Email = val;
                                break;
                            case "password":
                                new_user.Password = val;
                                break;
                            case "catchphrase":
                                new_user.Catchphrase = val;
                                break;
                            case "occupation":
                                new_user.Occupation = val;
                                break;
                            case "bio":
                                new_user.Bio = val;
                                break;
                            case "gender":
                                new_user.Gender = val;
                                break;
                            case "fb_token":
                                new_user.Fb_token = val;
                                break;
                            case "fb_id":
                                new_user.Fb_id = val;
                                break;
                        }
                    }
                    else
                    {
                        response = "Error: Broken key-value pair";
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                        return response;
                    }
                }
                //Add to DB
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
                string exec_result = db.registerUser(new_user);
                if (exec_result.ToLower().Contains("success"))
                {
                    response = exec_result;
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response = exec_result;
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                }
                return response;
            }
            else
            {
                response = "Error: Invalid token count ("+usr_details.Length+") >> " + inbound_payload;
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                return response;
            }
        }

        public void signIn(Stream streamdata)
        {
            StreamReader reader = new StreamReader(streamdata);
            string inbound_payload = reader.ReadToEnd();
            string response = "";
            reader.Close();
            reader.Dispose();
            //Process form submission
            User new_user = new User();
            inbound_payload = HttpContext.Current.Server.UrlDecode(inbound_payload);
            string[] usr_details = inbound_payload.Split('&');
            if (usr_details.Length == 2)
            {
                foreach (string kv_pair in usr_details)
                {
                    if (kv_pair.Contains('='))
                    {
                        string var = kv_pair.Split('=')[0];
                        string val = kv_pair.Split('=')[1];

                        switch (var)
                        {
                            case "username":
                                new_user.Username = val;
                                break;                      
                            case "password":
                                new_user.Password = val;
                                break;
                        }
                    }
                    else
                    {
                        response = "Error: Broken key-value pair";
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                        break;
                    }
                }
                //Read from DB here
                string exec_result = db.signIn(new_user);
                if (exec_result.ToLower().Contains("isvaliduser=true"))
                {
                    response = "Success: " + exec_result;
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response = "Error: " + exec_result;
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                }
            }
            else
            {
                response = "Error: Invalid token count (" + usr_details.Length + ") >> " + inbound_payload;
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
            }

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            //WebOperationContext.Current.OutgoingResponse.StatusDescription =  res;
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Payload", response);
        }

        public string addMessage(Stream streamdata)
        {
            StreamReader reader = new StreamReader(streamdata);
            string inbound_payload = reader.ReadToEnd();
            string response = "";
            reader.Close();
            reader.Dispose();
            //Process form submission
            Message new_msg = new Message();
            DateTime d = DateTime.Now;
            new_msg.Message_time = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds; //d.Year + "/" + d.Month + "/" + d.Day + " " + d.TimeOfDay ;
            //Set other fields
            inbound_payload = HttpContext.Current.Server.UrlDecode(inbound_payload);
            string[] msg_details = inbound_payload.Split('&');
            if (msg_details.Length == 5)
            {
                foreach (string kv_pair in msg_details)
                {
                    if (kv_pair.Contains('='))
                    {
                        string var = kv_pair.Split('=')[0];
                        string val = kv_pair.Split('=')[1];

                        switch (var)
                        {
                            case "message_id":
                                new_msg.Message_id = val;
                                break;
                            case "message":
                                new_msg.Msg = val;
                                break;
                            case "message_status":
                                new_msg.Message_status = Convert.ToInt16(val);
                                break;
                            case "message_sender":
                                new_msg.Message_sender = val;
                                break;
                            case "message_receiver":
                                new_msg.Message_receiver = val;
                                break;
                        }
                    }
                    else
                    {
                        response = "Error: Broken key-value pair";
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                        return response;
                    }
                }
                //Insert to DB
                User sender = db.getUser(new_msg.Message_sender);
                new_msg.Event_id = sender.Event_id;//store event where icebreaking for statistical purposes.

                string exec_result = db.addMessage(new_msg);
                if (exec_result.ToLower().Contains("success"))
                {
                    response = exec_result;
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    //Send notifcation to receiver    > reg_token_rec,reg_token_sen,m.getId,SRV_REC
                    string reg_token_rec = db.getUserToken(new_msg.Message_receiver);
                    string reg_token_sen = db.getUserToken(new_msg.Message_sender);
                    /*String url_params = String.Format("registration_ids:{"+
                                            "registration_id:\"{0}\"," +
                                            "registration_id:\"{1}\"}&" +
                                            "priority=high&"+
                                            "data:{"+
                                            "\"m_id\":\"{2}\",\"status\":\"{3}\"}",reg_token_rec,reg_token_sen,
                                            new_msg.Message_id,DBServerTools.ICEBREAK_SERV_RECEIVED);*/

                    string title = new_msg.Message_status>=DBServerTools.ICEBREAK?"New IceBreak Request":"New Message";
                    
                    if (sender!=null)
                    {
                        string fname = DBServerTools.isEmpty(sender.Fname) ? "X" : sender.Fname;
                        string lname = DBServerTools.isEmpty(sender.Lname) ? "X" : sender.Lname;
                        string name = "";
                        if (!fname.Equals("X") && !lname.Equals("X"))
                            name = fname + " " + lname[0] + '.';
                        else
                            name = "Anonymous";

                        string text = name + " would like to get to know you.";
                        /*string notif = "{" +
                                            "\"notification\":{" +
                                            "\"title\": \"" + title + "\"," +
                                            "\"text\": \"" + text + "\"}," +
                                            "\"data\": {" +
                                            "\"msg_id\": \"" + new_msg.Message_id + "\"}," +
                                            "\"to\": \"" + reg_token_rec + "\"}";*/
                        string notif = "{" +
                                "\"data\": {" +
                                "\"msg_id\": \"" + new_msg.Message_id + "\"}," +
                                "\"to\": \"" + reg_token_rec + "\"}";
                        //send push notification to receiver
                        sendNotification(notif);

                        notif = "{" +
                                "\"data\": {" +
                                "\"msg_id\": \"" + new_msg.Message_id + "\"}," +
                                "\"to\": \"" + reg_token_sen + "\"}";
                        //send push notification to sender
                        sendNotification(notif);
                    }
                    else
                    {
                        response = exec_result;
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                        db.addError(ErrorCodes.ENOTIF, "Can't send notification because sender is NULL", "addMessage");
                    }
                }
                else
                {
                    response = exec_result;
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    db.addError(ErrorCodes.ENOTIF, "Can't send notification because addMessage wasn't successful: " +response, "addMessage");
                }
            }
            else
            {
                response = "Error: Invalid token count (" + msg_details.Length + ") >> " + inbound_payload;
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                db.addError(ErrorCodes.ENOTIF, response, "addMessage");
            }
            return response;
        }

        public void sendNotification(string url_params)
        {
            WebRequest req = WebRequest.Create("https://fcm.googleapis.com/fcm/send?" + url_params);
            req.Method = "POST";
            req.ContentType = "application/json";
            req.Headers.Add("Authorization", "key=AIzaSyAbyhAF4s4HzZqYcHztCnk9Xcgpjd4Wt1U");
            req.ContentLength = url_params.Length;
            Stream dataStream = req.GetRequestStream();

            dataStream.Write(Encoding.UTF8.GetBytes(url_params), 0, url_params.Length);
            dataStream.Close ();

            WebResponse response = req.GetResponse();

            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Save error to DB -- TODO: fix this
            if(!((HttpWebResponse)response).StatusCode.ToString().Contains("OK"))
                db.addError(ErrorCodes.ENOTIF, ((HttpWebResponse)response).StatusCode + ":>" + responseFromServer, "sendNotification");
            
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
        }

        public string setUniqueUserToken(Stream streamdata)
        {
            string username="", token="";
            StreamReader reader = new StreamReader(streamdata);
            string inbound_payload = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            //Process posted user token
            inbound_payload = HttpContext.Current.Server.UrlDecode(inbound_payload);
            string[] usr_token = inbound_payload.Split('&');
            if (usr_token.Length == 2)
            {
                foreach (string kv_pair in usr_token)
                {
                    if (kv_pair.Contains('='))
                    {
                        string var = kv_pair.Split('=')[0];
                        string val = kv_pair.Split('=')[1];

                        switch (var)
                        {
                            case "username":
                                username = val;
                                break;
                            case "token":
                                token = val;
                                break;
                        }
                    }
                }
                if(username.Length>0 && token.Length>0)
                {
                    List<Message> messages = db.checkUserInbox(username);
                    foreach(Message m in messages)
                    {
                        string notif = "{" +
                                "\"data\": {" +
                                "\"msg_id\": \"" + m.Message_id + "\"}," +
                                "\"to\": \"" + username + "\"}";
                        //send push notification to sender
                        sendNotification(notif);
                    }
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    return db.setUniqueUserToken(username, token);
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                    return "Username or Token are null: username==null?" + (username.Length <= 0) + ", token==null?" + (token.Length <= 0);
                }
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                return "Invalid parameter count (" + usr_token.Length + ")";
            }
        }

        public List<Event> getAllEvents()
        {
            return db.getAllEvents();
        }

        public List<User> getUsersAtEvent(string eventId)
        {
            long id;
            if (long.TryParse(eventId, out id))
                return db.getUsersAtEvent(long.Parse(eventId));
            else return null;
        }

        /*public string updateUserMailbox(Stream streamdata)
        {
            StreamReader reader = new StreamReader(streamdata);
            string inbound_payload = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            //Process form submission
            Message new_msg = new Message();
            inbound_payload = HttpContext.Current.Server.UrlDecode(inbound_payload);
            string[] msg_details = inbound_payload.Split('&');
            if (msg_details.Length >= 1)
            {
                foreach (string s in msg_details)
                {
                    if (s.Contains("="))
                    {
                        string[] kv_pair = s.Split('=');
                        switch (kv_pair[0])
                        {
                            case "Message_status":
                                new_msg.Message_status = Convert.ToInt16(kv_pair[1]);
                                break;
                            case "Message_receiver":
                                new_msg.Message_receiver = kv_pair[1];
                                break;
                            case "Message_sender":
                                new_msg.Message_sender = kv_pair[1];
                                break;
                        }
                    }
                    else
                        return "Fail: Broken key-value pair.";
                }
                return db.updateUserMailbox(new_msg);
            }
            else return "Fail: Empty data.";
        }*/

        public User getUser(string username)
        {
            return db.getUser(username);
        }

        public string removeUser(string handle)
        {
            string rem_result = db.removeUser(handle);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            //WebOperationContext.Current.OutgoingResponse.StatusDescription = rem_result;
            if (rem_result.ToLower().Contains("success"))
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            else
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
            return rem_result;
        }

        public string userUpdate(string handle, Stream streamdata)
        {
            StreamReader reader = new StreamReader(streamdata);
            string inbound_payload = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();

            /*WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            WebOperationContext.Current.OutgoingResponse.StatusDescription = "Not enough URL params";
            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Response", "<Response goes here>");
            return "Some response";*/
            long now = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            inbound_payload = HttpContext.Current.Server.UrlDecode(inbound_payload);
            string[] usr_details = inbound_payload.Split('&');
            User new_user = new User();
            if (handle.Length >= 0)
            {
                new_user.Username = handle;
                foreach (string usr in usr_details)
                {
                    if (usr.Contains('='))
                    {
                        string var = usr.Split('=')[0];
                        string val = usr.Split('=')[1];

                        switch (var.ToLower())
                        {
                            case "fname":
                                new_user.Fname = val;
                                break;
                            case "lname":
                                new_user.Lname = val;
                                break;
                            case "username":
                                if (!val.Equals(handle))
                                    return "User handle in URL does not match with user handle in parameters.";
                                new_user.Username = val;
                                break;
                            case "email":
                                new_user.Email = val;
                                break;
                            case "password":
                                new_user.Password = val;
                                break;
                            case "access_level":
                                new_user.Access_level = Convert.ToUInt16(val);
                                break;
                            case "event_id":
                                new_user.Event_id = Convert.ToUInt16(val);
                                break;
                            case "age":
                                new_user.Age = Convert.ToUInt16(val);
                                break;
                            case "gender":
                                new_user.Gender = val.ToLower();
                                break;
                            case "occupation":
                                new_user.Occupation = val;
                                break;
                            case "bio":
                                new_user.Bio = val;
                                break;
                            case "catchphrase":
                                new_user.Catchphrase = val;
                                break;
                            case "fb_token":
                                new_user.Fb_token = val;
                                break;
                            case "fb_id":
                                new_user.Fb_id = val;
                                break;
                            case "last_seen":
                                new_user.Last_Seen = now;
                                break;
                            default:
                                db.addError(ErrorCodes.EUSR,"Unknown attribute '" + var + "'", "");
                                break;
                        }
                    }
                    else
                    {
                        return "Broken key-value pair.";
                    }
                }

                string result = db.updateUserDetails(new_user);
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";

                if (result.ToLower().Contains("success"))
                {
                    //WebOperationContext.Current.OutgoingResponse.StatusDescription = "Successfully updated user.";
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    //WebOperationContext.Current.OutgoingResponse.StatusDescription = result;
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                }
                return result;
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
                WebOperationContext.Current.OutgoingResponse.StatusDescription = "Not enough URL params";
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                return "Not enough URL params";
            }
        }

        public List<Message> checkUserInbox(string username)
        {
            return db.checkUserInbox(username);
        }

        public List<Message> checkUserOutbox(string sender)
        {
            return db.checkUserOutbox(sender);
        }

        public Message getMessageById(string msg_id)
        {
            return db.getMessageById(msg_id);
        }

        public Event getEvent(string event_id)
        {
            return db.getEvent(event_id);
        }

        public Metadata getMeta(string record)
        {
            return db.getMeta(record);
        }

        public static double HaversineInKM(double lat1, double long1, double lat2, double long2)
        {
            double dlong = (long2 - long1) * _d2r;
            double dlat = (lat2 - lat1) * _d2r;
            double a = Math.Pow(Math.Sin(dlat / 2D), 2D) + Math.Cos(lat1 * _d2r) * Math.Cos(lat2 * _d2r) * Math.Pow(Math.Sin(dlong / 2D), 2D);
            double c = 2D * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1D - a));
            double d = _eQuatorialEarthRadius * c;

            return d;
        }

        public List<Event> getNearbyEvents(string lat, string lng, string strRange)
        {
            double meLat = 0.0, meLng=0.0, range=0.0;
            if (double.TryParse(lat, out meLat))
            {
                meLat = double.Parse(lat);
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                return null;
            }
            
            if (double.TryParse(lng, out meLng))
            {
                meLng = double.Parse(lng);
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                return null;
            }
            
            if (double.TryParse(strRange, out range))
            {
                range = double.Parse(strRange);
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                return null;
            }

            List<Event> valid_events = new List<Event>();
            if(meLat!=0.0 && meLng!=0.0 && range!=0.0)
            {
                List<Event> events = db.getAllEvents();
                foreach(Event e in events)
                {
                    Point ep = e.getOrigin();
                    if (!ep.isZero())
                    {
                        double distance_km = HaversineInKM(meLat, meLng, ep.Lat, ep.Lng);
                        if (distance_km <= range)
                            valid_events.Add(e);
                    }
                }
            }
            return valid_events;
        }

        public List<string> getNearbyEventIds(string lat, string lng, string range)
        {
            List<Event> events = getNearbyEvents(lat,lng,range);
            List<string> event_ids = new List<string>();
            foreach(Event e in events)
            {
                event_ids.Add(Convert.ToString(e.Id));
            }
            return event_ids;
        }

        public List<IBException> getExceptions()
        {
            return db.getExceptions();
        }

        #region Statistics

        /******Master Stats*************/

        public int getTotalIcebreakCount()
        {
            return db.getTotalIcebreakCount();
        }

        public int getTotalSuccessfulIcebreakCount()
        {
            return db.getTotalSuccessfulIcebreakCount();
        }

        public int getTotalIcebreakCountBetweenTime(string start, string end)
        {
            long s,e;
            if (long.TryParse(start, out s) && long.TryParse(end, out e))
                return db.getTotalIcebreakCountBetweenTime(long.Parse(start), long.Parse(end));
            else return ErrorCodes.ECONV;
        }

        public int getTotalSuccessfullIcebreakCountBetweenTime(string start, string end)
        {
            long s, e;
            if (long.TryParse(start, out s) && long.TryParse(end, out e))
                return db.getTotalSuccessfulIcebreakCountBetweenTime(long.Parse(start), long.Parse(end));
            else return ErrorCodes.ECONV;
        }

        /******User Stats*************/

        public int getUserIcebreakCount(string username)
        {
            HttpContext.Current.Response.ContentType = "text/plain";
            if (!String.IsNullOrEmpty(username))
                return db.getUserIcebreakCount(username);
            else return ErrorCodes.ECONV;
        }

        public int getUserSuccessfulIcebreakCount(string username)
        {
            if(!String.IsNullOrEmpty(username))
                return db.getUserSuccessfulIcebreakCount(username);
            else return ErrorCodes.ECONV;
        }

        public int getUserIcebreakCountAtEvent(string username, string event_id)
        {
            long id;
            if (long.TryParse(event_id, out id) && !String.IsNullOrEmpty(username))
                return db.getUserIcebreakCountAtEvent(username, long.Parse(event_id));
            else return ErrorCodes.ECONV;
        }

        public int getUserSuccessfulIcebreakCountAtEvent(string username, string event_id)
        {
            long id;
            if (long.TryParse(event_id, out id) && !String.IsNullOrEmpty(username))
                return db.getUserSuccessfulIcebreakCountAtEvent(username, long.Parse(event_id));
            else return ErrorCodes.ECONV;
        }

        public int getUserIcebreakCountBetweenTime(string username, string start, string end)
        {
            long s, e;
            if (long.TryParse(start, out s) && long.TryParse(end, out e))
                return db.getUserIcebreakCountBetweenTime(username, long.Parse(start), long.Parse(end));
            else return ErrorCodes.ECONV;
        }

        public int getUserSuccessfulIcebreakCountBetweenTime(string username, string start, string end)
        {
            long s,e;
            if (long.TryParse(start, out s) && long.TryParse(end, out e) && !String.IsNullOrEmpty(username))
                return db.getUserSuccessfulIcebreakCountBetweenTime(username, long.Parse(start), long.Parse(end));
            else return ErrorCodes.ECONV;
        }

        public int getUserIcebreakCountBetweenTimeAtEvent(string username, string start, string end, string event_id)
        {
            long s, e, id;
            if (long.TryParse(start, out s) && long.TryParse(end, out e) && long.TryParse(event_id, out id) && !String.IsNullOrEmpty(username))
                return db.getUserIcebreakCountBetweenTimeAtEvent(username, long.Parse(start), long.Parse(end), long.Parse(event_id));
            else return ErrorCodes.ECONV;
        }

        public int getUserSuccessfulIcebreakCountBetweenTimeAtEvent(string username, string start, string end, string event_id)
        {
            long s, e, id;
            if (long.TryParse(start, out s) && long.TryParse(end, out e) && long.TryParse(event_id, out id) && !String.IsNullOrEmpty(username))
                return db.getUserSuccessfulIcebreakCountBetweenTimeAtEvent(username, long.Parse(start), long.Parse(end), long.Parse(event_id));
            else return ErrorCodes.ECONV;
        }

        public int getMaxUserIcebreakCountAtOneEvent(string username)
        {
            return db.getMaxUserIcebreakCountAtOneEvent(username).Value;
        }

        public int getMaxUserSuccessfulIcebreakCountAtOneEvent(string username)
        {
            return db.getMaxUserSuccessfulIcebreakCountAtOneEvent(username).Value;
        }

        public int getUserIcebreakCountXHoursApart(string username, string hours)
        {
            long hrs;
            if (long.TryParse(hours, out hrs))
                return db.getUserIcebreaksXHoursApart(username, long.Parse(hours)).Count;
            else return ErrorCodes.ECONV;
        }

        public int getUserSuccessfulIcebreakCountXHoursApart(string username, string hours)
        {
            long hrs;
            if (long.TryParse(hours, out hrs))
                return db.getUserSuccessfulIcebreaksXHoursApart(username, long.Parse(hours)).Count;
            else return ErrorCodes.ECONV;
        }

        public List<Event> getUserEventHistory(string username)
        {
            return db.getUserEventHistory(username);
        }

        public List<Achievement> getUserAchievements(string username)
        {
            return db.getUserAchievements(username);
        }

        /******Event Stats************/

        public int getEventIcebreakCount(string event_id)
        {
            long id;
            if (long.TryParse(event_id, out id))
                return db.getEventIcebreakCount(long.Parse(event_id));
            else return ErrorCodes.ECONV;
        }

        public int getEventIcebreakCountBetweenTime(string event_id, string start, string end)
        {
            long id, s, e;
            if (long.TryParse(event_id, out id) && long.TryParse(start, out s) && long.TryParse(end, out e))
                return db.getEventIcebreakCountBetweenTime(long.Parse(event_id), long.Parse(start), long.Parse(end));
            else return ErrorCodes.ECONV;
        }

        public int getEventSuccessfulIcebreakCountBetweenTime(string event_id, string start, string end)
        {
            long id, s, e;
            if (long.TryParse(event_id, out id) && long.TryParse(start, out s) && long.TryParse(end, out e))
                return db.getEventSuccessfulIcebreakCountBetweenTime(long.Parse(event_id), long.Parse(start), long.Parse(end));
            else return ErrorCodes.ECONV;
        }

        #endregion Statistics

        public Achievement getAchievement(string ach_id)
        {
            long id;
            if (long.TryParse(ach_id, out id))
                return db.getAchievement(long.Parse(ach_id));
            else return null;
        }

        public Reward getReward(string rew_id)
        {
            long id;
            if (long.TryParse(rew_id, out id))
                return db.getReward(long.Parse(rew_id));
            else return null;
        }

        public List<Achievement> getAllAchievements()
        {
            return db.getAllAchievements();
        }

        public List<Reward> getAllRewards()
        {
            return db.getAllRewards();
        }

        public string ping(string username)
        {
            string res = db.ping(username);
            if (res.ToLower().Equals("success"))
            {
                List<Achievement> usr_achs = db.getUserAchievements(username);
                List<Achievement> new_usr_achs = db.updateUserAchievements(username);
                if(usr_achs.Count < new_usr_achs.Count)
                {
                    User usr = db.getUser(username);
                    //Has gotten new achievements - send notification
                    for (int i=usr_achs.Count;i<new_usr_achs.Count;i++)
                    {
                        string notif = "{" +
                                "\"data\": {" +
                                "\"Achievement_id\": \"" + new_usr_achs.ElementAt(i).Id + "\"}," +
                                "\"to\": \"" + db.getUserToken(username) + "\"}";
                        sendNotification(notif);
                        
                        //TODO: Add to User_Achievements bridging table
                        
                        //Increase User points
                        usr.Points += new_usr_achs.ElementAt(i).Value;
                    }
                    db.updateUserDetails(usr);//update user points
                    //db.addError(123, "["+ (new_usr_achs.Count - usr_achs.Count) + "] new achievements for user ["+username+"].", "ping");
                }
            }
            return res;
        }
    }
}