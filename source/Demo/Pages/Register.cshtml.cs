using Demo.Pages.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Demo.Pages
{
    public class RegisterModel : PageModel
    {
        public String failMessage = "";
        public String completeMessage = "";
        public String username = "";
        public String password = "";
        public String checkpassword = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            username = Request.Form["username"];
            password = Request.Form["password"];
            checkpassword = Request.Form["checkpassword"];

            if (username.Length == 0)
            {
                failMessage = "Bạn chưa nhập Tài khoản!";
                return;
            }
            else if (username.Length < 6)
            {
                failMessage = "Tài khoản phải hơn 6 kí tự";
                return;
            }
            else if (password.Length == 0)
            {
                failMessage = "Bạn chưa nhập Mật khẩu";
                return;
            }
            else if (password.Length < 6)
            {
                failMessage = "Mật khẩu phải hơn 6 kí tự";
                return;
            }
            else if (checkpassword.Length == 0)
            {
                failMessage = "Bạn chưa nhập Xác nhận mật khẩu";
                return;
            }
            else if (checkpassword.Length < 6)
            {
                failMessage = "Xác nhận mật khẩu phải hơn 6 kí tự";
                return;
            }
            else if (!password.Equals(checkpassword))
            {
                failMessage = "Xác nhận mật khẩu phải giống mật khẩu";
                return;
            }

            try
            {
                String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
                using (SqlConnection connection = new SqlConnection(@connectionString))
                {
                    connection.Open();
                    String sql = "insert into account(username, password, role) values (@username, @password, @role)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@role", "user");
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                failMessage = ex.Message;
                return;
            }

            username = ""; password = ""; checkpassword = "";
            completeMessage = "Thêm tài khoản thành công";
        }
        private bool checkDigit(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]) == true)
                    return true;
            }
            return false;
        }
    }
}
