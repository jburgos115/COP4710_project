using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Pages.ShopDisplay
{
    public class ShopOrdersModel : PageModel
    {
		public List<OrderInfo> OrderList { get; set; }
		private readonly IConfiguration _configuration;
		private string connectionString;

		public ShopOrdersModel(IConfiguration configuration)
		{
			_configuration = configuration;
			connectionString = _configuration["ConnectionStrings:DefaultConnection"];
			OrderList = new List<OrderInfo>();
		}
		public void OnGet(int shopID)
		{
			try
			{
				//Read appsettings for connection string
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					int id = Int32.Parse(User.FindFirst("UserID").Value);

					String myCommand = "SELECT OrderID, Name, BuyQuantity, TotalCost FROM[Orders] o JOIN[Products] p ON p.shopID = @ShopID AND p.productID = o.productID";

					SqlCommand cmd = new SqlCommand(myCommand, connection);
					cmd.Parameters.Add("@shopID", SqlDbType.Int).Value = shopID;

					//Read the database
					using (SqlDataReader dataReader = cmd.ExecuteReader())
					{
						while (dataReader.Read())
						{
							OrderInfo order = new OrderInfo();
							order.OrderID = (int)dataReader["OrderID"];
							order.Product = dataReader["Name"].ToString();
							order.Quantity = (int)dataReader["BuyQuantity"];
							order.Cost = (decimal)dataReader["TotalCost"];
							OrderList.Add(order);
						}
					}

					cmd.Dispose();
					connection.Close();
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = "Sorry, we are unable to process your request at this time. Please try again later.";
				Response.Redirect("/../Index");
			}
		}
		public class OrderInfo
		{
			public int OrderID { get; set; }
			public string Product { get; set; }
			public int Quantity { get; set; }
			public decimal Cost { get; set; }
		}
	}
}
