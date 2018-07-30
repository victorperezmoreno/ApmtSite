using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AppConstants
/// </summary>
public class AppConstants
{
  public static class LblMessage
  {
    public const string Forecolor = "#D8000C";
    public const string BackgroundColor = "#FFD2D2";
    public const string AppointmentsDatasetEmpty = "No data in appointments tables";   
  }

  public static class ScheduleTimeSlot
  {
    public const int TimeSlotToDisplayInScheduleInMinutes = 30;
  }

  public static class TimeToBookAnAppointment
  {
    public const string LatestTimeToBookAnAppointment = "18:30";
  }

	public AppConstants()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}