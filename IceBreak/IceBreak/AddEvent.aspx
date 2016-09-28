<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" ClientIDMode="Static" AutoEventWireup="true" CodeBehind="AddEvent.aspx.cs" Inherits="IceBreak.AddEvent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="scripts/scripts.js"></script>
    <script src="scripts/jquery.backstretch.min.js"></script>
     <script async="async" defer="defer" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDAAQZBI76K_oRkxy-1qAyMC2w8AnfimZM&libraries=places"></script>
   <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.5.8/angular.min.js"></script>
     <link rel="stylesheet" href="stylesheets/addeventform.css"/>
    
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
                            <input type="text" id="eventname" name="eventname" placeholder="enter your event name..." runat="server" onkeydown = "return (event.keyCode!=13);"/>
                       </div>
                        <div class="form-group" style="text-align:left">
                             <label for="EventAddress">Event Address</label><span id="address_span" runat="server" style="color:red; display:none"> - Please enter your event address.</span>
                             <input  type="text" readonly="readonly" id="eventaddress" name="eventaddress" placeholder="Use Google Map to enter a location" runat="server"/>
                       </div>
                         <div class="form-group" style="text-align:left">
                             <label for="gps">GPS Coordinates</label><span id="gps_span" runat="server" style="color:red; display:none"> - Please enter your gps coordinates.</span>
                             <input  id="gps" type="text" readonly="readonly" name="gps" placeholder="enter your gps coordinates..." runat="server" />
                       </div>
                        <div class="form-group" style="text-align:left">
                            <label for="EventDescription">Event Description</label><span id="descrip_span" runat="server" style="color:red; display:none"> - Please enter your event description.</span>
                            <textarea class="form-control" rows="4" id="eventdescrip" name="eventdescrip" placeholder="enter your event description..."  runat="server" onkeydown = "return (event.keyCode!=13);"></textarea>
                        </div>
                        <div class="form-group" style="text-align:left">
                            <label for="Event Date ">Event Start Date</label><span id="date_span" runat="server" style="color:red; display:none"> - Please enter your event start date.</span>
                            <input type="date" id="date" name="date" runat="server"/>
                        </div>
                         <div class="form-group" style="text-align:left">
                            <label for="Event End Date ">Event End Date</label><span id="end_date_span" runat="server" style="color:red; display:none"> - Please enter your event end date.</span>
                            <input type="date" id="event_end_date" name="event_end_date" runat="server"/>
                        </div>
                        <div class="form-group" style="text-align:left">
                            <label for="Event Time">Event Time</label><span id="time_span" runat="server" style="color:red; display:none"> - Please enter your event time.</span>
                            <input type="time" id="time" name="time" runat="server"/>
                        </div>
                       
                         <div class="form-group" style="text-align:left">
                            <label for="Event end time">Event End Time</label><span id="end_time_span" runat="server" style="color:red; display:none"> - Please enter your event end time.</span>
                            <input type="time" id="end_time" name="Event end time" runat="server" onkeydown = "return (event.keyCode!=13);"/>
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
                             <input type="text" id="meeting_place_1" placeholder="enter meeting place 1" runat="server" onkeydown = "return (event.keyCode!=13);"/>
                           </div>
                             <div class="form-group"style="text-align:left">
                                <input type="text" id="meeting_place_2" placeholder="enter meeting place 2" runat="server" onkeydown = "return (event.keyCode!=13);"/>
                             </div>
                             <div class="form-group"style="text-align:left">
                                 <input type="text" id="meeting_place_3" placeholder="enter meeting place 3" runat="server" onkeydown = "return (event.keyCode!=13);"/>
                            </div>
                             <div class="form-group"style="text-align:left">
                                  <input type="text" id="meeting_place_4" placeholder="enter meeting place 4" runat="server" onkeydown = "return (event.keyCode!=13);"/>
                            </div>
                             <div class="form-group"style="text-align:left">
                                  <input type="text" id="meeting_place_5" placeholder="enter meeting place 5" runat="server" onkeydown = "return (event.keyCode!=13);"/>
                             </div>
                        <div class="form-group" style="text-align:left">  
                         <label>Upload Photo</label>                         
                        <asp:FileUpload id="FileUpload" runat="server" CssClass="UploadButton AddButton" /> <span id="upload" runat="server" style="color:red;display:none"></span>
                     
                        </div>
                        <asp:Button runat="server" ID="btnAddButton"  UseSubmitBehavior="true" CssClass="AddButton" OnClick="btnAdd_Event" Text="ADD EVENT"></asp:Button>
                   </div>
               </div>
            </div>
    </div>

    
   <script type="text/javascript">
       function showAlert()
       {
           $('#alertModal').modal('show');
       }
       function showAlert2() {
           $('#alertModal2').modal('show');
       }
       function initMap() {
           var map = new google.maps.Map(document.getElementById('map'), {
               center: { lat: -33.8688, lng: 151.2195 },
               zoom: 13
           });
           var input = /** @type {!HTMLInputElement} */(
               document.getElementById('pac-input'));

           var types = document.getElementById('type-selector');
           map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
           map.controls[google.maps.ControlPosition.TOP_LEFT].push(types);

           var autocomplete = new google.maps.places.Autocomplete(input);
           autocomplete.bindTo('bounds', map);


           var infowindow = new google.maps.InfoWindow();
           var marker = new google.maps.Marker({
               map: map,
               anchorPoint: new google.maps.Point(0, -29)

           });

           autocomplete.addListener('place_changed', function () {
               infowindow.close();
               marker.setVisible(false);
               var place = autocomplete.getPlace();
               if (!place.geometry) {
                   window.alert("Autocomplete's returned place contains no geometry");
                   return;
               }

               // If the place has a geometry, then present it on a map.
               if (place.geometry.viewport) {
                   map.fitBounds(place.geometry.viewport);
               } else {
                   map.setCenter(place.geometry.location);
                   map.setZoom(17);  // Why 17? Because it looks good.
               }
               marker.setIcon(/** @type {google.maps.Icon} */({
                   url: place.icon,
                   size: new google.maps.Size(71, 71),
                   origin: new google.maps.Point(0, 0),
                   anchor: new google.maps.Point(17, 34),
                   scaledSize: new google.maps.Size(35, 35)
               }));
              
               marker.setPosition(place.geometry.location);
               marker.setVisible(true);

               document.getElementById('gps').value = place.geometry.location;

               document.getElementById('eventaddress').value = place.formatted_address;

               var address = '';
               if (place.address_components) {
                   address = [
                     (place.address_components[0] && place.address_components[0].short_name || ''),
                     (place.address_components[1] && place.address_components[1].short_name || ''),
                     (place.address_components[2] && place.address_components[2].short_name || '')
                   ].join(' ');
               }
               
               infowindow.setContent('<div><strong>' + place.name + '</strong><br>' + address);
               infowindow.open(map, marker);
           });

           // Sets a listener on a radio button to change the filter type on Places
           // Autocomplete.
           function setupClickListener(id, types) {
               var radioButton = document.getElementById(id);
               radioButton.addEventListener('click', function () {
                   autocomplete.setTypes(types);
               });
           }

           setupClickListener('changetype-all', []);
           setupClickListener('changetype-address', ['address']);
           setupClickListener('changetype-establishment', ['establishment']);
           setupClickListener('changetype-geocode', ['geocode']);

       }
       google.maps.event.addDomListener(window, "load", initMap);

   </script>
     
   

</asp:Content>
