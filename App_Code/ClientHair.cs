using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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

  public string UppercaseFirstLetter(string stringToConvert)
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