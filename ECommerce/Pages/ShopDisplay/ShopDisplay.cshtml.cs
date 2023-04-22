using ECommerce.Data;
using ECommerce.Pages.LoginRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace ECommerce.Pages.Shop
{
	public class ShopDisplayModel : PageModel
	{

		private readonly ApplicationDbContext _db;

		public Model.Shop Shop { get; set; }

		public Model.User User { get; set; }
		public IEnumerable<Model.Products> Products { get; set; }

		public ShopDisplayModel(ApplicationDbContext db)
		{
			_db = db;
		}
		public void OnGet(int shopID)
		{
			Shop = _db.Shop.Find(shopID); //Automatically opens db connection and executes SQL queries
			Products = _db.Products;
		}

		public async Task<IActionResult> OnPostDelete(int pid)
		{
			int x = 0;
			string shopTemp = (string)Request.Query["shopID"];
			Int32.TryParse(shopTemp, out x);

			var deleteProd = _db.Products.Find(pid);
			try
			{
				if (deleteProd != null)
				{
					_db.Products.Remove(deleteProd);
					await _db.SaveChangesAsync();
					TempData["success"] = "Product Deleted Successfully";
				}
				else
					throw new Exception();
			}
			catch (Exception ex)
			{
				TempData["error"] = "Sorry, we are unable to process your request at this time. Please try again later.";
			}
			return RedirectToPage("../ShopDisplay/ShopDisplay", "shopID", x);
		}
	}
}
