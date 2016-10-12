<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="ViewEvent.aspx.cs" Inherits="IceBreak.ViewEvent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="stylesheets/ViewEvent.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="page-content-wrapper">
     <header class="image-bg-fluid-height" id ="EventImage" runat="server">
        
    </header>

    <!-- Content Section -->
    <section>
        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <h1 class="section-heading" id="EventName" runat="server"></h1>                    
                    <p class="lead section-lead" id ="EventDate" runat="server"></p>
                    <p class="lead section-lead" id ="EventStart" runat="server"></p>
                    <p class="lead section-lead" id ="EventEnd" runat="server"></p>
                    <p class="lead section-lead" id ="EndTime" runat="server"></p>
                    <p class="lead section-lead" id ="EventAddress" runat="server"></p>
                    <p class="section-paragraph" id ="EVentDescription" runat="server"></p>
                </div>
            </div>
        </div>
    </section>

    <!-- Fixed Height Image Aside -->
    <!-- Image backgrounds are set within the full-width-pics.css file. -->
    <aside class="image-bg-fixed-height"></aside>
    </div>
     
</asp:Content>
