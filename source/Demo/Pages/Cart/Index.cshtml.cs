using Demo.Pages.MilkTea;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Demo.Pages.Cart
{
	public class IndexModel : PageModel
	{
		public List<MilkTeaInfo> listMilkTeas = new List<MilkTeaInfo>();
		public string SearchQuery { get; set; }
		public void OnGet(string searchQuery)
		{
			try
			{
				String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
				using (SqlConnection connection = new SqlConnection(@connectionString))
				{
					connection.Open();

					string sql = "SELECT * FROM cart";

					if (!string.IsNullOrEmpty(searchQuery))
					{
						sql += " WHERE name LIKE '%' + @searchQuery + '%'";
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
								MilkTeaInfo milkteaInfo = new MilkTeaInfo();
								milkteaInfo.id = "" + reader.GetInt32(0);
								milkteaInfo.img = reader.GetString(1);
								milkteaInfo.name = reader.GetString(2);
								milkteaInfo.cost = "" + reader.GetInt32(3);
								milkteaInfo.create_at = "" + reader.GetInt32(4);

								listMilkTeas.Add(milkteaInfo);
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

		public void OnPost() {
			String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
			using (SqlConnection connection = new SqlConnection(@connectionString))
			{
				connection.Open();
				String sql = "DELETE FROM cart";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.ExecuteNonQuery();
				}

				Response.Redirect("/Cart/Index");
			}
		}
	}
}
