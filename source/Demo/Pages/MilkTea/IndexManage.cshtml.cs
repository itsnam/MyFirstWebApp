using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;
using Demo.Pages.Employees;

namespace Demo.Pages.MilkTea
{
    public class IndexManageModel : PageModel
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

					string sql = "SELECT * FROM trasua";

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
								milkteaInfo.create_at = reader.GetDateTime(4).ToString();

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
	}
}
