using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;
using Demo.Pages.Employees;

namespace Demo.Pages.MilkTea
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

        public void OnPost()
        {
            try
            {
                string itemImage = Request.Form["itemImage"];
                string itemName = Request.Form["itemName"];
                string itemCost = Request.Form["itemCost"];
                string quantity = Request.Form["itemQuantity"];

                String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
                using (SqlConnection connection = new SqlConnection(@connectionString))
                {
                    connection.Open();
                    String sql = "insert into cart(itemImage, itemName, itemCost, quantity) values (@itemImage, @itemName, @itemCost, @quantity) " +
                        "UPDATE cart set quantity = (SELECT SUM(quantity) AS quantity " +
						"FROM cart GROUP BY itemName HAVING COUNT(itemName) > 1) where itemName = " +
                        "(SELECT itemName FROM cart GROUP BY itemName HAVING COUNT(itemName) > 1); " +
                        "DELETE t1 FROM cart t1 JOIN cart t2 ON t1.itemName = t2.itemName AND t1.itemId > t2.itemId;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@itemImage", itemImage);
                        command.Parameters.AddWithValue("@itemName", itemName);
                        command.Parameters.AddWithValue("@itemCost", itemCost);
                        command.Parameters.AddWithValue("@quantity", quantity);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            TempData["Message"] = "Đã thêm vào giỏ hàng";
            Response.Redirect("/MilkTea/Index");
        }

    }

    public class MilkTeaInfo
	{
		public String img;
		public String id;
		public String name;
		public String cost;
		public String create_at;
	}
}
