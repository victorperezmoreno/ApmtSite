using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {  
        TxtAppointmentSummary.Text = DateTime.Today.ToShortDateString();
        PopulateGridWithAppointmentData(TxtAppointmentSummary.Text.Trim());
        /*Filling up the checkboxlist, services, names and times dropdownlist*/
        LoadServicesAndStylistForAppointmentBooking();
        TxtAppointmentDate.Text = DateTime.Today.ToShortDateString();
        DetermineWhetherDateForBookingAppointmentSelectedIsToday();
      }
    }

    private void PopulateGridWithAppointmentData(string dateSelected)
    {
      var dtSetAppointmentInformation = LoadAppointmentInformation(dateSelected);
      if (dtSetAppointmentInformation.Tables.Count == 0)
      {
        AssignMessageToLabelWithResultFromDataBaseOperation(AppConstants.LblMessage.Forecolor, AppConstants.LblMessage.BackgroundColor, AppConstants.LblMessage.AppointmentsDatasetEmpty);
      }
      else
        {
          if (dtSetAppointmentInformation.Tables.Count > 2)
          {
            CreateTableScheduleWhenAppointmentsBooked(dtSetAppointmentInformation);
          }
          else
          {
            CreateEmptyTableSchedule(dtSetAppointmentInformation);
          }  
        }
    }

    private DataSet LoadAppointmentInformation(string dateSelected)
    {
      var dtSetAppointmentsPerSpecificDate = new DataSet();
      using (SqlConnection connection = HairStylistConnectionString.Connection())
      {
        try
        {
          //StoredPro_RetrieveAppointmentDetails       
          var command = new SqlCommand("StoredPro_RetrieveAppointmentDetails", connection);
          command.CommandType = CommandType.StoredProcedure;

          command.Parameters.Add("@AppointmentDate", SqlDbType.NVarChar, 10).Value = dateSelected;
          //Create adapter to pull appointment data from DB
          var adapterAppointments = new SqlDataAdapter();
          adapterAppointments.SelectCommand = command;

          adapterAppointments.Fill(dtSetAppointmentsPerSpecificDate);
          adapterAppointments.Dispose();
          command.Dispose();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          string str;
          str = "Source:" + ex.Source;
          str += "\n" + "Message:" + ex.Message;
          AssignMessageToLabelWithResultFromDataBaseOperation(AppConstants.LblMessage.Forecolor, AppConstants.LblMessage.BackgroundColor, str);
          //LblMessageToUser.Text = "Error reading from Database. Please try again.";
          //LblMessageToUser.Visible = true;
        }
        finally
        {
          //connection.Close();
        }
      }
      return dtSetAppointmentsPerSpecificDate;
    }

    private void CreateEmptyTableSchedule(DataSet dtSetWithNoAppointments)
    {
      //Create columns to display to user
      var dtTableFinalScheduleToDisplay = AddColumnsToTableScheduleToDisplay(dtSetWithNoAppointments);
      //Create the rows with time frames starting at 10 AM and ending at 7 PM
      AddRowsToEmptyTableScheduleToDisplay(dtSetWithNoAppointments, dtTableFinalScheduleToDisplay);
      //Bind data loaded in table to appointments grid
      grdViewSchedule.DataSource = dtTableFinalScheduleToDisplay;
      grdViewSchedule.DataBind();
    }

    private static void AddRowsToEmptyTableScheduleToDisplay(DataSet dtSetAppointmentsData, DataTable dtTableFinalScheduleToDisplay)
    {
      var dtTableStylistList = dtSetAppointmentsData.Tables[0];
      var dtTableWorkingHours = dtSetAppointmentsData.Tables[1];
      DataRow drTimeSlotData;
      for (int timeSlot = 0; timeSlot < dtTableWorkingHours.Rows.Count; timeSlot++)
      {
        drTimeSlotData = dtTableFinalScheduleToDisplay.NewRow();
        drTimeSlotData["No"] = dtTableWorkingHours.Rows[timeSlot]["Id"];
        drTimeSlotData["Time"] = dtTableWorkingHours.Rows[timeSlot]["StartTime"] + "-" + dtTableWorkingHours.Rows[timeSlot]["EndTime"];
        //Add the row to datatable
        dtTableFinalScheduleToDisplay.Rows.Add(drTimeSlotData);
      }
    }

    //Create final datable to display to user
    private void CreateTableScheduleWhenAppointmentsBooked(DataSet dtSetAppointmentsData)
    {
      //Create columns to display to user
      var dtTableFinalScheduleToDisplay = AddColumnsToTableScheduleToDisplay(dtSetAppointmentsData);
      //Create the rows with time frames starting at 10 AM and ending at 7 PM
      AddRowsToTableScheduleWithDataToDisplay(dtSetAppointmentsData, dtTableFinalScheduleToDisplay);
      //Bind data loaded in table to appointments grid
      grdViewSchedule.DataSource = dtTableFinalScheduleToDisplay;
      grdViewSchedule.DataBind();

    }
  
    private static void AddRowsToTableScheduleWithDataToDisplay(DataSet dtSetAppointmentsData, DataTable dtTableFinalScheduleToDisplay)
    {
      var dtTableStylistList = dtSetAppointmentsData.Tables[0];
      var dtTableWorkingHours = dtSetAppointmentsData.Tables[1];
      var dtTableAppointmentsForAllStylists = dtSetAppointmentsData.Tables[2];
      DataRow drTimeSlotData;
      //int totalSlotsToCreateForSameCustomer = 0;
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
        //Add the row to datatable
        dtTableFinalScheduleToDisplay.Rows.Add(drTimeSlotData);
      }
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

    //Load Services and Stylists from DB into group checkbox and dropdownbox
    private void LoadServicesAndStylistForAppointmentBooking()
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
            LblMessageToUser.Text = "There are no hair lenght options in Database";
            LblMessageToUser.Visible = true;
            //ClassLibrary.Popup.Message("There are no hair lenght options in Database");
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
            LblMessageToUser.Text = "There are no services in Database";
            LblMessageToUser.Visible = true;
            //ClassLibrary.Popup.Message("There are no services in Database");
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
            LblMessageToUser.Text = "There are no stylist in Database";
            LblMessageToUser.Visible = true;
            //ClassLibrary.Popup.Message("There are no stylist in Database");
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
            LblMessageToUser.Text = "There are no service start times in Database";
            LblMessageToUser.Visible = true;
            //ClassLibrary.Popup.Message("There are no service start times in Database");
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
            LblMessageToUser.Text = "There are no Customer Names in Database";
            LblMessageToUser.Visible = true;
            //ClassLibrary.Popup.Message("There are no Customer names in Database");
          }

          command.Dispose();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          string str;
          str = "Source:" + ex.Source;
          str += "\n" + "Message:" + ex.Message;
          AssignMessageToLabelWithResultFromDataBaseOperation(AppConstants.LblMessage.Forecolor, AppConstants.LblMessage.BackgroundColor, str);
          //LblMessageToUser.Text = "Error reading from the database.";
          LblMessageToUser.Visible = true;
        }
        finally
        {
          adapterDropDownListForBookingAppointments.Dispose();
          //connection.Close();
        }
      }
    }

    protected void ApmtButton_Click(object sender, EventArgs e)
    {
      Page.Validate("BookingInfoGroup");
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
        Appointment.StartingTime = DDLBeginTime.SelectedItem.Text.ToString().Trim();
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
        var dtTableServicesDurationAndProcessingTime = RetrieveServicesDurationAndProcessingTime(Appointment);
        //Create DataTable for services to be booked for a customer.
        var dtTableServicesToBeBooked = BuildServicesSchedule(Appointment, dtTableServicesDurationAndProcessingTime);
        var dtTableStylistSchedule = RetrieveStylistSchedule(Appointment);
        
        if (DetermineTimeSlotsAvailabilityForServicesToBeBooked(dtTableStylistSchedule, dtTableServicesToBeBooked))
        {
          LblMessageToUser.Text = "Please select another time";
        }
        else
        {
          string bookingDateSelected = BookServices(dtTableServicesToBeBooked);
          TxtAppointmentSummary.Text = bookingDateSelected;
          PopulateGridWithAppointmentData(bookingDateSelected);
        }
      }
    }

    private DataTable RetrieveServicesDurationAndProcessingTime(AppointmentDetails customerAppointment)
    {
      var dtTableServicesDetails = new DataTable();
      /*Placed connection string in a class in App_Code  to improve DB connection callings*/
      using (SqlConnection connection = HairStylistConnectionString.Connection())
      {
        try
        {
          var command = new SqlCommand("StoredPro_RetrieveServiceDurationAndProcessingTime", connection);
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.Add("@HairLenght", SqlDbType.TinyInt).Value = (int)customerAppointment.CustomerHairLength;
          
          var adapterServicesDetails = new SqlDataAdapter();
          adapterServicesDetails.SelectCommand = command;

          adapterServicesDetails.Fill(dtTableServicesDetails);
          dtTableServicesDetails.PrimaryKey = new DataColumn[] { dtTableServicesDetails.Columns["Id"] };
          command.Dispose();
          adapterServicesDetails.Dispose();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          string str;
          str = "Source:" + ex.Source;
          str += "\n" + "Message:" + ex.Message;
          AssignMessageToLabelWithResultFromDataBaseOperation(AppConstants.LblMessage.Forecolor, AppConstants.LblMessage.BackgroundColor, str);
          //LblMessageToUser.Text = "Error reading from the database.";
          LblMessageToUser.Visible = true;
        }
        finally
        {
          //connection.Close();
        }
      }
      return dtTableServicesDetails;
    }

    private static DataTable BuildServicesSchedule(AppointmentDetails appointmentData, DataTable dtTblServicesDuration)
    {
      //Datable with services to be performed and proposed schedule
      var dtTblServicesToBeBooked = CreateDataTableForServicesToBeBooked();
      ProjectStructs.TimeSlotsStartingAndEndingTimes currentTimeSlotService;
      currentTimeSlotService.serviceStartTime = TimeSpan.ParseExact(appointmentData.StartingTime, "g", null);
      currentTimeSlotService.serviceEndTime = TimeSpan.Zero;

      foreach (char c in appointmentData.Services)
      {
        byte serviceNumber = (byte)Char.GetNumericValue(c);
        DataRow drService = dtTblServicesDuration.Rows.Find(serviceNumber);
        int serviceDuration = ReturnServiceDurationAccordingToHairLenght(appointmentData, drService);
        
        TimeSpan minutesServiceDuration = new TimeSpan(0, serviceDuration, 0);
        int totalSlotsToCreateForSameCustomer =  (int)minutesServiceDuration.TotalMinutes / AppConstants.ScheduleTimeSlot.TimeSlotToDisplayInScheduleInMinutes;
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
            DateTime.Now, appointmentData.RegisteredBy, appointmentData.Cancelled,
            appointmentData.CancellationReason);
          currentTimeSlotService.serviceStartTime = currentTimeSlotService.serviceEndTime;
        }
        
        SetupStartingTimeForNextServiceToBeBooked(currentTimeSlotService, drService);
      }
      return dtTblServicesToBeBooked;
    }

    private static int ReturnServiceDurationAccordingToHairLenght(AppointmentDetails appointmentData, DataRow drService)
    {
      int serviceDuration = 0;

      if ((int)appointmentData.CustomerHairLength == 1)
        serviceDuration = Int32.Parse(drService["DurationAboveShoulder"].ToString());
      else
        serviceDuration = Int32.Parse(drService["DurationBelowShoulder"].ToString());

      return serviceDuration;
    }

    private static void SetupStartingTimeForNextServiceToBeBooked(ProjectStructs.TimeSlotsStartingAndEndingTimes nextTimeSlotService, DataRow drService)
    {
      int processingTime = 0;
      if (Int32.TryParse(drService["ProcessingTime"].ToString(), out processingTime))
        processingTime = Int32.Parse(drService["ProcessingTime"].ToString());

      nextTimeSlotService.serviceStartTime = nextTimeSlotService.serviceEndTime + new TimeSpan(0, processingTime, 0);
    } 

    private DataTable RetrieveStylistSchedule(AppointmentDetails customerAppointment)
    {
      var dtTableStylistAppointments = new DataTable();
      /*Placed connection string in a class in App_Code  to improve DB connection callings*/
      using (SqlConnection connection = HairStylistConnectionString.Connection())
      {
        try
        {
          var command = new SqlCommand("StoredPro_RetrieveStylistSchedule", connection);
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.Add("@AppointmentDate", SqlDbType.NVarChar, 10).Value = customerAppointment.DesiredDate;
          command.Parameters.Add("@StylistId", SqlDbType.TinyInt).Value = customerAppointment.HairStylist;
          
          var adapterStylistAppointments = new SqlDataAdapter();
          adapterStylistAppointments.SelectCommand = command;

          adapterStylistAppointments.Fill(dtTableStylistAppointments);
          command.Dispose();
          adapterStylistAppointments.Dispose();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          string str;
          str = "Source:" + ex.Source;
          str += "\n" + "Message:" + ex.Message;
          AssignMessageToLabelWithResultFromDataBaseOperation(AppConstants.LblMessage.Forecolor, AppConstants.LblMessage.BackgroundColor, str);
          //LblMessageToUser.Text = "Error reading from the database.";
          LblMessageToUser.Visible = true;
        }
        finally
        {
          //connection.Close();
        }
      }
      return dtTableStylistAppointments;
    }

    private bool DetermineTimeSlotsAvailabilityForServicesToBeBooked(DataTable scheduleDetails, DataTable dtTblServicesToBeBooked)
    {
      bool timeSlotOccupiedByOtherService = false;
      //Check whether the schedule for new services do not collide with services to perform during the day
      if ((scheduleDetails != null) && (scheduleDetails.Rows.Count > 0))
      {
        scheduleDetails.PrimaryKey = new DataColumn[] { scheduleDetails.Columns["Id"] };
        bool flag = true;
        while (flag == true)
        {
          foreach (DataRow currentServiceToBeBooked in dtTblServicesToBeBooked.Rows)
          {
            foreach (DataRow currentScheduledService in scheduleDetails.Rows)
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
      dtTblForServicesToBeBooked.Columns.Add("RegistrationDate", typeof(DateTime));
      dtTblForServicesToBeBooked.Columns.Add("RegisteredBy", typeof(int));
      dtTblForServicesToBeBooked.Columns.Add("Cancelled", typeof(bool));
      dtTblForServicesToBeBooked.Columns.Add("CancellationReason", typeof(string));
      return dtTblForServicesToBeBooked;
    }

    private string BookServices(DataTable dtTblServicesToBeBooked)
    {
      string appointmentDateSelected ="";
      using (SqlConnection connection = HairStylistConnectionString.Connection())
      {
        try
        {
          //Insert services data in Database
          var command = new SqlCommand("StoredPro_InsertAppointmentsBatch", connection);
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.Add("@AppointmentDetails", SqlDbType.Structured).Value = dtTblServicesToBeBooked;
          command.Parameters.Add("@OperationStatus", SqlDbType.NVarChar, 23).Direction = ParameterDirection.Output;


          appointmentDateSelected = dtTblServicesToBeBooked.Rows[0].Field<DateTime>("AppointmentDate").ToShortDateString();
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
          AssignMessageToLabelWithResultFromDataBaseOperation(AppConstants.LblMessage.Forecolor, AppConstants.LblMessage.BackgroundColor, str);        
          //LblMessageToUser.Text = "Error inserting into the database.";
          LblMessageToUser.Visible = true;
        }
        finally
        {
          //connection.Close();
        }
      }
      return appointmentDateSelected;
    }

    protected void TxtAppointmentDateChange(object sender, EventArgs e)
    {
      DetermineWhetherDateForBookingAppointmentSelectedIsToday();

    }

    private void DetermineWhetherDateForBookingAppointmentSelectedIsToday()
    {
      if (TxtAppointmentDate.Text.Trim() != "")
      {
        DateTime dateSelected = Convert.ToDateTime(TxtAppointmentDate.Text.ToString().Trim());
        if (dateSelected.Date == DateTime.Now.Date)
        {
          DisableStartTimesBeforeTimeOfTheDayIfTodaysDateIsSelected();
        }
        else
        {
          DDLBeginTime.SelectedIndex = 0;
        }
      }
    }

    private void DisableStartTimesBeforeTimeOfTheDayIfTodaysDateIsSelected()
    {
      int index = 0;
      DateTime easternTime = RetrieveEasternTimeZoneFromUTCTime();
      TimeSpan timeOfDay = easternTime.TimeOfDay;
      foreach (ListItem timeSlot in DDLBeginTime.Items)
      {
        TimeSpan serviceStartTime = TimeSpan.Parse(timeSlot.Text.ToString().Trim());
        if (serviceStartTime < timeOfDay)
        {
          timeSlot.Attributes.Add("disabled", "disabled");
        }
        else
        {
          DDLBeginTime.SelectedIndex = index;
          break;
        }
        index++;
      }
    }

    protected void ServicesList_ServerValidation(object source, ServerValidateEventArgs args)
    {
      args.IsValid = ChkBxListServices.SelectedItem != null;
    }

  //Insert new customer
    protected void BtnNewCustomer_Click(object sender, EventArgs e)
    {
      Page.Validate("RegistrationInfoGroup");
      if (Page.IsValid == true)
      {
        ProjectStructs.Client customer = CreateAHashOfNewCustomerDataEntered();
        var Client = ClientHair.CreateCustomerObjectAndAssignData(customer);
        RegisterCustomer(Client);
        PopulateCustomerNamesComboBox(LoadCustomerNamesIntoDataTable());
      }
    }

    private ProjectStructs.Client CreateAHashOfNewCustomerDataEntered()
    {
      ProjectStructs.Client customer;
      customer.id = 1;//arbitrary as i do not need the id for inserting new customers
      customer.firstName = TxtFirstName.Text.ToLower().Trim();
      customer.lastName = TxtLastName.Text.ToLower().Trim();
      customer.phoneCell = new string(TxtPhoneCell.Text.Trim().Where(char.IsDigit).ToArray());
      customer.email = TxtEmail.Text.Trim();
      return customer;
    }

    //Save customer data into DB
    public void RegisterCustomer(ClientHair clientToRegister)
    {
      using (SqlConnection connection = HairStylistConnectionString.Connection())
      {
        try
        {
          var command = ClientHair.InsertNewCustomerDataIntoDB(clientToRegister, connection);
          int insertResult = ClientHair.DetermineWhetherCustomerAlreadyExistInDB(command);
          string message = ClientHair.GetMessageResultFromInsertingCustomer(command);
          DisplayInsertResultToUser(message, insertResult);
          LblMessageToUser.Visible = true;
          command.Dispose();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          string str;
          str = "Source:" + ex.Source;
          str += "\n" + "Message:" + ex.Message;
          AssignMessageToLabelWithResultFromDataBaseOperation(AppConstants.LblMessage.Forecolor, AppConstants.LblMessage.BackgroundColor, str);
          //LblMessageToUser.Text = "Error inserting into the database.";
          LblMessageToUser.Visible = true;
        }
        finally
        {
          //connection.Close();
        }
      }
    }

    public void DisplayInsertResultToUser(string messageFromInserting, int insertResult)
    {
      switch (insertResult)
      {
        //Success Inserting
        case 0:
          AssignMessageToLabelWithResultFromDataBaseOperation("#4F8A10", "#DFF2BF", messageFromInserting);
          break;
        //Error inserting in Stored Procedure
        case 1:
          AssignMessageToLabelWithResultFromDataBaseOperation("#D8000C", "#FFD2D2", messageFromInserting);
          break;
        //Customer already exists in Database
        case 2:
          AssignMessageToLabelWithResultFromDataBaseOperation("#9F6000", "#FEEFB3", messageFromInserting);
          break;
      }
    }
 
     private void AssignMessageToLabelWithResultFromDataBaseOperation(string labelForeColor, string labelBackgroundColor, string messageToUser)
    {
      LblMessageToUser.ForeColor = System.Drawing.ColorTranslator.FromHtml(labelForeColor);
      LblMessageToUser.Style["background-color"] = labelBackgroundColor;
      LblMessageToUser.Text = messageToUser;
      LblMessageToUser.Visible = true;      
    }    

    private void PopulateCustomerNamesComboBox(DataTable dtTableCustomerNames)
    {
      if (dtTableCustomerNames.Rows.Count != 0)
          {
            AssignDataTableToCustomerNamesComboBox(dtTableCustomerNames);
          }
          else
          {
            LblMessageToUser.Text = "There are no Customer names in Database";
            LblMessageToUser.Visible = true;
            //ClassLibrary.Popup.Message("There are no Customer names in Database");
          }
    }
    
    private void AssignDataTableToCustomerNamesComboBox(DataTable dtTableCustomerNames)
    {
      CmbBxName.DataSource = dtTableCustomerNames;
      CmbBxName.DataTextField = "FullName";
      CmbBxName.DataValueField = "Id";
      CmbBxName.DataBind();
      CmbBxName.Items.Insert(0, new ListItem("Please Select"));
      CmbBxName.SelectedIndex = 0;
    }

    private DataTable LoadCustomerNamesIntoDataTable()
    {
      var dtTableComboBoxCustomerNames = new DataTable();
      using (SqlConnection connection = HairStylistConnectionString.Connection())
      {
        try
        {
          //Load Services and styliyst data from DB
          var command = new SqlCommand("StoredPro_PopulateCustomerNamesComboBox", connection);
          command.CommandType = CommandType.StoredProcedure;
          var adapterComboBoxCustomerNames = new SqlDataAdapter();
          adapterComboBoxCustomerNames.SelectCommand = command;
          adapterComboBoxCustomerNames.Fill(dtTableComboBoxCustomerNames);
          command.Dispose();
          adapterComboBoxCustomerNames.Dispose();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          string str;
          str = "Source:" + ex.Source;
          str += "\n" + "Message:" + ex.Message;
          AssignMessageToLabelWithResultFromDataBaseOperation(AppConstants.LblMessage.Forecolor, AppConstants.LblMessage.BackgroundColor, str);
          //LblMessageToUser.Text = "Error reading from the database.";
          LblMessageToUser.Visible = true;
        }
        finally
        { 
          //connection.Close();
        }
      }
      return dtTableComboBoxCustomerNames;
    }
   
   protected void OutBusinessHours_ServerValidation(object source, ServerValidateEventArgs args)
   {
     DateTime dateSelected = Convert.ToDateTime(TxtAppointmentDate.Text.ToString().Trim());
     DateTime easternTime = RetrieveEasternTimeZoneFromUTCTime();
     
     if (dateSelected.Date == easternTime.Date)
     {
       TimeSpan timeOfDay = easternTime.TimeOfDay;
       TimeSpan latestTimeToBookAnAppointment = TimeSpan.Parse(AppConstants.TimeToBookAnAppointment.LatestTimeToBookAnAppointment);
       if (timeOfDay > latestTimeToBookAnAppointment)
       {
         args.IsValid = false;
       }
       else
       {
         args.IsValid = true;
       }
     }
   }

   private static DateTime RetrieveEasternTimeZoneFromUTCTime()
   {
     DateTime utcTime = DateTime.UtcNow;
     TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
     DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, estTimeZone);
     return easternTime;
   }

    protected void TxtSummaryDateChange(object sender, EventArgs e)
    {
      PopulateGridWithAppointmentData(TxtAppointmentSummary.Text.Trim());
    }
}