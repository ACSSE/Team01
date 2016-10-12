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
         <asp:Button ID="delete" runat="server" Text=""  style="display:none" OnClick="Delete" ></asp:Button>

</div>

    <script type="text/javascript">
        
        function Delete()
        {
            document.getElementById("delete").click();
        }
    </script>
</asp:Content>
