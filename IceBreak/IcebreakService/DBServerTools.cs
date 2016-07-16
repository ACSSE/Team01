using System;
using System.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace IcebreakServices
{
    public class DBServerTools
    {
        private string dbConnectionString = "Server=tcp:icebreak-server.database.windows.net,1433;Initial Catalog=IcebreakDB;Persist Security Info=False;User ID=superuser;Password=Breakingtheice42;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader dataReader;

        public List<User> getUsers()
        {
            List<User> users = new List<User>();
            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                cmd = new SqlCommand("SELECT * FROM dbo.Users",conn);
                dataReader = cmd.ExecuteReader();
                while(dataReader.Read())
                {
                    users.Add(new User()
                    {
                        Fname = ((string)dataReader.GetValue(0)),
                        Lname = ((string)dataReader.GetValue(1)),
                        Username = ((string)dataReader.GetValue(2)),
                        Email = ((string)dataReader.GetValue(3))
                    });
                }
                dataReader.Close();
                cmd.Dispose();
                conn.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return users;
        }

        public string registerUser(User user)
        {
            try
            {
                conn = new SqlConnection(dbConnectionString);
                conn.Open();
                string query = "INSERT INTO dbo.Users VALUES(@fname,@lname,@username,@email,@password)";
                cmd = new SqlCommand(query, conn);

                /*cmd.Parameters.Add("@fname",SqlDbType.VarChar);
                cmd.Parameters.Add("@lname", SqlDbType.VarChar);
                cmd.Parameters.Add("@username", SqlDbType.VarChar);
                cmd.Parameters.Add("@email", SqlDbType.VarChar);
                cmd.Parameters.Add("@password", SqlDbType.VarChar);

                cmd.Parameters["@fname"].Value = user.Fname;
                cmd.Parameters["@lname"].Value = user.Lname;
                cmd.Parameters["@username"].Value = user.Username;
                cmd.Parameters["@email"].Value = user.Email;
                cmd.Parameters["@password"].Value = user.Password;*/

                cmd.Parameters.AddWithValue(@"fname", user.Fname);
                cmd.Parameters.AddWithValue(@"lname", user.Lname);
                cmd.Parameters.AddWithValue(@"username", user.Username);
                cmd.Parameters.AddWithValue(@"email", user.Email);
                cmd.Parameters.AddWithValue(@"password", user.Password);

                //cmd.Prepare();
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Success";
        }
    }
}