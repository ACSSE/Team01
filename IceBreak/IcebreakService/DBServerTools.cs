using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        public static int NORMAL_USR = 0;
        public static int SECOND_LVL_USR = 1;
        public static string NO_EMAIL = "<No email specified>";
        public static string NO_OCC = "<No occupation specified>";
        public static string NO_BIO = "<No bio specified>";
        public static string NO_PHRASE = "<No catchphrase specified>";
        public static string NO_GENDER = "Unspecified";
        public static string RW_CLAIMED = "CLAIMED";
        public static string SNAP_AT_EVENT_ID_NAME = "snap_at_event";

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

                    if(user.Access_level>0)
                    {
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET access_level=@lvl WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"lvl", user.Access_level);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Email!=null)
                    {
                        if (user.Email.Length <= 0)
                            user.Email = NO_EMAIL;
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET email=@email WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"email", user.Email);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Age > 0)
                    {
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET Age=@age WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"age", user.Age);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Occupation != null)
                    {
                        if (user.Occupation.Length <= 0)
                            user.Occupation = NO_OCC;
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET Occupation=@occupation WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"occupation", user.Occupation);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Bio != null)
                    {
                        if (user.Bio.Length <= 0)
                            user.Bio = NO_BIO;
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET Bio=@bio WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"bio", user.Bio);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Catchphrase != null)
                    {
                        if (user.Catchphrase.Length <= 0)
                            user.Catchphrase = NO_PHRASE;
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET Catchphrase=@cp WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"cp", user.Catchphrase);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Gender != null)
                    {
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET Gender=@gender WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"gender", user.Gender);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Event_id > 0)
                    {
                        //Update Users table
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET event_id=@id WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"id", user.Event_id);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();

                        //Add to bridging table
                        addUserEvent(user.Username, user.Event_id);
                    }
                    if (user.Fname != null)
                    {
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET fname=@fname WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"fname", user.Fname);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Lname != null)
                    {
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET lname=@lname WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"lname", user.Lname);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Password != null)
                    {
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET pwd=@pwd WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"pwd", Hash.HashString(user.Password));
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Pitch > 0)
                    {
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET [pitch]=@pitch WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"pitch", user.Pitch);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Fb_token != null)
                    {
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET fb_token=@token WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"token", user.Fb_token);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Fb_id != null)
                    {
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET fb_id=@id WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"id", user.Fb_id);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Points > 0)
                    {
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET Achievement_points=@pts WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"pts", user.Points);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Last_Seen > 0)
                    {
                        cmd = new SqlCommand("UPDATE [dbo].[Users] SET last_seen=@seen WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.Parameters.AddWithValue(@"seen", user.Last_Seen);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Username != null)
                    {
                        if (userExists(user).ToLower().Contains("exists=false"))//Make sure username has not been taken
                        {
                            cmd = new SqlCommand("UPDATE [dbo].[Users] SET username=@username WHERE username=@usr", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
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

        public string imageUpload(string name, byte[] data)
        {
            if (!String.IsNullOrEmpty(name) && data!=null)
            {
                if (name.Contains(";"))
                {
                    if (name[0] == ';')
                        name = name.Substring(1);//Remove first slash if it exists

                    //Write file data
                    string[] dirs = name.Split(';');//get directory structure
                    string dir = "";
                    for (int i = 0; i < dirs.Length - 1; i++)//last element would be the filename
                        dir += dirs[i] + '/';
                    var path = Path.Combine(HostingEnvironment.MapPath("~/images/" + dir), dirs[dirs.Length - 1]);//Path.Combine(@"C:\UploadedImages\" + name);
                    File.WriteAllBytes(path, data);

                    //Write Metadata
                    TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                    int since_epoch = (int)t.TotalSeconds;
                    Metadata meta = new Metadata()
                    {
                        Entry = (dir + dirs[dirs.Length - 1]).Replace("/", "|"),
                        Meta = "dmd=" + Convert.ToString(since_epoch)
                    };
                    addMeta(meta);
                    return "Success";
                }
                else return "Error: Invalid path.";
            }
            else
            {
                return "Error: Cannot write to root";
            }

        }
        public string deleteReward(string evntid)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("DELETE FROM [dbo].[Rewards] WHERE Event_id=@evntid", conn);
                cmd.Parameters.AddWithValue(@"evntid", evntid);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                return "Success";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "removeReward");
                return "Error:" + e.Message;
            }
        }
        public string deleteEvent(string evntid)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("DELETE FROM [dbo].[Events] WHERE event_id=@evntid", conn);
                cmd.Parameters.AddWithValue(@"evntid", evntid);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                return "Success";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "removeEvent");
                return "Error:" + e.Message;
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
                    messages.Add(new Message()
                    {
                        Message_id = Convert.ToString(dataReader.GetValue(0)),
                        Msg = Convert.ToString(dataReader.GetValue(1)),
                        Message_status = Convert.ToInt16(dataReader.GetValue(2)),
                        Message_sender = Convert.ToString(dataReader.GetValue(3)),
                        Message_receiver = Convert.ToString(dataReader.GetValue(4)),
                        Message_time = long.Parse(Convert.ToString(dataReader.GetValue(5)))
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
                        Message_time = long.Parse(Convert.ToString(dataReader.GetValue(5)))
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
        public List<Event> getEventsforUser(string username)
        {
            conn = new SqlConnection(dbConnectionString);

            List<Event> events = new List<Event>();
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Events] WHERE username=@username", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"username", username);

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    events.Add(new Event()
                    {
                        Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                        Title = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Address = (string)dataReader.GetValue(3),
                        Gps_location = (string)dataReader.GetValue(4),
                        AccessCode = int.Parse(Convert.ToString(dataReader.GetValue(5))),
                        Date = long.Parse(Convert.ToString(dataReader.GetValue(6))),
                        Meeting_Places = (string)dataReader.GetValue(7),
                        End_Date = long.Parse(Convert.ToString(dataReader.GetValue(8)))
                    });
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception e)
            {
                //File.WriteAllLines(Path.Combine(HostingEnvironment.MapPath("~/logs/"), new DateTime() + ".log"), new String[] { e.Message });
                addError(ErrorCodes.EUSR, e.Message, "getEventsforUser");
            }
            return events;
        }

        public List<User> getUsersAtEvent(long id)
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
                    //Get rid of NULLs
                    string fname = Convert.IsDBNull(dataReader.GetValue(0)) ? "X" : Convert.ToString((string)dataReader.GetValue(0));
                    string lname = Convert.IsDBNull(dataReader.GetValue(1)) ? "X" : Convert.ToString((string)dataReader.GetValue(1));
                    //string email = Convert.IsDBNull(dataReader.GetValue(2)) ? NO_EMAIL : Convert.ToString((string)dataReader.GetValue(2));
                    string username = Convert.ToString((string)dataReader.GetValue(4));

                    //int lvl = Convert.IsDBNull(dataReader.GetValue(5)) ? 0 : dataReader.GetInt32(5);
                    //long ev = Convert.IsDBNull(dataReader.GetValue(6)) ? 0 : dataReader.GetInt32(6);
                    int age = Convert.IsDBNull(dataReader.GetValue(7)) ? 0 : dataReader.GetInt32(7);

                    string bio = Convert.IsDBNull(dataReader.GetValue(8)) ? NO_BIO : Convert.ToString((string)dataReader.GetValue(8));
                    string catchphrase = Convert.IsDBNull(dataReader.GetValue(9)) ? NO_PHRASE : Convert.ToString((string)dataReader.GetValue(9));
                    string occupation = Convert.IsDBNull(dataReader.GetValue(10)) ? NO_OCC : Convert.ToString((string)dataReader.GetValue(10));
                    string gender = Convert.IsDBNull(dataReader.GetValue(11)) ? NO_GENDER : Convert.ToString((string)dataReader.GetValue(11));

                    long pts = Convert.IsDBNull(dataReader.GetValue(15)) ? 0 : dataReader.GetInt32(15);
                    String seen = Convert.IsDBNull(dataReader.GetValue(16)) ? "0" : dataReader.GetString(16);
                    double ls = Double.Parse(seen);//last seen date

                    string p = Convert.IsDBNull(dataReader.GetValue(17)) ? "0.0" : dataReader.GetString(17);
                    double pitch = double.Parse(p);

                    users.Add(new User
                    {
                        Fname = fname,
                        Lname = lname,
                        Username = username,
                        Occupation = occupation,
                        Age = age,
                        Bio = bio,
                        Gender = gender,
                        Catchphrase = catchphrase,
                        Points = pts,
                        Last_Seen = (long)Math.Ceiling(ls),
                        Pitch = pitch
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
                        string query = "UPDATE [dbo].[Messages] SET Message_status=@status,Message=@message,event_id=@ev_id WHERE Message_id=@message_id";
                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue(@"message_id", m.Message_id);
                        cmd.Parameters.AddWithValue(@"status", m.Message_status);
                        cmd.Parameters.AddWithValue(@"ev_id", m.Event_id);
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
                    string query = "INSERT INTO [dbo].[Messages] VALUES(@message_id,@message,@status,@sender,@receiver,@time,@ev_id)";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"message_id", m.Message_id);
                    cmd.Parameters.AddWithValue(@"message", m.Msg);
                    cmd.Parameters.AddWithValue(@"ev_id", m.Event_id);
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

        public Event getLastEvent()
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT TOP 1 * FROM dbo.Events ORDER BY event_id DESC", conn);

                dataReader = cmd.ExecuteReader();
                Event e = null;
                while (dataReader.Read())
                {
                    e = new Event()
                    {
                        Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                        Title = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Address = (string)dataReader.GetValue(3),
                        //Radius = (int)dataReader.GetValue(4),
                        Gps_location = (string)dataReader.GetValue(4),
                        AccessCode = int.Parse(Convert.ToString(dataReader.GetValue(5))),
                        Date = long.Parse(Convert.ToString(dataReader.GetValue(6))),
                        Meeting_Places = (string)dataReader.GetValue(7),
                        End_Date = long.Parse(Convert.ToString(dataReader.GetValue(8)))
                    };
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return e;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "getLastEvent");
                /*return new Event
                {
                    Title = "<Error>",
                    Description = e.Message
                };*/
                return null;
            }
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
                        Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                        Title = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Address = (string)dataReader.GetValue(3),
                        //Radius = (int)dataReader.GetValue(4),
                        Gps_location = (string)dataReader.GetValue(4),
                        AccessCode = int.Parse(Convert.ToString(dataReader.GetValue(5))),
                        Date = long.Parse(Convert.ToString(dataReader.GetValue(6))),
                        Meeting_Places = (string)dataReader.GetValue(7),
                        End_Date = long.Parse(Convert.ToString(dataReader.GetValue(8)))
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

        public Achievement getAchievement(long ach_id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Achievements] WHERE Achievement_id=@ach_id", conn);
                cmd.Parameters.AddWithValue(@"ach_id", ach_id);

                dataReader = cmd.ExecuteReader();
                Achievement a = null;
                if(dataReader.HasRows)
                {
                    dataReader.Read();
                    a = new Achievement()
                    {
                        Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                        Name = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Value = int.Parse(Convert.ToString(dataReader.GetValue(3))),
                        Target = int.Parse(Convert.ToString(dataReader.GetValue(4))),
                        Method = (string)dataReader.GetValue(5)
                    };
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return a;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EACH, e.Message, "getAchievement");
                return null;
            }
        }

        public string addUserAchievement(Achievement ach, string username)
        {
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                string query = "INSERT INTO [dbo].[User_Achievements](Achievement_id,username,date)" +
                    "VALUES(@id,@usr,@date)";
                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(@"id", ach.Id);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"date", (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();

                return "Success";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "addUserAchievement");
                return "Error:" + e.Message;
            }
        }

        #region Rewards
        public Reward getRewardForEvent(string event_id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Rewards] WHERE Event_id=@id", conn);
                cmd.Parameters.AddWithValue(@"id", event_id);

                dataReader = cmd.ExecuteReader();
                Reward reward = null;

                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    reward = new Reward()
                    {
                        Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                        Name = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Owner = (string)dataReader.GetValue(3),
                        Value = (int)dataReader.GetValue(4),
                        Event_ID = long.Parse(Convert.ToString(dataReader.GetValue(5)))
                    };
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return reward;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EREW, e.Message, "getRewardForEvent");
                return null;
            }
        }

        public List<Reward> getRewardsForEvent(string event_id)
        {
            List<Reward> rewards = null;
            conn = new SqlConnection(dbConnectionString);
            try
            {
                rewards = new List<Reward>();
                conn.Open();
                
                cmd = new SqlCommand("SELECT * FROM [dbo].[Rewards] WHERE Event_id=@id", conn);
                cmd.Parameters.AddWithValue(@"id", event_id);

                dataReader = cmd.ExecuteReader();
                Reward reward = null;

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        reward = new Reward()
                        {
                            Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                            Name = (string)dataReader.GetValue(1),
                            Description = (string)dataReader.GetValue(2),
                            Owner = (string)dataReader.GetValue(3),
                            Value = (int)dataReader.GetValue(4),
                            Event_ID = long.Parse(Convert.ToString(dataReader.GetValue(5)))
                        };
                        rewards.Add(reward);
                    }
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return rewards;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EREW, e.Message, "getRewardsForEvent");
                return null;
            }
        }

        public List<Reward> getUserRewardsAtEvent(string username, string event_id)
        {
            List<Reward> rewards = null;
            conn = new SqlConnection(dbConnectionString);
            try
            {
                rewards = new List<Reward>();
                conn.Open();
                
                cmd = new SqlCommand("SELECT * FROM [dbo].[User_Rewards] WHERE event_id=@id AND username=@usr", conn);
                cmd.Parameters.AddWithValue(@"id", event_id);
                cmd.Parameters.AddWithValue(@"usr", username);

                dataReader = cmd.ExecuteReader();

                Reward reward = null;

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        reward = new Reward();

                        long rw_id = long.Parse(Convert.ToString(dataReader.GetValue(1)));

                        reward.Id = rw_id;
                        reward.Code = Convert.ToString(dataReader.GetValue(3));
                        reward.Date = long.Parse(Convert.ToString(dataReader.GetValue(4)));
                        reward.Event_ID = long.Parse(Convert.ToString(dataReader.GetValue(5)));
                        reward.Owner = username;

                        rewards.Add(reward);
                    }
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                List<Reward> new_rewards = new List<Reward>();
                foreach (Reward rew in rewards)
                {
                    Reward rwd = getReward(rew.Id);
                    if (reward != null)
                    {
                        rwd.Code = rew.Code;
                        rwd.Date = rew.Date;
                        rwd.Event_ID = rew.Event_ID;
                        rwd.Owner = rew.Owner;

                        new_rewards.Add(rwd);
                    }
                }
                return new_rewards;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EREW, e.Message, "getUserRewardsAtEvent");
                return null;
            }
        }

        public Reward getUserReward(string username, string rw_id)
        {
            Reward reward = null;
            long id = 0;
            if (long.TryParse(rw_id, out id))
                reward = getReward(id);
            else return null;
            if (reward != null)
            {
                conn = new SqlConnection(dbConnectionString);
                try
                {
                    conn.Open();

                    cmd = new SqlCommand("SELECT * FROM [dbo].[User_Rewards] WHERE Reward_id=@id AND username=@usr", conn);
                    cmd.Parameters.AddWithValue(@"id", rw_id);
                    cmd.Parameters.AddWithValue(@"usr", username);

                    dataReader = cmd.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        dataReader.Read();
                        //reward = new Reward();

                        long rwd_id = long.Parse(Convert.ToString(dataReader.GetValue(1)));

                        reward.Id = rwd_id;
                        reward.Code = Convert.ToString(dataReader.GetValue(3));
                        reward.Date = long.Parse(Convert.ToString(dataReader.GetValue(4)));
                        reward.Event_ID = long.Parse(Convert.ToString(dataReader.GetValue(5)));
                        reward.Owner = username;
                    }

                    dataReader.Close();
                    cmd.Dispose();
                    conn.Close();

                    return reward;
                }
                catch (Exception e)
                {
                    addError(ErrorCodes.EREW, e.Message, "getUserReward");
                    return null;
                }
            }
            else return null;
        }

        public List<Reward> getRewardsCreatedByUser(string username)
        {
            List<Reward> rewards = null;
            conn = new SqlConnection(dbConnectionString);
            try
            {
                rewards = new List<Reward>();
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Rewards] WHERE Reward_owner=@owner", conn);
                cmd.Parameters.AddWithValue(@"owner", username);

                dataReader = cmd.ExecuteReader();
                Reward reward = new Reward();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        long rw_id = long.Parse(Convert.ToString(dataReader.GetValue(1)));

                        reward.Id = rw_id;
                        reward.Code = Convert.ToString(dataReader.GetValue(3));
                        reward.Date = long.Parse(Convert.ToString(dataReader.GetValue(4)));
                        reward.Event_ID = long.Parse(Convert.ToString(dataReader.GetValue(5)));
                        reward.Owner = username;

                        rewards.Add(reward);
                    }
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                List<Reward> new_rewards = new List<Reward>();
                foreach (Reward rew in rewards)
                {
                    reward = getReward(rew.Id);
                    if (reward != null)
                    {
                        reward.Code = rew.Code;
                        reward.Date = rew.Date;
                        reward.Event_ID = rew.Event_ID;
                        reward.Owner = rew.Owner;

                        new_rewards.Add(reward);
                    }
                }
                return new_rewards;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EREW, e.Message, "getRewardsCreatedByUser");
                return null;
            }
        }

        public List<Reward> getRewardsAtEvent(string event_id)
        {
            List<Reward> rewards = null;
            conn = new SqlConnection(dbConnectionString);
            try
            {
                rewards = new List<Reward>();
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Rewards] WHERE Event_id=@event_id", conn);
                cmd.Parameters.AddWithValue(@"event_id", event_id);

                dataReader = cmd.ExecuteReader();
                Reward reward = new Reward();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        reward = new Reward()
                        {
                            Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                            Name = (string)dataReader.GetValue(1),
                            Description = (string)dataReader.GetValue(2),
                            Owner = (string)dataReader.GetValue(3),
                            Value = (int)dataReader.GetValue(4)
                        };
                        rewards.Add(reward);
                    }
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                return rewards;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EREW, e.Message, "getRewardsAtEvent");
                return null;
            }
        }

        public Reward getReward(long rew_id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Rewards] WHERE Reward_id=@rew_id", conn);
                cmd.Parameters.AddWithValue(@"rew_id", rew_id);

                dataReader = cmd.ExecuteReader();
                Reward reward = new Reward();

                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    reward = new Reward()
                    {
                        Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                        Name = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Owner = (string)dataReader.GetValue(3),
                        Value = (int)dataReader.GetValue(4),
                        Event_ID = long.Parse(Convert.ToString(dataReader.GetValue(5)))
                    };
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return reward;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EREW, e.Message, "getReward");
                return null;
            }
        }

        public List<Reward> getUserPreparedRewardsAtEvent(string username, long event_id)
        {
            List<Reward> rewards=null;
            conn = new SqlConnection(dbConnectionString);
            try
            {
                rewards = new List<Reward>();
                conn.Open();
                
                cmd = new SqlCommand("SELECT * FROM [dbo].[User_Rewards] WHERE event_id=@id "
                    + "AND NOT Reward_code=@code AND username=@usr AND NOT Reward_code=0 AND NOT Reward_date=0", conn);
                cmd.Parameters.AddWithValue(@"id", event_id);
                cmd.Parameters.AddWithValue(@"code", RW_CLAIMED);
                cmd.Parameters.AddWithValue(@"usr", username);

                dataReader = cmd.ExecuteReader();
                Reward reward = null;

                if (dataReader.HasRows)
                {
                    while(dataReader.Read())
                    {
                        long rw_id = long.Parse(Convert.ToString(dataReader.GetValue(1)));

                        reward.Id = rw_id;
                        reward.Code = Convert.ToString(dataReader.GetValue(3));
                        reward.Date = long.Parse(Convert.ToString(dataReader.GetValue(4)));
                        reward.Event_ID = long.Parse(Convert.ToString(dataReader.GetValue(5)));
                        reward.Owner = username;

                        rewards.Add(reward);
                    }
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                List<Reward> new_rewards = new List<Reward>();
                foreach (Reward rew in rewards)
                {
                    reward = getReward(rew.Id);
                    if (reward != null)
                    {
                        reward.Code = rew.Code;
                        reward.Date = rew.Date;
                        reward.Event_ID = rew.Event_ID;
                        reward.Owner = rew.Owner;

                        new_rewards.Add(reward);
                    }
                }
                return new_rewards;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EREW, e.Message, "getUserPreparedRewardsAtEvent");
                return null;
            }
        }

        public List<Reward> getAllRewards()
        {
            List<Reward> rewards = new List<Reward>();
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Rewards]", conn);

                dataReader = cmd.ExecuteReader();
                Reward reward = null;
                while (dataReader.Read())
                {
                    reward = new Reward()
                    {
                        Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                        Name = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Owner = (string)dataReader.GetValue(3),
                        Value = (int)dataReader.GetValue(4)
                    };
                    rewards.Add(reward);
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return rewards;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EREW, e.Message, "getAllRewards");
                return null;
            }
        }

        public string updateReward(Reward rw, int access_lvl)
        {
            try
            {
                //Security checks
                if (access_lvl < CAN_EDIT_EVENTS)
                    return "Error:You do not have the necessary permissions to execute this action.";


                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                SqlCommand cmd = null;

                string q = "SELECT * FROM [dbo].[Rewards] WHERE Reward_id=@id";
                cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue(@"id", rw.Id);
                SqlDataReader readr = cmd.ExecuteReader();
                readr.Read();
                if (!readr.HasRows)
                {
                    //Clean up
                    cmd.Dispose();
                    readr.Close();
                    conn.Close();
                    return "Error:No Reward matching the provided id was found. ";
                }
                //Clean up
                cmd.Dispose();
                readr.Close();

                if (conn == null)
                    conn = new SqlConnection(dbConnectionString);
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();


                if (!isEmpty(rw.Name))
                {
                    string query = "UPDATE [dbo].[Rewards] SET Reward_name=@name  WHERE Reward_id=@id";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"id", rw.Id);
                    cmd.Parameters.AddWithValue(@"name", rw.Name);
                    cmd.ExecuteNonQuery();
                }
                if (!isEmpty(rw.Description))
                {
                    string query = "UPDATE [dbo].[Rewards] SET Reward_description=@descrip WHERE Reward_id=@id";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"id", rw.Id);
                    cmd.Parameters.AddWithValue(@"descrip", rw.Description);
                    cmd.ExecuteNonQuery();
                }
                if (!isEmpty(rw.Owner))
                {
                    string query = "UPDATE [dbo].[Rewards] SET Reward_owner=@owner  WHERE Reward_id=@id";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"id", rw.Id);
                    cmd.Parameters.AddWithValue(@"owner", rw.Owner);
                    cmd.ExecuteNonQuery();
                }
                if (rw.Event_ID>0)
                {
                    string query = "UPDATE [dbo].[Rewards] SET Event_id=@ev_id  WHERE Reward_id=@id";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"id", rw.Id);
                    cmd.Parameters.AddWithValue(@"ev_id", rw.Event_ID);
                    cmd.ExecuteNonQuery();
                }
                if (rw.Value >= 0)
                {
                    string query = "UPDATE [dbo].[Rewards] SET Reward_value=@value WHERE Reward_id=@id";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"id", rw.Id);
                    cmd.Parameters.AddWithValue(@"value", rw.Value);
                    cmd.ExecuteNonQuery();
                }


                cmd.Dispose();
                conn.Close();


                return "Success";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "updateReward");
                return "Error:" + e.Message;
            }
        }

        public string addReward(Reward rw, int access_lvl)
        {
            if (access_lvl < CAN_EDIT_EVENTS)
                return "Error:You do not have the necessary permissions to execute this action.";
            try
            {

                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                string query = "INSERT INTO [dbo].[Rewards](Reward_name,Reward_description,Reward_owner,Reward_value,Event_id)" +
                    "VALUES(@name,@descrip,@owner,@value,@eventid)";
                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(@"name", rw.Name);
                cmd.Parameters.AddWithValue(@"descrip", rw.Description);
                cmd.Parameters.AddWithValue(@"owner", rw.Owner);
                cmd.Parameters.AddWithValue(@"value", rw.Value);
                cmd.Parameters.AddWithValue(@"eventid", rw.Event_ID);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();

                return "Success";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "addReward");
                return "Error:" + e.Message;
            }
        }

        public string addUserReward(Reward rw)
        {
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                string query = "INSERT INTO [dbo].[User_Rewards](Reward_id,username,Reward_code,Reward_date,event_id)" +
                    "VALUES(@id,@usr,@code,@date,@event_id)";
                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(@"id", rw.Id);
                cmd.Parameters.AddWithValue(@"usr", rw.Owner);
                cmd.Parameters.AddWithValue(@"code", rw.Code);
                cmd.Parameters.AddWithValue(@"date", rw.Date);
                cmd.Parameters.AddWithValue(@"event_id", rw.Event_ID);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();

                return "Success";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "addUserReward");
                return "Error:" + e.Message;
            }
        }

        public string getImageIDsAtEvent(string event_id)
        {
            if (String.IsNullOrEmpty(event_id))
                return "Error: Empty event ID.";
            int id;
            if (!int.TryParse(event_id, out id))
                return "Error: Invalid event ID.";

            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query record
                cmd = new SqlCommand("SELECT * FROM [dbo].[Metadata] WHERE [Metadata_entry_data] LIKE @entry", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"entry", "%" + SNAP_AT_EVENT_ID_NAME + "="+ event_id + "%");

                dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    //Get image paths separated by a semi-colon
                    string meta = "";
                    while (dataReader.Read())
                    {
                        var file_path = dataReader.GetValue(1);
                        string[] attributes = file_path.ToString().Split(';');//get attributes
                        //look for filename
                        foreach (string attr in attributes)
                        {
                            if (attr.Contains("="))
                            {
                                string var = attr.Split('=')[0];
                                string val = attr.Split('=')[1];
                                if (var.ToLower().Equals("filename"))
                                {
                                    meta += val + ";";
                                    break;
                                }
                            }
                            else
                            {
                                return "Error: Attribute has no assignment ('=').";
                            }
                        }
                    }
                    //Some security checks
                    if (meta.Length <= 1)
                        return "Error: No records found."; 
                    //Remove last ';'
                    if (meta[meta.Length - 1] == ';') meta = meta.Substring(0, meta.Length - 1);

                    dataReader.Close();
                    cmd.Dispose();
                    conn.Close();

                    return meta;
                }else return "Error: No records found.";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EMETA, e.Message, "getMeta");
                return "Error: " + e.Message;
            }
        }

        public string claimReward(string username, string rw_id, string event_id, string code)
        {
            Reward rw = new Reward();
            rw.Id = long.Parse(rw_id);
            rw.Code = code;
            rw.Event_ID = long.Parse(event_id);
            rw.Owner = username;
            rw.Date = (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;

            List<Reward> rwds = getUserRewardsAtEvent(username, event_id);
            if (rwds == null)
                return addUserReward(rw);
            if(rwds.Count<=0)
                return addUserReward(rw);
            //Else update if Reward in user Rewards list
            foreach (Reward r in rwds)
                if (r.Id == long.Parse(rw_id))
                    return updateUserReward(username, r.Id, event_id, code);
            //Else is new Reward [not in user Rewards list]
            return addUserReward(rw);
        }

        public string updateUserReward(string username, long rw_id, string event_id, string code)
        {
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                /*string query = "UPDATE [dbo].[User_Rewards] SET Reward_code=@code WHERE Reward_id=@id AND username=@usr AND event_id=@ev_id";
                
                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(@"id", rw_id);
                cmd.Parameters.AddWithValue(@"ev_id", event_id);
                cmd.Parameters.AddWithValue(@"code", code);
                cmd.Parameters.AddWithValue(@"usr", username);

                cmd.ExecuteNonQuery();*/
                //
                string query = "UPDATE [dbo].[User_Rewards] SET Reward_code=@code, event_id=@ev_id WHERE Reward_id=@id AND username=@usr AND event_id=@ev_id";

                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(@"id", rw_id);
                cmd.Parameters.AddWithValue(@"ev_id", event_id);
                cmd.Parameters.AddWithValue(@"code", code);
                cmd.Parameters.AddWithValue(@"usr", username);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();

                if (code.Equals(RW_CLAIMED))
                {
                    //Get reward
                    Reward rwd = getReward(rw_id);
                    //Decrease user pts.
                    User u = getUser(username);
                    u.Points -= rwd.Value;
                    //Update user points
                    updateUserDetails(u);
                }

                return "Success";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "addReward");
                return "Error:" + e.Message;
            }
        }

        public string redeemReward(string username, string rw_id, string event_id, string code, string new_code)
        {
            List<Reward> rwds = getUserRewardsAtEvent(username, event_id);
            if (rwds != null)
            {
                bool found = false;
                foreach (Reward r in rwds)
                {
                    if (r.Code.Equals(code))
                    {
                        found = true;
                        break;
                    }
                }
                if (found)//Reward with matching code was found
                {
                    try
                    {
                        if (new_code.Equals(RW_CLAIMED))
                        {
                            //Get reward
                            Reward rwd = getReward(long.Parse(rw_id));
                            if (rwd != null)
                            {
                                //Decrease user pts.
                                User u = getUser(username);
                                if (u.Points >= rwd.Value)
                                {
                                    //Update User_Reward
                                    conn = new SqlConnection(dbConnectionString);
                                    conn.Open();

                                    string query = "UPDATE [dbo].[User_Rewards] SET Reward_code=@code, event_id=@ev_id WHERE Reward_id=@id AND username=@usr AND event_id=@ev_id";

                                    cmd = new SqlCommand(query, conn);

                                    cmd.Parameters.AddWithValue(@"id", rw_id);
                                    cmd.Parameters.AddWithValue(@"ev_id", event_id);
                                    cmd.Parameters.AddWithValue(@"code", new_code);
                                    cmd.Parameters.AddWithValue(@"usr", username);

                                    cmd.ExecuteNonQuery();

                                    cmd.Dispose();
                                    conn.Close();
                                    //Decrease user points
                                    u.Points -= rwd.Value;
                                    //Update user points
                                    updateUserDetails(u);
                                    //Send notification
                                    string notif = "{" +
                                    "\"data\": {" +
                                    "\"rwd_id\": \"" + rw_id + "\"}," +
                                    "\"to\": \"" + getUserToken(username) +
                                    "\"}";
                                    sendNotification(notif);
                                }
                                else
                                {
                                    string notif = "{" +
                                    "\"data\": {" +
                                    "\"rwd_id\": \"" + rw_id + "\"}," +
                                    "\"to\": \"" + getUserToken(username) +
                                    "\"}";
                                    sendNotification(notif);

                                    return "Error: Insufficient points.";
                                }
                            }
                            return "Successfully redeemed reward.";
                        }else return "Error: Reward not found.";
                    }
                    catch (Exception e)
                    {
                        addError(ErrorCodes.EEVENT, e.Message, "redeemReward");
                        return "Error:" + e.Message;
                    }
                }
                else
                {
                    return "Error: No rewards for this user at this event matching that code.";
                }
            }
            else
            {
                return "Error: No rewards for this user at this event.";
            }
        }

        #endregion Rewards

        public List<Achievement> getAllAchievements()
        {
            List<Achievement> achievements = new List<Achievement>();
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Achievements]", conn);

                dataReader = cmd.ExecuteReader();
                Achievement ach = null;
                while (dataReader.Read())
                {
                    ach = new Achievement()
                    {
                        Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                        Name = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Value = int.Parse(Convert.ToString(dataReader.GetValue(3))),
                        Target = int.Parse(Convert.ToString(dataReader.GetValue(4))),
                        Method = (string)dataReader.GetValue(5),
                        DateAchieved = 0
                    };
                    achievements.Add(ach);
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return achievements;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EACH, e.Message, "getAllAchievements");
                return null;
            }
        }

        public List<User> searchUser(string query)
        {
            List<User> users = null;
            conn = new SqlConnection(dbConnectionString);
            try
            {
                users = new List<User>();
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Users] WHERE username LIKE @q OR fname LIKE @q OR lname LIKE @q OR bio LIKE @q", conn);
                cmd.Parameters.AddWithValue(@"q", "%"+query +"%");

                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        User user = new User();

                        string fname = Convert.IsDBNull(dataReader.GetValue(0)) ? "X" : Convert.ToString((string)dataReader.GetValue(0));
                        string lname = Convert.IsDBNull(dataReader.GetValue(1)) ? "X" : Convert.ToString((string)dataReader.GetValue(1));
                        string email = Convert.IsDBNull(dataReader.GetValue(2)) ? NO_EMAIL : Convert.ToString((string)dataReader.GetValue(2));
                        string usr = Convert.IsDBNull(dataReader.GetValue(4)) ? "wtf?" : Convert.ToString((string)dataReader.GetValue(4));

                        int lvl = Convert.IsDBNull(dataReader.GetValue(5)) ? 0 : dataReader.GetInt32(5);
                        long ev = Convert.IsDBNull(dataReader.GetValue(6)) ? 0 : dataReader.GetInt32(6);
                        int age = Convert.IsDBNull(dataReader.GetValue(7)) ? 0 : dataReader.GetInt32(7);

                        string bio = Convert.IsDBNull(dataReader.GetValue(8)) ? NO_BIO : Convert.ToString((string)dataReader.GetValue(8));
                        string catchphrase = Convert.IsDBNull(dataReader.GetValue(9)) ? NO_PHRASE : Convert.ToString((string)dataReader.GetValue(9));
                        string occupation = Convert.IsDBNull(dataReader.GetValue(10)) ? NO_OCC : Convert.ToString((string)dataReader.GetValue(10));
                        string gender = Convert.IsDBNull(dataReader.GetValue(11)) ? NO_GENDER : Convert.ToString((string)dataReader.GetValue(11));

                        long pts = Convert.IsDBNull(dataReader.GetValue(15)) ? 0 : dataReader.GetInt32(15);
                        String seen = Convert.IsDBNull(dataReader.GetValue(16)) ? "0" : dataReader.GetString(16);
                        double ls = Double.Parse(seen);//last seen date

                        user.Fname = fname;
                        user.Lname = lname;
                        user.Email = email;
                        user.Username = usr;
                        user.Access_level = lvl;
                        user.Event_id = ev;
                        user.Age = age;
                        user.Bio = bio;
                        user.Catchphrase = catchphrase;
                        user.Occupation = occupation;
                        user.Gender = gender;
                        user.Points = pts;
                        user.Last_Seen = (long)Math.Ceiling(ls);

                        users.Add(user);
                    }
                }

                conn.Close();
                dataReader.Close();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "searchUser");
            }
            return users;
        }

        public List<Achievement> getUserAchievements(string username)
        {
            List<Achievement> achievements = new List<Achievement>();
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[User_Achievements] WHERE username=@usr", conn);
                cmd.Parameters.AddWithValue(@"usr", username);

                dataReader = cmd.ExecuteReader();
                Achievement ach = null;

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ach = new Achievement();

                        ach.Id = long.Parse(Convert.ToString(dataReader.GetValue(1)));
                        ach.DateAchieved = long.Parse(Convert.ToString(dataReader.GetValue(3)));
                        achievements.Add(ach);
                    }
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                //Get Achievement
                foreach (Achievement a in achievements)
                {
                    ach = getAchievement(a.Id);
                    a.Name = ach.Name;
                    a.Description = ach.Description;
                    a.Target = ach.Target;
                    a.Value = ach.Value;
                    a.Method = ach.Method;
                    //Get User Achievement points
                    int count = getUserAchievementPoints(a, username);
                    a.Pts = count;
                }

                //See if there are new Achievements
                /*List<Achievement> new_usr_achs = getNewUserAchievements(username);
                if (achievements.Count < new_usr_achs.Count)
                {
                    User usr = getUser(username);
                    //Has gotten new achievements - send notification
                    for (int i = achievements.Count; i < new_usr_achs.Count; i++)
                    {
                        string notif = "{" +
                                "\"data\": {" +
                                "\"Achievement_id\": \"" + new_usr_achs.ElementAt(i).Id + "\"}," +
                                "\"to\": \"" + getUserToken(username) + "\"}";
                        sendNotification(notif);

                        //TODO: Add to User_Achievements bridging table

                        //Increase User points
                        usr.Points += new_usr_achs.ElementAt(i).Value;
                    }
                    updateUserDetails(usr);//update user points
                }*/
                return achievements;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EACH, e.Message, "getUserAchievements");
                return null;
            }
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
            dataStream.Close();

            WebResponse response = req.GetResponse();

            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Save error to DB -- TODO: fix this
            if (!((HttpWebResponse)response).StatusCode.ToString().Contains("OK"))
                addError(ErrorCodes.ENOTIF, ((HttpWebResponse)response).StatusCode + ":>" + responseFromServer, "sendNotification");

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
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
                    m.Message_time = long.Parse(Convert.ToString(dataReader.GetValue(5)));
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

        public List<Event> getUserEventHistory(string username)
        {
            List<Event> events = null;
            conn = new SqlConnection(dbConnectionString);
            try
            {
                events = new List<Event>();
                conn.Open();
                //Get user's unread messages and Icebreaks
                cmd = new SqlCommand("SELECT DISTINCT * FROM [dbo].[User_Event] WHERE username=@usr", conn);
                cmd.Parameters.AddWithValue(@"usr", username);

                dataReader = cmd.ExecuteReader();
                List<string> ids = new List<string>();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        string id = Convert.ToString(dataReader.GetValue(2));
                        ids.Add(id);
                    }
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                foreach (string id in ids)
                    events.Add(getEvent(id));

                return events;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EMSG, e.Message, "getUserEventHistory");
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

        public string ping(string username)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                //Update last seen
                string last_seen = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);

                conn.Open();
                //Get user's unread messages and Icebreaks
                cmd = new SqlCommand("UPDATE [dbo].[Users] SET last_seen=@seen WHERE username=@usr", conn);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"seen", last_seen);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();

                return "Success";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EMSG, e.Message, "ping");
                return "Error: " + e.Message;
            }
        }

        public int getUserAchievementPoints(Achievement ach, string username)
        {
            int count = 0;
            switch (ach.Method.ToUpper())
            {
                case "A":
                    count = getUserIcebreakCount(username);
                    break;
                case "B":
                    count = getUserSuccessfulIcebreakCount(username);
                    break;
                case "C":
                    count = getMaxUserIcebreakCountAtOneEvent(username).Value;
                    break;
                case "D":
                    count = getMaxUserSuccessfulIcebreakCountAtOneEvent(username).Value;
                    break;
                case "E":
                    count = getUserIcebreaksXHoursApart(username, 2).Count;
                    break;
                case "F":
                    count = getUserSuccessfulIcebreaksXHoursApart(username, 2).Count;
                    break;
                case "AB"://get unsuccessful Icebreak count
                    count = getUserIcebreakCount(username) - getUserSuccessfulIcebreakCount(username);
                    break;
                default:
                    addError(ErrorCodes.EACH, "Unkown method '" + ach.Method.ToUpper() + "'", "updateUserAchievements");
                    break;
            }
            return count;
        }

        public List<Achievement> getNewUserAchievements(string username)
        {
            List<Achievement> achievements = getAllAchievements();
            List<Achievement> usr_achievements = getUserAchievements(username);

            foreach (Achievement ach in achievements)
            {
                bool ach_found = false;
                foreach (Achievement usr_ach in usr_achievements)
                {
                    if (ach.Id == usr_ach.Id)
                    {
                        ach_found = true;
                        break;
                    }
                }
                if (!ach_found)
                {
                    //current achievement not in user's list of achievements - check if eligible
                    int count = getUserAchievementPoints(ach, username);
                    //If user qualifies for achievement
                    if (count >= ach.Target)
                    {
                        //Add to list of achievements
                        ach.Pts = count;
                        usr_achievements.Add(ach);
                    }
                }
            }
            return usr_achievements;
        }

        public List<Message> getUserSuccessfulIcebreaksXHoursApart(string username, long hours)
        {
            List<Message> max_icebreaks_x_hours_apart = new List<Message>();
            List<Message> icebreaks_x_hours_apart = new List<Message>();
            List<Message> icebreaks = getUserSuccessfulIcebreaks(username);
            //insertionSortMessagesByTime(icebreaks);

            long interval = 60 * 60 * hours; //hours in seconds
            if (icebreaks != null)
            {
                if (icebreaks.Count > 0)
                {
                    for (int i = 0; i < icebreaks.Count; i++)
                    {
                        Message outer_ib = icebreaks.ElementAt(i);
                        icebreaks_x_hours_apart = new List<Message>();
                        //For each Icebreak
                        for (int j = 0; j < icebreaks.Count; j++)
                        {
                            //Look for other Icebreaks in the x hour range - yourself included
                            Message inner_ib = icebreaks.ElementAt(j);
                            //For each one, look for all Icebreaks within the 2 hour range
                            long min = outer_ib.Message_time;
                            long max = outer_ib.Message_time + interval;
                            if (inner_ib.Message_time >= min && inner_ib.Message_time <= max)
                            {
                                icebreaks_x_hours_apart.Add(inner_ib);
                            }
                        }
                        //If the max is less than inner max, replace max list items
                        if (max_icebreaks_x_hours_apart.Count < icebreaks_x_hours_apart.Count)
                        {
                            max_icebreaks_x_hours_apart = new List<Message>();
                            foreach (Message m in icebreaks_x_hours_apart)
                                max_icebreaks_x_hours_apart.Add(m);
                        }
                    }
                }
            }
            return max_icebreaks_x_hours_apart;
        }

        public List<Message> getUserIcebreaksXHoursApart(string username, long hours)
        {
            List<Message> max_icebreaks_x_hours_apart = new List<Message>();
            List<Message> icebreaks_x_hours_apart = new List<Message>();
            List<Message> icebreaks = getUserIcebreaks(username);
            //insertionSortMessagesByTime(icebreaks);

            //for (int i = 0; i < icebreaks.Count; i++)
            //    addError(123,"Sorted list[" + i + "], " + icebreaks.ElementAt(i).Message_time, "getMaxUserIcebreakCountInHours");

            long interval = 60 * 60 * hours; //hours in seconds
            if (icebreaks != null)
            {
                if (icebreaks.Count > 0)
                {
                    for (int i = 0; i < icebreaks.Count; i++)
                    {
                        Message outer_ib = icebreaks.ElementAt(i);
                        icebreaks_x_hours_apart = new List<Message>();
                        //For each Icebreak
                        for (int j = 0; j < icebreaks.Count; j++)
                        {
                            //Look for other Icebreaks in the x hour range - yourself included
                            Message inner_ib = icebreaks.ElementAt(j);
                            //For each one, look for all Icebreaks within the 2 hour range
                            long min = outer_ib.Message_time;
                            long max = outer_ib.Message_time + interval;
                            if (inner_ib.Message_time >= min && inner_ib.Message_time <= max)
                            {
                                icebreaks_x_hours_apart.Add(inner_ib);
                            }
                        }
                        //If the max is less than inner max, replace max list items
                        if(max_icebreaks_x_hours_apart.Count < icebreaks_x_hours_apart.Count)
                        {
                            max_icebreaks_x_hours_apart = new List<Message>();
                            foreach (Message m in icebreaks_x_hours_apart)
                                max_icebreaks_x_hours_apart.Add(m);
                        }
                    }
                }
            }
            return max_icebreaks_x_hours_apart;
        }

        public void insertionSortMessagesByTime(List<Message> messages)
        {
            for(int i = 1; i < messages.Count; i++)
            {
                int j = i;
                while (j > 0 && messages.ElementAt(i).Message_time < messages.ElementAt(j - 1).Message_time)
                {
                    swap(messages.ElementAt(i), messages.ElementAt(i - 1));
                    --j;
                }
            }
        }

        public void swap(Message a, Message b)
        {
            Message msg = a + b;//msg really is just b;
        }

        public KeyValuePair<Event,int> getMaxUserIcebreakCountAtOneEvent(string username)
        {
            //Get highest Icebreak count from all events the user has been to
            int max_ib_count = 0;
            Event max_ib_ev = null;
            List<Event> usr_evt_hist = getUserEventHistory(username);
            foreach (Event e in usr_evt_hist)
            {
                int temp_count = getUserIcebreakCountAtEvent(username, e.Id);
                if (temp_count > max_ib_count)
                {
                    max_ib_count = temp_count;
                    max_ib_ev = e;
                }
            }

            //Check if they qualify for it.
            /*if (max_ib_count >= ach.Target)
            {
                //They qualify
            }*/
            return new KeyValuePair<Event, int>(max_ib_ev,max_ib_count);
        }

        public KeyValuePair<Event, int> getMaxUserSuccessfulIcebreakCountAtOneEvent(string username)
        {
            //Get highest Icebreak count from all events the user has been to
            int max_ib_count = 0;
            Event max_ib_ev = null;
            List<Event> usr_evt_hist = getUserEventHistory(username);
            foreach (Event e in usr_evt_hist)
            {
                int temp_count = getUserSuccessfulIcebreakCountAtEvent(username, e.Id);
                if (temp_count > max_ib_count)
                {
                    max_ib_count = temp_count;
                    max_ib_ev = e;
                }
            }
            return new KeyValuePair<Event, int>(max_ib_ev, max_ib_count);
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

                //Update Metadata
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                ulong since_epoch = (ulong)t.TotalSeconds;
                Metadata meta = new Metadata()
                {
                    Entry = "event="+Convert.ToString(ev.Id),
                    Meta = "dmd=" + Convert.ToString(since_epoch)
                };
                updateMeta(meta);

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

                    //Write Metadata
                    List<Event> events = getAllEvents();
                    string eventId = "";
                    foreach (Event e in events)
                    {
                        //if(e.Title.Equals(ev.Title) && e.Gps_location.Equals(ev.Gps_location) && e.Radius==ev.Radius)
                        if (e.isEqualTo(ev))
                        {        
                            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                            ulong since_epoch = (ulong)t.TotalSeconds;
                            Metadata meta = new Metadata()
                            {
                                Entry = "event="+eventId,
                                Meta = "dmd=" + Convert.ToString(since_epoch)
                            };
                            addMeta(meta);
                            eventId = Convert.ToString(e.Id);
                        }
                    }

                    return "Success";
                }else return "Error:Invalid Event. Remember that the access code has to be >= 4 chars and no fields can be null.";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "addEvent");
                return "Error:"+e.Message;
            }
        }

        public string addUserEvent(string username, long event_id)
        {
            try
            {
                double now = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

                if(conn==null)
                    conn = new SqlConnection(dbConnectionString);
                if(conn.State==ConnectionState.Closed)
                    conn.Open();

                string query = "INSERT INTO [dbo].[User_Event](username,event_id,date) " +
                    "VALUES(@usr,@id,@date)";
                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"id", event_id);
                cmd.Parameters.AddWithValue(@"date", now);
                
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                //conn.Close();
                return "Success";
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "addUserEvent");
                return "Error:" + e.Message;
            }
        }

        public User getUser(string username)
        {
            User user = null;
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Users] WHERE username=@username", conn);
                cmd.Parameters.AddWithValue(@"username", username);

                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    user= new User();

                    while (dataReader.Read())
                    {
                        string fname = Convert.IsDBNull(dataReader.GetValue(0)) ? "X" : Convert.ToString((string)dataReader.GetValue(0));
                        string lname = Convert.IsDBNull(dataReader.GetValue(1)) ? "X" : Convert.ToString((string)dataReader.GetValue(1));
                        string email = Convert.IsDBNull(dataReader.GetValue(2)) ? NO_EMAIL : Convert.ToString((string)dataReader.GetValue(2));

                        int lvl = Convert.IsDBNull(dataReader.GetValue(5)) ? 0 : dataReader.GetInt32(5);
                        long ev = Convert.IsDBNull(dataReader.GetValue(6)) ? 0 : dataReader.GetInt32(6);
                        int age = Convert.IsDBNull(dataReader.GetValue(7)) ? 0 : dataReader.GetInt32(7);

                        string bio = Convert.IsDBNull(dataReader.GetValue(8)) ? NO_BIO : Convert.ToString((string)dataReader.GetValue(8));
                        string catchphrase = Convert.IsDBNull(dataReader.GetValue(9)) ? NO_PHRASE : Convert.ToString((string)dataReader.GetValue(9));
                        string occupation = Convert.IsDBNull(dataReader.GetValue(10)) ? NO_OCC : Convert.ToString((string)dataReader.GetValue(10));
                        string gender = Convert.IsDBNull(dataReader.GetValue(11)) ? NO_GENDER : Convert.ToString((string)dataReader.GetValue(11));

                        long pts = Convert.IsDBNull(dataReader.GetValue(15)) ? 0 : dataReader.GetInt32(15);
                        String seen = Convert.IsDBNull(dataReader.GetValue(16)) ? "0" : dataReader.GetString(16);
                        double ls = Double.Parse(seen);//last seen date

                        string p = Convert.IsDBNull(dataReader.GetValue(17)) ? "0.0" : dataReader.GetString(17);
                        double pitch = double.Parse(p);

                        user.Fname = fname;
                        user.Lname = lname;
                        user.Email = email;
                        user.Username = username;
                        user.Access_level = lvl;
                        user.Event_id = ev;
                        user.Age = age;
                        user.Bio = bio;
                        user.Catchphrase = catchphrase;
                        user.Occupation = occupation;
                        user.Gender = gender;
                        user.Points = pts;
                        user.Last_Seen = (long)Math.Ceiling(ls);
                        user.Pitch = pitch;
                    }
                }

                conn.Close();
                dataReader.Close();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "getUser");
            }
            return user;
        }

        public List<IBException> getExceptions()
        {
            List<IBException> exceptions = new List<IBException>();
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                string query = "SELECT * FROM [dbo].[Exceptions]";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            exceptions.Add(
                                        new IBException
                                        (
                                            int.Parse(Convert.ToString(reader.GetValue(0))),
                                            Convert.ToString(reader.GetValue(1)),
                                            Convert.ToString(reader.GetValue(2)),
                                            long.Parse(Convert.ToString(reader.GetValue(3)))
                                        ));
                        }
                    }
                    reader.Close();
                } else addError(ErrorCodes.EEXCEP, "Null data reader object.", "getExceptions");
                cmd.Dispose();
            }
            catch(Exception e)
            {
                addError(ErrorCodes.EEXCEP, e.Message, "getExceptions");
            }
            if (conn != null)
                conn.Close();
            return exceptions;
        }

        public static int getRandomNumber(int max)
        {
            double r = new Random().NextDouble();
            return Convert.ToInt32(Math.Floor(r * max));
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
                    //Get rid of NULLs
                    string fname = Convert.IsDBNull(dataReader.GetValue(0)) ? "X" : Convert.ToString((string)dataReader.GetValue(0));
                    string lname = Convert.IsDBNull(dataReader.GetValue(1)) ? "X" : Convert.ToString((string)dataReader.GetValue(1));
                    string email = Convert.IsDBNull(dataReader.GetValue(2)) ? NO_EMAIL : Convert.ToString((string)dataReader.GetValue(2));
                    string username = Convert.ToString((string)dataReader.GetValue(4));

                    int lvl = Convert.IsDBNull(dataReader.GetValue(5)) ? 0 : dataReader.GetInt32(5);
                    long ev = Convert.IsDBNull(dataReader.GetValue(6)) ? 0 : dataReader.GetInt32(6);
                    int age = Convert.IsDBNull(dataReader.GetValue(7)) ? 0 : dataReader.GetInt32(7);

                    string bio = Convert.IsDBNull(dataReader.GetValue(8)) ? NO_BIO : Convert.ToString((string)dataReader.GetValue(8));
                    string catchphrase = Convert.IsDBNull(dataReader.GetValue(9)) ? NO_PHRASE : Convert.ToString((string)dataReader.GetValue(9));
                    string occupation = Convert.IsDBNull(dataReader.GetValue(10)) ? NO_OCC : Convert.ToString((string)dataReader.GetValue(10));
                    string gender = Convert.IsDBNull(dataReader.GetValue(11)) ? NO_GENDER : Convert.ToString((string)dataReader.GetValue(11));

                    long pts = Convert.IsDBNull(dataReader.GetValue(15)) ? 0 : dataReader.GetInt32(15);
                    String seen = Convert.IsDBNull(dataReader.GetValue(16)) ? "0" : dataReader.GetString(16);
                    double ls = Double.Parse(seen);//last seen date
                    user.Fname = fname;
                    user.Lname = lname;
                    user.Email = email;
                    user.Username = username;
                    user.Access_level = lvl;
                    user.Event_id = ev;
                    user.Age = age;
                    user.Bio = bio;
                    user.Catchphrase = catchphrase;
                    user.Occupation = occupation;
                    user.Gender = gender;
                    user.Points = pts;
                    user.Last_Seen = (long)Math.Ceiling(ls);
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return user;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EUSR, e.Message, "getUserByFbId");
                return null;
            }
            
        }

        public List<Event> getAllEvents()
        {
            List<Event> events = new List<Event>();
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Events]", conn);
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    events.Add(new Event()
                    {
                        Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                        Title = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Address = (string)dataReader.GetValue(3),
                        //Radius = (int)dataReader.GetValue(4),
                        Gps_location = (string)dataReader.GetValue(4),
                        AccessCode = int.Parse(Convert.ToString(dataReader.GetValue(5))),
                        Date = long.Parse(Convert.ToString(dataReader.GetValue(6))),
                        Meeting_Places = (string)dataReader.GetValue(7),
                        End_Date = long.Parse(Convert.ToString(dataReader.GetValue(8)))
                        
                    });
                }
                
                dataReader.Close();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "getAllEvents");
                //File.WriteAllLines(Path.Combine(HostingEnvironment.MapPath("~/logs/"), new DateTime()+".log"),new String[] { e.Message});
            }
            return events;
        }

        public List<Event> getAllSearchedEvents(string result)
        {
            List<Event> events = new List<Event>();
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM [dbo].[Events] WHERE event_title LIKE @res OR event_address LIKE @res", conn);
                cmd.Parameters.AddWithValue("@res", "%" + result + "%");


                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    events.Add(new Event()
                    {
                        Id = long.Parse(Convert.ToString(dataReader.GetValue(0))),
                        Title = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Address = (string)dataReader.GetValue(3),
                        Gps_location = (string)dataReader.GetValue(4),
                        AccessCode = int.Parse(Convert.ToString(dataReader.GetValue(5))),
                        Date = long.Parse(Convert.ToString(dataReader.GetValue(6))),
                        Meeting_Places = (string)dataReader.GetValue(7),
                        End_Date = long.Parse(Convert.ToString(dataReader.GetValue(8)))

                    });
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EEVENT, e.Message, "getAllSearchedEvents");
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

        public long getUserEventId(string username)
        {
            long id = 0;
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Users] WHERE username=@user", conn);
                cmd.Parameters.AddWithValue(@"user", username);

                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    id = long.Parse(Convert.ToString(dataReader.GetValue(6)));
                }
                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                return id;
            }
            catch (Exception ex)
            {
                addError(ErrorCodes.EUSR, ex.Message, "getUserEventId");
                return -1;
            }
        }

        #region Statistics
        /*public int getAllIcebreakCount()
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
        }*/

        /******Master Stats************/
        public int getTotalIcebreakCount()
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_status>@stat", conn);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getTotalIcebreakCount");
                return -1;
            }
        }

        public int getTotalSuccessfulIcebreakCount()
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_status>@stat AND NOT Message=@msg", conn);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                cmd.Parameters.AddWithValue(@"msg", "ICEBREAK");

                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getTotalSuccessfulIcebreakCount");
                return -1;
            }
        }

        public int getTotalIcebreakCountBetweenTime(long start, long end)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_time>=@start AND Message_time<=@end AND "
                    + "Message_status>@stat", conn);
                cmd.Parameters.AddWithValue(@"start", start);
                cmd.Parameters.AddWithValue(@"end", end);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getTotalIcebreakCountBetweenTime");
                return -1;
            }
        }

        public int getTotalSuccessfulIcebreakCountBetweenTime(long start, long end)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_time>=@start AND Message_time<=@end AND "
                    + "Message_status>@stat AND NOT Message=@msg", conn);
                cmd.Parameters.AddWithValue(@"start", start);
                cmd.Parameters.AddWithValue(@"end", end);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                cmd.Parameters.AddWithValue(@"msg", "ICEBREAK");
                
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getTotalIcebreakCountBetweenTime");
                return -1;
            }
        }

        /******User Stats**************/
        public int getUserIcebreakCount(string username)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                
                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_sender=@usr AND Message_status>@stat", conn);
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

        public List<Message> getUserIcebreaks(string username)
        {
            conn = new SqlConnection(dbConnectionString);
            List<Message> icebreaks = new List<Message>();
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_sender=@usr AND Message_status>@stat", conn);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Message m = new Message();

                    m.Message_id = Convert.ToString(dataReader.GetValue(0));
                    m.Msg = Convert.ToString(dataReader.GetValue(1));
                    m.Message_status = Convert.ToInt16(dataReader.GetValue(2));
                    m.Message_sender = Convert.ToString(dataReader.GetValue(3));
                    m.Message_receiver = Convert.ToString(dataReader.GetValue(4));
                    m.Message_time = long.Parse(Convert.ToString(dataReader.GetValue(5)));

                    icebreaks.Add(m);
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                return icebreaks;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.ESTATS, e.Message, "getUserIcebreaks");
                return null;
            }
        }

        public int getUserSuccessfulIcebreakCount(string username)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_sender=@usr AND Message_status>@stat AND NOT Message=@msg", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                cmd.Parameters.AddWithValue(@"msg", "ICEBREAK");
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getAllSuccessfulUserIcebreakCount");
                return -1;
            }
        }

        public List<Message> getUserSuccessfulIcebreaks(string username)
        {
            List<Message> icebreaks = new List<Message>();
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_sender=@usr AND Message_status>@stat AND NOT Message=@msg", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                cmd.Parameters.AddWithValue(@"msg", "ICEBREAK");
                
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Message m = new Message();

                    m.Message_id = Convert.ToString(dataReader.GetValue(0));
                    m.Msg = Convert.ToString(dataReader.GetValue(1));
                    m.Message_status = Convert.ToInt16(dataReader.GetValue(2));
                    m.Message_sender = Convert.ToString(dataReader.GetValue(3));
                    m.Message_receiver = Convert.ToString(dataReader.GetValue(4));
                    m.Message_time = long.Parse(Convert.ToString(dataReader.GetValue(5)));

                    icebreaks.Add(m);
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                return icebreaks;
            }
            catch (Exception e)
            {
                addError(ErrorCodes.ESTATS, e.Message, "getAllSuccessfulUserIcebreakCount");
                return null;
            }
        }

        public int getUserIcebreakCountAtEvent(string username, long event_id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_sender=@usr AND Message_status>@stat AND "
                                        + "event_id=@id", conn);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                cmd.Parameters.AddWithValue(@"id", event_id);
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getAllSuccessfulUserIcebreakCountAtEvent");
                return -1;
            }
        }

        public int getUserSuccessfulIcebreakCountAtEvent(string username, long event_id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_sender=@usr AND Message_status>@stat AND "
                                        + "NOT Message=@msg AND event_id=@id", conn);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                cmd.Parameters.AddWithValue(@"msg", "ICEBREAK");
                cmd.Parameters.AddWithValue(@"id", event_id);
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getAllSuccessfulUserIcebreakCountAtEvent");
                return -1;
            }
        }

        public int getUserIcebreakCountBetweenTime(string username, long start, long end)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_time>=@start AND Message_time<=@end AND "
                    + "Message_status>@stat AND Message_sender=@usr", conn);
                cmd.Parameters.AddWithValue(@"start", start);
                cmd.Parameters.AddWithValue(@"end", end);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getAllSuccessfulUserIcebreakCountBetweenTime");
                return -1;
            }
        }

        public int getUserSuccessfulIcebreakCountBetweenTime(string username, long start, long end)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                /*cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_sender=@usr AND Message_status>@stat AND "
                        +"NOT Message=@msg", conn);*/
                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_time>=@start AND Message_time<=@end AND "
                    + "Message_status>@stat AND Message_sender=@usr AND NOT Message=@msg", conn);
                cmd.Parameters.AddWithValue(@"start", start);
                cmd.Parameters.AddWithValue(@"end", end);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                cmd.Parameters.AddWithValue(@"msg", "ICEBREAK");
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getAllSuccessfulUserIcebreakCountBetweenTime");
                return -1;
            }
        }

        public int getUserIcebreakCountBetweenTimeAtEvent(string username, long start, long end, long event_id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                
                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_time>=@start AND Message_time<=@end AND "
                    + "Message_status>@stat AND Message_sender=@usr AND event_id=@id", conn);
                cmd.Parameters.AddWithValue(@"start", start);
                cmd.Parameters.AddWithValue(@"end", end);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                cmd.Parameters.AddWithValue(@"id", event_id);
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getUserIcebreakCountBetweenTimeAtEvent");
                return -1;
            }
        }

        public int getUserSuccessfulIcebreakCountBetweenTimeAtEvent(string username, long start, long end, long event_id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                /*cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_sender=@usr AND Message_status>@stat AND "
                        +"NOT Message=@msg", conn);*/
                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_time>=@start AND Message_time<=@end AND "
                    + "Message_status>@stat AND Message_sender=@usr AND NOT Message=@msg AND event_id=@id", conn);
                cmd.Parameters.AddWithValue(@"start", start);
                cmd.Parameters.AddWithValue(@"end", end);
                cmd.Parameters.AddWithValue(@"usr", username);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                cmd.Parameters.AddWithValue(@"id", event_id);
                cmd.Parameters.AddWithValue(@"msg", "ICEBREAK");
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getUserSuccessfulIcebreakCountBetweenTimeAtEvent");
                return -1;
            }
        }

        /******Event Stats************/
        public int getEventIcebreakCount(long event_id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                
                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE event_id=@event_id AND Message_status>@stat", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"event_id", event_id);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getEventIcebreakCount");
                return -1;
            }
        }

        public int getEventIcebreakCountBetweenTime(long id, long start, long end)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                
                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_time>=@start AND Message_time<=@end AND "
                    + "Message_status>@stat AND event_id=@id", conn);
                cmd.Parameters.AddWithValue(@"id", id);
                cmd.Parameters.AddWithValue(@"start", start);
                cmd.Parameters.AddWithValue(@"end", end);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getEventIcebreakCountBetweenTime");
                return -1;
            }
        }

        public int getEventSuccessfulIcebreakCountBetweenTime(long id, long start, long end)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_time>=@start AND Message_time<=@end AND "
                    + "Message_status>@stat AND event_id=@id AND NOT Message=@msg", conn);
                cmd.Parameters.AddWithValue(@"id", id);
                cmd.Parameters.AddWithValue(@"start", start);
                cmd.Parameters.AddWithValue(@"end", end);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                cmd.Parameters.AddWithValue(@"msg", "ICEBREAK");
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getEventSuccessfulIcebreakCountBetweenTime");
                return -1;
            }
        }

        public int getEventUnsuccessfulIcebreakCountBetweenTime(long id, long start, long end)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();

                cmd = new SqlCommand("SELECT * FROM [dbo].[Messages] WHERE Message_time>=@start AND Message_time<=@end AND "
                    + "Message_status>@stat AND event_id=@id AND Message=@msg", conn);
                cmd.Parameters.AddWithValue(@"id", id);
                cmd.Parameters.AddWithValue(@"start", start);
                cmd.Parameters.AddWithValue(@"end", end);
                cmd.Parameters.AddWithValue(@"stat", ICEBREAK);
                cmd.Parameters.AddWithValue(@"msg", "ICEBREAK");
                int count = 0;
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
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
                addError(ErrorCodes.ESTATS, e.Message, "getEventSuccessfulIcebreakCountBetweenTime");
                return -1;
            }
        }

        #endregion Statistics

        #region Metadata
        public bool addMeta(Metadata metadata)
        {
            //Some security checks
            if (metadata == null)
                return false;
            if (String.IsNullOrEmpty(metadata.Entry) || String.IsNullOrEmpty(metadata.Meta))
                return false;
            if ( metadata.Entry.ToLower().Equals("null") || metadata.Meta.ToLower().Equals("null"))
                return false;

            try
            {
                Metadata m = getMeta(metadata.Entry);
                if (m.Entry.ToLower().Equals("null") || m.Meta.ToLower().Equals("null"))
                {
                    conn = new SqlConnection(dbConnectionString);
                    conn.Open();

                    string query = "INSERT INTO [dbo].[Metadata](Metadata_entry_name,Metadata_entry_data) VALUES(@entry_name,@entry_data)";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue(@"entry_name", metadata.Entry);
                    cmd.Parameters.AddWithValue(@"entry_data", metadata.Meta);

                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    conn.Close();
                    return true;
                }
                else updateMeta(metadata);
            }
            catch(Exception e)
            {
                addError(ErrorCodes.EMETA, e.Message, "addMeta");
            }
            return false;
        }

        public bool updateMeta(Metadata meta)
        {
            //Some security checks
            if (meta == null)
                return false;
            if (String.IsNullOrEmpty(meta.Entry) || String.IsNullOrEmpty(meta.Meta))
                return false;
            if (meta.Entry.ToLower().Equals("null") || meta.Meta.ToLower().Equals("null"))
                return false;

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
            if (String.IsNullOrEmpty(record))
                return new Metadata() { Entry="null", Meta="null"};

            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query record
                cmd = new SqlCommand("SELECT * FROM [dbo].[Metadata] WHERE Metadata_entry_name=@entry", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"entry", record);
                
                dataReader = cmd.ExecuteReader();
                dataReader.Read();
                if (dataReader.HasRows)
                {
                    string entry = "", meta = "";
                    var entr = dataReader.GetValue(0);
                    if (Convert.IsDBNull(entr))
                    {
                        return new Metadata() { Entry = "null", Meta = "Error:" + Convert.ToString(ErrorCodes.EDMD_NOT_SET) };
                    }
                    else entry = Convert.ToString(entr);
                    var data = dataReader.GetValue(1);
                    if (Convert.IsDBNull(data))
                    {
                        return new Metadata() { Entry = "null", Meta = "Error:" + Convert.ToString(ErrorCodes.EDMD_NOT_SET) };
                    }
                    else meta = Convert.ToString(data);

                    Metadata metadata = new Metadata();
                    metadata.Entry = entry;
                    metadata.Meta = meta;

                    dataReader.Close();
                    cmd.Dispose();
                    conn.Close();

                    return metadata;
                }
                else return new Metadata() { Entry = "null", Meta = "null" };
            }
            catch (Exception e)
            {
                addError(ErrorCodes.EMETA, e.Message, "getMeta");
                return new Metadata() { Entry="null",Meta=e.Message};
            }
        }
        #endregion Metadata
    }
}