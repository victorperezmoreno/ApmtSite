<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error Page</title>
</head>
<body>
  <header>
    <a href="/"></a>
    <div id="top">
      <div id="logo">
        <a href="../../Default">
          <img src="../../App_Themes/Darkbrown/Logo/SalonLogo.png" /></a>
      </div>

      <div id="social-media">
        <p>Connect with us </p>
        <ul>
          <li><a href="https://www.facebook.com/hairbyyeonfamilyhairsalon">
            <img src="../../App_Themes/Darkbrown/Social%20icons/Facebook.png" /></a> </li>
          <li><a href="https://www.instagram.com/hairbyyeon/">
            <img src="../../App_Themes/Darkbrown/Social%20icons/Instagram.png" />
          </a></li>
        </ul>
        <p>Contact us today for more info 703 968 6750</p>
      </div>
    </div>
  </header>
  <h2>Error:</h2>
  <p></p>
  <asp:Label ID="FriendlyErrorMsg" runat="server" Text="Label" Font-Size="Large" Style="color: red"></asp:Label>

  <asp:Panel ID="DetailedErrorPanel" runat="server" Visible="false">
    <p>&nbsp;</p>
    <h4>Detailed Error:</h4>
    <p>
      <asp:Label ID="ErrorDetailedMsg" runat="server" Font-Size="Small" /><br />
    </p>

    <h4>Error Handler:</h4>
    <p>
      <asp:Label ID="ErrorHandler" runat="server" Font-Size="Small" /><br />
    </p>

    <h4>Detailed Error Message:</h4>
    <p>
      <asp:Label ID="InnerMessage" runat="server" Font-Size="Small" /><br />
    </p>
    <p>
      <asp:Label ID="InnerTrace" runat="server" />
    </p>
  </asp:Panel>
</body>
</html>
