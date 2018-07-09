using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for AppointmentDetails
/// </summary>
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

  //Get services selected in a string - calling a method in ClassLibrary
  public void DetermineServicesToBePerformed(CheckBoxList checkboxesList)
  {
    Services = ClassLibrary.SelectedServicesAppendedString(checkboxesList);
  }

}