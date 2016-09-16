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

        public IBUserRequestService()
        {
            db = new DBServerTools();
        }

        public void addEvent(Stream streamdata)
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

                        switch (var)
                        {
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
                            case "radius":
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
                                    return;
                                }
                        }
                    }
                    else
                    {
                        response = "Error: Broken key-value pair";
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                        break;
                    }
                }
                //Add event to DB
                string exec_res = db.addEvent(ev);

                List<Event> events = db.getEvents();
                string eventId = "";
                foreach(Event e in events)
                {
                    //TODO: validate with user admin attribute for event
                    if(e.Title.Equals(ev.Title) && e.Gps_location.Equals(ev.Gps_location) && e.Radius==ev.Radius)
                    {
                        eventId = Convert.ToString(e.Id);
                    }
                }
                if(exec_res.ToLower().Contains("success"))
                {
                    response = "Success: " + exec_res.ToString();
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("req_event_icon", eventId);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response = "Query Execution Error: " + exec_res;
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                }
            }
            else
            {
                response = "Error: Invalid token count";
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
            }
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
                int since_epoch = (int)t.TotalSeconds;
                Metadata meta = new Metadata() { Entry=dir+'/'+dirs[dirs.Length-1],Meta=Convert.ToString(since_epoch)};
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
            if (usr_details.Length >= 5)
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
            new_msg.Message_time = String.Format("{0:yyyy-MM-dd H-mm-ss}",d); //d.Year + "/" + d.Month + "/" + d.Day + " " + d.TimeOfDay ;
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
                    User sender = db.getUser(new_msg.Message_sender);
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
                        db.addError(ErrorCodes.ENOTIF, "Can't send notification because send is NULL", "addMessage");
                    }
                }
                else
                {
                    response = exec_result;
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    db.addError(ErrorCodes.ENOTIF, "Can't send notification because addMessage wasn't successful", "addMessage");
                }
            }
            else
            {
                response = "Error: Invalid token count (" + msg_details.Length + ") >> " + inbound_payload;
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
            }
            return response;
        }

        /*
        { "notification": {
    "title": "Portugal vs. Denmark",
    "text": "5 to 1"
  },
  "to" : "bk3RNwTe3H0:CI2k_HHwgIpoDKCIZvvDMExUdFQ3P1..."
}
        */

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

        public List<Event> readEvents()
        {
            return db.getEvents();
        }

        public List<User> getUsersAtEvent(string eventId)
        {
            return db.getUsersAtEvent(Convert.ToUInt16(eventId));
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
            inbound_payload = HttpContext.Current.Server.UrlDecode(inbound_payload);
            string[] usr_details = inbound_payload.Split('&');
            User new_user = new User();
            if(handle.Length>=0)
            {
                new_user.Username = handle;
                foreach (string usr in usr_details)
                {
                    if (usr.Contains('='))
                    {
                        string var = usr.Split('=')[0];
                        string val = usr.Split('=')[1];

                        switch (var)
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
                                new_user.Gender = val;
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

        public List<User> getUserContacts()
        {
            string[] fnames = { "Adrian", "Chanel", "Jacob", "George", "Chayenne", "Lois" };
            string[] lnames = { "Jones", "Jonas", "Brown", "Black", "Victor", "Travis" };
            List<User> users = new List<User>();
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                users.Add(new User()
                {
                    Fname = fnames[rand.Next(0,fnames.Length)],
                    Lname = lnames[rand.Next(0, lnames.Length)],
                    Bio = "<insert bio here>"
                });
            }
            return users;
        }

        public int getRandomNumber(int max)
        {
            double r = new Random().NextDouble();
            return Convert.ToInt16(Math.Floor(r * max));
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

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public void getUsersIcebreakCount()
        {
            //if(HttpContext.Current.Request["getUsersIcebreakCountCallback"]!=null)
                
            //string response = "getUsersIcebreakCount([{" + db.getUsersIcebreakCount() + "}])";
            string response = "getUsersIcebreakCount({var1:0,var2:1,var3:2});";
            //WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Write(response);
        }

        public Metadata getMeta(string record)
        {
            record = record.Replace('|','/');
            return db.getMeta(record);
        }
    }
}
