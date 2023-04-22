using ECommerce.Data;
using ECommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ECommerce.Pages.Update
{
	public class UpdateModel : PageModel
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _configuration;
		private string connectionString;
		public Model.Products Products { get; set; }
		public UpdateModel(ApplicationDbContext db, IConfiguration configuration)
		{
			_db = db;
			_configuration = configuration;
			connectionString = _configuration["ConnectionStrings:DefaultConnection"];
		}
		public void OnGet(int pid)
		{
			Products = _db.Products.Find(pid); //Automatically opens db connection and executes SQL queries
		}

		public async Task<IActionResult> OnPost(Model.Products products, int pid, int sid)
		{
			var updateProd = _db.Products.Find(pid);
			try
			{
				//Read appsettings for connection string
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					//Update User details in User table
					String myCommand = "UPDATE [Products] SET Name = @Name, Description = @Description, Price = @Price, Quantity = @Quantity WHERE ProductID = @ProductID";

					SqlCommand cmd = new SqlCommand(myCommand, connection);
					cmd = new SqlCommand(myCommand, connection);

					cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = products.Name;
					cmd.Parameters.Add("@Description", SqlDbType.Text).Value = products.Description;
					cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = products.Price;
					cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = products.Quantity;
					cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = pid;
					cmd.Parameters.Add("@ShopID", SqlDbType.Int).Value = sid;

					int success = Convert.ToInt32(cmd.ExecuteNonQuery());

					//Check if query was unsuccessful
					if (success < 1)
					{
						TempData["error"] = "There was an error updating your profile, please try again later.";
					}

					cmd.Dispose();
					connection.Close();

					TempData["success"] = "Profile Updated Successfully";
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = "Sorry, we are unable to process your request at this time. Please try again later.";
			}

			return RedirectToPage("../ShopDisplay/ShopDisplay", "ShopID", new { ShopID = updateProd.ShopID });
		}
	}
}
