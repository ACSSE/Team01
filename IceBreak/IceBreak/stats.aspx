<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master.Master" CodeBehind="stats.aspx.cs" Inherits="IceBreak.stats" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>IceBreak | Statistics</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>
        $(function ()
        {
            $("#tabs").tabs();
            $("#personal").click(function () { $("#tab_events").hide(); $("#tab_redeem").hide(); $("#tab_personal").show(); });
            $("#events").click(function () { $("#tab_personal").hide(); $("#tab_redeem").hide(); $("#tab_events").show(); });
            $("#redeem").click(function () { $("#tab_personal").hide(); $("#tab_events").hide(); $("#tab_redeem").show(); });
        });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div id="tabs">
        <ul>
            <li><a id="personal" href="#tab_personal" runat="server">Personal stats</a></li>
            <li><a id="events" href="#tab_events" runat="server">Events you created</a></li>
            <li><a id="redeem" href="#tab_redeem" runat="server">Redeem User Rewards</a></li>
            <!--<li><a href="#tabs-3">Master</a></li>-->
        </ul>
        <div id="tab_personal">
            <div id="personal_canvas_container" style="width:100%;height:auto;margin:auto;" runat="server">

            </div>
        </div>
        <div id="tab_events">
            <div style="width:350px;height:90px;margin:auto;">
                <h2 id="welcome_msg" runat="server" align="center"></h2>
                <asp:Label runat="server">Show statistics for: </asp:Label>
                <asp:DropDownList id="dd_events" runat="server"></asp:DropDownList>
                <asp:Button id="btnViewStatsForEvent" runat="server" Text="Go" OnClick="btnViewStatsForEvent_Click"/>
            </div>
            <div id="canvas_container" style="width:100%;height:auto;margin:auto;" runat="server">

            </div>
        </div>
        <div id="tab_redeem">
            <div id="js_redeem_notifications" style="width:600px;height:auto;margin:auto;border:1px solid #0094ff;border-radius:10px;">
            </div>
            <div style="width:400px;height:50px;margin:auto;">
                <h3 style="text-align:center;">Search user</h3>
            </div>
            <div style="width:340px;height:50px;margin:auto;">
                <asp:TextBox BorderWidth="1" BorderColor="Black" BackColor="White" Width="250px" ID="search_box" runat="server"></asp:TextBox>
                <asp:Button style="margin:auto;" Text="Search" ID="btnSearchUser" runat="server" OnClick="btnSearchUser_Click"/>
            </div>
            <div style="border-radius:100px;width:100px;height:100px;margin:auto;display:block;visibility:visible;" id="loading_user_ico" runat="server">
                <img style="border-radius:100px;border:1px solid #343434;" src="http://icebreak.azurewebsites.net/images/public_res/ring-alt.gif" height="100" width="100" alt="Profile image"/>
            </div>
            <div style="width:auto;height:auto;margin:auto;" id="redeem_container" runat="server">
                
            </div>
        </div>
</asp:Content>