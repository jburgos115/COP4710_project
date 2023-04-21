using ECommerce.Data;
using ECommerce.Pages.LoginRegister;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace ECommerce.Pages.Shop
{
	public class ShopDisplayModel : PageModel
	{

        private readonly ApplicationDbContext _db;

		public Model.Shop Shop { get; set; }
		public IEnumerable<Model.Products> Products { get; set; }

		public ShopDisplayModel(ApplicationDbContext db)
		{
			_db = db;
		}
		public void OnGet(int id)
		{
			Shop = _db.Shop.Find(id); //Automatically opens db connection and executes SQL queries
			Products = _db.Products;
		}
	}
}
