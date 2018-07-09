using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Classes and methods commonly used in interaction with the user
/// </summary>
public class ClassLibrary
{
/// <summary>
/// Method to call stored procedure and return data from database
/// </summary>
/*  public T CallStoredProcedure<T>(connectionString, procName, params object[] parameters)
{
    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        var server_return = connection.Query<T> (procName, parameters, commandType:  CommandType.StoredProcedure);
        if (server_return != default(T))
            return server_return;
        }
        return default(T);
} */
  
  /// <summary> 
/// Generate a string with selected options in a checkboxlist 
/// </summary> 

  public static bool UserSelectedtAtLeastOneService(CheckBoxList checkboxesList)
  {
    foreach (ListItem item in checkboxesList.Items)
    {
      if (item.Selected == true)
      {
        return true;
      }
    }
    return false;
  }
  
  public static string SelectedServicesAppendedString(CheckBoxList checkboxesList)
  {
    int selectedServicesCount = checkboxesList.Items.Cast<ListItem>().Count(li => li.Selected);
    StringBuilder strSelectedServices = new StringBuilder();
    foreach (ListItem item in checkboxesList.Items)
    {
      if (item.Selected == true)
      {
        // Append selected value to string
        strSelectedServices.Append(item.Value.ToString());
      }
    }
    return strSelectedServices.ToString();
  }
  
/// <summary> 
/// A JavaScript alert 
/// </summary> 
public static class Popup
{

/// <summary> 
/// Displays a client-side JavaScript alert in the browser. 
/// </summary> 
/// <param name="message">The message to appear in the popup window.</param> 
public static void Message(string message)
{
   // Cleans the message to allow single quotation marks 
   string cleanMessage = message.Replace("'", "\\'");
   string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

   // Gets the executing web page 
   Page page = HttpContext.Current.CurrentHandler as Page;

   // Checks if the handler is a Page and that the script isn't allready on the Page 
   if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
   {
      page.ClientScript.RegisterClientScriptBlock(typeof(Popup), "alert", script);
   }
}
  
  
}
  
}
