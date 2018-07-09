<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="true" CodeFile="Default3.aspx.cs" Inherits="_Default" %>

<%@ Register Src="~/Controls/AppointmentForm.ascx" TagPrefix="uc1" TagName="AppointmentForm" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMC" Runat="Server">
  <uc1:AppointmentForm runat="server" ID="AppointmentForm" />
</asp:Content>

