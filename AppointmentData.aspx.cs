using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;


public partial class AppointmentData : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {

      CmbBxName.Focus();
      DDLBeginTime.SelectedIndex = 0;
      DDLStylist.SelectedIndex = 0;
      /*Filling up the checkboxlist, services, names and times dropdownlist*/
      LoadServicesAndStylist();
    }
  }

  //Load Services and Stylists from DB into group checkbox and dropdownbox
  private void LoadServicesAndStylist()
  {
    var PopulateDropDownListForBookingAppointments = new DataSet();
    var adapterDropDownListForBookingAppointments = new SqlDataAdapter();
    /*Placed connection string in a class in App_Code  to improve DB connection callings*/
    using (SqlConnection connection = HairStylistConnectionString.Connection())
    {
      try
      {
        //Load Services and styliyst data from DB
        var command = new SqlCommand("StoredPro_PopulateDropDownListForBookingAppointments", connection);
        command.CommandType = CommandType.StoredProcedure;
        adapterDropDownListForBookingAppointments.SelectCommand = command;
        adapterDropDownListForBookingAppointments.Fill(PopulateDropDownListForBookingAppointments);

        if (PopulateDropDownListForBookingAppointments.Tables[0].Rows.Count != 0)
        {
          DDLHairLength.DataSource = PopulateDropDownListForBookingAppointments.Tables[0];
          DDLHairLength.DataTextField = "HairLength";
          DDLHairLength.DataValueField = "Id";
          DDLHairLength.DataBind();
        }
        else
        {
          ClassLibrary.Popup.Message("There are no hair lenght options in Database");
        }

        if (PopulateDropDownListForBookingAppointments.Tables[1].Rows.Count != 0)
        {
          ChkBxListServices.DataSource = PopulateDropDownListForBookingAppointments.Tables[1];
          ChkBxListServices.DataTextField = "Service";
          ChkBxListServices.DataValueField = "Id";
          ChkBxListServices.DataBind();
        }
        else
        {
          ClassLibrary.Popup.Message("There are no services in Database");
        }

        if (PopulateDropDownListForBookingAppointments.Tables[2].Rows.Count != 0)
        {
          DDLStylist.DataSource = PopulateDropDownListForBookingAppointments.Tables[2];
          DDLStylist.DataTextField = "Stylist";
          DDLStylist.DataValueField = "Id";
          DDLStylist.DataBind();
        }
        else
        {
          ClassLibrary.Popup.Message("There are no stylist in Database");
        }

        if (PopulateDropDownListForBookingAppointments.Tables[3].Rows.Count != 0)
        {
          DDLBeginTime.DataSource = PopulateDropDownListForBookingAppointments.Tables[3];
          DDLBeginTime.DataTextField = "StartTime";
          DDLBeginTime.DataValueField = "Id";
          DDLBeginTime.DataBind();
        }
        else
        {
          ClassLibrary.Popup.Message("There are no service start times in Database");
        }

        if (PopulateDropDownListForBookingAppointments.Tables[4].Rows.Count != 0)
        {
          CmbBxName.DataSource = PopulateDropDownListForBookingAppointments.Tables[4];
          CmbBxName.DataTextField = "FullName";
          CmbBxName.DataValueField = "Id";
          CmbBxName.DataBind();
          CmbBxName.Items.Insert(0, new ListItem("Please Select"));
          CmbBxName.SelectedIndex = 0;
        }
        else
        {
          ClassLibrary.Popup.Message("There are no Customer names in Database");
        }

        command.Dispose();
      }
      catch (System.Data.SqlClient.SqlException ex)
      {
        string str;
        str = "Source:" + ex.Source;
        str += "\n" + "Message:" + ex.Message;
        LblMessageToUser.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D8000C");
        LblMessageToUser.Style["background-color"] = "#FFD2D2";
        LblMessageToUser.Text = "Error reading from the database.";
        LblMessageToUser.Visible = true;
      }
      finally
      {
        adapterDropDownListForBookingAppointments.Dispose();
        connection.Close();
      }
    }
  }

  /* Pull data from Database to generate the appointments based on services selected */
  private DataSet LoadServicesDetailsAndAppointments(AppointmentDetails customerAppointment)
  {
    var dtSetServicesDetailsAndAppointments = new DataSet();
    /*Placed connection string in a class in App_Code  to improve DB connection callings*/
    using (SqlConnection conn = HairStylistConnectionString.Connection())
    {
      try
      {
        //Retrieve Services and Styliyst data from DB
        var command = new SqlCommand("StoredPro_RetrieveStylistScheduleAndServiceDetails", conn);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@AppointmentDate", SqlDbType.NVarChar);
        command.Parameters.Add("@StylistId", SqlDbType.TinyInt);
        command.Parameters.Add("@HairLenght", SqlDbType.TinyInt); //Creo que no necesito esto
        command.Parameters.Add("@NoRows", SqlDbType.Int).Direction = ParameterDirection.Output;

        command.Parameters["@AppointmentDate"].Value = customerAppointment.DesiredDate;
        command.Parameters["@StylistId"].Value = customerAppointment.HairStylist;
        command.Parameters["@HairLenght"].Value = (int)customerAppointment.CustomerHairLength;

        var adapterServicesDetailsAndAppointments = new SqlDataAdapter();
        adapterServicesDetailsAndAppointments.SelectCommand = command;

        adapterServicesDetailsAndAppointments.Fill(dtSetServicesDetailsAndAppointments);
        command.Dispose();
        adapterServicesDetailsAndAppointments.Dispose();     
      }
      catch (System.Data.SqlClient.SqlException ex)
      {
        string str;
        str = "Source:" + ex.Source;
        str += "\n" + "Message:" + ex.Message;
        LblMessageToUser.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D8000C");
        LblMessageToUser.Style["background-color"] = "#FFD2D2";
        LblMessageToUser.Text = "Error reading from the database.";
        LblMessageToUser.Visible = true;
      }
      finally
      {
        conn.Close(); 
      }
    }
    return dtSetServicesDetailsAndAppointments;
  }
  
  private bool DetermineTimeSlotsAvailabilityForServicesToBeBooked(DataSet scheduleDetails, DataTable dtTblServicesToBeBooked)
  {
    bool timeSlotOccupiedByOtherService = false;
    //Check whether the schedule for new services do not collide with services to perform during the day
    if ((scheduleDetails.Tables[1] != null) && (scheduleDetails.Tables[1].Rows.Count > 0))
    {
      var dtTblScheduledServices = new DataTable();
      dtTblScheduledServices = scheduleDetails.Tables[1];
      dtTblScheduledServices.PrimaryKey = new DataColumn[] { dtTblScheduledServices.Columns["Id"] }; 
      bool flag = true;
      while (flag == true)
      {
        foreach (DataRow currentServiceToBeBooked in dtTblServicesToBeBooked.Rows)
        {
          foreach (DataRow currentScheduledService in dtTblScheduledServices.Rows)
          {
            if (currentServiceToBeBooked["StartTime"].ToString() == currentScheduledService["StartTime"].ToString())
            {
              timeSlotOccupiedByOtherService = true;
              //foreach (DataRow newTimeSlotService in dtTblScheduledServices.Rows)
              //{
                
              //  //Adicionar 30 mins to starting time? O crear una lista de timeslots disponibles
              //  //newTimeSlotService["StartTime"] = newTimeSlotService["StartTime"];
              //}
              break;
            }
          }
          if (timeSlotOccupiedByOtherService == true)
          {
            break;
          }
        }
        if (timeSlotOccupiedByOtherService == true)
        {
          flag = false;
        }
      }
    }
    return timeSlotOccupiedByOtherService;
  }

  private static DataTable BuildServicesSchedule(AppointmentDetails appointmentDetails, DataTable dtTblServicesDuration)
  {
    //Datable with services to be performed and proposed schedule
    var dtTblServicesToBeBooked = CreateDataTableForServicesToBeBooked();

    TimeSpan serviceStartTime = TimeSpan.ParseExact(appointmentDetails.StartingTime, "g", null);

    foreach (char c in appointmentDetails.Services)
    {
      byte serviceNumber = (byte)Char.GetNumericValue(c);
      DataRow drService = dtTblServicesDuration.Rows.Find(serviceNumber);

      int processingTime = 0;
      if (Int32.TryParse(drService["ProcessingTime"].ToString(), out processingTime))
        processingTime = Int32.Parse(drService["ProcessingTime"].ToString());

      int serviceDuration;
      if ((int)appointmentDetails.CustomerHairLength == 1)
        serviceDuration = Int32.Parse(drService["DurationAboveShoulder"].ToString());
      else
        serviceDuration = Int32.Parse(drService["DurationBelowShoulder"].ToString());
      //Calculate end time based on services duration time
      TimeSpan minutesServiceDuration = new TimeSpan(0, serviceDuration, 0);
      TimeSpan serviceEndTime = serviceStartTime + minutesServiceDuration;

      string startingTime = string.Format("{0:D2}:{1:D2}", serviceStartTime.Hours, serviceStartTime.Minutes);
      string endingTime = string.Format("{0:D2}:{1:D2}", serviceEndTime.Hours, serviceEndTime.Minutes);
      //Add services details row to datable
      dtTblServicesToBeBooked.Rows.Add(appointmentDetails.DesiredDate, startingTime,
        endingTime, serviceNumber, appointmentDetails.HairStylist,
        (int)appointmentDetails.CustomerHairLength, appointmentDetails.IdCustomer,
        DateTime.Now, appointmentDetails.RegisteredBy, appointmentDetails.Cancelled,
        appointmentDetails.CancellationReason);
      //Setting up starting time for next service\
      TimeSpan minutesProcessingInterval = new TimeSpan(0, processingTime, 0);
      serviceStartTime = serviceEndTime + minutesProcessingInterval;
    }
    return dtTblServicesToBeBooked;
  }

  private static DataTable CreateDataTableForServicesToBeBooked()
  {
    var dtTblForServicesToBeBooked = new DataTable();

    dtTblForServicesToBeBooked.Columns.Add("AppointmentDate", typeof(DateTime));
    dtTblForServicesToBeBooked.Columns.Add("StartTime", typeof(string));
    dtTblForServicesToBeBooked.Columns.Add("EndTime", typeof(string));
    dtTblForServicesToBeBooked.Columns.Add("IdService", typeof(int));
    dtTblForServicesToBeBooked.Columns.Add("IdStylist", typeof(int));
    dtTblForServicesToBeBooked.Columns.Add("IdHairLength", typeof(int));
    dtTblForServicesToBeBooked.Columns.Add("IdCustomer", typeof(int));
    //dtTblServicesToBeBooked.Columns.Add("LastName", typeof(string));
    //dtTblServicesToBeBooked.Columns.Add("Phone", typeof(string));
    dtTblForServicesToBeBooked.Columns.Add("RegistrationDate", typeof(DateTime));
    dtTblForServicesToBeBooked.Columns.Add("RegisteredBy", typeof(int));
    dtTblForServicesToBeBooked.Columns.Add("Cancelled", typeof(bool));
    dtTblForServicesToBeBooked.Columns.Add("CancellationReason", typeof(string));
    return dtTblForServicesToBeBooked;
  }

  protected void ApmtButton_Click(object sender, EventArgs e)
  {
     Page.Validate();
     if (Page.IsValid == true)
     {
       int CustomerId = 0;
       if (int.TryParse(CmbBxName.SelectedValue.ToString().Trim(), out CustomerId))
         CustomerId = Int16.Parse(CmbBxName.SelectedValue.ToString().Trim());

       int HairStylistId = 0;
       if (int.TryParse(DDLStylist.SelectedValue.ToString().Trim(), out HairStylistId))
         HairStylistId = Int16.Parse(DDLStylist.SelectedValue.ToString().Trim());

       var Appointment = new AppointmentDetails();
       Appointment.Id = 1; //Arbitrary as I do not need the Id for inserting new services
       Appointment.DesiredDate = TxtAppointmentDate.Text.Trim();
       Appointment.StartingTime = DDLBeginTime.SelectedValue.ToString().Trim();
       Appointment.EndingTime = ""; //Arbitrary as I do not need the EndingTime for inserting
       Appointment.DetermineServicesToBePerformed(ChkBxListServices);
       Appointment.HairStylist = HairStylistId;
       Appointment.DetermineCustomerHairLenght(DDLHairLength.SelectedIndex);
       Appointment.IdCustomer = CustomerId;
       Appointment.RegistrationDate = DateTime.Now; //For initialization purposes
       Appointment.RegisteredBy = 1; //Arbitrary until we decide how to allow access to app
       Appointment.Cancelled = false; //For initialization purposes
       Appointment.CancellationReason = ""; //For initialization purposes

       /* Get schedule for stylist on date picked by user, also select services timeframe*/
       var dtSetServicesDetailsAndAppointments = LoadServicesDetailsAndAppointments(Appointment);
       var dtTblServicesDuration = CreateDataTableForServicesDuration(dtSetServicesDetailsAndAppointments);
       //Create DataTable for services to be booked for a customer.
       var dtTblServicesToBeBooked = BuildServicesSchedule(Appointment, dtTblServicesDuration);

       if (DetermineTimeSlotsAvailabilityForServicesToBeBooked(dtSetServicesDetailsAndAppointments, dtTblServicesToBeBooked))
       {
         LblMessageToUser.Text = "Please select another time";
       }
       else
       {
         BookServices(dtTblServicesToBeBooked);
       }
     }
  }

  private void BookServices(DataTable dtTblServicesToBeBooked)
  {
    using (SqlConnection connection = HairStylistConnectionString.Connection())
    {
      try
      {
        //Insert services data in Database
        var command = new SqlCommand("StoredPro_InsertAppointmentsBatch", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@AppointmentDetails", SqlDbType.Structured).Value = dtTblServicesToBeBooked;
        command.Parameters.Add("@OperationStatus", SqlDbType.NVarChar, 23).Direction = ParameterDirection.Output;

        connection.Open();
        command.ExecuteNonQuery();
        LblMessageToUser.Text = command.Parameters["@OperationStatus"].Value.ToString().Trim();
        LblMessageToUser.Visible = true;
        command.Dispose();
      }
      catch (System.Data.SqlClient.SqlException ex)
      {
        string str;
        str = "Source:" + ex.Source;
        str += "\n" + "Message:" + ex.Message;
        LblMessageToUser.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D8000C");
        LblMessageToUser.Style["background-color"] = "#FFD2D2";
        LblMessageToUser.Text = "Error inserting into the database.";
        LblMessageToUser.Visible = true;
      }
      finally
      {
        connection.Close();
      }
    }
  }

  private static DataTable CreateDataTableForServicesDuration(DataSet dtSetServicesDetailsAndAppointments)
  {
    var dtTblServicesDuration = new DataTable();
    dtTblServicesDuration = dtSetServicesDetailsAndAppointments.Tables[0];
    dtTblServicesDuration.PrimaryKey = new DataColumn[] { dtTblServicesDuration.Columns["Id"] };
    return dtTblServicesDuration;
  }

  protected void CalendarAptDate_DayRender(object sender, DayRenderEventArgs e)
  {
    if (e.Day.Date < DateTime.Now.Date)
    {
      e.Day.IsSelectable = false;
      e.Cell.ForeColor = System.Drawing.Color.Gray;
    }
  }
 
  protected void TxtAppointmentDateChange(object sender, EventArgs e)
  {
    if (TxtAppointmentDate.Text.Trim() != "")
    {
      DateTime dateSelected = Convert.ToDateTime(TxtAppointmentDate.Text.ToString().Trim());
      if (dateSelected.Date == DateTime.Now.Date)
      {
        TimeSpan time = DateTime.Now.TimeOfDay;  
        foreach (ListItem timeSlot in DDLBeginTime.Items)
        {
          TimeSpan serviceStartTime = TimeSpan.Parse(timeSlot.Text.ToString().Trim());
          if (serviceStartTime < time)
          {
            timeSlot.Attributes.Add("disabled", "disabled");
          }
        }
                
      }
    }

  }
  
  protected void ServicesList_ServerValidation(object source, ServerValidateEventArgs args)
  {
    args.IsValid = ChkBxListServices.SelectedItem != null;
  }
}