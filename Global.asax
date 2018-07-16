<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Optimization" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
      RouteConfig.RegisterRoutes(System.Web.Routing.RouteTable.Routes);
      BundleTable.Bundles.Add(new StyleBundle("~/StyleSheets").IncludeDirectory("~/App_Themes/Darkbrown", "*.css"));
      BundleTable.Bundles.Add(new ScriptBundle("~/jquery").IncludeDirectory("~/Scripts/jquery", "jquery-3.2.1.min.js"));
      BundleTable.Bundles.Add(new ScriptBundle("~/jqueryui").IncludeDirectory("~/Scripts/jqueryui", "jquery-ui1.12.1.min.js"));
      BundleTable.Bundles.Add(new ScriptBundle("~/jqueryinputmask").IncludeDirectory("~/Scripts/inputmask", "jquery.inputmask.bundle.min.js"));
      BundleTable.Bundles.Add(new ScriptBundle("~/jqueryinputmaskphonemin").IncludeDirectory("~/Scripts/inputmask/phone-codes", "phone.min.js"));
      BundleTable.Bundles.Add(new ScriptBundle("~/jqueryinputmaskphoneru").IncludeDirectory("~/Scripts/inputmask/phone-codes", "phone-ru.min.js"));
      BundleTable.Bundles.Add(new ScriptBundle("~/jqueryinputmaskphonebe").IncludeDirectory("~/Scripts/inputmask/phone-codes", "phone-be.min.js"));
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
