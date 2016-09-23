using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace IcebreakServices
{
    public class DBServerTools
    {
        public static int SENT = 0;
        public static int SERV_RECEIVED = 1;
        public static int DELIVERED = 2;
        public static int READ = 3;
        public static int ICEBREAK = 100;
        public static int ICEBREAK_SERV_RECEIVED = 101;
        public static int ICEBREAK_DELIVERED = 102;
        public static int ICEBREAK_ACCEPTED = 103;
        public static int ICEBREAK_REJECTED = 104;
        public static int ICEBREAK_DONE = 105;
        public static int CONN_CLOSED = 106;
        public static int EXISTS = 107;
        public static int DEXISTS = 108;
        public static int CAN_EDIT_EVENTS = 1;
        public static string NO_EMAIL = "<No email specified>";
        public static string NO_OCC = "<No occupation specified>";
        public static string NO_BIO = "<No bio specified>";
        public static string NO_PHRASE = "<No catchphrase specified>";
        public static string NO_GENDER = "Unspecified";

        private string dbConnectionString = "Server=tcp:icebreak-server.database.windows.net,1433;Initial Catalog=IcebreakDB;Persist Security Info=False;User ID=superuser;Password=Breakingtheice42;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader dataReader;

        public string updateUserDetails(User user)
        {
            if(isEmpty(user.Username))
                return "Error: Empty username";
            if (userExists(user).ToLower().Contains("exists=true"))
            {
                try
                {
                    conn = new SqlConnection(dbConnectionString);
                    conn.Open();

                    if(user.Access_level>=0)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET access_level=@lvl WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"lvl", user.Access_level);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Email!=null)
                    {
                        if (user.Email.Length <= 0)
                            user.Email = NO_EMAIL;
                        cmd = new SqlCommand("UPDATE dbo.Users SET email=@email WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"email", user.Email);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Age > 0)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET Age=@age WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"age", user.Age);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Occupation != null)
                    {
                        if (user.Occupation.Length <= 0)
                            user.Occupation = NO_OCC;
                        cmd = new SqlCommand("UPDATE dbo.Users SET Occupation=@occupation WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"occupation", user.Occupation);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Bio != null)
                    {
                        if (user.Bio.Length <= 0)
                            user.Bio = NO_BIO;
                        cmd = new SqlCommand("UPDATE dbo.Users SET Bio=@bio WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"bio", user.Bio);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Catchphrase != null)
                    {
                        if (user.Catchphrase.Length <= 0)
                            user.Catchphrase = NO_PHRASE;
                        cmd = new SqlCommand("UPDATE dbo.Users SET Catchphrase=@cp WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"cp", user.Catchphrase);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Gender != null)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET Gender=@gender WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"gender", user.Gender);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Event_id > 0)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET event_id=@id WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"id", user.Event_id);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Fname != null)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET fname=@fname WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"fname", user.Fname);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Lname != null)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET lname=@lname WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"lname", user.Lname);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Password != null)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET pwd=@pwd WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"pwd", Hash.HashString(user.Password));
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Fb_token != null)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET fb_token=@token WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"token", user.Fb_token);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Fb_id != null)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET fb_id=@id WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"id", user.Fb_id);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Username != null)
                    {
                        if (userExists(user).ToLower().Contains("exists=false"))//Make sure username has not been taken
                        {
                            cmd = new SqlCommand("UPDATE dbo.Users SET username=@username WHERE username=@usr", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                            cmd.Parameters.AddWithValue(@"username", user.Username);
                            cmd.Parameters.AddWithValue(@"usr", user.Username);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            //Clean up
                            if (cmd != null)
                                cmd.Dispose();
                            if (conn != null)
                                if (conn.State == System.Data.ConnectionState.Open)
                                    conn.Close();
                            cmd.Dispose();

                            return "Success: Exists=true";
                        }
                    }

                    //Clean up
                    if (cmd != null)
                        cmd.Dispose();
                    if (conn != null)
                        if (conn.State == System.Data.ConnectionState.Open)
                            conn.Close();
                        cmd.Dispose();

                    return "Success";
                }
                catch (Exception e)
                {
                    addError(ErrorCodes.EUSR, e.Message, "updateUserDetails");
                    return "Error: " + e.Message;
                }
            }
            else
            {
                return "Error: User does not exist.";
            }
        }

        public string userFbIdExists(User user)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query ID
                cmd = new SqlCommand("SELECT * FROM dbo.Users WHERE fb_id=@id", conn);
                cmd.Parameters.AddWithValue(@"id", user.Fb_id);

                dataReader = cmd.ExecuteReader();
                dataReader.Read();

                string username = Convert.ToString(dataReader.GetValue(4));

                bool hasRows = dataReader.HasRows;

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                if (hasRows)
                {
                    return username;
                }//else there was no user found with that ID
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "userFbIdExists");
                return e.Message;
            }
            return "Exists=false";
        }

        public string userExists(User user)
        {

            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Users] WHERE username=@usr", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", user.Username);

                dataReader = cmd.ExecuteReader();
                dataReader.Read();

                bool hasRows = dataReader.HasRows;

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                if(hasRows)
                {
                    return "Exists=true";
                }//else there was no user found with those credentials.
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "removeUser");
                return e.Message;
            }
            return "Exists=false";
        }

        public string removeUser(string handle)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("DELETE FROM [dbo].[Users] WHERE username=@usr", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", handle);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                return "Success";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "removeUser");
                return "Error:"+e.Message;
            }
        }

        public List<Message> checkUserInbox(string user)
        {
            List<Message> messages = new List<Message>();
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Get user's unread messages and Icebreaks
                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_receiver=@usr AND NOT "
                    + "Message_status=@read AND NOT "
                    + "Message_status=@done", conn);
                cmd.Parameters.AddWithValue(@"read", READ);
                cmd.Parameters.AddWithValue(@"done", ICEBREAK_DONE);
                cmd.Parameters.AddWithValue(@"usr", user);
                
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string event_date = Convert.ToString(dataReader.GetValue(5));
                    event_date = event_date.Replace('/', '-');
                    event_date = event_date.Replace(':', '-');
                    messages.Add(new Message()
                    {
                        Message_id = Convert.ToString(dataReader.GetValue(0)),
                        Msg = Convert.ToString(dataReader.GetValue(1)),
                        Message_status = Convert.ToInt16(dataReader.GetValue(2)),
                        Message_sender = Convert.ToString(dataReader.GetValue(3)),
                        Message_receiver = Convert.ToString(dataReader.GetValue(4)),
                        Message_time = (long)(dataReader.GetValue(5))
                    });
                }
                
                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return messages;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "checkUserInbox");
                messages.Add(new Message()
                {
                    Message_receiver = Convert.ToString("YOU["+user+"]"),
                    Message_sender = Convert.ToString("SERVER"),
                    Message_time = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds,
                    Message_status = -1,
                    Msg = e.Message,
                });
                return messages;
            }
        }

        public List<Message> checkUserOutbox(string sender)
        {
            List<Message> messages = new List<Message>();
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Get user's unread messages and Icebreaks
                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_sender=@sender "
                    + "AND NOT Message_status=@read "
                    + "AND NOT Message_status=@done", conn);

                cmd.Parameters.AddWithValue(@"sender", sender);
                cmd.Parameters.AddWithValue(@"read", READ);
                cmd.Parameters.AddWithValue(@"done", ICEBREAK_DONE);

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    messages.Add(new Message()
                    {
                        Message_id = Convert.ToString(dataReader.GetValue(0)),
                        Msg = Convert.ToString(dataReader.GetValue(1)),
                        Message_status = Convert.ToInt16(dataReader.GetValue(2)),
                        Message_sender = Convert.ToString(dataReader.GetValue(3)),
                        Message_receiver = Convert.ToString(dataReader.GetValue(4)),
                        Message_time = (long)(dataReader.GetValue(5))
                    });
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return messages;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "checkUserOutbox");
                messages.Add(new Message()
                {
                    Message_receiver = Convert.ToString("YOU[" + sender + "]"),
                    Message_sender = Convert.ToString("SERVER"),
                    Message_time = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds,
                    Message_status = -1,
                    Msg = e.Message,
                });
                return messages;
            }
        }

        /*public string updateUserMailbox(Message m)
        {
            string exist_check = userExists(new User() { Username = m.Message_receiver });
            if (exist_check.Equals("Exists=true"))
            {
                try
                {
                    if (m.Message_id != null && m.Message_status != null)
                    {
                        if (m.Message_id.Length>MSG_ID_MIN_LEN && m.Message_status>0)
                        {
                            conn = new SqlConnection(dbConnectionString);
                        conn.Open();
                        if (m.Message_status < 100)
                        {
                            cmd = new SqlCommand("UPDATE [Messages] SET Message_status=@status WHERE Message_id=@message_id", conn);
                            cmd.Parameters.AddWithValue(@"status", m.Message_status);
                            //cmd.Parameters.AddWithValue(@"receiver", m.Message_receiver);
                            cmd.Parameters.AddWithValue(@"message_id", m.Message_id);
                        }
                        else
                        {
                            cmd = new SqlCommand("UPDATE [Messages] SET Message_status=@status WHERE Message_id=@message_id", conn);
                            cmd.Parameters.AddWithValue(@"status", m.Message_status);
                            cmd.Parameters.AddWithValue(@"message_id", m.Message_id);
                        }
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        conn.Close();
                        return "Success";
                    }
                    else
                    {
                        return "Exception: Message ID or status are NULL.";
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            else
            {
                return "Exception: User not found";
            }
        }*/

        public string getUserToken(string username)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                string token = "";
                //Query user
                cmd = new SqlCommand("SELECT [user_token] FROM [dbo].[Users] WHERE username=@usr", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", username);

                dataReader = cmd.ExecuteReader();
                dataReader.Read();

                token = Convert.ToString(dataReader.GetValue(0));

                return token;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "getUserToken");
                return "Error:" + e.Message;
            }
        }

        public List<User> getUsersAtEvent(int id)
        {
            #region ForRelease
            /*if(id == 0)//Prevent anyone from reading data from people that are not at any event
                return null;*/
            # endregion
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Users] WHERE event_id=@id", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"id", id);

                dataReader = cmd.ExecuteReader();
                List<User> users = new List<User>();
                while (dataReader.Read())
                {
                    //Nasty code for now
                    //Get rid of NULLs
                    string fname = Convert.IsDBNull(dataReader.GetValue(0)) ? "X" : Convert.ToString(dataReader.GetValue(0));
                    string lname = Convert.IsDBNull(dataReader.GetValue(1)) ? "X" : Convert.ToString(dataReader.GetValue(1));
                    string username = Convert.IsDBNull(dataReader.GetValue(4)) ? "X" : Convert.ToString(dataReader.GetValue(4));
                    int age = Convert.IsDBNull(dataReader.GetValue(7)) ? 0 : Convert.ToUInt16(dataReader.GetValue(7));
                    string bio = Convert.IsDBNull(dataReader.GetValue(8)) ? "X" : Convert.ToString(dataReader.GetValue(8));
                    string catchphrase = Convert.IsDBNull(dataReader.GetValue(9)) ? "X" : Convert.ToString(dataReader.GetValue(9));
                    string occupation = Convert.IsDBNull(dataReader.GetValue(10)) ? "X" : Convert.ToString(dataReader.GetValue(10));
                    string gender = Convert.IsDBNull(dataReader.GetValue(11)) ? "X" : Convert.ToString(dataReader.GetValue(11));
                    //Get rid of empties
                    fname = isEmpty(fname) ? "X" : fname;
                    lname = isEmpty(lname) ? "X" : lname;
                    bio = isEmpty(bio)? "X" : bio;
                    catchphrase = isEmpty(catchphrase) ? "X" : catchphrase;
                    occupation = isEmpty(occupation)? "X" : occupation;
                    gender = isEmpty(gender) ? "X" : gender;
                    //End of nasty code
                    users.Add(new User
                    {
                        Fname = fname,
                        Lname = lname,
                        Username = username,
                        Occupation = occupation,
                        Age = age,
                        Bio = bio,
                        Gender = gender,
                        Catchphrase = catchphrase
                    });
                }
                
                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return users;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "getUsersAtEvent");
                /*return new List<User>
                { new User
                    {
                        Fname = "<Error>",
                        Lname = e.Message
                    }
                };*/
                return null;
            }
        }

        public int msgIdExists(string id, SqlConnection conn)
        {
            /* try
             {
                 conn = new SqlConnection(dbConnectionString);
                 conn.Open();*/
            if (conn != null)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    string query = "SELECT * FROM [dbo].[Messages] WHERE Message_id=@message_id";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"message_id", id);

                    SqlDataReader r = cmd.ExecuteReader();
                    bool hasRows = r.HasRows;
                    cmd.Dispose();
                    r.Close();
                    //conn.Close();
                    if (hasRows)
                    {
                        return EXISTS;
                    }else return DEXISTS;
                }else return CONN_CLOSED;
            }else return CONN_CLOSED;
            /*catch (Exception e)
            {
                addError(ErrorCodes.EMSG, e.Message, "msgIdExists");
                return -1;
            }*/
        }

        public string setUniqueUserToken(string username, string token)
        {
            try
            {
                if (userExists(new User() { Username = username }).ToLower().Contains("exists=true"))
                {
                    //update user entry - add id
                    conn = new SqlConnection(dbConnectionString);
                    conn.Open();

                    string query = "UPDATE [dbo].[Users] SET user_token=@token WHERE username=@user";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"token", token);
                    cmd.Parameters.AddWithValue(@"user", username);

                    //cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    conn.Close();
                    return "SUPD";
                }
                else
                {
                    return "EUDNE";
                }
            }catch(Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "setUniqueUserToken");
                return "Error:" + e.Message;
            }
        }

        public string addError(int code, string error, string method)
        {
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                string query = "INSERT INTO [dbo].[Exceptions] VALUES(@ex_code,@ex_msg,@ex_method,@timestamp)";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue(@"ex_code", code);
                cmd.Parameters.AddWithValue(@"ex_msg", error);
                cmd.Parameters.AddWithValue(@"ex_method", method);
                cmd.Parameters.AddWithValue(@"timestamp", (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                return "Success";
            }
            catch (Exception e)
            {
                return "Error:" + e.Message;
            }
        }

        public string addMessage(Message m)
        {
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                int msgState = msgIdExists(m.Message_id, conn);

                if (msgState == EXISTS)
                {
                    if(m.Message_status!=ICEBREAK && m.Message_status != SENT)
                    {
                        string query = "UPDATE [dbo].[Messages] SET Message_status=@status,Message=@message WHERE Message_id=@message_id";
                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue(@"message_id", m.Message_id);
                        cmd.Parameters.AddWithValue(@"status", m.Message_status);
                        cmd.Parameters.AddWithValue(@"message", m.Msg);
                        cmd.ExecuteNonQuery();

                        cmd.Dispose();
                        conn.Close();
                        return "Success: Updated";
                    }
                    else
                    {
                        conn.Close();
                        return "Success: Already up-to-date";
                    }
                }
                else if(msgState == DEXISTS)
                {
                    string query = "INSERT INTO [dbo].[Messages] VALUES(@message_id,@message,@status,@sender,@receiver,@time)";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"message_id", m.Message_id);
                    cmd.Parameters.AddWithValue(@"message", m.Msg);
                    if (m.Message_status == ICEBREAK)
                    {
                        //Store ICEBREAK_SERV_RECEIVED instead
                        m.Message_status = ICEBREAK_SERV_RECEIVED;
                    }
                    else if (m.Message_status == SENT)
                    {
                        //Store SERV_RECEIVED instead
                        m.Message_status = SERV_RECEIVED;
                    }
                    cmd.Parameters.AddWithValue(@"status", m.Message_status);
                    cmd.Parameters.AddWithValue(@"sender", m.Message_sender);
                    cmd.Parameters.AddWithValue(@"receiver", m.Message_receiver);
                    cmd.Parameters.AddWithValue(@"time", m.Message_time);

                    //cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    conn.Close();
                    return "Success";
                }
                else if(msgState == CONN_CLOSED)
                {
                    return "Error:Connection closed.";
                }
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EMSG, e.Message, "addMessage");
                return "Error:" + e.Message;
            }
            return "Error:Unknown Response.";
        }

        public Event getEvent(string event_id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Events] WHERE event_id=@event_id", conn);
                cmd.Parameters.AddWithValue(@"event_id", event_id);

                dataReader = cmd.ExecuteReader();
                Event e = null;
                while (dataReader.Read())
                {
                    e = new Event()
                    {
                        Id = Convert.ToUInt32(dataReader.GetValue(0)),
                        Title = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Address = (string)dataReader.GetValue(3),
                        //Radius = (int)dataReader.GetValue(4),
                        Gps_location = (string)dataReader.GetValue(4),
                        AccessCode = Convert.ToUInt16(dataReader.GetValue(5)),
                        Date = Convert.ToUInt32(dataReader.GetValue(6)),
                        Meeting_Places = (string)dataReader.GetValue(7),
                        End_Date = Convert.ToUInt32(dataReader.GetValue(8))
                    };
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return e;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "getEvent");
                /*return new Event
                {
                    Title = "<Error>",
                    Description = e.Message
                };*/
                return null;
            }
        }

        public Message getMessageById(string msg_id)
        {
            Message m = null;
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Get user's unread messages and Icebreaks
                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_id=@id", conn);
                cmd.Parameters.AddWithValue(@"id", msg_id);

                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    m = new Message();
                    dataReader.Read();

                    m.Message_id = Convert.ToString(dataReader.GetValue(0));
                    m.Msg = Convert.ToString(dataReader.GetValue(1));
                    m.Message_status = Convert.ToInt16(dataReader.GetValue(2));
                    m.Message_sender = Convert.ToString(dataReader.GetValue(3));
                    m.Message_receiver = Convert.ToString(dataReader.GetValue(4));
                    m.Message_time = (long)(dataReader.GetValue(5));
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                return m;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EMSG, e.Message, "getMessageById");
                return null;
            }
        }

        public static bool isEmpty(string s)
        {
            if(s == null)
                return true;

            String temp = new String(s.ToCharArray());
            temp = temp.Replace(" ", "");
            //if (System.Text.RegularExpressions.Regex.IsMatch(s, "\\s*$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            //    return true;

            if (temp.Length <= 0 || temp.Equals(" "))
                return true;
            //Passes checks
            return false;
        }

        public string registerUser(User user)
        {
            if(isEmpty(user.Username))
               return "Error:Empty username";

            string exist_check = userExists(user);

            if (exist_check.ToLower().Contains("exists=false"))//insert new user if they don't exist in DB
            {
                try
                {
                    conn = new SqlConnection(dbConnectionString);
                    conn.Open();
                    string query = "INSERT INTO dbo.Users(fname,lname,email,pwd,username,access_level,event_id,"+
                        "Age,Bio,Catchphrase,Occupation,Gender,fb_token,fb_id) VALUES(@fname,@lname,@email,"+
                        "@password,@username,@access_lvl,@event_id,@age,@bio,@catchphrase,@occupation,@gender,@fb_token,@fb_id)";
                    cmd = new SqlCommand(query, conn);

                    if (isEmpty(user.Password))
                        return "Error:Empty password";
                    if (isEmpty(user.Gender))
                        user.Gender = NO_GENDER;
                    if (isEmpty(user.Email))
                        user.Email = NO_EMAIL;
                    if (isEmpty(user.Catchphrase))
                        user.Catchphrase = NO_PHRASE;
                    if (isEmpty(user.Bio))
                        user.Bio = NO_BIO;
                    if (isEmpty(user.Occupation))
                        user.Occupation = NO_OCC;
                    if (isEmpty(user.Fname))
                        user.Fname = "";
                    if (isEmpty(user.Lname))
                        user.Lname = "";
                    if (isEmpty(user.Fb_token))
                        user.Fb_token = "";
                    if (isEmpty(user.Fb_id))
                        user.Fb_id = "";

                    cmd.Parameters.AddWithValue(@"fname", user.Fname);
                    cmd.Parameters.AddWithValue(@"lname", user.Lname);
                    cmd.Parameters.AddWithValue(@"email", user.Email);
                    cmd.Parameters.AddWithValue(@"password", Hash.HashString(user.Password));
                    cmd.Parameters.AddWithValue(@"username", user.Username);
                    cmd.Parameters.AddWithValue(@"access_lvl", user.Access_level);
                    cmd.Parameters.AddWithValue(@"event_id", user.Event_id);
                    cmd.Parameters.AddWithValue(@"age", user.Age);
                    cmd.Parameters.AddWithValue(@"bio", user.Bio);
                    cmd.Parameters.AddWithValue(@"catchphrase", user.Catchphrase);
                    cmd.Parameters.AddWithValue(@"occupation", user.Occupation);
                    cmd.Parameters.AddWithValue(@"gender", user.Gender);
                    cmd.Parameters.AddWithValue(@"fb_token", user.Fb_token);
                    cmd.Parameters.AddWithValue(@"fb_id", user.Fb_id);

                    //cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    conn.Close();

                    return "Success";
                }
                catch (Exception e)
                {
                    addError(ErrorCodes.EUSR, e.Message, "registerUser");
                    return "Error:"+e.Message;
                }
            }
            else//User exists or there was an exception
            {
                if (exist_check.ToLower().Contains("exists=true"))
                {
                    return updateUserDetails(user);//update user info instead
                }else return exist_check;//most likely an exception was thrown if it gets here
            }
        }

        public string updateEvent(Event ev, int access_lvl)
        {
            try
            {
                //Security checks
                if (access_lvl<CAN_EDIT_EVENTS)
                    return "Error:You do not have the necessary permissions to execute this action.";
                if (ev.Id<=0 || ev.AccessCode<=0)
                    return "Error:Invalid Event ID or access code.";
                if(isEmpty(ev.Manager))
                    return "Error:Invalid Event Manager.";

                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                SqlCommand cmd = null;

                string q = "SELECT * FROM [dbo].[Events] WHERE event_id=@id AND event_access_code=@code AND username=@manager";
                cmd = new SqlCommand(q,conn);
                cmd.Parameters.AddWithValue(@"id", ev.Id);
                cmd.Parameters.AddWithValue(@"code", ev.AccessCode);
                cmd.Parameters.AddWithValue(@"manager", ev.Manager);
                SqlDataReader readr = cmd.ExecuteReader();
                readr.Read();
                if(!readr.HasRows)
                {
                    //Clean up
                    cmd.Dispose();
                    readr.Close();
                    conn.Close();
                    return "Error:No Event found matching the provided credentials [i.e. event id, access code, manager]";
                }
                //Clean up
                cmd.Dispose();
                readr.Close();

                if(conn==null)
                    conn = new SqlConnection(dbConnectionString);
                if(conn.State==System.Data.ConnectionState.Closed)
                    conn.Open();

                if (ev.AccessCode>0)
                {
                    string query = "UPDATE [dbo].[Events] SET event_access_code=@code WHERE event_id=@id AND event_access_code=@code AND username=@manager";
                    cmd = new SqlCommand(query,conn);
                    cmd.Parameters.AddWithValue(@"code", ev.AccessCode);
                    cmd.Parameters.AddWithValue(@"id", ev.Id);
                    cmd.Parameters.AddWithValue(@"manager", ev.Manager);
                    cmd.ExecuteNonQuery();
                }
                if (!isEmpty(ev.Address))
                {
                    string query = "UPDATE [dbo].[Events] SET event_address=@address  WHERE event_id=@id AND event_access_code=@code AND username=@manager";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"address", ev.Address);
                    cmd.Parameters.AddWithValue(@"code", ev.AccessCode);
                    cmd.Parameters.AddWithValue(@"id", ev.Id);
                    cmd.Parameters.AddWithValue(@"manager", ev.Manager);
                    cmd.ExecuteNonQuery();
                }
                if (ev.Date>0)
                {
                    string query = "UPDATE [dbo].[Events] SET event_date=@event_date  WHERE event_id=@id AND event_access_code=@code AND username=@manager";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"event_date", ev.Date);
                    cmd.Parameters.AddWithValue(@"code", ev.AccessCode);
                    cmd.Parameters.AddWithValue(@"id", ev.Id);
                    cmd.Parameters.AddWithValue(@"manager", ev.Manager);
                    cmd.ExecuteNonQuery();
                }
                if (!isEmpty(ev.Description))
                {
                    string query = "UPDATE [dbo].[Events] SET event_description=@description  WHERE event_id=@id AND event_access_code=@code AND username=@manager";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"description", ev.Description);
                    cmd.Parameters.AddWithValue(@"code", ev.AccessCode);
                    cmd.Parameters.AddWithValue(@"id", ev.Id);
                    cmd.Parameters.AddWithValue(@"manager", ev.Manager);
                    cmd.ExecuteNonQuery();
                }
                if (ev.End_Date>0)
                {
                    string query = "UPDATE [dbo].[Events] SET event_end_date=@event_end_date  WHERE event_id=@id AND event_access_code=@code AND username=@manager";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"event_end_date", ev.End_Date);
                    cmd.Parameters.AddWithValue(@"code", ev.AccessCode);
                    cmd.Parameters.AddWithValue(@"id", ev.Id);
                    cmd.Parameters.AddWithValue(@"manager", ev.Manager);
                    cmd.ExecuteNonQuery();
                }
                if (!isEmpty(ev.Gps_location))
                {
                    string query = "UPDATE [dbo].[Events] SET event_gps_location=@gps_location  WHERE event_id=@id AND event_access_code=@code AND username=@manager";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"gps_location", ev.Gps_location);
                    cmd.Parameters.AddWithValue(@"code", ev.AccessCode);
                    cmd.Parameters.AddWithValue(@"id", ev.Id);
                    cmd.Parameters.AddWithValue(@"manager", ev.Manager);
                    cmd.ExecuteNonQuery();
                }
                if (!isEmpty(ev.Meeting_Places))
                {
                    string query = "UPDATE [dbo].[Events] SET event_meeting_places=@meeting_places  WHERE event_id=@id AND event_access_code=@code AND username=@manager";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"meeting_places", ev.Meeting_Places);
                    cmd.Parameters.AddWithValue(@"code", ev.AccessCode);
                    cmd.Parameters.AddWithValue(@"id", ev.Id);
                    cmd.Parameters.AddWithValue(@"manager", ev.Manager);
                    cmd.ExecuteNonQuery();
                }
                /*if (ev.Radius>0)
                {
                    string query = "UPDATE [dbo].[Events] SET event_radius=@radius  WHERE event_id=@id AND event_access_code=@code AND username=@manager";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"radius", ev.Radius);
                    cmd.Parameters.AddWithValue(@"code", ev.AccessCode);
                    cmd.Parameters.AddWithValue(@"id", ev.Id);
                    cmd.Parameters.AddWithValue(@"manager", ev.Manager);
                    cmd.ExecuteNonQuery();
                }*/
                if (!isEmpty(ev.Title))
                {
                    string query = "UPDATE [dbo].[Events] SET event_title=@title  WHERE event_id=@id AND event_access_code=@code AND username=@manager";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"title", ev.Title);
                    cmd.Parameters.AddWithValue(@"code", ev.AccessCode);
                    cmd.Parameters.AddWithValue(@"id", ev.Id);
                    cmd.Parameters.AddWithValue(@"manager", ev.Manager);
                    cmd.ExecuteNonQuery();
                }
                if (!isEmpty(ev.Manager))
                {
                    string query = "UPDATE [dbo].[Events] SET username=@manager  WHERE event_id=@id AND event_access_code=@code AND username=@manager";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"manager", ev.Manager);
                    cmd.Parameters.AddWithValue(@"code", ev.AccessCode);
                    cmd.Parameters.AddWithValue(@"id", ev.Id);
                    cmd.ExecuteNonQuery();
                }

                cmd.Dispose();
                conn.Close();

                return "Success";
            }
            catch(Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "updateEvent");
                return "Error:"+e.Message;
            }
        }

        public string addEvent(Event ev, int access_lvl)
        {
            if (access_lvl < CAN_EDIT_EVENTS)
                return "Error:You do not have the necessary permissions to execute this action.";

            try
            {
                if (ev.isValidForAdding())
                {
                    conn = new SqlConnection(dbConnectionString);
                    conn.Open();

                    string query = "INSERT INTO [dbo].[Events](event_title,event_description,event_address," +
                        "event_gps_location,event_access_code,event_date,event_meeting_places,event_end_date,username) " +
                        "VALUES(@title,@desc,@addr,@loc_gps,@acc_code,@event_date,@meeting_places,@end_date,@manager)";
                    cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue(@"title", ev.Title);
                    cmd.Parameters.AddWithValue(@"desc", ev.Description);
                    cmd.Parameters.AddWithValue(@"addr", ev.Address);
                    //cmd.Parameters.AddWithValue(@"radius", ev.Radius);
                    cmd.Parameters.AddWithValue(@"loc_gps", ev.Gps_location);
                    cmd.Parameters.AddWithValue(@"acc_code", ev.AccessCode);
                    cmd.Parameters.AddWithValue(@"event_date", ev.Date);
                    cmd.Parameters.AddWithValue(@"meeting_places", ev.Meeting_Places);
                    cmd.Parameters.AddWithValue(@"end_date", ev.End_Date);
                    cmd.Parameters.AddWithValue(@"manager", ev.Manager);


                    //cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    conn.Close();

                    return "Success";
                }else return "Error:Invalid Event. Remember that the access code has to be >= 4 chars and no fields can be null.";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "addEvent");
                return "Error:"+e.Message;
            }
        }

        public User getUser(string username)
        {
            
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Users] WHERE username=@username", conn);
                cmd.Parameters.AddWithValue(@"username", username);

                dataReader = cmd.ExecuteReader();
                User user = new User();
                while (dataReader.Read())
                {
                    user.Fname = (string)dataReader.GetValue(0);
                    user.Lname = (string)dataReader.GetValue(1);
                    user.Email = (string)dataReader.GetValue(2);
                    user.Username = (string)dataReader.GetValue(4);
                    user.Access_level = (int)dataReader.GetValue(5);
                    user.Age = (int)dataReader.GetValue(7);
                    user.Bio = (string)dataReader.GetValue(8);
                    user.Catchphrase = (string)dataReader.GetValue(9);
                    user.Occupation = (string)dataReader.GetValue(10);
                    user.Gender = (string)dataReader.GetValue(11);
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return user;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "getUser");
                /*return new User
                {
                    Fname = "<Error>",
                    Lname = e.Message
                };*/
                return null;
            }
            
        }

        public User getUserByFbId(string id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Users] WHERE fb_id=@id", conn);
                cmd.Parameters.AddWithValue(@"id", id);

                dataReader = cmd.ExecuteReader();
                User user = new User();
                while (dataReader.Read())
                {
                    user.Fname = (string)dataReader.GetValue(0);
                    user.Lname = (string)dataReader.GetValue(1);
                    user.Occupation = (string)dataReader.GetValue(10);
                    user.Access_level = (int)dataReader.GetValue(5);
                    user.Email = (string)dataReader.GetValue(2);
                    user.Username = (string)dataReader.GetValue(4);
                    user.Age = (int)dataReader.GetValue(7);
                    user.Bio = (string)dataReader.GetValue(8);
                    user.Gender = (string)dataReader.GetValue(11);
                    user.Catchphrase = (string)dataReader.GetValue(9);
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return user;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "getUserByFbId");
                /*return new User
                {
                    Fname = "<Error>",
                    Lname = e.Message
                };*/
                return null;
            }
            
        }

        public List<Event> getAllEvents()
        {
            int i = 0;
            List<Event> events = new List<Event>();
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Events]", conn);
                //cmd.Parameters.AddWithValue(@"id", id);
                //cmd.Parameters.AddWithValue(@"pwd", hashed_input_pwd);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    addError(0,"ID:"+ dataReader.GetValue(0) + ", sdate: "+ dataReader.GetValue(6) + ", edate: " + dataReader.GetValue(8),">getAllEvents");
                    events.Add(new Event()
                    {
                        Id =  Convert.ToUInt32(dataReader.GetValue(0)),
                        Title = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Address = (string)dataReader.GetValue(3),
                        //Radius = (int)dataReader.GetValue(4),
                        Gps_location = (string)dataReader.GetValue(4),
                        AccessCode = Convert.ToUInt16(dataReader.GetValue(5)),
                        Date = Convert.ToUInt32(dataReader.GetValue(6)),
                        Meeting_Places = (string)dataReader.GetValue(7),
                        End_Date = Convert.ToUInt32(dataReader.GetValue(8))
                    });
                    i++;
                }
                
                dataReader.Close();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "getAllEvents:" + i);
                //File.WriteAllLines(Path.Combine(HostingEnvironment.MapPath("~/logs/"), new DateTime()+".log"),new String[] { e.Message});
            }
            return events;
        }

       

        public string signIn(User user)
        {
            bool isValidUser = false;
            //Hash input to compare with hashed DB credentials
            //string hashed_input_usr = Hash.HashString(user.Username);
            string hashed_input_pwd = Hash.HashString(user.Password);

            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM dbo.Users WHERE username=@usr AND pwd=@pwd", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", user.Username);
                cmd.Parameters.AddWithValue(@"pwd", hashed_input_pwd);

                dataReader = cmd.ExecuteReader();
                dataReader.Read();

                if(dataReader.HasRows)
                {
                    isValidUser = true;
                }//else there was no user found with those credentials.

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "signIn");
                return e.Message;
            }
            return "isValidUser=" + isValidUser;
        }

        //Stats stuff
        public string getUsersIcebreakCount()
        {
            string graph_data="";
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM dbo.Users", conn);
                dataReader = cmd.ExecuteReader();

                List<string> users = new List<string>();

                while (dataReader.Read())
                {
                    string name = "";
                    string fname = Convert.IsDBNull(dataReader.GetValue(0)) ? "X" : Convert.ToString(dataReader.GetValue(0));
                    string lname = Convert.IsDBNull(dataReader.GetValue(1)) ? "X" : Convert.ToString(dataReader.GetValue(1));
                    string username = Convert.IsDBNull(dataReader.GetValue(4)) ? "X" : Convert.ToString(dataReader.GetValue(4));
                    
                    //Get rid of empties
                    fname = isEmpty(fname) ? "X" : fname;
                    lname = isEmpty(lname) ? "X" : lname;
                    if (fname.Equals("X") || lname.Equals("X"))
                        name = username;
                    else
                        name = fname + " " + lname;

                    users.Add(username + ";" + name);
                }
                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                foreach (string usr in users)
                {
                    string username = usr.Split(';')[0];
                    string name = usr.Split(';')[1];
                    graph_data += name + ":" + getUserIcebreakCount(username) + ";";
                }

                if (graph_data.Contains(";"))
                    return graph_data.Substring(0, graph_data.Length - 1);//remove last ;
                else return graph_data;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.ESTATS, e.Message, "getUsersIcebreakCount");
                return "Error:"+e.Message;
            }
        }

        public int getUserIcebreakCount(string username)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM dbo.Messages WHERE Message_sender=@usr AND Message_status>@stat", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while(dataReader.Read())
                {
                    count++;
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                return count;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.ESTATS, e.Message, "getUserIcebreakCount");
                return -1;
            }
        }

        //Metadata stuff
        public bool addMeta(Metadata metadata)
        {
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                string query = "INSERT INTO [dbo].[Metadata](Metadata_entry_name,Metadata_entry_data) VALUES(@entry_name,@entry_data)";
                SqlCommand cmd = new SqlCommand(query,conn);

                cmd.Parameters.AddWithValue(@"entry_name", metadata.Entry);
                cmd.Parameters.AddWithValue(@"entry_data", metadata.Meta);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
                return true;
            }
            catch(Exception e)
            {
                addError(ErrorCodes.EMETA, e.Message, "addMeta");
            }
            return false;
        }

        public bool updateMeta(Metadata meta)
        {
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                string query = "UPDATE [dbo].[Metadata] SET Metadata_entry_name=@entry,Metadata_entry_data=@data WHERE "+
                    "Metadata_entry_name=@entry";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue(@"entry",meta.Entry);
                cmd.Parameters.AddWithValue(@"data", meta.Meta);

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                return true;
            }
            catch(Exception e)
            {
                addError(ErrorCodes.EMETA, e.Message, "updateMeta");
            }
            return false;
        }

        public Metadata getMeta(string record)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query record
                cmd = new SqlCommand("SELECT * FROM [dbo].[Metadata] WHERE Metadata_entry_name=@entry", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"entry", record);
                
                dataReader = cmd.ExecuteReader();
                dataReader.Read();

                string entry="", meta="";
                var entr = dataReader.GetValue(0);
                if (Convert.IsDBNull(entr))
                {
                    return new Metadata() { Entry = "null", Meta = "Error:" + Convert.ToString(ErrorCodes.EDMD_NOT_SET) };
                }else entry = Convert.ToString(entr);
                var data = dataReader.GetValue(1);
                if (Convert.IsDBNull(data))
                {
                    return new Metadata() { Entry = "null", Meta = "Error:" + Convert.ToString(ErrorCodes.EDMD_NOT_SET) };
                }else meta = Convert.ToString(data);

                Metadata metadata = new Metadata();
                metadata.Entry = entry;
                metadata.Meta = meta;

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                return metadata;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EMETA, e.Message, "getMeta");
                return new Metadata() { Entry="null",Meta=e.Message};
            }
        }
    }
}