<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="AddEvent.aspx.cs" Inherits="IceBreak.AddEvent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="scripts/scripts.js"></script>
    <script src="scripts/jquery.backstretch.min.js"></script>
    <link rel="stylesheet" href="stylesheets/addeventform.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div id="page-content-wrapper">
        <div class="register-container container">
            <div class="row">
                 <div class="iphone span5">
                    <img src="images/iphone.png" alt=""/>
                 </div>
                    <div class="form">
                        <h2>Add Event To <strong style="color:#59D0F7">IceBreak</strong></></h2>
                        <div class="form-group" style="text-align:left">
                            <label for="eventname">Event Name</label>
                            <input type="text" id="eventname" name="eventname" placeholder="enter your event name..."/>
                       </div>
                        <div class="form-group" style="text-align:left">
                             <label for="EventAddress">Event Address</label>
                             <input type="text" id="eventaddress" name="eventaddress" placeholder="enter your event address..."/>
                       </div>
                        <div class="form-group" style="text-align:left">
                            <label for="EventDescription">Event Description</label>
                            <textarea class="form-control" rows="4" id="EventDescrip" name="eventdescrip" placeholder="enter your event description..."></textarea>
                        </div>
                        <div class="form-group" style="text-align:left">
                            <label for="Event Date ">Event Date</label>
                            <input type="date" id="date" name="date"/>
                        </div>
                        <div class="form-group" style="text-align:left">
                            <label for="Even Time">Event Time</label>
                            <input type="time" id="time" name="time"/>
                        </div>
                        <button type="submit">ADD EVENT</button>
                   </div>
               </div>
            </div>
    </div>






</asp:Content>
