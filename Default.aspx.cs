using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Utilities;
using Controller;
using Model;
using ProjectStructs;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      { 
        TxtAppointmentSummary.Text = ClassLibrary.RetrieveEasternTimeZoneFromUTCTime().ToShortDateString();
        // Populate schedule with today's appointments
        var appointmentsGrid = new AppointmentsSchedule();
        AssignFinalScheduleToDisplayToScheduleGridview(
                appointmentsGrid.PopulateGridWithAppointmentData(TxtAppointmentSummary.Text.Trim()));
       // PopulateGridWithAppointmentData(TxtAppointmentSummary.Text.Trim());
        /*Filling up the checkboxlist, services, names and times dropdownlist*/
        LoadServicesAndStylistForAppointmentBooking();
        TxtAppointmentDate.Text = ClassLibrary.RetrieveEasternTimeZoneFromUTCTime().ToShortDateString();
        DetermineWhetherDateForBookingAppointmentSelectedIsToday();
      }
    }

    private void AssignFinalScheduleToDisplayToScheduleGridview(DataTable dtTableFinalScheduleToDisplay)
    {
      grdViewSchedule.DataSource = dtTableFinalScheduleToDisplay;
      grdViewSchedule.DataBind();
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
            AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.WarningForecolor, AppConstants.LblMessage.WarningBackgroundColor, "There are no hair lenght options in Database");
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
            AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.WarningForecolor, AppConstants.LblMessage.WarningBackgroundColor, "There are no services in Database");
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
            AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.WarningForecolor, AppConstants.LblMessage.WarningBackgroundColor, "There are no stylist in Database");
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
            AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.WarningForecolor, AppConstants.LblMessage.WarningBackgroundColor, "There are no service start times in Database");
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
            AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.WarningForecolor, AppConstants.LblMessage.WarningBackgroundColor, "There are no Customer Names in Database");
          }

          command.Dispose();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          string str;
          str = "Source:" + ex.Source;
          str += "\n" + "Message:" + ex.Message;
          AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.ErrorForecolor, AppConstants.LblMessage.ErrorBackgroundColor, str);
        }
        finally
        {
          adapterDropDownListForBookingAppointments.Dispose();
        }
      }
    }

    protected void ApmtButton_Click(object sender, EventArgs e)
    {
      Page.Validate("BookingInfoGroup");
      if (Page.IsValid == true)
      {
        var appointment = CreateAppointmentObjectAndAssignData();
        //AppointmenttStructs.Appointment customerAppointment = CreateAHashOfNewAppointmentDataEntered();
        int operationResultFromSavingAppointment = appointment.SaveAppointment(appointment); 
   
        DisplayResultMessageFromInsertingAppointment(operationResultFromSavingAppointment);
        TxtAppointmentSummary.Text = appointment.DesiredDate;
        var appointmentsGrid = new AppointmentsSchedule();
        AssignFinalScheduleToDisplayToScheduleGridview(
                appointmentsGrid.PopulateGridWithAppointmentData(TxtAppointmentSummary.Text.Trim()));
      }
      DetermineWhetherDateForBookingAppointmentSelectedIsToday();
      UpdtPanelMessageCenter.Update();
      UpdtPanelScheduleGridView.Update();
      ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "VanishMessageCenterApmt", "vanishMessageCenter();", true);
    }

    //private AppointmentDetails CreateAppointmentObjectAndAssignData(AppointmenttStructs.Appointment structAppointmentData)
    //{
    //  var appointment = new AppointmentDetails();
    //  appointment.Id = structAppointmentData.id;
    //  appointment.DesiredDate = structAppointmentData.desiredDate;
    //  appointment.StartingTime = structAppointmentData.startingTime;
    //  appointment.EndingTime = "";
    //  appointment.Services = structAppointmentData.services;
    //  appointment.HairStylist = structAppointmentData.hairStylist;
    //  appointment.DetermineCustomerHairLenght(structAppointmentData.hairLenght);
    //  appointment.IdCustomer = structAppointmentData.idCustomer;
    //  appointment.RegistrationDate = structAppointmentData.registrationDate;
    //  appointment.RegisteredBy = structAppointmentData.registeredBy;
    //  appointment.Cancelled = structAppointmentData.cancelled;
    //  appointment.CancellationReason = "";

    //  return appointment;
    //}

    private AppointmentDetails CreateAppointmentObjectAndAssignData()
    {
      int CustomerId = 0;
      if (int.TryParse(CmbBxName.SelectedValue.ToString().Trim(), out CustomerId))
        CustomerId = Int16.Parse(CmbBxName.SelectedValue.ToString().Trim());

      int HairStylistId = 0;
      if (int.TryParse(DDLStylist.SelectedValue.ToString().Trim(), out HairStylistId))
        HairStylistId = Int16.Parse(DDLStylist.SelectedValue.ToString().Trim());

      var customerAppointment = new AppointmentDetails();
      customerAppointment.Id = 1;//arbitrary as i do not need the id for inserting new customers
      customerAppointment.DesiredDate = TxtAppointmentDate.Text.Trim();
      customerAppointment.StartingTime = DDLBeginTime.SelectedItem.Text.ToString().Trim();
      customerAppointment.EndingTime = ""; //Arbitrary as I do not need the EndingTime for inserting
      customerAppointment.Services = ClassLibrary.SelectedServicesAppendedString(ChkBxListServices);
      customerAppointment.HairStylist = (DDLStylist.SelectedIndex);
      customerAppointment.DetermineCustomerHairLenght(DDLHairLength.SelectedIndex);
      customerAppointment.IdCustomer = CustomerId;
      customerAppointment.RegistrationDate = ClassLibrary.RetrieveEasternTimeZoneFromUTCTime(); //For initialization purposes
      customerAppointment.RegisteredBy = 1; //Arbitrary until we decide how to allow access to app
      customerAppointment.Cancelled = false; //For initialization purposes
      customerAppointment.CancellationReason = ""; //For initialization purposes
      return customerAppointment;
    }

    private void DisplayResultMessageFromInsertingAppointment(int operationResult)
    {
      switch (operationResult)
      {
        //Error in Stored Procedure when inserting
        case 0:
          AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.SuccessForecolor, AppConstants.LblMessage.SuccessBackgroundColor, "Appointment Booked!");
          break;
        //Appointment Booked! 
        case 1:
          AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.ErrorForecolor, AppConstants.LblMessage.ErrorBackgroundColor, "Error in Stored Procedure when inserting");
          break;
        //Please select another time
        case 2:
          AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.WarningForecolor, AppConstants.LblMessage.WarningBackgroundColor, "Please select another time");
          break;
      }
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
        DateTime easternTime = ClassLibrary.RetrieveEasternTimeZoneFromUTCTime();
        if (dateSelected.Date == easternTime.Date)
        {
          DisableStartTimesBeforeTimeOfTheDayIfTodaysDateIsSelected();
        }
        else
        {
          DDLBeginTime.Enabled = true;
          DDLBeginTime.SelectedIndex = 0;
        }
      }
    }

    private void DisableStartTimesBeforeTimeOfTheDayIfTodaysDateIsSelected()
    {
      int index = 0;
      DateTime easternTime = ClassLibrary.RetrieveEasternTimeZoneFromUTCTime();
      TimeSpan timeOfDay = easternTime.TimeOfDay;
      TimeSpan latestTimeToBookAnAppointment = TimeSpan.Parse(AppConstants.TimeToBookAnAppointment.LatestTimeToBookAnAppointment);
      if (timeOfDay > latestTimeToBookAnAppointment)
      {
        DDLBeginTime.Enabled = false;
      }
      else
      {
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
    }

    protected void ServicesList_ServerValidation(object source, ServerValidateEventArgs args)
    {
      args.IsValid = ChkBxListServices.SelectedItem != null;
    }

    protected void BtnNewCustomer_Click(object sender, EventArgs e)
    {
      Page.Validate("RegistrationInfoGroup");
      if (Page.IsValid == true)
      {
        DataStructs.Client customer = CreateAHashOfNewCustomerDataEntered();
        var Client =   ClientHair.CreateCustomerObjectAndAssignData(customer);
        //int operationResultFromInsertingCustomer = Falta por implementar
        RegisterCustomer(Client);
        PopulateCustomerNamesComboBox(LoadCustomerNamesIntoDataTable());
        UpdtPanelMessageCenter.Update();
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "VanishMessageCenterNewCust", "vanishMessageCenter();", true);
      }
    }

    private DataStructs.Client CreateAHashOfNewCustomerDataEntered()
    {
      DataStructs.Client customer;
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
          AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.ErrorForecolor, AppConstants.LblMessage.ErrorBackgroundColor, str);
        }
      }
    }

    public void DisplayInsertResultToUser(string messageFromInserting, int insertResult)
    {
      switch (insertResult)
      {
        //Success Inserting
        case 0:
          AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.SuccessForecolor, AppConstants.LblMessage.SuccessBackgroundColor, messageFromInserting);
          break;
        //Error inserting in Stored Procedure
        case 1:
          AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.ErrorForecolor, AppConstants.LblMessage.ErrorBackgroundColor, messageFromInserting);
          break;
        //Customer already exists in Database or Timeslot already occupied
        case 2:
          AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.WarningForecolor, AppConstants.LblMessage.WarningBackgroundColor, messageFromInserting);
          break;
      }
    }
 
     private void AssignMessageToLabelWithResultFromOperation(string labelForeColor, string labelBackgroundColor, string messageToUser)
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
            AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.ErrorForecolor, AppConstants.LblMessage.ErrorBackgroundColor, "There are no Customer names in Database");
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
          AssignMessageToLabelWithResultFromOperation(AppConstants.LblMessage.ErrorForecolor, AppConstants.LblMessage.ErrorBackgroundColor, str);
        }
      }
      return dtTableComboBoxCustomerNames;
    }
   
   protected void OutBusinessHours_ServerValidation(object source, ServerValidateEventArgs args)
   {
     DateTime dateSelected = Convert.ToDateTime(TxtAppointmentDate.Text.ToString().Trim());
     DateTime easternTime = ClassLibrary.RetrieveEasternTimeZoneFromUTCTime();
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

    protected void TxtSummaryDateChange(object sender, EventArgs e)
    {
      // Update schedule according to date selected by user
      var appointmentsGrid = new AppointmentsSchedule();
      AssignFinalScheduleToDisplayToScheduleGridview(
                appointmentsGrid.PopulateGridWithAppointmentData(TxtAppointmentSummary.Text.Trim())); 
    }

    private void Page_Error(object sender, EventArgs e)
    {
      // Get last error from the server.
      Exception exc = Server.GetLastError();

      // Handle specific exception.
      if (exc is InvalidOperationException)
      {
        // Pass the error on to the error page.
        Server.Transfer("~/Errors/ErrorPage.aspx?handler=Page_Error%20-%20Default.aspx",
            true);
      }
    }
}