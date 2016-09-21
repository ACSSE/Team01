<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Event.aspx.cs" Inherits="IceBreak.Event" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="page-content-wrapper">
    <div class="container" id="EventView" runat="server">

        <!-- Page Heading -->
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header"> Events
                    <small><p id="date"></p></small>
                </h1>
            </div>
        </div>

    </div>
</div>
   
<a href="AddEvent.aspx">
<div class="floatingContainer">
	<div class="actionButton">
		<span class="glyphicon glyphicon-plus glyphicon-center" style="font-size:x-large;color:white;"></span>
	</div>
</div>
</a>
<a id="EditButton" href="YourEvents.aspx" runat="server">

</a>

    <script>
        function startTime() {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var day = dd + ' ' + mm + ' ' + yyyy;
            var h = today.getHours();
            var m = today.getMinutes();
            var s = today.getSeconds();
            m = checkTime(m);
            s = checkTime(s);
            document.getElementById('date').innerHTML =
            day + ' ' + h + ":" + m + ":" + s;
            var t = setTimeout(startTime, 500);
        }
        function checkTime(i) {
            if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
            return i;
        }
        window.onload = startTime
    </script>
</asp:Content>

