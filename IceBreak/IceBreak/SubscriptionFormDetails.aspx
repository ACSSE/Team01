<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="SubscriptionFormDetails.aspx.cs" Inherits="IceBreak.SubscriptionFormDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="scripts/jquery.backstretch.min.js"></script>
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="stylesheets/SubscriptionForm.css" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


        <div class ="thecontainer">
             <h1>Become an Event Manager today!</h1>


            <div class="contentform">


                <div class="leftcontact">
                    
              
                    <div class="form-group">
                        <p class="heading2">Company <span>*</span></p>
                        <span class="icon-case"><i class="fa fa-home"></i></span>
                        <input type="text" name="society" id="society" data-rule="required" data-msg="Vérifiez votre saisie sur les champs : Le champ 'Société' doit être renseigné." />
                        <div class="validation"></div>
                    </div>

                     <div class="form-group">
                        <p class="heading2">E-mail <span>*</span></p>
                        <span class="icon-case"><i class="fa fa-envelope-o"></i></span>
                        <input type="email" name="email" id="email" data-rule="email" data-msg="Vérifiez votre saisie sur les champs : Le champ 'E-mail' est obligatoire." />
                        <div class="validation"></div>
                    </div>
                    
                    <div class="form-group">
                        <p class="heading2">Phone number <span>*</span></p>
                        <span class="icon-case"><i class="fa fa-phone"></i></span>
                        <input type="text" name="phone" id="phone" data-rule="maxlen:10" data-msg="Vérifiez votre saisie sur les champs : Le champ 'Téléphone' doit être renseigné. Minimum 10 chiffres" />
                        <div class="validation"></div>
                    </div>

                    <div class="form-group">
                        <p class="heading2">Company Address <span>*</span></p>
                        <span class="icon-case"><i class="fa fa-location-arrow"></i></span>
                        <input type="text" name="adresse" id="adresse" data-rule="required" data-msg="Vérifiez votre saisie sur les champs : Le champ 'Adresse' doit être renseigné." />
                        <div class="validation"></div>
                    </div>

                    <div class="form-group">
                        <p class="heading2">Postcode <span>*</span></p>
                        <span class="icon-case"><i class="fa fa-map-marker"></i></span>
                        <input type="text" name="postal" id="postal" data-rule="required" data-msg="Vérifiez votre saisie sur les champs : Le champ 'Code postal' doit être renseigné." />
                        <div class="validation"></div>
                    </div>

                    <div class="form-group">
                       <p class="heading2">City <span>*</span></p>
                        <span class="icon-case"><i class="fa fa-building-o"></i></span>
                        <input type="text" name="ville" id="ville" data-rule="required" data-msg="Vérifiez votre saisie sur les champs : Le champ 'Ville' doit être renseigné." />
                        <div class="validation"></div>
                    </div>
                </div>

                <div class="rightcontact">

                    <div class="form-group">
                       <p class="heading2">Card Number <span>*</span></p>
                        <span class="icon-case"><i class="fa fa-info"></i></span>
                        <input type="text" name="fonction" id="fonction" data-rule="required" data-msg="Vérifiez votre saisie sur les champs : Le champ 'Fonction' doit être renseigné." />
                        <div class="validation"></div>
                    </div>

                    <div class="form-group">
                        <p class="heading2">Card Holder Name <span>*</span></p>
                        <span class="icon-case"><i class="fa-user-md"></i></span>
                        <input type="text" name="sujet" id="holder" data-rule="required" data-msg="Vérifiez votre saisie sur les champs : Le champ 'Sujet' doit être renseigné." />
                        <div class="validation"></div>
                    </div>

                    <div class="form-group">
                        <p class="heading2">Verification Code<span>*</span></p>
                        <span class="icon-case"><i class="fa fa-code"></i></span>
                        <input type="text" name="sujet" id="cvv" data-rule="required" data-msg="Vérifiez votre saisie sur les champs : Le champ 'Sujet' doit être renseigné." />
                        <div class="validation"></div>
                    </div>

                    <div class="form-group">
                        <p class="heading2">Expiry Month<span>*</span></p>
                        <span class="icon-case"><i class="fa fa-calendar"></i></span>
                        <input type="text" name="sujet" id="month" data-rule="required" Value="Example- 01" data-msg="Vérifiez votre saisie sur les champs : Le champ 'Sujet' doit être renseigné." />
                        <div class="validation"></div>
                    </div>
                    <div class="form-group">
                        <p class="heading2">Expiry Year<span>*</span></p>
                        <span class="icon-case"><i class="fa fa-calendar"></i></span>
                        <input type="text" name="sujet" id="year" data-rule="required" data-msg="Vérifiez votre saisie sur les champs : Le champ 'Sujet' doit être renseigné." />
                        <div class="validation"></div>
                    </div>
                    <div class="form-group">
                        <p class="heading2">Card Security Number<span>*</span></p>
                        <span class="icon-case"><i class="fa-bank"></i></span>
                        <input type="text" name="sujet" id="year" data-rule="required" data-msg="Vérifiez votre saisie sur les champs : Le champ 'Sujet' doit être renseigné." />
                        <div class="validation"></div>
                    </div>
                </div>
            </div>
             <asp:Button runat="server" ID="btnChangeAccess"  UseSubmitBehavior="true" CssClass="SButton" OnClick="ChangeAccess" Text="Submit"></asp:Button>

        </div>
	
    <script type="text/javascript">
         jQuery(document).ready(function () {
             $.backstretch("images/pic4.jpg");
         });
     </script>

</asp:Content>
