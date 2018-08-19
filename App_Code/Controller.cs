using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Model;
using Utilities;
using ProjectStructs;

/// <summary>
/// Summary description for Controller
/// </summary>
namespace Controller
{
  public class AppointmentDetails
  {
    public int Id { get; set; }
    public string DesiredDate { get; set; }
    public string StartingTime { get; set; }
    public string EndingTime { get; set; }
    public string Services { get; set; }
    public int HairStylist { get; set; }
    public HairLenght CustomerHairLength { get; set; }
    public int IdCustomer { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int RegisteredBy { get; set; }
    public bool Cancelled { get; set; }
    public string CancellationReason { get; set; }

    public enum HairLenght //Types of hair handled by stylists
    {
      AboveShoulder = 1,
      BelowShoulder = 2
    }

    public AppointmentDetails() //Constructor
    {
      CustomerHairLength = new HairLenght(); //Initialize enum HairLenght
    }

    public void DetermineCustomerHairLenght(int customerHairLenght)
    {
      if (customerHairLenght == 0)
        CustomerHairLength = HairLenght.AboveShoulder;
      else
        CustomerHairLength = HairLenght.BelowShoulder;
    }

    public int SaveAppointment(AppointmentDetails appointmentInfo)
    {

      int resultFromInsertingAppointment = 0;
      /* Get schedule for stylist on date picked by user, also select services timeframe*/
      var serviceDurationAndProcessingTime = new ServiceDurationAndProcessingTimes();
      var dtTableServicesDurationAndProcessingTime = serviceDurationAndProcessingTime.RetrieveServicesDurationAndProcessingTime(appointmentInfo);
      //Create DataTable for services to be booked for a customer.
      var dtTableServicesToBeBooked = BuildServicesSchedule(appointmentInfo, dtTableServicesDurationAndProcessingTime);
      var stylistSchedule = new StylistSchedule();
      var dtTableStylistSchedule = stylistSchedule.RetrieveStylistSchedule(appointmentInfo);
      if (!DetermineTimeSlotsAvailabilityForServicesToBeBooked(dtTableStylistSchedule, dtTableServicesToBeBooked))
      {
        var appointmentToBeScheduled = new AppointmentSchedule();
        if (!appointmentToBeScheduled.InsertAppointment(dtTableServicesToBeBooked))
        {
          resultFromInsertingAppointment = 0;  //Error inserting in Stored Procedure
        }
        else
        {
          resultFromInsertingAppointment = 1; //Appointment Booked! 
        }
      }
      else
      {
        resultFromInsertingAppointment = 2; //Please select another time
      }
      return resultFromInsertingAppointment;
    }

    private DataTable BuildServicesSchedule(AppointmentDetails appointmentData, DataTable dtTblServicesDuration)
    {
      //Datable with services to be performed and proposed schedule
      var dtTblServicesToBeBooked = CreateDataTableForServicesToBeBooked();
      //ProjectStructs.TimeSlotsStartingAndEndingTimes currentTimeSlotService;
      DataStructs.TimeSlotsStartingAndEndingTimes currentTimeSlotService;
      currentTimeSlotService.serviceStartTime = TimeSpan.ParseExact(appointmentData.StartingTime, "g", null);
      currentTimeSlotService.serviceEndTime = TimeSpan.Zero;

      foreach (char c in appointmentData.Services)
      {
        byte serviceNumber = (byte)Char.GetNumericValue(c);
        DataRow drService = dtTblServicesDuration.Rows.Find(serviceNumber);
        int serviceDuration = ReturnServiceDurationAccordingToHairLenght(appointmentData, drService);

        TimeSpan minutesServiceDuration = new TimeSpan(0, serviceDuration, 0);
        int totalSlotsToCreateForSameCustomer = (int)minutesServiceDuration.TotalMinutes / AppConstants.ScheduleTimeSlot.TimeSlotToDisplayInScheduleInMinutes;
        for (int timeSlotCounter = 1; timeSlotCounter <= totalSlotsToCreateForSameCustomer; timeSlotCounter++)
        {
          //Calculate end time based on services duration time
          currentTimeSlotService.serviceEndTime = currentTimeSlotService.serviceStartTime + new TimeSpan(0, AppConstants.ScheduleTimeSlot.TimeSlotToDisplayInScheduleInMinutes, 0);
          appointmentData.StartingTime = string.Format("{0:D2}:{1:D2}", currentTimeSlotService.serviceStartTime.Hours, currentTimeSlotService.serviceStartTime.Minutes);
          appointmentData.EndingTime = string.Format("{0:D2}:{1:D2}", currentTimeSlotService.serviceEndTime.Hours, currentTimeSlotService.serviceEndTime.Minutes);
          //Add services details row to datable
          dtTblServicesToBeBooked.Rows.Add(appointmentData.DesiredDate, appointmentData.StartingTime,
            appointmentData.EndingTime, serviceNumber, appointmentData.HairStylist,
            (int)appointmentData.CustomerHairLength, appointmentData.IdCustomer,
            ClassLibrary.RetrieveEasternTimeZoneFromUTCTime(), appointmentData.RegisteredBy, appointmentData.Cancelled,
            appointmentData.CancellationReason);
          currentTimeSlotService.serviceStartTime = currentTimeSlotService.serviceEndTime;
        }

        SetupStartingTimeForNextServiceToBeBooked(currentTimeSlotService, drService);
      }
      return dtTblServicesToBeBooked;
    }

    private DataTable CreateDataTableForServicesToBeBooked()
    {
      var dtTblForServicesToBeBooked = new DataTable();

      dtTblForServicesToBeBooked.Columns.Add("AppointmentDate", typeof(DateTime));
      dtTblForServicesToBeBooked.Columns.Add("StartTime", typeof(string));
      dtTblForServicesToBeBooked.Columns.Add("EndTime", typeof(string));
      dtTblForServicesToBeBooked.Columns.Add("IdService", typeof(int));
      dtTblForServicesToBeBooked.Columns.Add("IdStylist", typeof(int));
      dtTblForServicesToBeBooked.Columns.Add("IdHairLength", typeof(int));
      dtTblForServicesToBeBooked.Columns.Add("IdCustomer", typeof(int));
      dtTblForServicesToBeBooked.Columns.Add("RegistrationDate", typeof(DateTime));
      dtTblForServicesToBeBooked.Columns.Add("RegisteredBy", typeof(int));
      dtTblForServicesToBeBooked.Columns.Add("Cancelled", typeof(bool));
      dtTblForServicesToBeBooked.Columns.Add("CancellationReason", typeof(string));
      return dtTblForServicesToBeBooked;
    }

    private int ReturnServiceDurationAccordingToHairLenght(AppointmentDetails appointmentData, DataRow drService)
    {
      int serviceDuration = 0;

      if ((int)appointmentData.CustomerHairLength == 1)
        serviceDuration = Int32.Parse(drService["DurationAboveShoulder"].ToString());
      else
        serviceDuration = Int32.Parse(drService["DurationBelowShoulder"].ToString());

      return serviceDuration;
    }

    private void SetupStartingTimeForNextServiceToBeBooked(DataStructs.TimeSlotsStartingAndEndingTimes nextTimeSlotService, DataRow drService)
    {
      int processingTime = 0;
      if (Int32.TryParse(drService["ProcessingTime"].ToString(), out processingTime))
        processingTime = Int32.Parse(drService["ProcessingTime"].ToString());

      nextTimeSlotService.serviceStartTime = nextTimeSlotService.serviceEndTime + new TimeSpan(0, processingTime, 0);
    }

    private bool DetermineTimeSlotsAvailabilityForServicesToBeBooked(DataTable scheduleDetails, DataTable dtTblServicesToBeBooked)
    {
      bool timeSlotOccupiedByOtherService = false;
      //Check whether the schedule for new services do not collide with services to perform during the day
      if ((scheduleDetails != null) && (scheduleDetails.Rows.Count > 0))
      {
        scheduleDetails.PrimaryKey = new DataColumn[] { scheduleDetails.Columns["Id"] };
        foreach (DataRow currentServiceToBeBooked in dtTblServicesToBeBooked.Rows)
        {
          foreach (DataRow currentScheduledService in scheduleDetails.Rows)
          {
            if (currentServiceToBeBooked["StartTime"].ToString() == currentScheduledService["StartTime"].ToString())
            {
              timeSlotOccupiedByOtherService = true;

              break;
            }
          }
        }
      }
      return timeSlotOccupiedByOtherService;
    }
  }

  public class AppointmentsSchedule
  {
    public DataTable PopulateGridWithAppointmentData(string dateSelected)
    {
      var appointmentInformation = new AppointmentInformation();
      var dtSetAppointmentInformation = appointmentInformation.LoadAppointmentDetails(dateSelected);
      var dtTableSchedule = new DataTable();
      if (dtSetAppointmentInformation.Tables.Count != 0)
      {
        //var bookings = new Schedule();
        dtTableSchedule = CreateTableSchedule(dtSetAppointmentInformation);
      }
      return dtTableSchedule;
    }

    private DataTable CreateTableSchedule(DataSet dtSetAppointmentsData)
    {
      //Create columns to display to user
      var dtTableFinalScheduleToDisplay = AddColumnsToTableScheduleToDisplay(dtSetAppointmentsData);
      //Create the rows with time frames starting at 10 AM and ending at 7 PM
      AddRowsToTableScheduleToDisplay(dtSetAppointmentsData, dtTableFinalScheduleToDisplay);
      //Bind data loaded in table to appointments grid
      return dtTableFinalScheduleToDisplay;
    }

    private static DataTable AddColumnsToTableScheduleToDisplay(DataSet dtSetAppointmentsData)
    {
      var dtTableStylistList = dtSetAppointmentsData.Tables[0];
      var dtTableFinalScheduleToDisplay = new DataTable();
      dtTableFinalScheduleToDisplay.Columns.Add("No", typeof(int));
      dtTableFinalScheduleToDisplay.Columns.Add("Time", typeof(string));
      foreach (DataRow currentStylistColumn in dtTableStylistList.Rows)
      {
        dtTableFinalScheduleToDisplay.Columns.Add(currentStylistColumn["Stylist"].ToString(), typeof(string));
      }
      return dtTableFinalScheduleToDisplay;
    }

    private static void AddRowsToTableScheduleToDisplay(DataSet dtSetAppointmentsData, DataTable dtTableFinalScheduleToDisplay)
    {
      var dtTableStylistList = dtSetAppointmentsData.Tables[0];
      var dtTableWorkingHours = dtSetAppointmentsData.Tables[1];
      //Datarow Initialization
      DataRow drTimeSlotData;
      if (dtSetAppointmentsData.Tables.Count > 2)
      {
        var dtTableAppointmentsForAllStylists = dtSetAppointmentsData.Tables[2];
        for (int timeSlot = 0; timeSlot < dtTableWorkingHours.Rows.Count; timeSlot++)
        {
          drTimeSlotData = dtTableFinalScheduleToDisplay.NewRow();
          drTimeSlotData["No"] = dtTableWorkingHours.Rows[timeSlot]["Id"];
          drTimeSlotData["Time"] = dtTableWorkingHours.Rows[timeSlot]["StartTime"] + "-" + dtTableWorkingHours.Rows[timeSlot]["EndTime"];
          //Search appointments for each stylist
          foreach (DataRow currentRowStylist in dtTableStylistList.Rows)
          {
            foreach (DataRow currentRowAppointments in dtTableAppointmentsForAllStylists.Rows)
            {
              if ((currentRowStylist["Stylist"].ToString() == currentRowAppointments["Stylist"].ToString()) && (dtTableWorkingHours.Rows[timeSlot]["StartTime"].ToString() == currentRowAppointments["StartTime"].ToString()))
              {
                //Add service and customer name to stylist column for each timeframe
                drTimeSlotData[currentRowStylist["Stylist"].ToString()] = currentRowAppointments["Service"].ToString() + " - " + currentRowAppointments["FirstName"].ToString() + " " + currentRowAppointments["LastName"].ToString().Trim();
                break;
              }
            }
          }
          dtTableFinalScheduleToDisplay.Rows.Add(drTimeSlotData);
        }
      }
      else
      {
        for (int timeSlot = 0; timeSlot < dtTableWorkingHours.Rows.Count; timeSlot++)
        {
          drTimeSlotData = dtTableFinalScheduleToDisplay.NewRow();
          drTimeSlotData["No"] = dtTableWorkingHours.Rows[timeSlot]["Id"];
          drTimeSlotData["Time"] = dtTableWorkingHours.Rows[timeSlot]["StartTime"] + "-" + dtTableWorkingHours.Rows[timeSlot]["EndTime"];
          //Add the row to datatable
          dtTableFinalScheduleToDisplay.Rows.Add(drTimeSlotData);
        }
      }
    }

  }

  //public class Schedule
  //{
    ///<summary>
    ///Moving data from DataTable to Class Object
    ///<summary>
    //public static IList<AppointmentDetails> ConvertTo<object>(DataTable table)
    //{
    //  if (table == null)
    //    return null;

    //  List<DataRow> rows = new List<DataRow>();

    //  foreach (DataRow row in table.Rows)
    //    rows.Add(row);

    //  return ConvertTo<object>(rows);
    //}
  
  //}
}