using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
//Copyrigth Victor Perez Moreno
public partial class _Default : BasePage
{
  
  //Create string list to save the stylist name to create columns. 8 chairs in salon
  string[] stylistList = new string[8];
  
  protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        TxtAppointmentDate.Text = DateTime.Today.ToShortDateString();
        PopulateGridWithwithAppointmentData(TxtAppointmentDate.Text.Trim());
      }
    }

  private void PopulateGridWithwithAppointmentData(string dateSelected)
  {
    if (VerifyThatAppointmentsExistForDateSelected(dateSelected) == true)
    {
      createTableSchedule(LoadAppointmentInformation(dateSelected));
    }
    else
    {
      ClassLibrary.Popup.Message("There are no appointments for date selected");
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
        command.Parameters.Add("@OperationStatus", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
        
        //command.Parameters.Add("@AppointmentDate", SqlDbType.NVarChar);
        //command.Parameters.Add("@NoRows", SqlDbType.Int).Direction = ParameterDirection.Output;

        //command.Parameters["@AppointmentDate"].Value = dateSelected;
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
        Message.Text = "Error reading from Database. Please try again.";
        Message.Visible = true;
      }
      finally
      {
        connection.Close();
      }
    }

    return dtSetAppointmentsPerSpecificDate;
  }

  private bool VerifyThatAppointmentsExistForDateSelected(string requestedDate)
  {
    bool availableAppointmentsForDateSelected = false;
    /*Placed connection string in a class in App_Code  to improve DB connection callings*/
    using (SqlConnection connection = HairStylistConnectionString.Connection())
       {
        try
         {
          //StoredPro_RetrieveAppointmentDetails       
          var command = new SqlCommand("StoredPro_RetrieveAppointmentsForDateSelected", connection);
          command.CommandType = CommandType.StoredProcedure;

          command.Parameters.Add("@AppointmentDate", SqlDbType.NVarChar, 10).Value = requestedDate;
          command.Parameters.Add("@OperationStatus", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
        

          //command.Parameters.Add("@AppointmentDate", SqlDbType.NVarChar);
          //command.Parameters.Add("@NoRows", SqlDbType.Int).Direction = ParameterDirection.Output;
          
          //command.Parameters["@AppointmentDate"].Value = requestedDate;
          
          command.ExecuteNonQuery();
          availableAppointmentsForDateSelected = Convert.ToBoolean(command.Parameters["@NoRows"].Value);
          
          command.Dispose();
         }
         catch (System.Data.SqlClient.SqlException ex)
          {
           string str;
           str = "Source:" + ex.Source;
           str += "\n" + "Message:" + ex.Message;
           Message.Text = "Error reading from Database. Please try again.";
           Message.Visible = true;
          }
         finally
          {
           connection.Close();
          }
         }
    return availableAppointmentsForDateSelected; 
        }
     
    //Create final datable to display to user
    private void createTableSchedule(DataSet dtSetAppointmentsData)
   {
    var dtTblStylistList = dtSetAppointmentsData.Tables[0];
    var dtTblWorkingHours = dtSetAppointmentsData.Tables[1];
    var dtTblAppointmentsForAllStylists = dtSetAppointmentsData.Tables[2];

    //Create columns to display to user
    var dtFinalScheduleToDisplay = new DataTable();
    dtFinalScheduleToDisplay.Columns.Add("No", typeof(int));
    dtFinalScheduleToDisplay.Columns.Add("Time", typeof(string));   
    foreach (DataRow currentStylistColumn in dtTblStylistList.Rows)
    {
      dtFinalScheduleToDisplay.Columns.Add(currentStylistColumn["Stylist"].ToString(), typeof(string));
    }

    //Create the rows with time frames starting at 10 AM and ending at 7 PM
    DataRow drTimeSlotData;
    for (int timeSlot = 0; timeSlot < dtTblWorkingHours.Rows.Count; timeSlot++)
    {
      drTimeSlotData = dtFinalScheduleToDisplay.NewRow();
      drTimeSlotData["No"] = dtTblWorkingHours.Rows[timeSlot]["Id"];
      drTimeSlotData["Time"] = dtTblWorkingHours.Rows[timeSlot]["StartTime"] + " - " + dtTblWorkingHours.Rows[timeSlot]["EndTime"];

      //Search for appointments for each stylist
      foreach (DataRow currentRowStylist in dtTblStylistList.Rows)
      {
          foreach (DataRow currentRowAppointments in dtTblAppointmentsForAllStylists.Rows)
            {
             if ((currentRowStylist["Stylist"].ToString() == currentRowAppointments["Stylist"].ToString()) && (dtTblWorkingHours.Rows[timeSlot]["StartTime"].ToString() == currentRowAppointments["StartTime"].ToString()))
              {
                //Add service and customer name to stylist column for each timeframe
                drTimeSlotData[currentRowStylist["Stylist"].ToString()] = currentRowAppointments["Service"].ToString() + " - " + currentRowAppointments["FirstName"].ToString() + " " + currentRowAppointments["LastName"].ToString().Trim();
                break;
              }
            }
        }
      
      //Add the row to datatable
      dtFinalScheduleToDisplay.Rows.Add(drTimeSlotData);
    }

    //Bind data loaded in table to appointments grid
    grdViewSchedule.DataSource = dtFinalScheduleToDisplay;
    grdViewSchedule.DataBind();
       
  }

  protected void CalendarAptDate_DayRender(object sender, DayRenderEventArgs e)
  {
    /* Comented por testing purposes, comments must be removed when site is ready
   if (e.Day.Date < DateTime.Now.Date)
    {
      e.Day.IsSelectable = false;
      e.Cell.ForeColor = System.Drawing.Color.Gray;
    } */
  }
  protected void CalendarAptDate_SelectionChanged(object sender, EventArgs e)
  {
    PopulateGridWithwithAppointmentData(TxtAppointmentDate.Text.Trim());
  }
}