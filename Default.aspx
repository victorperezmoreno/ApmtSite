<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" ClientIDMode="Static" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>Appointment Booking</title>
  <link href="App_Themes/Darkbrown/jquery-ui.theme.min.css" rel="stylesheet" />
  <script src="Scripts/jquery/jquery-3.2.1.min.js"></script>
  <script src="Scripts/jqueryui/jquery-ui-1.12.1.min.js"></script>
  <script src="Scripts/inputmask/jquery.inputmask.bundle.min.js"></script>
  <script src="Scripts/inputmask/phone-codes/phone.min.js"></script>
  <script src="Scripts/inputmask/phone-codes/phone-ru.min.js"></script>
  <script src="Scripts/inputmask/phone-codes/phone-be.min.js"></script>
  
  <script> 
    //calendar for summary of appointments on a given date
    $(function () {
      $("#TxtAppointmentSummary").datepicker({
        minDate: -2, maxDate: "+1M +10D", beforeShowDay: function (date) {
          var day = date.getDay();
          return [(day != 0), ''];
        }
      });
    });
    //Hide customer registration success/failure message
    function vanishMessageCenter() {
      $(document).ready(function () {
        $('.Attention').delay(5000).fadeOut('slow');
      });
    }
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
        //txtbox.focus();
        return false;
      }
      else {
        txtbox.style.borderColor = "#e2e2e2";
        ValidatorEnable(document.getElementById("RequiredFieldValidator7"), false);
      }
    }

    //calendar for booking accordion 
    $(function () {
      var dateTime = new Date();
      var hour = dateTime.getHours();
      var minute = dateTime.getMinutes();
      if ((hour >= 18) && (minute >= 30)) {
        $("#TxtAppointmentDate").datepicker({
          minDate: 1, maxDate: "+1M +10D", beforeShowDay: function (date) {
            var day = date.getDay();
            return [(day != 0), ''];
          }
        });
      }
      else {
        $("#TxtAppointmentDate").datepicker({
          minDate: 0, maxDate: "+1M +10D", beforeShowDay: function (date) {
            var day = date.getDay();
            return [(day != 0), ''];
          }
        });
      }
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
        //txtbox.focus();
        return false;
      }
      else {
        txtbox.style.borderColor = "#e2e2e2";
        ValidatorEnable(document.getElementById("RequiredFieldValidator8"), false);
      }
    }

   //As the page is partially updated with the Ajax UpdatePanel
   //This code helps to redisplay the datepicker after booking an appointment--%>
    $(document).ready(function () {
      Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
      function EndRequestHandler(sender, args) {
        $('#TxtAppointmentSummary').datepicker({
          minDate: -2, maxDate: "+1M +10D", beforeShowDay: function (date) {
            var day = date.getDay();
            return [(day != 0), ''];
          }
        });
      }
    });
  </script>
    
  <webopt:bundlereference ID="Bundlereference1" runat="server" Path="~/StyleSheets" /> 
  <webopt:bundlereference ID="Bundlereference2" runat="server" Path="~/jquery" />
  <webopt:bundlereference ID="Bundlereference3" runat="server" Path="~/jqueryui" />
  <webopt:bundlereference ID="Bundlereference4" runat="server" Path="~/jqueryinputmask" /> 
  <webopt:bundlereference ID="Bundlereference5" runat="server" Path="~/jqueryinputmaskphonemin" />
  <webopt:bundlereference ID="Bundlereference6" runat="server" Path="~/jqueryinputmaskphoneru" />
  <webopt:bundlereference ID="Bundlereference7" runat="server" Path="~/jqueryinputmaskphonebe" />
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
              <li><a href="https://www.facebook.com/hairbyyeonfamilyhairsalon">
                <img src="../App_Themes/Darkbrown/Social icons/Facebook.png" /></a> </li>
              <li><a href="https://www.instagram.com/hairbyyeon/">
                <img src="../App_Themes/Darkbrown/Social icons/Instagram.png" />
              </a></li>
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
                  <asp:UpdatePanel ID="UpdtPanelAppointment" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="False">
                    <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="ApmtButton" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                      <div class="divNewApmtTableCell">
                        <asp:Label ID="LblName" runat="server" Text="Name: "></asp:Label>
                        <ajaxToolkit:ComboBox ID="CmbBxName" runat="server" CssClass="WindowsStyle" AutoCompleteMode="Append"
                          MaxLength="0" Style="display: inline;" onclientblur="OnClientBlur">
                        </ajaxToolkit:ComboBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="CmbBxName$CmbBxName_TextBox" CssClass="ErrorMessage" Display="Dynamic" ValidationGroup="BookingInfoGroup" ErrorMessage="Enter a Name" InitialValue="Please Select">Enter a Name</asp:RequiredFieldValidator>
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
                          ValidationGroup="BookingInfoGroup" ErrorMessage="Pick at least one service">Pick a service</asp:CustomValidator>
                      </div>
                      <div class="divNewApmtTableCell">
                        <asp:Label ID="LblStylist" runat="server" Text="Stylist: "></asp:Label>
                        <asp:DropDownList ID="DDLStylist" runat="server"></asp:DropDownList>
                      </div>
                      <div class="divNewApmtTableCell">
                        <asp:Label ID="LblDate" runat="server" Text="Date: "></asp:Label><asp:TextBox ID="TxtAppointmentDate" runat="server" MaxLength="10" onblur="return validateDate(this);" OnTextChanged="TxtAppointmentDateChange" AutoPostBack="True"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="TxtAppointmentDate" CssClass="ErrorMessage" Display="Dynamic" ValidationGroup="BookingInfoGroup" ErrorMessage="Enter a date">Enter a date</asp:RequiredFieldValidator>
                        <br />
                        <asp:CustomValidator runat="server" ID="CustValOutBusinessHours" CssClass="ErrorMessage"
                          OnServerValidate="OutBusinessHours_ServerValidation" ValidationGroup="BookingInfoGroup"
                          ErrorMessage="We close at 7pm">We close at 7pm</asp:CustomValidator>
                      </div>
                    </ContentTemplate>
                  </asp:UpdatePanel>
                  <div class="divNewApmtTableCell">
                    <asp:UpdatePanel ID="UpdtPanelStartTime" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="False">
                      <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="TxtAppointmentDate" EventName="TextChanged" />
                      </Triggers>
                      <ContentTemplate>
                        <asp:Label ID="Label2" runat="server" Text="Start Time: "></asp:Label><asp:DropDownList ID="DDLBeginTime" runat="server"></asp:DropDownList>
                      </ContentTemplate>
                    </asp:UpdatePanel>     
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
                  <asp:UpdatePanel ID="UpdtePanelCustomerRegistration" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="False">
                    <ContentTemplate>
                      <div class="divNewCustomerTableCell">
                        <asp:Label ID="LblFirstName" runat="server" Text="First Name"></asp:Label>
                      </div>
                      <div class="divNewCustomerTableCell">
                        <asp:TextBox ID="TxtFirstName" runat="server" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 32) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122));" MaxLength="15" onblur="return validateFirstName(this);"></asp:TextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtFirstName" CssClass="ErrorMessage" Display="Dynamic" ValidationGroup="RegistrationInfoGroup" ErrorMessage="Enter customer first name">Please enter first name</asp:RequiredFieldValidator>
                      </div>
                      <div class="divNewCustomerTableCell">
                        <asp:Label ID="LblLastName" runat="server" Text="Last Name"></asp:Label>
                      </div>
                      <div class="divNewCustomerTableCell">
                        <asp:TextBox ID="TxtLastName" runat="server" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 32) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122));" MaxLength="20" onblur="return validateLastName(this);"></asp:TextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtLastName" CssClass="ErrorMessage" Display="Dynamic" ValidationGroup="RegistrationInfoGroup" ErrorMessage="Enter customer last name">Please enter last name</asp:RequiredFieldValidator>
                      </div>
                      <div class="divNewCustomerTableCell">
                        <asp:Label ID="LblPhoneNumber" runat="server" Text="Cell Phone"></asp:Label>
                        <asp:Label ID="LblPhonePlaceholder" runat="server" Text="(Only numbers)" CssClass="LabelPlaceHolder"></asp:Label>
                      </div>
                      <div class="divNewCustomerTableCell">
                        <asp:TextBox ID="TxtPhoneCell" runat="server" TextMode="Phone" PlaceHolder="(XXX) XXX-XXXX" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 9) || (event.keyCode >= 45 && event.keyCode <= 57));" MaxLength="14" onblur="return validatePhone(this);"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TxtPhoneCell" CssClass="ErrorMessage" Display="Dynamic" ValidationGroup="RegistrationInfoGroup" ErrorMessage="Enter a phone number">Please enter phone number</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtPhoneCell" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter a valid phone number"
                          ValidationExpression="(?:(?:(\s*\(?([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\)?\s*(?:[.-]\s*)?)([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})">Enter valid phone number</asp:RegularExpressionValidator>
                      </div>
                      <div class="divNewCustomerTableCell">
                        <asp:Label ID="LblEmail" runat="server" Text="Email"></asp:Label>
                      </div>
                      <div class="divNewCustomerTableCell">
                        <asp:TextBox ID="TxtEmail" runat="server" TextMode="Email" MaxLength="40" onblur="return validateEmail(this);"></asp:TextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TxtEmail" CssClass="ErrorMessage" Display="Dynamic" ValidationGroup="RegistrationInfoGroup" ErrorMessage="Enter customer email">Please enter email</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                          ControlToValidate="TxtEmail" ErrorMessage="Enter correct email" CssClass="ErrorMessage"
                          ValidationExpression="[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?"></asp:RegularExpressionValidator>
                      </div>
                      <div class="divNewCustomerTableCell">
                        <asp:Button ID="NewCustomerButton" runat="server" Text="Register!" OnClick="BtnNewCustomer_Click" CssClass="submitButton" ValidationGroup="RegistrationInfoGroup" />
                      </div>
                    </ContentTemplate>
                  </asp:UpdatePanel>
                </div>
              </Content>
            </ajaxToolkit:AccordionPane>
          </Panes>
        </ajaxToolkit:Accordion>
      </div>
       <div id="content">
         <asp:UpdatePanel ID="UpdtPanelMessageCenter" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="False">
            <ContentTemplate>       
             <asp:Label ID="LblMessageToUser" runat="server" CssClass="Attention" Text="Message to User" Visible="False"></asp:Label>
             <asp:TextBox ID="TxtAppointmentSummary"  CssClass="mainCalendar" runat="server" MaxLength="10" onblur="return validateAppointmentSummaryDate(this);" AutoPostBack="True" OnTextChanged="TxtSummaryDateChange"/>
            </ContentTemplate>
         </asp:UpdatePanel>
          
        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TxtAppointmentSummary" CssClass="ErrorMessage" Display="Dynamic" validationgroup="ScheduledAppointmentsGroup" ErrorMessage="Enter a date">Enter a date</asp:RequiredFieldValidator>
         <asp:UpdatePanel ID="UpdtPanelScheduleGridView" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="False">
           <Triggers>
             <asp:AsyncPostBackTrigger ControlID="TxtAppointmentSummary" EventName="TextChanged"/>
           </Triggers>
           <ContentTemplate>       
             <asp:GridView ID="grdViewSchedule" runat="server" CssClass="ScheduleGridview" RowStyle-CssClass="ColumnAuto">
             </asp:GridView>
           </ContentTemplate>
         </asp:UpdatePanel>
      </div>
      </div>
      <div id="footer">
        <p>&copy;Copyright 2018 Vicking Development all rights reserved.</p>
      </div>
      </div>
  </form>
</body>
</html>