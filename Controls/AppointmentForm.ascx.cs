using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;  
using System.Data.SqlClient;
using System.Text;


public partial class Controls_AppointmentForm : System.Web.UI.UserControl
{
   protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        DDLBeginTime.SelectedIndex = 0;
        //DDLEndTime.SelectedIndex = 0;
        //DDLService.SelectedIndex = 0;
        DDLStylist.SelectedIndex = 0;
        CalendarAptDate.SelectedDate = DateTime.Today;
        TxtDateSelected.Text = CalendarAptDate.SelectedDate.ToShortDateString();
        /*Filling up the group checkbox and services dropdownlist*/
        LoadServicesAndStylist();
      }
    }

    //Load Services and Stylists from DB into group checkbox and dropdownbox
    private void LoadServicesAndStylist()
    {
      var dtSetServicesAndStylist = new DataSet();
      var adapterServicesAndStylist = new SqlDataAdapter();
      /*Placed connection string in a class in App_Code  to improve DB connection callings*/
      using (SqlConnection conn = HairStylistConnectionString.Connection())
      {
        try
        {
          //Load Services and styliyst data from DB
          var command = new SqlCommand("StoredPro_StylistAndServicesListing", conn);
          command.CommandType = CommandType.StoredProcedure;
          adapterServicesAndStylist.SelectCommand = command;
          adapterServicesAndStylist.Fill(dtSetServicesAndStylist);

          if (dtSetServicesAndStylist.Tables[0].Rows.Count != 0)
          {
            ChkBxListServices.DataSource = dtSetServicesAndStylist.Tables[0];
            ChkBxListServices.DataTextField = "Service";
            ChkBxListServices.DataValueField = "Id";
            ChkBxListServices.DataBind();
          }
          else
          {
            ClassLibrary.Popup.Message("There are no services in Database");
          }

          if (dtSetServicesAndStylist.Tables[1].Rows.Count != 0)
            {
              DDLStylist.DataSource = dtSetServicesAndStylist.Tables[1];
              DDLStylist.DataTextField = "Stylist";
              DDLStylist.DataValueField = "Id";
              DDLStylist.DataBind();
            }
          else
          {
            ClassLibrary.Popup.Message("There are no stylist in Database");
          }
          command.Dispose();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          string str;
          str = "Source:" + ex.Source;
          str += "\n" + "Message:" + ex.Message;
          Message.Text = "An error ocurred while reading the data from the database. Please try again, if problem persist contact system admin.";
          Message.Visible = true;
        }
        finally
        {
          adapterServicesAndStylist.Dispose();
          conn.Close();
        }
      }
    }

  /* Pull data from Database to generate the appointments based on services selected */
    private void loadServicesDetailsAndAppointments(AppointmentDetails customerAppointment)
  {
      
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
          //command.Parameters.Add("@StartTime", SqlDbType.NVarChar);
          command.Parameters.Add("@NoRows", SqlDbType.Int).Direction = ParameterDirection.Output;

          command.Parameters["@AppointmentDate"].Value = customerAppointment.DesiredDate;
          command.Parameters["@StylistId"].Value = customerAppointment.HairStylist;
          command.Parameters["@HairLenght"].Value = (int)customerAppointment.CustomerHairLength;
         // command.Parameters["@StartTime"].Value = customerAppointment.StartingTime;

          var adapterServicesDetailsAndAppointments = new SqlDataAdapter();
          adapterServicesDetailsAndAppointments.SelectCommand = command;
          
          var dtSetServicesDetailsAndAppointments = new DataSet();
          adapterServicesDetailsAndAppointments.Fill(dtSetServicesDetailsAndAppointments);

          BookAppointment(dtSetServicesDetailsAndAppointments, customerAppointment, Convert.ToBoolean(command.Parameters["@NoRows"].Value));
          
          /*if (Convert.ToBoolean(command.Parameters["@NoRows"].Value) == false)
          {
            
          }
          else
          {

          } */

          command.Dispose();
          adapterServicesDetailsAndAppointments.Dispose();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          string str;
          str = "Source:" + ex.Source;
          str += "\n" + "Message:" + ex.Message;
          Message.Text = "An error ocurred while reading the data from the database. Please try again, if problem persist contact system admin.";
          Message.Visible = true;
        }
        finally
        {
          conn.Close();
        }
      }
    }
    
  /* Create a schedule for services selected and verify whether time slots are available for specified date */
    private void BookAppointment(DataSet scheduleDetails, AppointmentDetails appointmentDetails, bool existingAppointments)
    {   
      var dtTblServicesDuration = new DataTable();
      //As no appointments for stylist we only have one datable with services info (Tables[0])
      dtTblServicesDuration = scheduleDetails.Tables[0];
      dtTblServicesDuration.PrimaryKey = new DataColumn[] { dtTblServicesDuration.Columns["Id"] };
      
      //Datable with services to be performed and proposed schedule
      var dtTblServicesToBeBooked = new DataTable();
      
      dtTblServicesToBeBooked.Columns.Add("AppointmentDate", typeof(DateTime));
      dtTblServicesToBeBooked.Columns.Add("StartTime", typeof(string));
      dtTblServicesToBeBooked.Columns.Add("EndTime", typeof(string));
      dtTblServicesToBeBooked.Columns.Add("IdService", typeof(int));
      dtTblServicesToBeBooked.Columns.Add("IdStylist", typeof(int));
      dtTblServicesToBeBooked.Columns.Add("IdHairLength", typeof(int));
      dtTblServicesToBeBooked.Columns.Add("FirtsName", typeof(string));
      dtTblServicesToBeBooked.Columns.Add("LastName", typeof(string));
      dtTblServicesToBeBooked.Columns.Add("Phone", typeof(string));
      dtTblServicesToBeBooked.Columns.Add("RegistrationDate", typeof(DateTime));
      dtTblServicesToBeBooked.Columns.Add("RegisteredBy", typeof(int));
      dtTblServicesToBeBooked.Columns.Add("Cancelled", typeof(bool));
      dtTblServicesToBeBooked.Columns.Add("CancellationReason", typeof(string));
      

      //Setup starting time for service(s)
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
        string endingTime= string.Format("{0:D2}:{1:D2}", serviceEndTime.Hours, serviceEndTime.Minutes);
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
      
      //Check whether the schedule for new services do not collide with services to perform during the day
      if (existingAppointments == true)
      {
        var dtTblScheduledServices = new DataTable();
        dtTblScheduledServices = scheduleDetails.Tables[1];
        dtTblScheduledServices.PrimaryKey = new DataColumn[] { dtTblScheduledServices.Columns["Id"] };
        
        bool timeSlotOccupied = false;
        bool flag = true;
        while (flag== true)
        {
          foreach (DataRow currentServiceToBeBooked in dtTblServicesToBeBooked.Rows)
          {
            foreach (DataRow currentScheduledService in dtTblScheduledServices.Rows)
            {
              if (currentServiceToBeBooked["StartTime"].ToString() == currentScheduledService["StartTime"].ToString())
              {
                //
                timeSlotOccupied = true;
                foreach (DataRow newTimeSlotService in dtTblScheduledServices.Rows)
                {
                  //Adicionar 30 mins to starting time? O crear una lista de timeslots disponibles
                  newTimeSlotService["StartTime"] = newTimeSlotService["StartTime"];
                }
                break;
              }
            }
          }
        }
      }
      //Stored procedure to insert data from table created
      int insertResult = 0;
      using (SqlConnection connection = HairStylistConnectionString.Connection())
      {
        try
        {
          //Retrieve Services and Styliyst data from DB
      
          var command = new SqlCommand("StoredPro_InsertAppointmentsBatch", connection);
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.Add("@AppointmentDetails", SqlDbType.Structured).Value = dtTblServicesToBeBooked;
         
          //command.Parameters["@AppointmentDetails"].Value = tblServicesSchedule;
          connection.Open();     
          insertResult = command.ExecuteNonQuery();
          
          command.Dispose();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          string str;
          str = "Source:" + ex.Source;
          str += "\n" + "Message:" + ex.Message;
          Message.Text = "An error ocurred while inserting the data into the database. Please try again, if problem persist contact system admin.";
          Message.Visible = true;
        }
        finally
        {
          connection.Close();
        }
      }

      if (insertResult != 0)
      {
        Message.Visible = true; //display a popup with message from DB (Appointment Confirmed)
        MessageSentPara.Visible = true;
        ApointmentTable.Visible = true;
      }
       
    }

    protected void ApmtButton_Click(object sender, EventArgs e)
    {
      var customer = new AppointmentDetails();
      customer.Id = 1; //Arbitrary as I do not need the Id for inserting new services
      customer.DesiredDate = CalendarAptDate.SelectedDate.ToShortDateString().Trim();
      customer.StartingTime = DDLBeginTime.SelectedValue.ToString().Trim();
      customer.EndingTime = "";
      customer.DetermineServicesToBePerformed(ChkBxListServices);
      customer.HairStylist = DDLStylist.SelectedIndex + 1;
      customer.DetermineCustomerHairLenght(DDLHairLength.SelectedIndex);
      customer.IdCustomer = 1; //Still Arbitrary until I get a way to get Customer ID
      customer.RegistrationDate = DateTime.Now; //For initialization purposes
      customer.RegisteredBy = 1; //Arbitrary until we decide how to allow access to app
      customer.Cancelled = false; //For initialization purposes
      customer.CancellationReason = ""; //For initialization purposes
        
      /* Get schedule for stylist on date picked by user, also select services timeframe*/
      loadServicesDetailsAndAppointments(customer);
      //Necesito trabajar en este tema mas tarde 5/31/2018
      //customer.ClearAllControlsRecursive(this.Page.Controls);

    }
       
    protected void CalendarAptDate_DayRender(object sender, DayRenderEventArgs e)
    {
      if (e.Day.Date < DateTime.Now.Date)
      {
        e.Day.IsSelectable = false;
        e.Cell.ForeColor = System.Drawing.Color.Gray;
      }
    }

    protected void CalendarAptDate_SelectionChanged1(object sender, EventArgs e)
    {
      TxtDateSelected.Text = CalendarAptDate.SelectedDate.ToShortDateString();
    }
}