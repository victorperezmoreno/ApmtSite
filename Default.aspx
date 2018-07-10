<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" ClientIDMode="Static" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>Appointment Booking</title>
  <link href="Scripts/jquery-ui.min.css" rel="stylesheet" /> 
  <script src="Scripts/jquery-3.2.1.min.js"></script>
  <script src="Scripts/jquery-ui-1.12.1.js"></script>
  <script src="Scripts/jquery.inputmask.bundle.min.js"></script>
  <script src="Scripts/inputmask/phone-codes/phone.min.js"></script>
  <script src="Scripts/inputmask/phone-codes/phone-ru.min.js"></script>
  <script src="Scripts/inputmask/phone-codes/phone-be.min.js"></script>
  
  <script>
    //calendar for summary of appointments on a given date
    $(function () {
      $("#TxtAppointmentSummary").datepicker({ minDate: -2, maxDate: "+1M +10D" });
    });

    //Hide customer registration success/failure message
    $(document).ready(function () {
      $('.Attention').delay(3000).fadeOut('slow');
    });

    //Hide Validation Summary
    //$(document).ready(function () {
    //  $("input[type=text]").change(function () {
    //    Page_ClientValidate();
    //  });
    //});

    //Validate Services Checkbox
    function verifyCheckboxList(source, arguments) {
      var val = document.getElementById("ChkBxListServices");
      var col = val.getElementsByTagName("*");
      if (col != null) {
        for (i = 0; i < col.length; i++) {
          if (col.item(i).tagName == "INPUT") {
            if (col.item(i).checked) {
              arguments.IsValid = true;
              return;
            }
          }
        }
      }
      arguments.IsValid = false;
    }

    //Validate booking date textbox in accordion
    function validateDate(txtbox) {
      if (txtbox.value == "") {
        txtbox.style.borderColor = "Red";
        ValidatorEnable(document.getElementById("RequiredFieldValidator7"), true);
        txtbox.focus();
        return false;
      }
      else {
        txtbox.style.borderColor = "#e2e2e2";
        ValidatorEnable(document.getElementById("RequiredFieldValidator7"), false);
      }
    }

    //calendar for booking accordion
    $(function () {
      $("#TxtAppointmentDate").datepicker({ minDate: 0, maxDate: "+1M +10D" });
    });

    //Mask for telephone number
    $(document).ready(function () {
      $("#TxtPhoneCell").inputmask("(999)-999-9999");
    });

    //Validate FirstName textbox
    function validateFirstName(txtbox) {
      if (txtbox.value == "") {
        txtbox.style.borderColor = "Red"; 
        ValidatorEnable(document.getElementById("RequiredFieldValidator2"), true);
        return false;
      }
      else {
        txtbox.style.borderColor = "#e2e2e2"; 
        ValidatorEnable(document.getElementById("RequiredFieldValidator2"), false);
      }
    }

    //Validate LastName textbox
    function validateLastName(txtbox) {

      if (txtbox.value == "") {
        txtbox.style.borderColor = "Red";
        ValidatorEnable(document.getElementById("RequiredFieldValidator3"), true);
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
        return false;
      }
      else {
        txtbox.style.borderColor = "#e2e2e2";
        ValidatorEnable(document.getElementById("RequiredFieldValidator5"), false);
      }
    }

    //Validate AppointmentSummaryDate textbox in Main section
    function validateAppointmentSummaryDate(txtbox) {
      if (txtbox.value == "") {
        txtbox.style.borderColor = "Red";
        ValidatorEnable(document.getElementById("RequiredFieldValidator8"), true);
        txtbox.focus();
        return false;
      }
      else {
        txtbox.style.borderColor = "#e2e2e2";
        ValidatorEnable(document.getElementById("RequiredFieldValidator8"), false);
      }
    }
  </script>
     
</head>
<body>
  <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <div id="wrapper">
      <header>
        <a href="/"></a>
        <div id="top">
          <div id="logo">
            <a href="../Default">
              <img src="../App_Themes/Darkbrown/Logo/SalonLogo.png" /></a>
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
      <div id="content-wrapper">
      <div id="left-nav">
        <ajaxToolkit:Accordion ID="Accordion1" runat="server" HeaderCssClass="accordionHeaderCssClass" ContentCssClass="accordionContentCssClass" HeaderSelectedCssClass="accordionHeaderSelectedCss" FadeTransitions="true" TransitionDuration="500" RequireOpenedPane="false" AutoSize="None" SelectedIndex="0">
          <Panes>
            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
              <Header>Booking</Header>
              <Content>
                <div class="divNewApmtTable" id="AppointmentTable">
                  <div class="divNewApmtTableCell">
                    <asp:Label ID="Label1" runat="server" CssClass="Attention" Text="Appointment Confirmed" Visible="False"></asp:Label>
                  </div>
                  <div class="divNewApmtTableCell">
                    <asp:Label ID="LblName" runat="server" Text="Name: "></asp:Label>
                    <ajaxToolkit:ComboBox ID="CmbBxName" runat="server" CssClass="WindowsStyle" AutoCompleteMode="Append"
                      MaxLength="0" Style="display: inline;" onclientblur="OnClientBlur">
                    </ajaxToolkit:ComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="CmbBxName$CmbBxName_TextBox" CssClass="ErrorMessage" Display="Dynamic" validationgroup="BookingInfoGroup" ErrorMessage="Enter a Name" InitialValue="Please Select">Enter a Name</asp:RequiredFieldValidator>
                  </div>
                  <div class="divNewApmtTableCell">
                    <asp:Label ID="LblHairLength" runat="server" Text="Hair Length: "></asp:Label>
                    <asp:DropDownList ID="DDLHairLength" runat="server"></asp:DropDownList>
                  </div>
                  <div class="divNewApmtTableCell">
                    <asp:Label ID="LblServiceName" runat="server" Text="Service: "></asp:Label>
                  </div>
                  <div class="divNewApmtTableCell">
                    <asp:CheckBoxList ID="ChkBxListServices" runat="server" Width="150px"></asp:CheckBoxList><asp:CustomValidator runat="server" ID="CustValServicesList" CssClass="ErrorMessage"
                      ClientValidationFunction="verifyCheckboxList" OnServerValidate="ServicesList_ServerValidation" EnableClientScript="true" 
                      validationgroup="BookingInfoGroup" ErrorMessage="Pick at least one service">Pick a service</asp:CustomValidator>
                  </div>
                  <div class="divNewApmtTableCell">
                    <asp:Label ID="LblStylist" runat="server" Text="Stylist: "></asp:Label>
                    <asp:DropDownList ID="DDLStylist" runat="server"></asp:DropDownList>
                  </div>
                  <div class="divNewApmtTableCell">
                    <asp:Label ID="LblDate" runat="server" Text="Date: "></asp:Label>
                    <asp:TextBox ID="TxtAppointmentDate" runat="server" MaxLength="10" onblur="return validateDate(this);" OnTextChanged="TxtAppointmentDateChange" AutoPostBack="True"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="TxtAppointmentDate" CssClass="ErrorMessage" Display="Dynamic" validationgroup="BookingInfoGroup" ErrorMessage="Enter a date">Enter a date</asp:RequiredFieldValidator>
                  </div>
                  <div class="divNewApmtTableCell">
                    <asp:Label ID="Label2" runat="server" Text="Start Time: "></asp:Label><asp:DropDownList ID="DDLBeginTime" runat="server"></asp:DropDownList>
                  </div>
                  <div class="divNewApmtTableCell">
                    <asp:Button ID="ApmtButton" runat="server" OnClick="ApmtButton_Click" Text="Book Now!" CssClass="submitButton" validationgroup="BookingInfoGroup"/>
                  </div>
                </div>
              </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
              <Header>New User</Header>
              <Content>
                <div class="divNewCustomerTable" id="NewCustomerTable">
                  <div class="divNewCustomerTableCell">
                    <asp:Label ID="LblMessageToUser" runat="server" CssClass="Attention" Text="" Visible="False"></asp:Label>
                  </div>
                  <div class="divNewCustomerTableCell">
                    <asp:Label ID="LblFirstName" runat="server" Text="First Name"></asp:Label>
                  </div>
                  <div class="divNewCustomerTableCell">
                    <asp:TextBox ID="TxtFirstName" runat="server" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 32) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122));" MaxLength="15" onblur="return validateFirstName(this);"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtFirstName" CssClass="ErrorMessage" Display="Dynamic" validationgroup="RegistrationInfoGroup" ErrorMessage="Enter customer first name">Please enter first name</asp:RequiredFieldValidator>
                  </div>
                  <div class="divNewCustomerTableCell">
                    <asp:Label ID="LblLastName" runat="server" Text="Last Name"></asp:Label>
                  </div>
                  <div class="divNewCustomerTableCell">
                    <asp:TextBox ID="TxtLastName" runat="server" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 32) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122));" MaxLength="20" onblur="return validateLastName(this);"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtLastName" CssClass="ErrorMessage" Display="Dynamic" validationgroup="RegistrationInfoGroup" ErrorMessage="Enter customer last name">Please enter last name</asp:RequiredFieldValidator>
                  </div>
                  <div class="divNewCustomerTableCell">
                    <asp:Label ID="LblPhoneNumber" runat="server" Text="Cell Phone"></asp:Label>
                    <asp:Label ID="LblPhonePlaceholder" runat="server" Text="(Only numbers)" CssClass="LabelPlaceHolder"></asp:Label>
                  </div>
                  <div class="divNewCustomerTableCell">
                    <asp:TextBox ID="TxtPhoneCell" runat="server" PlaceHolder="(XXX) XXX-XXXX" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 9) || (event.keyCode >= 45 && event.keyCode <= 57));" MaxLength="14" onblur="return validatePhone(this);"></asp:TextBox><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TxtPhoneCell" CssClass="ErrorMessage" Display="Dynamic" validationgroup="RegistrationInfoGroup" ErrorMessage="Enter a phone number">Please enter phone number</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtPhoneCell" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter a valid phone number"
                      ValidationExpression="(?:(?:(\s*\(?([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\)?\s*(?:[.-]\s*)?)([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})">Enter valid phone number</asp:RegularExpressionValidator>
                  </div>
                  <div class="divNewCustomerTableCell">
                    <asp:Label ID="LblEmail" runat="server" Text="Email"></asp:Label>
                  </div>
                  <div class="divNewCustomerTableCell">
                    <asp:TextBox ID="TxtEmail" runat="server" MaxLength="20" onblur="return validateEmail(this);"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TxtEmail" CssClass="ErrorMessage" Display="Dynamic" validationgroup="RegistrationInfoGroup" ErrorMessage="Enter customer email">Please enter email</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                      ControlToValidate="TxtEmail" ErrorMessage="Enter correct email" CssClass="ErrorMessage"
                      ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                  </div>
                  <div class="divNewCustomerTableCell">
                    <asp:Button ID="NewCustomerButton" runat="server" Text="Register!" OnClick="BtnNewCustomer_Click" CssClass="submitButton" validationgroup="RegistrationInfoGroup" />
                  </div>
                </div>
              </Content>
            </ajaxToolkit:AccordionPane>
          </Panes>
        </ajaxToolkit:Accordion>
      </div>
       <div id="content">
        <asp:Label ID="Message" runat="server" CssClass="Attention" Text="Message to User" Visible="False"></asp:Label>
        <asp:TextBox ID="TxtAppointmentSummary"  CssClass="mainCalendar" runat="server" MaxLength="10" onblur="return validateAppointmentSummaryDate(this);" AutoPostBack="True" OnTextChanged="TxtSummaryDateChange"/>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TxtAppointmentSummary" CssClass="ErrorMessage" Display="Dynamic" validationgroup="ScheduledAppointmentsGroup" ErrorMessage="Enter a date">Enter a date</asp:RequiredFieldValidator>
        <asp:GridView ID="grdViewSchedule" runat="server" CssClass="ScheduleGridview" RowStyle-CssClass="ColumnAuto">
        </asp:GridView>
      </div>
      </div>
      <div id="footer">
        <p>&copy;Copyright 2018 Vicking Development all rights reserved.</p>
      </div>
      </div>
  </form>
</body>
</html>
