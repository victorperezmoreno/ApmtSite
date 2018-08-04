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
    public const string SuccessForecolor = "#4F8A10";
    public const string SuccessBackgroundColor = "#DFF2BF";
    public const string ErrorForecolor = "#D8000C";
    public const string ErrorBackgroundColor = "#FFD2D2";
    public const string WarningForecolor = "#9F6000";
    public const string WarningBackgroundColor = "#FEEFB3";
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