<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="YourEvents.aspx.cs" Inherits="IceBreak.YourEvents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-content-wrapper">
    <div class="container" id="EventView" runat="server">
        <!-- Page Heading -->
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header"> Your Events
                   
                </h1>
            </div>
        </div>
    </div>      
        <div hidden="hidden">
         <asp:Button ID="delete" runat="server"  OnCommand="Delete" ></asp:Button>
        </div>
         <input type="hidden" id="eventid" name = "EventId" value = '<%=this.EventID %>' />
</div>

    <script type="text/javascript">
        var eventid;
        window.onload = function () {
            eventid = document.getElementById("eventid");
        }
        function Delete(id)
        {
            var deleteButton = document.getElementById("<%= delete.ClientID %>");
            eventid.value = id;
            deleteButton.click();
        }
    </script>
</asp:Content>
