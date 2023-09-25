using Demo.Pages.MilkTea;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Demo.Pages.Employees
{
    public class SalesModel : PageModel
    {
        public List<MilkTeaInfo> listMilkTeas = new List<MilkTeaInfo>();
        public string SearchQuery { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public void OnGet(string searchQuery)
        {
            try
            {
                String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
                using (SqlConnection connection = new SqlConnection(@connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM salecart";

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
                    sql = "SELECT * FROM thongtinbanhang";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Name = reader.GetString(0);
                                Address = reader.GetString(1);
                                PhoneNumber = reader.GetString(2);
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(PhoneNumber)) {
                        sql = "DELETE FROM salecart";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
        public void OnPost(){
			String connectionString = System.IO.File.ReadAllText("connectdatabaselittlemilktea.txt");
            using (SqlConnection connection = new SqlConnection(@connectionString)) {
				connection.Open();
				String sql = "INSERT INTO thongtingiaohang (name, address, phoneNumber) SELECT name, address, phoneNumber FROM thongtinbanhang; " +
					         "INSERT INTO shippercart (itemImage, itemName, itemCost, quantity) SELECT itemImage, itemName, itemCost, quantity FROM salecart; " +
                             "DELETE FROM salecart; DELETE FROM thongtinbanhang";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.ExecuteNonQuery();
				}
				Response.Redirect("/Employees/Sales");
			}

		}

	}
}
