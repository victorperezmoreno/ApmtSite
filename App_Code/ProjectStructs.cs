using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProjectStructs
/// </summary>
public class ProjectStructs
{
  public struct Client
  {
    public int id;
    public string firstName;
    public string lastName;
    public string phoneCell;
    public string email;
  }

  public struct TimeSlotsStartingAndEndingTimes
  {
    public TimeSpan serviceStartTime;
    public TimeSpan serviceEndTime;
  }

	public ProjectStructs()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}