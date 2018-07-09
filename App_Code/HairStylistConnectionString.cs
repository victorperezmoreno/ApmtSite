using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for HairStylistConnectionString
/// Class for calling string connection for HairStylist DB from different webforms
/// </summary>
public static class HairStylistConnectionString 
{
	public static SqlConnection Connection()
	{
		//
    //  Get connection string for Hair Appointment database
		//    
      SqlConnection DBConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["HairAppointmentConnectionString"].ConnectionString);
      return DBConnection;
    
	}
}