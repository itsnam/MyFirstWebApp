using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Demo.Pages.Employees
{
    public class CreateModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public String failMessage = "";
        public String completeMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() {
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
            else if (checkDigit(employeeInfo.age) == false)
            {
                failMessage = "Tuổi được nhập phải là con số!";
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
            else if (employeeInfo.job.Length == 0)
            {
                failMessage = "Bạn chưa nhập Bộ phận!";
                return;
            }

            try
            {
                String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
                using (SqlConnection connection = new SqlConnection(@connectionString)) { 
                    connection.Open();
                    String sql = "insert into nhanvien(name, age, email, phone, job) values (@name, @age, @email, @phone, @job)";
                    using (SqlCommand command = new SqlCommand(sql, connection)) { 
                        command.Parameters.AddWithValue("@name", employeeInfo.name);
                        command.Parameters.AddWithValue("@age", employeeInfo.age);
                        command.Parameters.AddWithValue("@email", employeeInfo.email);
                        command.Parameters.AddWithValue("@phone", employeeInfo.phone);
                        command.Parameters.AddWithValue("@job", employeeInfo.job);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) {
                failMessage = ex.Message;
                return;
            }

            employeeInfo.name = ""; employeeInfo.age = ""; employeeInfo.email = ""; employeeInfo.phone = ""; employeeInfo.job = "";
            completeMessage = "Thêm nhân viên mới thành công!";

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
