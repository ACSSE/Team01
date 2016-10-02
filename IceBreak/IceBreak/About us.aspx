<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="About us.aspx.cs" Inherits="IceBreak.About_us" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="stylesheets/profile.css" />
 <link rel="stylesheet" href="~/stylesheets/sidebar.css" runat="server"/>
 <script src="js/hover.js"></script>
    <script>
        $(document).ready(function ()
        {
            if ($(document).hasClass("active"))
            {
                $(document).removeClass("active");
            }
            $(".menu_about").addClass("active");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id="page-content-wrapper">
        <style>
        #container 
        {
           position:fixed;
           background-image:url(../images/profile.jpg);
            width:100%;
            height:120vh;
        }
        .AboutGallery 
        {
            width:100%;
            margin:50px auto;
            padding-left:360px;
            padding-top:180px
        }
        ul {
            list-style :none;
        }
        .AboutGallery li {
            height: 300px;
            width:300px;
            margin:25px;
            overflow:hidden;
            float:left;
            z-index:0;
             -webkit-border-radius: 50%;
            -moz-border-radius:50%;
            border-radius:50%;
 
        }

        .AboutGallery li.bottom {
            position:absolute;
            width: 300px;
	        height: 450px;
            z-index:1;
            margin-top: -70px;
           

            -webkit-transition:  all 1s ease;
            -moz-transition: all 1s ease;
            -ms-transition:  all 1s ease;
            -o-transition: all 1s ease;
            transition:  all 1s ease;
            opacity:0;
        }
        .AboutGallery li img {
          
            width: 300px;
	        height: 450px;
            z-index:1;
            margin-top: -70px;
           

            -webkit-transition:  all 1s ease;
            -moz-transition: all 1s ease;
            -ms-transition:  all 1s ease;
            -o-transition: all 1s ease;
            transition:  all 1s ease;
        }
           
        .AboutGallery li:hover img { 
  -webkit-transform: none;
     -moz-transform: none;
       -o-transform: none;
      -ms-transform: none;
          transform: none;
           
            top: 170px; 
            left: 0px;
            height: 90%; 
            width: 100%;
           position:fixed;
      
            z-index: 3;
                }
    </style>
   <div id ="container">
    <div class="AboutGallery">
        <ul>
            <li><img src ="images/AboutUs/SLOGAN.jpg" /><img class="bottom" src ="images/AboutUs/SLOGAN2.jpg" /></li>
            <li><img src ="images/Company.png" /><a href ="company.aspx"><img class ="bottom" src ="images/AboutUs/Company2.jpg" /></a></li>
            <li><img src ="images/founder.png" /><a href ="foundervid.aspx"><img class ="bottom"src ="images/About3.jpeg"/></a></li>
        </ul>
    </div>
    
       </div>  
         
         
     </div>
</asp:Content>
