<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppointmentData.aspx.cs" Inherits="AppointmentData" ClientIDMode="Static" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta charset="utf-8"/>
  <meta name="viewport" content="width=device-width, initial-scale=1"/>
  <title>Appointment Booking</title> 
  <link href="Scripts/jquery-ui.min.css" rel="stylesheet" />
  <script src="Scripts/jquery-3.2.1.js"></script>
  <script src="Scripts/jquery-ui-1.12.1.js"></script>
  
  <script type="text/javascript">

    //Hide Appointment Confirmed success/failure message
    $(document).ready(function () {
      $('.Attention').delay(3000).fadeOut('slow');
    });

    //Hide Validation Summary
    $(document).ready(function () {
      $("input[type=text]").change(function () {
        Page_ClientValidate();
      });
    });
    
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
  </script>
</head>

<body>
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      <div class="divNewApmtTable" id="AppointmentTable">
        <div class="divNewApmtTableCell">
          <asp:Label ID="LblMessageToUser" runat="server" CssClass="Attention" Text="Appointment Confirmed" Visible="False"></asp:Label>
        </div>
        <div class="divNewApmtTableCell">
          <asp:Label ID="LblName" runat="server" Text="Name: "></asp:Label>
          <ajaxToolkit:ComboBox ID="CmbBxName" runat="server" CssClass="WindowsStyle" AutoCompleteMode="Append"
            MaxLength="0" Style="display: inline;" onclientblur="OnClientBlur">
          </ajaxToolkit:ComboBox>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="CmbBxName$CmbBxName_TextBox" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter a Name" InitialValue="Please Select">Enter a Name</asp:RequiredFieldValidator>
        </div>
        <div class="divNewApmtTableCell">
          <asp:Label ID="LblHairLength" runat="server" Text="Hair Length: "></asp:Label>
          <asp:DropDownList ID="DDLHairLength" runat="server"></asp:DropDownList>
        </div>
        <div class="divNewApmtTableCell">
          <asp:Label ID="LblServiceName" runat="server" Text="Service: "></asp:Label>
        </div>
        <div class="divNewApmtTableCell">
          <asp:CheckBoxList ID="ChkBxListServices" runat="server" width="150px" ></asp:CheckBoxList><asp:CustomValidator runat="server" ID="CustValServicesList" CssClass="ErrorMessage"
            ClientValidationFunction="verifyCheckboxList" OnServerValidate="ServicesList_ServerValidation"
            EnableClientScript="true" ErrorMessage="Pick at least one service">Pick a service</asp:CustomValidator>
        </div>
        <div class="divNewApmtTableCell">
          <asp:Label ID="LblStylist" runat="server" Text="Stylist: "></asp:Label>
          <asp:DropDownList ID="DDLStylist" runat="server"></asp:DropDownList>
        </div>
        <div class="divNewApmtTableCell">
          <asp:Label ID="LblDate" runat="server" Text="Date: "></asp:Label>
          <asp:TextBox ID="TxtAppointmentDate" runat="server" MaxLength="10" onblur="return validateDate(this);" OnTextChanged="TxtAppointmentDateChange" AutoPostBack="True"></asp:TextBox>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="TxtAppointmentDate" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter a date">Enter a date</asp:RequiredFieldValidator>
        </div>
        <div class="divNewApmtTableCell">
          <asp:Label ID="Label1" runat="server" Text="Start Time: "></asp:Label><asp:DropDownList ID="DDLBeginTime" runat="server"></asp:DropDownList>
        </div>
        <div class="divNewApmtTableCell">
          <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="" CssClass="validationSummaryErrors"
            DisplayMode="List" HeaderText="<div class='ValidationHeader'>&nbsp;Please correct the following:</div>" />
        </div>
        <div class="divNewApmtTableCell">
          
          <asp:Button ID="ApmtButton" runat="server" OnClick="ApmtButton_Click" Text="Book Now!" CssClass="submitButton" />
        </div>
      </div>



     </form>
</body>
</html>
