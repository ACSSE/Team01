using System;
using System.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;

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
            string hashed_input_usr = Hash.HashString(user.Username);
            string hashed_input_pwd = Hash.HashString(user.Password);

            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM dbo.Users WHERE username=@usr", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", hashed_input_usr);
                cmd.Parameters.AddWithValue(@"pwd", hashed_input_pwd);

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
                    string query = "INSERT INTO dbo.Users VALUES(@fname,@lname,@email,@password,@username)";
                    cmd = new SqlCommand(query, conn);
                    //string s = Hash.HashPassword(user.Password);

                    cmd.Parameters.AddWithValue(@"fname", user.Fname);
                    cmd.Parameters.AddWithValue(@"lname", user.Lname);
                    cmd.Parameters.AddWithValue(@"username", Hash.HashString(user.Username));
                    cmd.Parameters.AddWithValue(@"email", user.Email);
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

        public string signIn(User user)
        {
            bool isValidUser = false;
            //Hash input to compare with hashed DB credentials
            string hashed_input_usr = Hash.HashString(user.Username);
            string hashed_input_pwd = Hash.HashString(user.Password);

            conn = new SqlConnection(dbConnectionString);
            try
            {
                conn.Open();
                //Query user
                cmd = new SqlCommand("SELECT * FROM dbo.Users WHERE username=@usr AND pwd=@pwd", conn);//WHERE 'username'=@usr AND 'pwd'=@pwd", conn);
                cmd.Parameters.AddWithValue(@"usr", hashed_input_usr);
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