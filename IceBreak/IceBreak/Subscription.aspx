<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="Subscription.aspx.cs" Inherits="IceBreak.Subscription" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="scripts/jquery.backstretch.min.js"></script>
    <link rel="stylesheet" href="stylesheets/Subscription.css"/>
    <link href="css/bootstrap.min.css" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="intro-header">
        <div class="container">

            <div class="row">
                <div class="col-lg-12">
                    <div class="intro-message">
                        <h1>Payment Options</h1>
                        <h3>You can choose how you want pay</h3>
                        <hr class="intro-divider">
                        <ul class="list-inline intro-social-buttons">
                            <li>
                                <a href="https://twitter.com/SBootstrap" class="btn btn-default btn-lg"><span class="network-name">Monthly R99</span></a>
                            </li>
                            <li>
                                <a href="https://github.com/IronSummitMedia/startbootstrap" class="btn btn-default btn-lg"> <span class="network-name">Yearly R999</span></a>
                            </li>
                            <li>
                                <a href="#" class="btn btn-default btn-lg"> <span class="network-name">Once Off</span></a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>

        </div>
        <!-- /.container -->

    </div>
    <script type="text/javascript">
         jQuery(document).ready(function () {
             $.backstretch("images/subscription.jpg");
         });
     </script>
</asp:Content>
