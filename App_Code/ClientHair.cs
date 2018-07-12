using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
/// <summary>
/// Summary description for ClientHair
/// </summary>

  public class ClientHair
  {
    public int IdCustomer { get; set; }
    public string FirstNameCustomer { get; set; }
    public string LastNameCustomer { get; set; }
    public string FullNameCustomer { get; set; }
    public string PhoneNumberCustomer { get; set; }
    public string EmailCustomer { get; set; }

    public ClientHair()
    {
    }

    public static ClientHair CreateCustomerObjectAndAssignData(NewCustomerStruct.Client customer)
    {
      var Client = new ClientHair();
      Client.IdCustomer = customer.id; //arbitrary as i do not need the id for inserting new customers
      Client.FirstNameCustomer = UppercaseFirstLetter(customer.firstName);
      Client.LastNameCustomer = UppercaseFirstLetter(customer.lastName);
      Client.PhoneNumberCustomer = customer.phoneCell;
      Client.EmailCustomer = customer.email;
      return Client;
    }

    public static SqlCommand InsertNewCustomerDataIntoDB(ClientHair clientToRegister, SqlConnection connection)
    {
      var command = new SqlCommand("StoredPro_InsertCustomerData", connection);
      command.CommandType = CommandType.StoredProcedure;
      command.Parameters.Add("@FirstName", SqlDbType.NVarChar, 20).Value = clientToRegister.FirstNameCustomer;
      command.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = clientToRegister.LastNameCustomer;
      command.Parameters.Add("@Phone", SqlDbType.NVarChar, 15).Value = clientToRegister.PhoneNumberCustomer;
      command.Parameters.Add("@Email", SqlDbType.NVarChar, 240).Value = clientToRegister.EmailCustomer;

      command.Parameters.Add("@OperationStatus", SqlDbType.NVarChar, 23).Direction = ParameterDirection.Output;
      command.Parameters.Add("@CustomerAlreadyExist", SqlDbType.TinyInt).Direction = ParameterDirection.Output;

      connection.Open();
      command.ExecuteNonQuery();
      return command;
    }

    public static int DetermineWhetherCustomerAlreadyExistInDB(SqlCommand command)
    {
      int insertResult = 0;
      if (Int32.TryParse(command.Parameters["@CustomerAlreadyExist"].Value.ToString(), out insertResult))
        insertResult = Int16.Parse(command.Parameters["@CustomerAlreadyExist"].Value.ToString());

      return insertResult;
    }

    public static string GetMessageResultFromInsertingCustomer(SqlCommand command)
    {
      string messageResult = "";
      if (command.Parameters["@OperationStatus"].Value.ToString() != null)
        messageResult = command.Parameters["@OperationStatus"].Value.ToString();

      return messageResult;
    }

    public static string UppercaseFirstLetter(string stringToConvert)
    {
      if (string.IsNullOrEmpty(stringToConvert))
      {
        return string.Empty;
      }
      char[] ConvertedUppercaseString = stringToConvert.ToCharArray();
      ConvertedUppercaseString[0] = char.ToUpper(ConvertedUppercaseString[0]);
      return new string(ConvertedUppercaseString);
    }
}