<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AppointmentForm.ascx.cs" Inherits="Controls_AppointmentForm" %>
<style type="text/css">
  .newStyle1 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
  }
  .auto-style2 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    font-weight: bold;
  }
  .auto-style5 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    font-weight: bold;
    height: 47px;
    width: 215px;
  }
  .auto-style12 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    font-weight: bold;
    height: 34px;
    width: 215px;
  }
  .auto-style14 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    font-weight: bold;
    height: 36px;
    width: 215px;
  }
  .auto-style22 {
    height: 34px;
  }
  .auto-style23 {
    height: 36px;
  }
  .auto-style24 {}
  .auto-style25 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    font-weight: bold;
    height: 17px;
    width: 215px;
  }
  .auto-style26 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    font-weight: bold;
    width: 215px;
  }
  .auto-style27 {
    margin: 0 auto;
    height: auto;
    vertical-align: middle;
  }
  .auto-style29 {
    margin: 0 auto;
    height: 23px;
    vertical-align: middle;
  }
  .auto-style30 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    font-weight: bold;
    height: 23px;
    width: 215px;
  }
  .auto-style32 {
    margin: 0 auto;
    height: 47px;
    vertical-align: middle;
  }
  .auto-style33 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    font-weight: bold;
    height: 24px;
    width: 215px;
  }
  .auto-style34 {
    height: 24px;
  }
  .auto-style35 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    font-weight: bold;
    height: 27px;
    width: 215px;
  }
  .auto-style36 {
    height: 27px;
  }
  .auto-style40 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    font-weight: bold;
    width: 215px;
    height: 140px;
  }
  .auto-style42 {
    height: 140px;
  }
  .auto-style44 {
    height: 110px;
  }
  .auto-style45 {
    font-family: Arial, Helvetica, sans-serif;
    font-size: small;
    font-weight: bold;
    height: 110px;
    width: 215px;
  }
</style>
<asp:UpdatePanel ID="UpdatePanel1" runat="server"> 
  <ContentTemplate>
   <div id="TableWrapper">
    <table width="750px" runat="server" id="ApointmentTable">
    <tr>
      <td colspan="4"><h1>Welcome to Hair by Yeon Online Booking!</h1></td>
    </tr>
    <tr>
      <td class="auto-style5">
        <asp:Label ID="FirstNameLabel" runat="server" Text="First Name:"></asp:Label>
      </td>
      <td class="auto-style32">
        <asp:TextBox ID="TxtFirstName" runat="server" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 32) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122));" MaxLength="15" Width="159px"></asp:TextBox>
      </td>
      <td class="auto-style32">&nbsp;</td>
      <td class="auto-style32">
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtFirstName" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter customer first name">*</asp:RequiredFieldValidator>
      </td>
    </tr>
      <tr>
        <td class="auto-style25">
          <asp:Label ID="LastNameLabel" runat="server" Text="Last Name:"></asp:Label>
        </td>
        <td class="auto-style27">
          <asp:TextBox ID="TxtLastName" runat="server" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 32) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122));" MaxLength="20"></asp:TextBox>
        </td>
        <td class="auto-style27">&nbsp;</td>
        <td class="auto-style27">
          <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtLastName" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter customer last name">*</asp:RequiredFieldValidator>
        </td>
      </tr>
      <tr>
        <td class="auto-style30">
          <asp:Label ID="PhoneNumberLabel" runat="server" Text="Phone Number:"></asp:Label>
        </td>
        <td class="auto-style29">                                                                                                        <%--|| (event.keyCode = 8))  --%>
          <asp:TextBox ID="TxtPhoneCell" runat="server" onkeydown="return ((event.keyCode >= 8 && event.keyCode <= 9) || (event.keyCode >= 45 && event.keyCode <= 57));" MaxLength="10" ></asp:TextBox>
        </td>
        <td class="auto-style29">&nbsp;</td>
        <td class="auto-style29">
          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtPhoneCell" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter a phone number">*</asp:RequiredFieldValidator>
          <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtPhoneCell" CssClass="ErrorMessage" Display="Dynamic" ErrorMessage="Enter a valid phone number" ValidationExpression="\d+">*</asp:RegularExpressionValidator>
        </td>
      </tr>
      <tr>
        <td class="auto-style12">
          <asp:Label ID="Label3" runat="server" Text="Hair Length"></asp:Label>
        </td>
        <td class="auto-style22" colspan="3">
          <asp:DropDownList ID="DDLHairLength" runat="server">
            <asp:ListItem>Above Shoulder</asp:ListItem>
            <asp:ListItem>Below Shoulder</asp:ListItem>
          </asp:DropDownList>
        </td>
      </tr>
      <tr>
        <td class="auto-style45">
          <asp:Label ID="ServiceName" runat="server" Text="Service"></asp:Label>
          :</td>
        <td class="auto-style44" colspan="3">
          <asp:CheckBoxList ID="ChkBxListServices" runat="server">
          </asp:CheckBoxList>
        </td>
      </tr>
    <tr>
      <td class="auto-style14">
        <asp:Label ID="StylistLabel" runat="server" Text="Stylist"></asp:Label>
        :</td>
      <td class="auto-style23" colspan="3">
        <asp:DropDownList ID="DDLStylist" runat="server">
        </asp:DropDownList>
      </td>
    </tr>
    <tr>
      <td class="auto-style40">
        <asp:Label ID="DateLabel" runat="server" Text="Date Selected:"></asp:Label>
        :</td>
      <td colspan="2" class="auto-style42">
        <asp:Calendar ID="CalendarAptDate" runat="server" Width="293px" FirstDayOfWeek="Monday" Height="103px" OnDayRender="CalendarAptDate_DayRender" OnSelectionChanged="CalendarAptDate_SelectionChanged1">
          <SelectedDayStyle BackColor="#000080" BorderColor="#FFCCCC" />
        </asp:Calendar>
      </td>
      <td class="auto-style42">
        <asp:TextBox ID="TxtDateSelected" runat="server" ReadOnly="True" Width="128px"></asp:TextBox>
      </td>
    </tr>
      <tr>
        <td class="auto-style35">
          <asp:Label ID="Label1" runat="server" Text="Start Time:"></asp:Label>
        </td>
        <td class="auto-style36" colspan="2">
          <asp:DropDownList ID="DDLBeginTime" runat="server">
            <asp:ListItem>10:00</asp:ListItem>
            <asp:ListItem>10:30</asp:ListItem>
            <asp:ListItem>11:00</asp:ListItem>
            <asp:ListItem>11:30</asp:ListItem>
            <asp:ListItem>12:00</asp:ListItem>
            <asp:ListItem>12:30</asp:ListItem>
            <asp:ListItem>13:00</asp:ListItem>
            <asp:ListItem>13:30</asp:ListItem>
            <asp:ListItem>14:00</asp:ListItem>
            <asp:ListItem>14:30</asp:ListItem>
            <asp:ListItem>15:00</asp:ListItem>
          </asp:DropDownList>
        </td>
        <td class="auto-style36">&nbsp;</td>
      </tr>
      <tr>
        <td class="auto-style26">&nbsp;</td>
        <td class="auto-style24" colspan="3">
          <asp:Button ID="ApmtButton" runat="server" OnClick="ApmtButton_Click" Text="Book Appointment" />
        </td>
      </tr>
      <tr>
        <td class="auto-style2" colspan="4">
          <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ErrorMessage" HeaderText="Please correct the following errors" ShowMessageBox="True" ShowSummary="False" />
        </td>
      </tr>
    </table>
   </div>
    <p runat="server" id="MessageSentPara" visible="False">Thank you, We will get in touch with you if necessary.</p>
    <asp:Label ID="Message" runat="server" CssClass="Attention" Text="Appointment Confirmed" Visible="False"></asp:Label>
  </ContentTemplate>
</asp:UpdatePanel>
<%--<<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
  <ProgressTemplate>
    <div class="PleaseWait">
      Please Wait...
    </div>
  </ProgressTemplate>
/asp:UpdateProgress>--%>
<script>
  $(function () {
    $('form').bind('submit', function () {
      if (Page_IsValid) {
        $('TableWrapper').slideUp(3000);
      }
    });
  });
  function pageLoad()
  {
    $('.Attention').animate({width: '600px'}, 3000).animate({width: '200px'}, 3000).fadeOut('slow');
  }
</script>


