using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProjectStructs
/// </summary>
/// 
namespace ProjectStructs
{
public class DataStructs
{
  public struct Client
  {
    public int id;
    public string firstName;
    public string lastName;
    public string phoneCell;
    public string email;
  }

  public struct Appointment
  {
    public int id;
    public string desiredDate;
    public string startingTime;
    public string endingTime;
    public string services;
    public int hairStylist;
    public int hairLenght;
    public int idCustomer;
    public DateTime registrationDate;
    public int registeredBy;
    public bool cancelled;
    public string cancellationReason;
  }

  public struct TimeSlotsStartingAndEndingTimes
  {
    public TimeSpan serviceStartTime;
    public TimeSpan serviceEndTime;
  }

	public DataStructs()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}
}