using Demo.Pages.Employees;
using Demo.Pages.MilkTea;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Demo.Pages.Cart
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
					String sql = "select * from cart where itemId = @id";
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
								milkteaInfo.create_at = "" + reader.GetInt32(4);
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
			milkteaInfo.create_at = Request.Form["quantity"];

			try
			{
				String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
				using (SqlConnection connection = new SqlConnection(@connectionString))
				{
					connection.Open();
					String sql = "update cart set quantity = @quantity where itemId = @id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@quantity", milkteaInfo.create_at);
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

			completeMessage = "Cập nhật số lượng thành công!";
			Response.Redirect("/Cart/Index");

		}
    }
}
