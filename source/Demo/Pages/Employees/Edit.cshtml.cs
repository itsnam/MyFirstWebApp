using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Demo.Pages.Employees
{
    public class EditModel : PageModel
    {
        public String completeMessage = "";
        public String failMessage = "";
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public void OnGet()
        {
            String id = Request.Query["id"];
            try{
                String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select * from nhanvien where id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                employeeInfo.id = "" + reader.GetInt32(0);
                                employeeInfo.name = reader.GetString(1);
                                employeeInfo.age = "" + reader.GetInt32(2);
                                employeeInfo.email = reader.GetString(3);
                                employeeInfo.phone = reader.GetString(4);
                                employeeInfo.job = reader.GetString(5);

                            }
                        }
                    }
                }
            }catch (Exception ex) {
                failMessage = ex.Message;
            }
        }

        public void OnPost() {
            employeeInfo.id = Request.Form["id"];
            employeeInfo.name = Request.Form["name"];
            employeeInfo.age = Request.Form["age"];
            employeeInfo.email = Request.Form["email"];
            employeeInfo.phone = Request.Form["phone"];
            employeeInfo.job = Request.Form["job"];
            if (employeeInfo.name.Length == 0)
            {
                failMessage = "Bạn chưa nhập Họ tên!";
                return;
            }
            else if (employeeInfo.age.Length == 0)
            {
                failMessage = "Bạn chưa nhập Tuổi!";
                return;
            }
            else if (employeeInfo.email.Length == 0)
            {
                failMessage = "Bạn chưa nhập Email!";
                return;
            }
            else if (employeeInfo.phone.Length == 0)
            {
                failMessage = "Bạn chưa nhập Số điện thoại!";
                return;
            }
            else if (employeeInfo.job.Length == 0)
            {
                failMessage = "Bạn chưa nhập Bộ phận!";
                return;
            }
            else if (checkDigit(employeeInfo.age) == false)
            {
                failMessage = "Tuổi được nhập phải là con số!";
                return;
            }
            else if (checkDigit(employeeInfo.phone) == false)
            {
                failMessage = "Số điện thoại được nhập phải là con số!";
                return;
            }
            else if (employeeInfo.phone.Length > 10)
            {
                failMessage = "Số điện thoại không quá 10 kí tự";
                return;
            }

            try
            {
                String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
                using (SqlConnection connection = new SqlConnection(@connectionString))
                {
                    connection.Open();
                    String sql = "update nhanvien set name = @name, age = @age, phone = @phone, job = @job where id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", employeeInfo.name);
                        command.Parameters.AddWithValue("@age", employeeInfo.age);
                        command.Parameters.AddWithValue("@email", employeeInfo.email);
                        command.Parameters.AddWithValue("@phone", employeeInfo.phone);
                        command.Parameters.AddWithValue("@job", employeeInfo.job);
                        command.Parameters.AddWithValue("@id", employeeInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) {
                failMessage = ex.Message;
                return;
            }

			completeMessage = "Chỉnh sửa nhân viên thành công!";
			Response.Redirect("/Employees/Index");

        }

        private bool checkDigit(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
    }
}
