using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class NewCustomer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      TxtFirstName.Focus();
      if (!IsPostBack)
      {
        
      }
    }


    protected void BtnNewCustomer_Click(object sender, EventArgs e)
    {

      var Client = new ClientHair();
      Client.IdCustomer = 1; //arbitrary as i do not need the id for inserting new services
      Client.FirstNameCustomer = Client.UppercaseFirstLetter(TxtFirstName.Text.ToLower().Trim());
      Client.LastNameCustomer = Client.UppercaseFirstLetter(TxtLastName.Text.ToLower().Trim());
      Client.PhoneNumberCustomer = new string(TxtPhoneCell.Text.Trim().Where(char.IsDigit).ToArray());
      //Client.PhoneNumberCustomer = TxtPhoneCell.Text.Trim(); 
      Client.EmailCustomer = TxtEmail.Text.Trim();

      RegisterCustomer(Client);  
    }
   //Save customer data into DB
    public void RegisterCustomer(ClientHair clientToRegister)
    {
      //Stored procedure to insert customer data
      using (SqlConnection connection = HairStylistConnectionString.Connection())
      {
        try
        {
          //Insert services data in Database
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


          int insertResult = 0;
          if (Int32.TryParse(command.Parameters["@CustomerAlreadyExist"].Value.ToString(), out insertResult))
            insertResult = Int16.Parse(command.Parameters["@CustomerAlreadyExist"].Value.ToString());
          switch (insertResult)
          {
              //Success Inserting
            case 0:
              LblMessageToUser.ForeColor = System.Drawing.ColorTranslator.FromHtml("#4F8A10");
              LblMessageToUser.Style["background-color"] = "#DFF2BF";
              LblMessageToUser.Text = command.Parameters["@OperationStatus"].Value.ToString().Trim();
              break;
              //Error inserting in Stored Procedure
            case 1:
              LblMessageToUser.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D8000C");
              LblMessageToUser.Style["background-color"] = "#FFD2D2";
              LblMessageToUser.Text = command.Parameters["@OperationStatus"].Value.ToString().Trim();
              break;
            //Customer already exists in Database
            case 2:
              LblMessageToUser.ForeColor = System.Drawing.ColorTranslator.FromHtml("#9F6000");
              LblMessageToUser.Style["background-color"] = "#FEEFB3";
              LblMessageToUser.Text = command.Parameters["@OperationStatus"].Value.ToString().Trim();
              break;
          }
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

}