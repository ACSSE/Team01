<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master.Master" CodeBehind="stats.aspx.cs" Inherits="IceBreak.stats" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>IceBreak | Statistics</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <div style="width:400px;height:200px;margin:auto;">
        <asp:Button name="btnEvents" id="btnEvents" runat="server" OnClick="btnEvents_Click" Text="Your events' stats" />
        <asp:Button name="btnIndividual" id="btnIndividual" runat="server" OnClick="btnIndividual_Click" Text="Your individual stats" />
    </div>
    <div style="width:100%;height:auto;margin:auto;" runat="server" id="container">
    </div>
    <!--<div style="width:500px;height:500px;margin:auto;">
        <canvas id="myChart" width="600" height="400"></canvas>
    </div>
    <div style="width:500px;height:500px;margin:auto;">
        <canvas id="myChart2" width="600" height="400"></canvas>
    </div>
    <div style="width:500px;height:500px;margin:auto;">
        <canvas id="myChart3" width="600" height="400"></canvas>
    </div>-->
</asp:Content>