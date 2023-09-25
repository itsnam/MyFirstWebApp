using Demo.Pages.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography.X509Certificates;

namespace Demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public String failMessage = "";
        public String completeMessage = "";
        public String username = "";
        public String password = "";
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost() { 
            username = Request.Form["username"];
            password = Request.Form["password"];
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
            try
            {
                String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "delete from thongtindonhang; delete from cart; select * from account where username = '" + username + "' and password = '" + password + "'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) {
                                string role = reader.GetString(2).Trim();
                                if (role.Equals("user"))
                                {
                                    Response.Redirect("/Milktea/Index");
                                }
								if (role.Equals("qltrasua"))
								{
									Response.Redirect("/MilkTea/IndexManage");
								}
                                else if (role.Equals("qlnhansu"))
                                {
                                    Response.Redirect("/Employees/Index");
                                }
                                else if (role.Equals("nhanvien"))
                                {
                                    Response.Redirect("/Employees/Sales");
                                }
                                else if (role.Equals("shipper"))
                                {
                                    Response.Redirect("/Employees/Shipper");
                                }
                            }
                            else {
                                failMessage = "Đăng nhập không thành công!";
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                failMessage = ex.Message;
            }
        }

    }
}