using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

using System.Web.Services;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
     
    }
    protected void ShowPanel_CheckedChanged(object sender, EventArgs e)
    {
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
      //string queryString = "default.aspx";
      //string newWin = "window.open('" + queryString + "');";
      //ClientScript.RegisterStartupScript(this.GetType(), "Popup", newWin, true);
    }

    //-- actual  webmethod that will fetch data from database based on what user typed  
[WebMethod]  
public static List<Employee> GetEmployeeData(string SearchParam)  
{  
        List<Employee> empList = new List<Employee>();  
    using (SqlConnection conn = HairStylistConnectionString.Connection())  
    {  
        //conn.ConnectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;  
        using (SqlCommand cmd = new SqlCommand())  
        {  
            cmd.CommandText = "select EmpName, EmpId from EmployeeTable where EmpName like @SearchParam + '%'";  
            cmd.Parameters.AddWithValue("@SearchParam", SearchParam);  
            cmd.Connection = conn;  
            conn.Open();  
            using (SqlDataReader sdr = cmd.ExecuteReader())  
            {  
                while (sdr.Read())  
                        empList.Add(new Employee() { EmpId = Convert.ToInt32(sdr["EmpName"]), EmpName = Convert.ToString(sdr["EmpId"]) });;  
            }  
            conn.Close();  
        }  
    }  
    return empList;  
}  
  
    //-- generate sample data for auto complete just to demonstrate  
    [WebMethod]  
    public static List<Employee> GetEmployeeDataSample(string SearchParam)  
    {  
        List<Employee> empList = new List<Employee>();  
  
        for (int i = 0; i < 10; i++)  
            empList.Add(new Employee() { EmpId = i + 1, EmpName = "EmpName " + (i + 1).ToString() });  
        return empList.Where(record => record.EmpName.ToLower().Contains(SearchParam)).ToList();  
    }  
  
    public class Employee  
    {  
        public int EmpId { get; set; }  
  
        public string EmpName { get; set; }  
    }  
}  
