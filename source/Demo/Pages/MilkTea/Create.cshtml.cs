using Demo.Pages.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Demo.Pages.MilkTea
{
    public class CreateModel : PageModel
    {
		public MilkTeaInfo milkteaInfo = new MilkTeaInfo();
		public String failMessage = "";
		public String completeMessage = "";
		public void OnGet()
		{
		}

		public void OnPost()
		{
			milkteaInfo.img = Request.Form["img"];
			milkteaInfo.name = Request.Form["name"];
			milkteaInfo.cost = Request.Form["cost"];

			if (milkteaInfo.name.Length == 0)
			{
				failMessage = "Bạn chưa nhập Tên!";
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
					String sql = "insert into trasua(img, name, cost) values (@img, @name, @cost)";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@img", milkteaInfo.img);
						command.Parameters.AddWithValue("@name", milkteaInfo.name);
						command.Parameters.AddWithValue("@cost", milkteaInfo.cost);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				failMessage = ex.Message;
				return;
			}

			milkteaInfo.img = ""; milkteaInfo.name = ""; milkteaInfo.cost = "";
			completeMessage = "Thêm trà sữa thành công!";

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
