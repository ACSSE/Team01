<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="EditEvent.aspx.cs" Inherits="IceBreak.EditEvent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id="page-content-wrapper">
        
        <div class="register-container container">
            <div class="row">
                 <div class="maps span5">
                     <input id="pac-input" " class="controls" type="text" placeholder="Enter a location"/>
                       
                        <div id="map"></div>
                      
                 </div>
                    <div class="form">
                        <h2>Add Event To <strong style="color:#59D0F7">IceBreak</strong></></h2>
                        <div class="form-group" style="text-align:left">
                            <label for="eventname"  runat="server">Event Name</label><span id="lbl_name" runat="server" style="color:red; display:none"> - Please enter your event name.</span>
                            <input type="text" id="eventname" name="eventname" runat="server" onkeydown = "return (event.keyCode!=13);"/>
                       </div>
                        <div class="form-group" style="text-align:left">
                             <label for="EventAddress">Event Address</label><span id="address_span" runat="server" style="color:red; display:none"> - Please enter your event address.</span>
                             <input type="text" id="eventaddress" name="eventaddress" disabled  runat="server"/>
                       </div>
                         <div class="form-group" style="text-align:left">
                             <label for="gps">GPS Coordinates</label><span id="gps_span" runat="server" style="color:red; display:none"> - Please enter your gps coordinates.</span>
                             <input id="gps" type="text" name="gps" disabled  runat="server" />
                       </div>
                        <div class="form-group" style="text-align:left">
                            <label for="EventDescription">Event Description</label><span id="descrip_span" runat="server" style="color:red; display:none"> - Please enter your event description.</span>
                            <textarea class="form-control" rows="4" id="eventdescrip" name="eventdescrip"   runat="server"></textarea>
                        </div>
                        <div class="form-group" style="text-align:left">
                            <label for="Event Date ">Event Date</label><span id="date_span" runat="server" style="color:red; display:none"> - Please enter your event date.</span>
                            <input type="date" id="date" name="date" runat="server"/>
                        </div>
                        <div class="form-group" style="text-align:left">
                            <label for="Event Time">Event Time</label><span id="time_span" runat="server" style="color:red; display:none"> - Please enter your event time.</span>
                            <input type="time" id="time" name="time" runat="server"/>
                        </div>
                         <div class="form-group" style="text-align:left">
                            <label for="Event end time">Event End Time</label><span id="end_time_span" runat="server" style="color:red; display:none"> - Please enter your event end time.</span>
                            <input type="time" id="end_time" name="Event end time" runat="server"/>
                        </div>
                        <div class="form-group" style="text-align:left">
                            <label for="Meeting Places">Meeting Places at Event</label><span id="meeting_span" runat="server" style="color:red; display:none"> - Please enter all event meeting places.</span>
                        </div>    
                        <div class="form-group" style="text-align:left">
                            <asp:DropDownList ID="NumEvents" runat="server" CssClass="dropdown-toggle" AutoPostBack="True">
                                <asp:ListItem Enabled="true" Text="Select number of meeting places" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            </asp:DropDownList>      
                            </div>                 
                           <div class="form-group"style="text-align:left">
                             <input type="text" id="meeting_place_1" placeholder="enter meeting place 1" runat="server"/>
                           </div>
                             <div class="form-group"style="text-align:left">
                                <input type="text" id="meeting_place_2" placeholder="enter meeting place 2" runat="server"/>
                             </div>
                             <div class="form-group"style="text-align:left">
                                 <input type="text" id="meeting_place_3" placeholder="enter meeting place 3" runat="server"/>
                            </div>
                             <div class="form-group"style="text-align:left">
                                  <input type="text" id="meeting_place_4" placeholder="enter meeting place 4" runat="server"/>
                            </div>
                             <div class="form-group"style="text-align:left">
                                  <input type="text" id="meeting_place_5" placeholder="enter meeting place 5" runat="server"/>
                             </div>
                        <div class="form-group" style="text-align:left">  
                         <label>Upload Photo</label>                         
                        <asp:FileUpload id="FileUpload" runat="server" CssClass="UploadButton AddButton" /> <span id="upload" runat="server" style="color:red;display:none"></span>
                     
                        </div>
                        <asp:Button runat="server" ID="btnUpdateButton"  UseSubmitBehavior="true" CssClass="AddButton" OnClick="btnUpdate_Event" Text="UPDATE EVENT"></asp:Button>
                   </div>
               </div>
            </div>
    </div>
</asp:Content>
