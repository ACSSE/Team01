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
            UriTemplate = "/setUniqueUserToken",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json)]
        string setUniqueUserToken(Stream streamdata);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/userUpdate/{handle}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json)]
        string userUpdate(string handle, Stream streamdata);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/addMessage",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json)]
        string addMessage(Stream streamdata);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/removeUser/{handle}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        string removeUser(string handle);

        [OperationContract]
        [WebGet(UriTemplate = "/getUsersAtEvent/{eventId}", ResponseFormat = WebMessageFormat.Json)]
        List<User> getUsersAtEvent(string eventId);

        [OperationContract]
        [WebGet(UriTemplate = "/getUser/{username}", ResponseFormat = WebMessageFormat.Json)]
        User getUser(string username);

        [OperationContract]
        [WebGet(UriTemplate = "/getMessageById/{msg_id}", ResponseFormat = WebMessageFormat.Json)]
        Message getMessageById(string msg_id);

        [OperationContract]
        [WebGet(
            UriTemplate = "/imageDownload/{fileName}",
            ResponseFormat = WebMessageFormat.Json, 
            BodyStyle =WebMessageBodyStyle.WrappedResponse)]
        string imageDownload(string fileName);

        [OperationContract]
        [WebGet(
            UriTemplate = "/checkUserInbox/{username}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedResponse)]
        List<Message> checkUserInbox(string username);

        [OperationContract]
        [WebGet(
            UriTemplate = "/checkUserOutbox/{sender}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedResponse)]
        List<Message> checkUserOutbox(string sender);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/imgUpload/{fileName}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        string imageUpload(string fileName, Stream fileStream);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/imageUploadWithMeta/{meta}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        string imageUploadWithMeta(string meta, Stream fileStream);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getImageIDsAtEvent/{event_id}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string getImageIDsAtEvent(string event_id);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/signup",
            BodyStyle = WebMessageBodyStyle.Bare)]
        //[WebInvoke(Method = "POST", UriTemplate = "usrRegPOST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]//, ResponseFormat = WebMessageFormat.Json, 
        string registerUser(Stream streamdata);

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
        string addEvent(Stream streamdata);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/updateEvent",
            BodyStyle = WebMessageBodyStyle.Bare)]
        string updateEvent(Stream streamdata);

        [OperationContract]
        [WebGet(UriTemplate = "/getNearbyEvents/{lat}/{lng}/{range}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Event> getNearbyEvents(string lat, string lng, string range);

        [OperationContract]
        [WebGet(UriTemplate = "/getNearbyEventIds/{lat}/{lng}/{range}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<string> getNearbyEventIds(string lat, string lng, string range);

        [OperationContract]
        [WebGet(UriTemplate = "/getNearbyEventIdsByNoise/{lat}/{lng}/{range}/{loudness}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<string> getNearbyEventIdsByNoise(string lat, string lng, string range, string loudness);

        [OperationContract]
        [WebGet(UriTemplate = "/getAllEvents", ResponseFormat = WebMessageFormat.Json, BodyStyle =WebMessageBodyStyle.Bare)]
        List<Event> getAllEvents();

        [OperationContract]
        [WebGet(UriTemplate = "/getEvent/{event_id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Event getEvent(string event_id);

        [OperationContract]
        [WebGet(UriTemplate = "/getAchievement/{ach_id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Achievement getAchievement(string ach_id);

        [OperationContract]
        [WebGet(UriTemplate = "/getAllAchievements", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Achievement> getAllAchievements();

        [OperationContract]
        [WebGet(UriTemplate = "/getUserAchievements/{username}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Achievement> getUserAchievements(string username);

        [OperationContract]
        [WebGet(UriTemplate = "/getUserAchievementPoints/{username}/{method}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int getUserAchievementPoints(string username, string method);

        #region Rewards

        [OperationContract]
        [WebGet(UriTemplate = "/getRewardForEvent/{event_id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Reward getRewardForEvent(string event_id);

        [OperationContract]
        [WebGet(UriTemplate = "/getRewardsForEvent/{event_id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Reward> getRewardsForEvent(string event_id);

        [OperationContract]
        [WebGet(UriTemplate = "/getUserRewardsAtEvent/{username}/{event_id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Reward> getUserRewardsAtEvent(string username, string event_id);

        [OperationContract]
        [WebGet(UriTemplate = "/getRewardsCreatedByUser/{username}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Reward> getRewardsCreatedByUser(string username);

        [OperationContract]
        [WebGet(UriTemplate = "/getReward/{rew_id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Reward getReward(string rew_id);

        [OperationContract]
        [WebGet(UriTemplate = "/getUserPreparedRewardsForEvent/{username}/{event_id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Reward> getUserPreparedRewardsForEvent(string username, string event_id);

        [OperationContract]
        [WebGet(UriTemplate = "/getAllRewards", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Reward> getAllRewards();

        /* For future Event Manager mobile app

        string updateReward(Reward rw, int access_lvl);

        string addReward(Reward rw, int access_lvl);*/
        [OperationContract]
        [WebGet(UriTemplate = "/addUserReward/{rw_id}/{code}/{date}/{event_id}/", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string addUserReward(string rw_id, string code, string date, string event_id);


        [OperationContract]
        [WebGet(UriTemplate = "/claimReward/{username}/{reward_id}/{event_id}/{code}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string claimReward(string username, string reward_id, string event_id, string code);

        [OperationContract]
        [WebGet(UriTemplate = "/redeemReward/{username}/{reward_id}/{event_id}/{code}/{new_code}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string redeemReward(string username, string reward_id, string event_id, string code, string new_code);

        #endregion Rewards

        [OperationContract]
        [WebGet(
            UriTemplate = "/ping/{username}",
            ResponseFormat = WebMessageFormat.Json)]
        string ping(string username);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getMeta/{record}",
            ResponseFormat = WebMessageFormat.Json)]
        Metadata getMeta(string record);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getExceptions",
            ResponseFormat = WebMessageFormat.Json)]
        List<IBException> getExceptions();

        #region Statistics
        /******Master Stats*************/
        [OperationContract]
        [WebGet(
            UriTemplate = "/getTotalIcebreakCount",
            ResponseFormat = WebMessageFormat.Json)]
        int getTotalIcebreakCount();

        [OperationContract]
        [WebGet(
            UriTemplate = "/getTotalSuccessfulIcebreakCount",
            ResponseFormat = WebMessageFormat.Json)]
        int getTotalSuccessfulIcebreakCount();

        [OperationContract]
        [WebGet(
            UriTemplate = "/getTotalIcebreakCountBetweenTime/{start}/{end}",
            ResponseFormat = WebMessageFormat.Json)]
        int getTotalIcebreakCountBetweenTime(string start, string end);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getTotalSuccessfullIcebreakCountBetweenTime/{start}/{end}",
            ResponseFormat = WebMessageFormat.Json)]
        int getTotalSuccessfullIcebreakCountBetweenTime(string start, string end);

        /******User Stats**************/
        [OperationContract]
        [WebGet(
            UriTemplate = "/getUserIcebreakCount/{username}",
            ResponseFormat = WebMessageFormat.Json)]
        int getUserIcebreakCount(string username);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getUserSuccessfulIcebreakCount/{username}",
            ResponseFormat = WebMessageFormat.Json)]
        int getUserSuccessfulIcebreakCount(string username);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getUserIcebreakCountAtEvent/{username}/{event_id}",
            ResponseFormat = WebMessageFormat.Json)]
        int getUserIcebreakCountAtEvent(string username, string event_id);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getUserSuccessfulIcebreakCountAtEvent/{username}/{event_id}",
            ResponseFormat = WebMessageFormat.Json)]
        int getUserSuccessfulIcebreakCountAtEvent(string username, string event_id);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getUserIcebreakCountBetweenTime/{username}/{start}/{end}",
            ResponseFormat = WebMessageFormat.Json)]
        int getUserIcebreakCountBetweenTime(string username, string start, string end);


        [OperationContract]
        [WebGet(
            UriTemplate = "/getUserSuccessfulIcebreakCountBetweenTime/{username}/{start}/{end}",
            ResponseFormat = WebMessageFormat.Json)]
        int getUserSuccessfulIcebreakCountBetweenTime(string username, string start, string end);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getUserIcebreakCountBetweenTimeAtEvent/{username}/{start}/{end}/{event_id}",
            ResponseFormat = WebMessageFormat.Json)]
        int getUserIcebreakCountBetweenTimeAtEvent(string username, string start, string end, string event_id);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getUserSuccessfulIcebreakCountBetweenTimeAtEvent/{username}/{start}/{end}/{event_id}",
            ResponseFormat = WebMessageFormat.Json)]
        int getUserSuccessfulIcebreakCountBetweenTimeAtEvent(string username, string start, string end, string event_id);

        [OperationContract]
        [WebGet(UriTemplate = "/getMaxUserIcebreakCountAtOneEvent/{username}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int getMaxUserIcebreakCountAtOneEvent(string username);

        [OperationContract]
        [WebGet(UriTemplate = "/getMaxUserSuccessfulIcebreakCountAtOneEvent/{username}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int getMaxUserSuccessfulIcebreakCountAtOneEvent(string username);

        [OperationContract]
        [WebGet(UriTemplate = "/getUserIcebreakCountXHoursApart/{username}/{hours}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int getUserIcebreakCountXHoursApart(string username, string hours);

        [OperationContract]
        [WebGet(UriTemplate = "/getUserSuccessfulIcebreakCountXHoursApart/{username}/{hours}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        int getUserSuccessfulIcebreakCountXHoursApart(string username, string hours);

        [OperationContract]
        [WebGet(UriTemplate = "/getUserEventHistory/{username}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        List<Event> getUserEventHistory(string username);

        /******Event Stats************/
        [OperationContract]
        [WebGet(
            UriTemplate = "/getEventIcebreakCount/{event_id}",
            ResponseFormat = WebMessageFormat.Json)]
        int getEventIcebreakCount(string event_id);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getEventSuccessfulIcebreakCountBetweenTime/{event_id}/{start}/{end}",
            ResponseFormat = WebMessageFormat.Json)]
        int getEventSuccessfulIcebreakCountBetweenTime(string event_id, string start, string end);

        [OperationContract]
        [WebGet(
            UriTemplate = "/getEventIcebreakCountBetweenTime/{event_id}/{start}/{end}",
            ResponseFormat = WebMessageFormat.Json)]
        int getEventIcebreakCountBetweenTime(string event_id, string start, string end);

        #endregion Statistics
    }
}
