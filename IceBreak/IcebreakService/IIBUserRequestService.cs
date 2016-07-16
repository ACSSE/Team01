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
        [WebGet(UriTemplate = "Users", ResponseFormat = WebMessageFormat.Json)]
        List<User> getUsers();

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/usrRegPOST",
            BodyStyle = WebMessageBodyStyle.Bare)]
        //[WebInvoke(Method = "POST", UriTemplate = "usrRegPOST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//, ResponseFormat = WebMessageFormat.Json, 
        void registerUser(Stream streamdata);
    }
}
