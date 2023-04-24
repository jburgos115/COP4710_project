using ECommerce.Data;
using ECommerce.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

namespace ECommerce.Pages.ShopDisplay
{
	[Authorize(Policy = "Basic")]
	public class updateModel : PageModel
    {
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _configuration;
		private string connectionString;
		public Model.Shop Shop { get; set; }
		public updateModel(ApplicationDbContext db, IConfiguration configuration)
		{
			_db = db;
			_configuration = configuration;
			connectionString = _configuration["ConnectionStrings:DefaultConnection"];
		}
		public void OnGet(int sid)
        {
			Shop = _db.Shop.Find(sid);
		}

		public async Task<IActionResult> OnPost(Model.Shop shop, int sid)
		{
			var updateShop = _db.Shop.Find(sid);
			try
			{
				//Read appsettings for connection string
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					//Update Shop details in Shop table
					String myCommand = "UPDATE [Shop] SET Name = @Name, Description = @Description WHERE ShopID = @ShopID";

					SqlCommand cmd = new SqlCommand(myCommand, connection);
					cmd = new SqlCommand(myCommand, connection);

					cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = shop.Name;
					cmd.Parameters.Add("@Description", SqlDbType.Text).Value = shop.Description;
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

			return RedirectToPage("../ShopDisplay/ShopDisplay", "ShopID", new { ShopID = sid });
		}
	}
}
