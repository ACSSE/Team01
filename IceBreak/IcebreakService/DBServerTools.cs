using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web.Hosting;

namespace IcebreakServices
{
    public class DBServerTools
    {
        private string dbConnectionString = "Server=tcp:icebreak-server.database.windows.net,1433;Initial Catalog=IcebreakDB;Persist Security Info=False;User ID=superuser;Password=Breakingtheice42;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader dataReader;

        public string updateUserDetails(User user)
        {
            if(userExists(user).Equals("Exists=true"))
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
                        cmd = new SqlCommand("UPDATE dbo.Users SET email=@email WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"email", user.Email);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Age >=0)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET Age=@age WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"age", user.Age);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Occupation != null)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET Occupation=@occupation WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"occupation", user.Occupation);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Bio != null)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET Bio=@bio WHERE username=@usr", conn);
                        cmd.Parameters.AddWithValue(@"bio", user.Bio);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }
                    if (user.Catchphrase != null)
                    {
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
                    if (user.Event_id >= 0)
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
                    if (user.Username != null)
                    {
                        cmd = new SqlCommand("UPDATE dbo.Users SET username=@username WHERE username=@usr", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                        cmd.Parameters.AddWithValue(@"username", user.Username);
                        cmd.Parameters.AddWithValue(@"usr", user.Username);
                        cmd.ExecuteNonQuery();
                    }

                    return "Success";
                }
                catch (Exception e)
                {
                    //TODO: Store exception to logs
                    return e.Message;
                }
            }
            else
            {
                return "User does not exist.";
            }
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
                    users.Add(new User
                    {
                        Fname = Convert.ToString(dataReader.GetValue(0)),
                        Lname = Convert.ToString(dataReader.GetValue(1)),
                        Email = Convert.ToString(dataReader.GetValue(2)),
                        Username = Convert.ToString(dataReader.GetValue(4))
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

        public string registerUser(User user)
        {
            string exist_check = userExists(user);
            if (exist_check.Equals("Exists=false"))
            {
                try
                {
                    conn = new SqlConnection(dbConnectionString);
                    conn.Open();
                    string query = "INSERT INTO dbo.Users(fname,lname,email,pwd,username,access_level,event_id) VALUES(@fname,@lname,@email,@password,@username,@access_lvl,@event_id)";
                    cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue(@"fname", user.Fname);
                    cmd.Parameters.AddWithValue(@"lname", user.Lname);
                    cmd.Parameters.AddWithValue(@"username", user.Username);//Hash.HashString(user.Username));
                    cmd.Parameters.AddWithValue(@"email", user.Email);
                    cmd.Parameters.AddWithValue(@"access_lvl", user.Access_level);
                    cmd.Parameters.AddWithValue(@"event_id", user.Event_id);
                    cmd.Parameters.AddWithValue(@"password", Hash.HashString(user.Password));

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
                return exist_check;
            }
        }

        public string addEvent(Event ev)
        {
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();
                string query = "INSERT INTO dbo.Events VALUES(@title,@desc,@addr,@radius,@loc_gps)";
                cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(@"title", ev.Title);
                cmd.Parameters.AddWithValue(@"desc", ev.Description);
                cmd.Parameters.AddWithValue(@"addr", ev.Address);//Hash.HashString(user.Username));
                cmd.Parameters.AddWithValue(@"radius", ev.Radius);
                cmd.Parameters.AddWithValue(@"loc_gps", ev.Gps_location);

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
                while(dataReader.Read())
                {
                    events.Add(new Event()
                    {
                        Id = (int)dataReader.GetValue(0),
                        Title = (string)dataReader.GetValue(1),
                        Description = (string)dataReader.GetValue(2),
                        Address = (string)dataReader.GetValue(3),
                        Radius = (int)dataReader.GetValue(4),
                        Gps_location = (string)dataReader.GetValue(5)
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