using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Controller;

/// <summary>
/// Summary description for Model
/// </summary>
/// 
namespace Model
{
  public class AppointmentSchedule
  {
    public bool InsertAppointment(DataTable dtTblServicesToBeBooked)
    {
      bool operationStatus = false;
      using (SqlConnection connection = HairStylistConnectionString.Connection())
      {
        try
        {
          //Insert services
          var command = new SqlCommand("StoredPro_InsertAppointmentsBatch", connection);
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.Add("@AppointmentDetails", SqlDbType.Structured).Value = dtTblServicesToBeBooked;
          command.Parameters.Add("@OperationStatus", SqlDbType.Bit).Direction = ParameterDirection.Output;

          operationStatus = Convert.ToBoolean(command.Parameters["@OperationStatus"].Value);
          connection.Open();
          command.ExecuteNonQuery(); 
          command.Dispose();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
          throw new System.Exception("Error accesing the Database or inserting services into the Database - InsertAppointmentsBatch.", ex);          }
      }
      return operationStatus;
    }
  }

  public class StylistSchedule
  {
    public DataTable RetrieveStylistSchedule(AppointmentDetails customerAppointment)
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
          throw new System.Exception("Error accesing the Database or retriving stylist schedule from Stored Procedure.", ex);          
        }
      }
      return dtTableStylistAppointments;
    }
  }

  public class ServiceDurationAndProcessingTimes
  {
    public DataTable RetrieveServicesDurationAndProcessingTime(AppointmentDetails customerAppointment)
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
          throw new System.Exception("Error accesing the Database or retriving service duration and processing time details from Stored Procedure.", ex);          
        }
      }
      return dtTableServicesDetails;
    }
  }

  public class AppointmentInformation
  {
    public DataSet LoadAppointmentDetails(string dateSelected)
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
           throw new System.Exception("Error accesing the Database or retriving appointment details from Stored Procedure.", ex);
        }
      }
      return dtSetAppointmentsPerSpecificDate;
    }
  }



}
  
