using ECommerce.Data;
using ECommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Pages.Shop
{
    public class ShopModel : PageModel
    {
		private readonly ApplicationDbContext _db;
		public IEnumerable<Model.Shop> Shop { get; set; } //Category objects
																  //Database context
		public IEnumerable<Model.Products> Products { get; set; }

		public ShopModel(ApplicationDbContext db)
		{
			_db = db;
		}

		public void OnGet()
        {
			Shop = _db.Shop; //Automatically opens db connection and executes SQL queries
			Products = _db.Products;
		}
    }
}
