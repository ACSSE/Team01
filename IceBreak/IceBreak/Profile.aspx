<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="IceBreak.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="stylesheets/profile.css" />
 <link rel="stylesheet" href="~/stylesheets/sidebar.css" runat="server"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-content-wrapper">
        <div class="background" >
            <img src="images/profile.jpg" style="position:absolute;background-size:5000px"/>
       
        <div id="img-circle" class="img-circle" style="margin-top:5%;position:absolute">
            <div style="text-align:center;width:500px">
             <asp:Label ID="lblName" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt;text-align:center"></asp:Label>
            <asp:Label ID="lblSurname" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt; text-align: center "></asp:Label>
                <br />
                <br />
            </div>
            
            <div id="DIV" class="img-circle" style="align-content:center" runat="server">
              <img class="img-circle"  />
         </div>
            <br />
            
           <div style="margin-top:100%;text-align:center;margin-left:100px">

           </div>
            
              <div style="text-align:center;width:500px">
                   <asp:Label ID="lblOccupation" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt; text-align:center"></asp:Label>
                 <br />
           
             <asp:Label ID="lblAge" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt; text-align:center"></asp:Label>
                 <br />
                 <br />
             <asp:Label ID="lblB" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt; text-align:center">Bio:</asp:Label>
                 <br />
                  <asp:Label ID="lblBio" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt; align-content:center"></asp:Label>
           </div>
             
        </div>


        <div id="con" style="text-align: center">
            <br />
            <br />
            <br />

            <div style="text-align: center; font-family: 'Bookman Old Style'; font-weight: 300; width: 350px; margin: auto;" id="inputs">
                <strong style="color: #59D0F7; font-size: 26pt">IceBreak User</strong>
                <h2>Current details:</></h2>
                <asp:Label ID="Label1" runat="server" Text="1.Name" Style="font-size: 11pt;"></asp:Label>
                <span id="txtName_lbl" runat="server"></span>
                
               
                <asp:TextBox ID="txtName" class="form-control" runat="server" Width="100%" Style="margin-right: 12%; height: 25px; border-color: black" Visible="True"></asp:TextBox>
                
                <asp:Label ID="Label2" runat="server" Text="2.Surname" Style="font-size: 11pt;"></asp:Label>
                <span id="txtSurname_lbl" runat="server"></span>
                <asp:TextBox ID="txtSurname" runat="server" class="form-control" Width="345px" Style="height: 25px; border-color: black" Visible="True"></asp:TextBox>

                <asp:Label ID="Label3" runat="server" Text="3.E-Mail" Style="font-size: 11pt;"></asp:Label>
                <span id="txtEmail_lbl" runat="server"></span>
                <asp:TextBox ID="txtEmail" runat="server" class="form-control" Width="345px" Style="height: 25px; border-color: black" Visible="True"></asp:TextBox>

                <asp:Label ID="Label4" runat="server" Text="4.Username" Style="font-size: 11pt;"></asp:Label>
                <span id="txtUser_lbl" runat="server"></span>
                <asp:TextBox ID="txtUser" runat="server" class="form-control" Width="345px" Style="height: 25px; border-color: black"></asp:TextBox>

                <asp:Label ID="Label5" runat="server" Text="5.Age" Style="font-size: 11pt;"></asp:Label>
                <span id="txtAge_lbl" runat="server"></span>
                <asp:TextBox ID="txtAge" runat="server" class="form-control" Width="345px" Style="height: 25px; border-color: black" ></asp:TextBox>

                <asp:Label ID="Label7" runat="server" Text="7.Catchphrase" Style="font-size: 11pt;"></asp:Label>
                <span id="txtConfirm_lbl" runat="server"></span>
                <asp:TextBox ID="txtCatch" runat="server" class="form-control" Width="345px" Style="height: 25px; border-color: black"></asp:TextBox>

                <asp:Label ID="Label6" runat="server" Text="6.Bio" Style="font-size: 11pt;"></asp:Label>
                <span id="txtPass_lbl" runat="server"></span>
                <asp:TextBox ID="txtBio" runat="server" class="form-control" Width="345px" Style="height: 80px; border-color: black" TextMode="MultiLine"></asp:TextBox>

                <asp:Button ID="update" runat="server" Text="Update Information" OnClick="Updatebutton_click" Height="49px" Width="139px" style="border-radius:5px;border-color:black;margin:auto;margin-top:20px;background-color: #59D0F7"/>

            </div>
        </div>
         </div>
    </div>

</asp:Content>
