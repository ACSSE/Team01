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
            $("#personal").click(function () { $("#tab_events").hide(); $("#tab_personal").show(); });
            $("#events").click(function () { $("#tab_personal").hide(); $("#tab_events").show(); });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div id="tabs">
        <ul>
            <li><a id="personal" href="#tab_personal" runat="server">Personal stats</a></li>
            <li><a id="events" href="#tab_events" runat="server">Events you created</a></li>
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
    </div>
</asp:Content>