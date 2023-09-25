using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Demo.Pages.Employees
{
    public class IndexModel : PageModel
    {
        public List<EmployeeInfo> listEmployees = new List<EmployeeInfo>();
        public string SearchQuery { get; set; }

        public void OnGet(string searchQuery)
        {
            try
            {
                String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
                using (SqlConnection connection = new SqlConnection(@connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM nhanvien";

                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        sql += " WHERE name LIKE '%' + @searchQuery + '%' OR email LIKE '%' + @searchQuery + '%' OR phone LIKE '%' + @searchQuery + '%' OR job LIKE '%' + @searchQuery + '%'";
                    }

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            command.Parameters.AddWithValue("@searchQuery", searchQuery);
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeInfo employeeInfo = new EmployeeInfo();
                                employeeInfo.id = "" + reader.GetInt32(0);
                                employeeInfo.name = reader.GetString(1);
                                employeeInfo.age = "" + reader.GetInt32(2);
                                employeeInfo.email = reader.GetString(3);
                                employeeInfo.phone = reader.GetString(4);
                                employeeInfo.job = reader.GetString(5);
                                employeeInfo.create_at = reader.GetDateTime(6).ToString();

                                listEmployees.Add(employeeInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class EmployeeInfo
    {
        public String id;
        public String name;
        public String age;
        public String email;
        public String phone;
        public String job;
        public String create_at;
    }
}
