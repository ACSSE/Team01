<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="IceBreak.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet"/>
             <link rel="stylesheet" href="stylesheets/profile.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div id="page-content-wrapper">
      <div id ="body" class="body">
</div>
      <div id="img-circle" class="img-circle">
         <img class="img-circle" src="images/Tevin.jpg"/>
      </div>
    <div id="list2" >
   <ol >
      <li style=" border-style: none;border-bottom: solid #ff0000;"><p><em>The Netherlands</em> is a country in ...</p></li>
      <li><p><em>The United States of America</em> is a federal constitutional ...</p></li>
      <li><p><em>The Philippines</em> officially known as the Republic ...</p></li>
      <li><p><em>The United Kingdom</em> of Great Britain and ...</p></li>
   </ol>
</div>
            

  </div>

</asp:Content>
