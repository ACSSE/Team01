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
        public static string NO_EMAIL = "<No email specified>";
        public static string NO_OCC = "<No occupation specified>";
        public static string NO_BIO = "<No bio specified>";
        public static string NO_PHRASE = "<No catchphrase specified>";

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
                    //TODO: Store exception to logs
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
                //TODO: Store exception to logs
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
                cmd = new SqlCommand("SELECT * FROM dbo.Users WHERE username=@usr", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
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
                //TODO: Store exception to logs
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
                cmd = new SqlCommand("DELETE FROM dbo.Users WHERE username=@usr", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", handle);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                return "Success";
            }
            catch (Exception e)
            {
                //TODO: Store exception to logs
                return e.Message;
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
                cmd = new SqlCommand("SELECT * FROM [Messages] WHERE Message_receiver=@usr AND NOT "
                    + "Message_status=@read AND NOT "
                    + "Message_status=@done", conn);
                cmd.Parameters.AddWithValue(@"read", READ);
                cmd.Parameters.AddWithValue(@"done", ICEBREAK_DONE);
                cmd.Parameters.AddWithValue(@"usr", user);
                
                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string date = Convert.ToString(dataReader.GetValue(5));
                    date = date.Replace('/', '-');
                    date = date.Replace(':', '-');
                    messages.Add(new Message()
                    {
                        Message_id = Convert.ToString(dataReader.GetValue(0)),
                        Message_receiver = Convert.ToString(dataReader.GetValue(4)),
                        Message_sender = Convert.ToString(dataReader.GetValue(3)),
                        Message_time = date,
                        Message_status = Convert.ToInt16(dataReader.GetValue(2)),
                        Msg = Convert.ToString(dataReader.GetValue(1)),
                    });
                }
                
                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return messages;
            }
            catch (Exception e)
            {
                //TODO: Store exception to logs
                messages.Add(new Message()
                {
                    Message_receiver = Convert.ToString("YOU["+user+"]"),
                    Message_sender = Convert.ToString("SERVER"),
                    Message_time = Convert.ToString(DateTime.Now),
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
                cmd = new SqlCommand("SELECT * FROM [Messages] WHERE Message_sender=@sender "
                    + "AND NOT Message_status=@read "
                    + "AND NOT Message_status=@done", conn);
                /*cmd.Parameters.AddWithValue(@"read", READ);
                cmd.Parameters.AddWithValue(@"done", ICEBREAK_DONE);
                cmd.Parameters.AddWithValue(@"receiver", receiver);*/
                cmd.Parameters.AddWithValue(@"sender", sender);
                cmd.Parameters.AddWithValue(@"read", sender);
                cmd.Parameters.AddWithValue(@"done", sender);

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    string date = Convert.ToString(dataReader.GetValue(5));
                    date = date.Replace('/', '-');
                    date = date.Replace(':', '-');
                    messages.Add(new Message()
                    {
                        Message_id = Convert.ToString(dataReader.GetValue(0)),
                        Message_receiver = Convert.ToString(dataReader.GetValue(4)),
                        Message_sender = Convert.ToString(dataReader.GetValue(3)),
                        Message_time = date,
                        Message_status = Convert.ToInt16(dataReader.GetValue(2)),
                        Msg = Convert.ToString(dataReader.GetValue(1))
                    });
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return messages;
            }
            catch (Exception e)
            {
                //TODO: Store exception to logs
                messages.Add(new Message()
                {
                    Message_receiver = Convert.ToString("YOU[" + sender + "]"),
                    Message_sender = Convert.ToString("SERVER"),
                    Message_time = Convert.ToString(DateTime.Now),
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
                cmd = new SqlCommand("SELECT user_token FROM dbo.Users WHERE username=@usr", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", username);

                dataReader = cmd.ExecuteReader();
                dataReader.Read();

                token = Convert.ToString(dataReader.GetValue(0));

                return token;
            }
            catch (Exception e)
            {
                return "Exception: " + e.Message;
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
                cmd = new SqlCommand("SELECT * FROM dbo.Users WHERE event_id=@id", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
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
                //TODO: Store exception to logs
                return new List<User>
                { new User
                    {
                        Fname = "<Error>",
                        Lname = e.Message
                    }
                };
            }
        }

        public int msgIdExists(string id, SqlConnection conn)
        {
            /*try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();*/
            if (conn != null)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    string query = "SELECT * FROM [Messages] WHERE Message_id=@message_id";
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
                    }
                    else
                        return DEXISTS;
                }
                else
                {
                    return CONN_CLOSED;
                }
            }
            else
            {
                return CONN_CLOSED;
            }
            /*}
            catch (Exception e)
            {
                //TODO: Logging
                return false;
            }*/
        }

        public string setUniqueUserToken(string username, string token)
        {
            if (userExists(new User() { Username = username }).ToLower().Contains("exists=true"))
            {
                //update user entry - add id
                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                string query = "UPDATE dbo.Users SET user_token=@token WHERE username=@user";
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
        }

        public string addError(int code, string error, string method)
        {
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();

                string query = "INSERT INTO dbo.Exceptions VALUES(@ex_code,@ex_msg,@ex_method)";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue(@"ex_code", code);
                cmd.Parameters.AddWithValue(@"ex_msg", error);
                cmd.Parameters.AddWithValue(@"ex_method", method);

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                return "SINS";
            }
            catch (Exception e)
            {
                return "Exception: " + e.Message;
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
                        string query = "UPDATE [Messages] SET Message_status=@status WHERE Message_id=@message_id";
                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue(@"message_id", m.Message_id);
                        cmd.Parameters.AddWithValue(@"status", m.Message_status);

                        //cmd.Prepare();
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
                    string query = "INSERT INTO [Messages] VALUES(@message_id,@message,@status,@sender,@receiver,@time)";
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
                    return "Success: Inserted";
                }
                else if(msgState == CONN_CLOSED)
                {
                    return "Exception: Connection closed.";
                }
            }
            catch (Exception e)
            {
                return "Exception: " + e.Message;
            }
            return "Exception: Unknown Response.";
        }

        public Event getEvent(string event_id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM dbo.Events WHERE event_id=@event_id", conn);
                cmd.Parameters.AddWithValue(@"event_id", event_id);

                dataReader = cmd.ExecuteReader();
                Event e = null;
                while (dataReader.Read())
                {
                    e = new Event()
                    {
                        Id = (int)dataReader.GetValue(0),
                        Title = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Address = (string)dataReader.GetValue(3),
                        Radius = (int)dataReader.GetValue(4),
                        Gps_location = (string)dataReader.GetValue(5),
                        AccessCode = (int)dataReader.GetValue(6),
                        Date = (string)dataReader.GetValue(7),
                        Time = (string)dataReader.GetValue(8)
                    };
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();
                return e;
            }
            catch (Exception e)
            {
                return new Event
                {
                    Title = "<Error>",
                    Description = e.Message
                };
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
                cmd = new SqlCommand("SELECT * FROM dbo.Messages WHERE Message_id=@id", conn);
                cmd.Parameters.AddWithValue(@"id", msg_id);

                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    m = new Message();
                    dataReader.Read();
        
                    string date = Convert.ToString(dataReader.GetValue(5));
                    date = date.Replace('/', '-');
                    date = date.Replace(':', '-');

                    m.Message_id = Convert.ToString(dataReader.GetValue(0));
                    m.Message_receiver = Convert.ToString(dataReader.GetValue(4));
                    m.Message_sender = Convert.ToString(dataReader.GetValue(3));
                    m.Message_time = date;
                    m.Message_status = Convert.ToInt16(dataReader.GetValue(2));
                    m.Msg = Convert.ToString(dataReader.GetValue(1));
                }

                dataReader.Close();
                cmd.Dispose();
                conn.Close();

                return m;
            }
            catch (Exception e)
            {
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
               return "Error: Empty username";

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
                        return "Error: Empty password";
                    if (isEmpty(user.Username))
                        return "Error: Empty username";
                    if (isEmpty(user.Gender))
                        return "Error: Empty gender";
                    if (isEmpty(user.Email))
                        user.Email = NO_EMAIL;
                    if (isEmpty(user.Catchphrase))
                        user.Catchphrase = NO_PHRASE;
                    if (isEmpty(user.Bio))
                        user.Bio = NO_BIO;
                    if (isEmpty(user.Occupation))
                        user.Occupation = NO_OCC;
                    if (isEmpty(user.Fname))
                        user.Fname = " ";
                    if (isEmpty(user.Lname))
                        user.Lname = " ";

                    cmd.Parameters.AddWithValue(@"fname", user.Fname);
                    cmd.Parameters.AddWithValue(@"lname", user.Lname);
                    cmd.Parameters.AddWithValue(@"email", user.Email);
                    cmd.Parameters.AddWithValue(@"password", Hash.HashString(user.Password));
                    cmd.Parameters.AddWithValue(@"username", user.Username);//Hash.HashString(user.Username));
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
                    return e.Message;
                }
            }
            else//User exists or there was an exception
            {
                if (exist_check.ToLower().Contains("exists=true"))
                {
                    return updateUserDetails(user);//update user info instead
                }
                return exist_check;//most likely an exception was thrown if it gets here
            }
        }

        public string addEvent(Event ev)
        {
            ev.Radius = 0;
            ev.Gps_location = "0";
            ev.AccessCode = 12;
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();
                string query = "INSERT INTO dbo.Events(event_title,event_description,event_address,event_radius,event_gps_location,access_id,date,time) VALUES(@title,@desc,@addr,@radius,@loc_gps,@acc_id,@date,@time)";
                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(@"title", ev.Title);
                cmd.Parameters.AddWithValue(@"desc", ev.Description);
                cmd.Parameters.AddWithValue(@"addr", ev.Address);//Hash.HashString(user.Username));
                cmd.Parameters.AddWithValue(@"radius", ev.Radius);
                cmd.Parameters.AddWithValue(@"loc_gps", ev.Gps_location);
                cmd.Parameters.AddWithValue(@"acc_id", ev.AccessCode);
                cmd.Parameters.AddWithValue(@"date", ev.Date);
                cmd.Parameters.AddWithValue(@"time", ev.Time);

                //cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();

                return "Success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public User getUser(string username)
        {
            
            conn = new SqlConnection(dbConnectionString);
            try
            {
                /*string usr = "";
                if (username.Length > 5)
                {
                    if (username.Contains("_"))
                    {
                        if (username.Substring(0, username.IndexOf('_')).Equals("user_") && username.Substring(username.LastIndexOf('_'), username.Length).Equals("_fb"))
                        {
                            usr = username.Substring(username.IndexOf('_'), username.LastIndexOf('_') - 1);
                            return getUserByFbId(usr);
                        }
                    }
                }*/
                //else return null;
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM dbo.Users WHERE username=@username", conn);
                cmd.Parameters.AddWithValue(@"username", username);

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
                return new User
                {
                    Fname = "<Error>",
                    Lname = e.Message
                };
                
            }
            
        }

        public User getUserByFbId(string id)
        {
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM dbo.Users WHERE fb_id=@id", conn);
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
                return new User
                {
                    Fname = "<Error>",
                    Lname = e.Message
                };
                
            }
            
        }

        public List<Event> getEvents()
        {
            List<Event> events = new List<Event>();
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM dbo.Events", conn);
                //cmd.Parameters.AddWithValue(@"id", id);
                //cmd.Parameters.AddWithValue(@"pwd", hashed_input_pwd);

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    events.Add(new Event()
                    {
                        Id = (int)dataReader.GetValue(0),
                        Title = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Address = (string)dataReader.GetValue(3),
                        Radius = (int)dataReader.GetValue(4),
                        Gps_location = (string)dataReader.GetValue(5),
                        AccessCode = (int)dataReader.GetValue(6),
                        Date = (string)dataReader.GetValue(7),
                        Time = (string)dataReader.GetValue(8)
                                       
                    });
                }
                
                dataReader.Close();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception e)
            {
                File.WriteAllLines(Path.Combine(HostingEnvironment.MapPath("~/logs/"), new DateTime()+".log"),new String[] { e.Message});
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
                return e.Message;
            }
            return "isValidUser=" + isValidUser;
        }
    }
}