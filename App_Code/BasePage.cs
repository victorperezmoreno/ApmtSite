using System;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
	private void Page_PreRender (Object sender, EventArgs e)
	{
    if (string.IsNullOrEmpty(this.Title) || this.Title.Equals("Untitled Page",
        StringComparison.CurrentCultureIgnoreCase))
    {
      throw new Exception("Page title cannot be \"Untitled Page\" or an empty string.");
    }
	}
  public BasePage()
  {
    this.PreRender += Page_PreRender;
  }
}