<%@ Page Title="Hair Appointment" Language="C#" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="true" CodeFile="MasterPageDefault.aspx.cs" Inherits="_Default" %>

 <%-- <script type="text/javascript"> //language = "Javascript"
    function showModal() {
      var answer = window.showModalDialog("AppointmentData.aspx", argsVariable,
    "dialogWidth:300px; dialogHeight:200px; center:yes");
    }
  </script>--%>

  <%--<meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>Appointment Booking</title>
  <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
  <link rel="stylesheet" href="/resources/demos/style.css" />
  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>--%>
  

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
  $(document).ready(function () {
    $("#<%=TxtAppointmentDate.ClientID %>").datepicker();
  });
        </script>
</asp:Content>
<asp:Content ID="Content2" Runat="Server" ContentPlaceHolderID="cpMC">
 <%-- <asp:Calendar ID="CalendarAptDate" runat="server" FirstDayOfWeek="Monday" OnDayRender="CalendarAptDate_DayRender" OnSelectionChanged="CalendarAptDate_SelectionChanged">
    <SelectedDayStyle BackColor="#000080" BorderColor="#FFCCCC" />
  </asp:Calendar>--%>
  
  <asp:Button ID="Button1" runat="server" OnClientClick="showModal()" Text="New Apmt" />
  <br />
  <asp:TextBox ID="TxtAppointmentDate" CssClass="AppointmentDateSelector" ClientIdMode="Static" runat="server"/>
  <br />
  <asp:Label ID="Message" runat="server" Text="Label"></asp:Label>
  <br />
  <asp:GridView ID="grdViewSchedule" runat="server" CssClass="ScheduleGridview" BorderStyle="Solid" RowStyle-CssClass="ColumnAuto">
  </asp:GridView>
  
</asp:Content>

