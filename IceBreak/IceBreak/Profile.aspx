<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="IceBreak.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="stylesheets/profile.css" />
    <script src="scripts/scripts.js"></script>
    <script src="scripts/jquery.backstretch.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-content-wrapper">
        <div id="body" class="body" style="position: relative">
        </div>

        <div id="img-circle" class="img-circle">
            <asp:Label ID="lblName" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt;text-align:center;  margin-left: 40%"></asp:Label>
            <asp:Label ID="lblSurname" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt; text-align: center "></asp:Label>
            <img class="img-circle" src="images/Tevin.jpg" />
            <br />
            <br />
           
             <asp:Label ID="lblOccupation" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt; text-align:match-parent;margin-left: 40%"></asp:Label>
                 <br />
           
             <asp:Label ID="lblAge" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt; text-align:match-parent;margin-left: 60%"></asp:Label>
                 <br />
            <br />
             <asp:Label ID="lblB" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt; text-align:match-parent;margin-left: 55%">Bio:</asp:Label>
            <br />
             <asp:Label ID="lblBio" runat="server" Style="font-family: 'Bookman Old Style'; font-size: 18pt; text-align:inherit;margin-left: 50%"></asp:Label>
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

                <asp:TextBox ID="txtName" class="form-control" runat="server" Width="345px" Style="margin-right: 12%; height: 25px; border-color: black" Visible="True"></asp:TextBox>

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
                <asp:TextBox ID="txtAge" runat="server" class="form-control" Width="345px" Style="height: 25px; border-color: black" TextMode="Number"></asp:TextBox>

                <asp:Label ID="Label6" runat="server" Text="6.Bio" Style="font-size: 11pt;"></asp:Label>
                <span id="txtPass_lbl" runat="server"></span>
                <asp:TextBox ID="txtBio" runat="server" class="form-control" Width="345px" Style="height: 25px; border-color: black"></asp:TextBox>

                <asp:Label ID="Label7" runat="server" Text="7.Catchphrase" Style="font-size: 11pt;"></asp:Label>
                <span id="txtConfirm_lbl" runat="server"></span>
                <asp:TextBox ID="txtCatch" runat="server" class="form-control" Width="345px" Style="height: 25px; border-color: black"></asp:TextBox>


            </div>
        </div>
        <br />
        <br />
        <br />



    </div>

</asp:Content>
