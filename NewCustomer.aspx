<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewCustomer.aspx.cs" Inherits="NewCustomer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <script src="Scripts/jquery-3.2.1.min.js"></script>
  <script src="Scripts/jquery.inputmask.bundle.min.js"></script>
  <script src="Scripts/inputmask/phone-codes/phone.min.js"></script>
  <script src="Scripts/inputmask/phone-codes/phone-ru.min.js"></script>
  <script src="Scripts/inputmask/phone-codes/phone-be.min.js"></script>
  <title>Register New Customer</title>
      <script type="text/javascript">
        //Mask for telephone number
        $(document).ready(function () {
          $("#TxtPhoneCell").inputmask("(999)-999-9999");
        });

        //Hide customer registration success/failure message
        $(document).ready(function () {
          $('.Attention').delay(3000).fadeOut('slow');
        });
      
      //Hide Validation Summary
      $(document).ready(function() {
        $("input[type=text]").change(function () {
         Page_ClientValidate();
        });
      });

    //Validate FirstName textbox
      function validateFirstName(txtbox) {
      if (txtbox.value == "") {
        txtbox.style.borderColor = "Red";
        ValidatorEnable(document.getElementById("RequiredFieldValidator2"), true);
        txtbox.focus();
        return false;
      }
      else
      {
        txtbox.style.borderColor = "#e2e2e2";
        ValidatorEnable(document.getElementById("RequiredFieldValidator2"), false);
      }
    }
    
    //Validate LastName textbox
      function validateLastName(txtbox) {

      if (txtbox.value == "") {
        txtbox.style.borderColor = "Red";
        ValidatorEnable(document.getElementById("RequiredFieldValidator3"), true);
        txtbox.focus();
        return false;
      }
      else {
        txtbox.style.borderColor = "#e2e2e2";
        ValidatorEnable(document.getElementById("RequiredFieldValidator3"), false);
      }
    }

    //Validate Phone number textbox
      function validatePhone(txtbox) {
      if (txtbox.value == "") {
        txtbox.style.borderColor = "Red";
        ValidatorEnable(document.getElementById("RequiredFieldValidator4"), true);
        txtbox.focus();
        return false;
      }
      else {
        txtbox.style.borderColor = "#e2e2e2";
        ValidatorEnable(document.getElementById("RequiredFieldValidator4"), false);
      }
    }

    //Validate Email textbox
    function validateEmail(txtbox) {
      if (txtbox.value == "") {
        txtbox.style.borderColor = "Red";
        ValidatorEnable(document.getElementById("RequiredFieldValidator5"), true);
        txtbox.focus();
        return false;
      }
      else {
        txtbox.style.borderColor = "#e2e2e2";
        ValidatorEnable(document.getElementById("RequiredFieldValidator5"), false);
      }
    }     
    </script>  
</head>

<body>
  <form id="form1" runat="server">
    <div id="wrapper">
      <header>
        <a href="/"></a>
        <div id="top">
          <div id="logo">
            <a href="../Default">
              <img src="../App_Themes/Darkbrown/Logo/CoffeeLogo.png" /></a>
          </div>

          <div id="social-media">
            <p>Connect with us </p>
            <ul>
              <li><a href="www.facebook.com">
                <img src="../App_Themes/Darkbrown/Social icons/Facebook.png" /></a> </li>
              <li><a href="www.tweeter.com">
                <img src="../App_Themes/Darkbrown/Social icons/Tweeter.png" />
              </a></li>
              <li><a href="www.youtube.com">
                <img src="../App_Themes/Darkbrown/Social icons/Youtube.png" /></a> </li>
            </ul>
            <p>Contact us today for more info 703 968 6750</p>
          </div>
        </div>
      </header>
      <div>
        <div class="divNewCustomerTable" id="NewCustomerTable">
          <div class="divNewCustomerTableCell">
            <p class="creatingTableHeader">Customer Registration</p>
          </div>
          <div class="divNewCustomerTableCell">
            <asp:Label ID="LblMessageToUser" runat="server" CssClass="Attention" Text="" Visible="False"></asp:Label>
          </div>
          <div class="divNewCustomerTableCell">
            <asp:Label ID="LblFirstName" runat="server" Text="First Name"></asp:Label>
          </div>
          <div class="divNewCustomerTableCell">
            <asp:TextBox ID="TxtFirstName" runat="server" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 32) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122));" MaxLength="15" onblur="return validateFirstName(this);"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtFirstName" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter customer first name">Please enter first name</asp:RequiredFieldValidator>
          </div>
          <div class="divNewCustomerTableCell">
            <asp:Label ID="LblLastName" runat="server" Text="Last Name"></asp:Label>
          </div>
          <div class="divNewCustomerTableCell">
            <asp:TextBox ID="TxtLastName" runat="server" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 32) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122));" MaxLength="20" onblur="return validateLastName(this);"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtLastName" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter customer last name">Please enter last name</asp:RequiredFieldValidator>
          </div>
          <div class="divNewCustomerTableCell">
            <asp:Label ID="LblPhoneNumber" runat="server" Text="Cell Phone"></asp:Label>
            <asp:Label ID="LblPhonePlaceholder" runat="server" Text="(Only numbers)" CssClass="LabelPlaceHolder"></asp:Label>
          </div>
          <div class="divNewCustomerTableCell">
            <asp:TextBox ID="TxtPhoneCell" runat="server" Name="TxtPhoneCell" PlaceHolder="(XXX) XXX-XXXX" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 9) || (event.keyCode >= 45 && event.keyCode <= 57));" MaxLength="14" onblur="return validatePhone(this);"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TxtPhoneCell" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter a phone number">Please enter phone number</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtPhoneCell" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter a valid phone number"
              ValidationExpression="(?:(?:(\s*\(?([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\)?\s*(?:[.-]\s*)?)([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})">Enter valid phone number</asp:RegularExpressionValidator>
          </div>
          <div class="divNewCustomerTableCell">
            <asp:Label ID="LblEmail" runat="server" Text="Email"></asp:Label>
          </div>
          <div class="divNewCustomerTableCell">
            <asp:TextBox ID="TxtEmail" runat="server" MaxLength="20" onblur="return validateEmail(this);"></asp:TextBox>
            <br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TxtEmail" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter customer email">Please enter email</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
              ControlToValidate="TxtEmail" ErrorMessage="Enter correct email" CssClass="ErrorMessage"
              ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
          </div>
          <div class="divNewCustomerTableCell">
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ForeColor="" CssClass="validationSummaryErrors"
              DisplayMode="List" HeaderText="<div class='ValidationHeader'>&nbsp;Please correct the following:</div>" />
          </div>
          <div class="divNewCustomerTableCell">
            <asp:Button ID="NewCustomerButton" runat="server" Text="Register!" OnClick="BtnNewCustomer_Click" CssClass="submitButton" />
          </div>
        </div>
      </div>
      <footer>
        <p>&copy;Copyright 2018 Vicking Development all rights reserved.</p>
      </footer>
    </div>
  </form>
</body>
</html>
