using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IceBreak
{
    public partial class master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Login(object sender, EventArgs e)
        {
            

            // string conString = "Server=tcp:icebreak-server.database.windows.net,1433;Initial Catalog=IcebreakDB;Persist Security Info=False;User ID={your_username};Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            Thread thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }
        public void Run()
        {
            string username = txtUsername.Value;
            string password = txtPassword.Value;

            byte[] bytes = new byte[1024];

            try
            {
                IPAddress ipAddress = IPAddress.Parse("icebreak.azurewebsites.net");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 80);
                Socket soc = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                soc.Connect(remoteEP);
                Response.Write("<script>alert('Socket connected to { 0}" + soc.RemoteEndPoint.ToString() +");</script>");

                // Encode the data string into a byte array.
                byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                // Send the data through the socket.
                int bytesSent = soc.Send(msg);

                // Receive the response from the remote device.
                int bytesRec = soc.Receive(bytes);
                Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                // Release the socket.
                soc.Shutdown(SocketShutdown.Both);
                soc.Close();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
    }
}