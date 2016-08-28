<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="IceBreak.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" >
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
   
      <style>
      
    </style>
    <div id="con" style="text-align:center">
        <br />
        <br />
        <br />
        <br />
        <br />
       
        <div style="width:200px;margin:auto;font-size:18pt;font-family:'Bookman Old Style'">
            <p>Profile</p>
        </div>
        
        <p style="font-weight:bold;font-size:13pt">
            Enter the following credentials:
        </p>
        
        <div style="text-align:center;font-family:'Bookman Old Style';font-weight:300;width:350px;margin:auto;" id="inputs">
            <asp:Label ID="Label1" runat="server" Text="1.Name" style="font-size:11pt;"></asp:Label>
            <span id="txtName_lbl" runat="server"></span>
            <asp:TextBox ID="txtName" class="form-control" runat="server" Width="345px" style="margin-right:12%;border-radius:5px;height:20px;border-color:black" ></asp:TextBox>
            
            <asp:Label ID="Label2" runat="server" Text="2.Surname" style="font-size:11pt;"></asp:Label>
            <span id="txtSurname_lbl" runat="server"></span>
            <asp:TextBox ID="txtSurname" runat="server" class="form-control" Width="345px" style="border-radius:5px;height:20px;border-color:black"></asp:TextBox>
            
            <asp:Label ID="Label3" runat="server" Text="3.E-Mail" style="font-size:11pt;"></asp:Label>
            <span id="txtEmail_lbl" runat="server"></span>
            <asp:TextBox ID="txtEmail" runat="server" class="form-control" Width="345px" style="border-radius:5px;height:20px;border-color:black"></asp:TextBox>
            
            <asp:Label ID="Label4" runat="server" Text="4.Username" style="font-size:11pt;"></asp:Label>
            <span id="txtUser_lbl" runat="server"></span>
            <asp:TextBox ID="txtUser" runat="server" class="form-control" Width="345px" style="border-radius:5px;height:20px;border-color:black"></asp:TextBox>
            
            <asp:Label ID="Label5" runat="server" Text="5.Age" style="font-size:11pt;"></asp:Label>
            <span id="txtAge_lbl" runat="server"></span>
            <asp:TextBox ID="txtAge" runat="server" class="form-control" Width="345px" style="border-radius:5px;height:20px;border-color:black"></asp:TextBox>
            
            <asp:Label ID="Label6" runat="server" Text="6.Bio" style="font-size:11pt;"></asp:Label>
            <span id="txtPass_lbl" runat="server"></span>
            <asp:TextBox  ID="txtPass" runat="server" class="form-control" Width="345px" style="border-radius:5px;border-color:black"></asp:TextBox>
            
            <asp:Label ID="Label7" runat="server" Text="7.Catchphrase" style="font-size:11pt;"></asp:Label>
            <span id="txtConfirm_lbl" runat="server"></span>
            <asp:TextBox  ID="txtConfirm" runat="server" class="form-control" Width="345px" style="border-radius:5px;height:20px;border-color:black"></asp:TextBox>
       
           
        </div> 
    </div>
    <br />
    <br />
    <br />  
            

  </div>

</asp:Content>
