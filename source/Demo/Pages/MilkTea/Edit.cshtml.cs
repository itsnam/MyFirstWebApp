using Demo.Pages.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Demo.Pages.MilkTea
{
    public class EditModel : PageModel
    {
		public String completeMessage = "";
		public String failMessage = "";
		public MilkTeaInfo milkteaInfo = new MilkTeaInfo();
		public void OnGet()
		{
			String id = Request.Query["id"];
			try
			{
				String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "select * from trasua where id = @id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								milkteaInfo.id = "" + reader.GetInt32(0);
								milkteaInfo.img = reader.GetString(1);
								milkteaInfo.name = reader.GetString(2);
								milkteaInfo.cost = "" + reader.GetInt32(3);
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

		public void OnPost()
		{
			milkteaInfo.id = Request.Form["id"];
			milkteaInfo.name = Request.Form["name"];
			milkteaInfo.cost = Request.Form["cost"];
			String a = Request.Form["img1"];
			if (!a.Equals(""))
			{
				milkteaInfo.img = Request.Form["img1"];
			}
			else
			{
				milkteaInfo.img = Request.Form["img"];
			}

			if (milkteaInfo.name.Length == 0)
			{
				failMessage = "Bạn chưa nhập Tên";
				return;
			}
			else if (checkDigit(milkteaInfo.cost) == false)
			{
				failMessage = "Giá được nhập phải là con số!";
				return;
			}
			else if (milkteaInfo.cost.Length == 0)
			{
				failMessage = "Bạn chưa nhập Giá!";
				return;
			}

			try
			{
				String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
				using (SqlConnection connection = new SqlConnection(@connectionString))
				{
					connection.Open();
					String sql = "update trasua set img = @img, name = @name, cost = @cost where id = @id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@img", milkteaInfo.img);
						command.Parameters.AddWithValue("@name", milkteaInfo.name);
						command.Parameters.AddWithValue("@cost", milkteaInfo.cost);
						command.Parameters.AddWithValue("@id", milkteaInfo.id);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				failMessage = ex.Message;
				return;
			}
			completeMessage = "Chỉnh sửa trà sữa thành công!";
			Response.Redirect("/MilkTea/IndexManage");
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
