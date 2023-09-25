using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace Demo.Pages.Cart
{
    public class InfoModel : PageModel
    {
		public String failMessage = "";
		public String completeMessage = "";
		public String name = "";
		public String address = "";
		public String phonenumber = "";
        public void OnGet()
		{
		}

		public void OnPost()
		{
			name = Request.Form["name"];
			address = Request.Form["address"];
			phonenumber = Request.Form["phonenumber"];

			if (name.Length == 0)
			{
				failMessage = "Bạn chưa nhập Họ tên!";
				return;
			}
			else if (address.Length == 0)
			{
				failMessage = "Bạn chưa nhập Địa chỉ";
				return;
			}
			else if (phonenumber.Length == 0)
			{
				failMessage = "Bạn chưa nhập Số điện thoại!";
				return;
			}
			else if (checkDigit(phonenumber) == false)
			{
				failMessage = "Số điện thoại được nhập phải là con số!";
				return;
			}
			else if (phonenumber.Length > 10)
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
					String sql = "insert into thongtindonhang (name, address, phonenumber) values (@name, @address, @phonenumber);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", name);
						command.Parameters.AddWithValue("@address", address);
                        command.Parameters.AddWithValue("@phonenumber", phonenumber);
                        command.ExecuteNonQuery();
					}
                    sql = "INSERT INTO thongtinbanhang (name, address, phoneNumber) SELECT name, address, phoneNumber FROM thongtindonhang; " +
						  "INSERT INTO salecart (itemImage, itemName, itemCost, quantity) SELECT itemImage, itemName, itemCost, quantity FROM cart; " +
						  "DELETE FROM cart; DELETE FROM thongtindonhang";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
			}
			catch (Exception ex)
			{
				failMessage = ex.Message;
				return;
			}

			name = ""; address = ""; phonenumber = "";
            completeMessage = "Cập nhật thông tin giao hàng thành công! Sẽ tự động chuyển trang sau 3 giây";
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
