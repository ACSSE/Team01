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
                if(exec_res.Equals("Success"))
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

        public void imageUpload(string name, Stream fileStream)
        {
            StreamReader reader = new StreamReader(fileStream);
            string inbound_payload = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();

            byte[] bytes = Convert.FromBase64String(inbound_payload);
            var path = Path.Combine(HostingEnvironment.MapPath("~/images/"), name);//Path.Combine(@"C:\UploadedImages\" + name);
            File.WriteAllBytes(path, bytes);

            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            //WebOperationContext.Current.OutgoingResponse.StatusDescription = e.Message;
            //WebOperationContext.Current.OutgoingResponse.Headers.Add("Payload", inbound_payload);
        }

        public string imageDownload(string fileName)
        {
            var path = Path.Combine(HostingEnvironment.MapPath("~/images/"), fileName);//Path.Combine(@"C:\UploadedImages\" + name);
            byte[] binFileArr = File.ReadAllBytes(path);
            string base64bin = Convert.ToBase64String(binFileArr);

            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            WebOperationContext.Current.OutgoingResponse.ContentType = "multi-part/form-data";
            WebOperationContext.Current.OutgoingResponse.ContentLength = binFileArr.Length;
            //WebOperationContext.Current.OutgoingResponse.StatusDescription = e.Message;
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Payload", base64bin);

            return base64bin;
        }

        public void registerUser(Stream streamdata)
        {
            StreamReader reader = new StreamReader(streamdata);
            string inbound_payload = reader.ReadToEnd();
            string response="";
            reader.Close();
            reader.Dispose();
            //Process form submission
            User new_user = new User();
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
                            case "fname":
                                new_user.Fname = val;
                                break;
                            case "lname":
                                new_user.Lname = val;
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
                        }
                    }
                    else
                    {
                        response = "Error: Broken key-value pair";
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                        break;
                    }
                }
                new_user.Access_level = 0;
                new_user.Event_id = 0;
                //Add to DB here
                string exec_result = db.registerUser(new_user);
                if (exec_result.Equals("Success"))
                {
                    response = "Success: " + new_user.ToString();
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response = "Query Execution Error: " + exec_result;
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
                }
            }
            else
            {
                response = "Error: Invalid token count ("+usr_details.Length+") >> " + inbound_payload;
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Conflict;
            }
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            WebOperationContext.Current.OutgoingResponse.StatusDescription =  response;
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Payload",response);
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

        public List<Event> readEvents()
        {
            return db.getEvents();
        }

        public List<User> getUsersAtEvent(string eventId)
        {
            return db.getUsersAtEvent(Convert.ToUInt16(eventId));
        }

        public string removeUser(string handle)
        {
            string rem_result = db.removeUser(handle);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            //WebOperationContext.Current.OutgoingResponse.StatusDescription = rem_result;
            if (rem_result.Equals("Success"))
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
                        }
                    }
                    else
                    {
                        return "Broken key-value pair.";
                    }
                }

                string result = db.updateUserDetails(new_user);
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
                
                if (result.Equals("Success"))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusDescription = "Successfully updated user.";
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusDescription = result;
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
    }
}
