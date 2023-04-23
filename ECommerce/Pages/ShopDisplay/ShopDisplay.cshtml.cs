using ECommerce.Data;
using ECommerce.Model;
using ECommerce.Pages.LoginRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ECommerce.Pages.Shop
{
	public class ShopDisplayModel : PageModel
	{

		private readonly ApplicationDbContext _db;

		private readonly IWebHostEnvironment _environment;

		public Model.Shop Shop { get; set; }

		public Model.User User { get; set; }
		public IEnumerable<Model.Products> Products { get; set; }

		public ShopDisplayModel(IWebHostEnvironment environment, ApplicationDbContext db)
		{
			_environment = environment;
			_db = db;
		}
		public void OnGet(int shopID)
		{
			Shop = _db.Shop.Find(shopID); //Automatically opens db connection and executes SQL queries
			Products = _db.Products;
		}

		public async Task<IActionResult> OnPostDelete(int pid, int sid)
		{
			var deleteProd = _db.Products.Find(pid);
			try
			{
				if (deleteProd != null)
				{
					_db.Products.Remove(deleteProd);
					await _db.SaveChangesAsync();

					string path = Path.Combine(this._environment.WebRootPath, "ProductImages");

					string uniqueIdentifier = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Concat(pid.ToString(), deleteProd.Name)));
					string imgPath = string.Concat(uniqueIdentifier, ".png");

					string finalPath = Path.Combine(path, imgPath);

					System.IO.File.Delete(finalPath);

					TempData["success"] = "Product Deleted Successfully";
				}
				else
					throw new Exception();
			}
			catch (Exception ex)
			{
				TempData["error"] = "Sorry, we are unable to process your request at this time. Please try again later.";
			}
			return RedirectToPage("../ShopDisplay/ShopDisplay", "ShopID", new { ShopID = sid });
		}
	}
}
