using System;
using System.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
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

        public string userExists(User user)
        {
            //Hash input to compare with hashed DB credentials
            //string hashed_input_usr = Hash.HashString(user.Username);
            string hashed_input_pwd = Hash.HashString(user.Password);

            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM dbo.Users WHERE username=@usr", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", user.Username);
                //cmd.Parameters.AddWithValue(@"pwd", hashed_input_pwd);

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

        public List<Event> getEvent()
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