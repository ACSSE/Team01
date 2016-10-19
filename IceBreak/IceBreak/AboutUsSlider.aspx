<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="AboutUsSlider.aspx.cs" Inherits="IceBreak.AboutUsSlider" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="w3-content" style="max-width:100%;position:relative">
    <img class="mySlides" src="images/AboutUs/SLOGAN2.jpg" style="width:100%;height:100%"/>
<img class="mySlides" src="images/AboutUs/Company2.jpg" style="width:100%;height:100%"/>
<img class="mySlides" src="images/AboutUs/Office1.jpg" style="width:100%;height:100%"/>

   
</div>
     <img class="w3-btn-floating" src="images/AboutUs/left_icon.png"  style="position:absolute;top:50%;left:0;width:2.5%;height:5%" onclick="plusDivs(-1)"/>

    <img class="w3-btn-floating" src="images/AboutUs/right_icon.png"  style="position:absolute;top:50%;right:0;width:2.5%;height:5%" onclick="plusDivs(1)"/>
    <script>

var slideIndex = 1;
showDivs(slideIndex);

function plusDivs(n) {
  showDivs(slideIndex += n);
}

function showDivs(n) {
  var i;
  var x = document.getElementsByClassName("mySlides");
  if (n > x.length) {slideIndex = 1}
  if (n < 1) {slideIndex = x.length}
  for (i = 0; i < x.length; i++) {
     x[i].style.display = "none";
  }
  x[slideIndex-1].style.display = "block";
}
</script>

 
</asp:Content>
