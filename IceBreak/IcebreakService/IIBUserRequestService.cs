using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace IcebreakServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITestService" in both code and config file together.
    [ServiceContract(Namespace ="http://icebreak.azurewebsites.net/")]
    public interface IIBUserRequestService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/userUpdate/{handle}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json)]
        string userUpdate(string handle, Stream streamdata);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/removeUser/{handle}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        string removeUser(string handle);

        [OperationContract]
        [WebGet(UriTemplate = "/getUsersAtEvent/{eventId}", ResponseFormat = WebMessageFormat.Json)]
        List<User> getUsersAtEvent(string eventId);

        [OperationContract]
        [WebGet(
            UriTemplate = "/imageDownload/{fileName}",
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle =WebMessageBodyStyle.WrappedResponse)]
        string imageDownload(string fileName);


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/imgUpload/{fileName}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        void imageUpload(string fileName,Stream fileStream);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/signup",
            BodyStyle = WebMessageBodyStyle.Bare)]
        //[WebInvoke(Method = "POST", UriTemplate = "usrRegPOST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//, ResponseFormat = WebMessageFormat.Json, 
        void registerUser(Stream streamdata);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/signin",
            BodyStyle = WebMessageBodyStyle.Bare)]
        void signIn(Stream streamdata);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/addEvent",
            BodyStyle = WebMessageBodyStyle.Bare)]
        void addEvent(Stream streamdata);

        /*[OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/addEventImg/{eventId}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        void addEventImg(string eventId, Stream streamdata);*/

        [OperationContract]
        [WebInvoke(Method = "GET",
        UriTemplate = "/getImage/{filename}",
        ResponseFormat = WebMessageFormat.Json)]
        string getImage(string filename);

        [OperationContract]
        [WebGet(UriTemplate = "/readEvents", ResponseFormat = WebMessageFormat.Json, BodyStyle =WebMessageBodyStyle.Bare)]
        List<Event> readEvents();
    }
}
