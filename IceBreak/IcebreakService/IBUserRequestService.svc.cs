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

        public List<User> getUsers()
        {
            return db.getUsers();
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
            //WebOperationContext.Current.OutgoingResponse.StatusDescription =  res;
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Payload",response);
        }
    }
}
