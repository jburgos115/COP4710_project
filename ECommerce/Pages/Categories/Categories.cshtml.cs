using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace ECommerce.Pages.Categories
{
    public class CategoriesModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IEnumerable<Model.Category> Category { get; set; } //Category objects
																  //Database context
		public IEnumerable<Model.Products> Products { get; set; }

		public IEnumerable<Model.Represents> Represents { get; set; }

		public CategoriesModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            Category = _db.Category; //Automatically opens db connection and executes SQL queries
            Products = _db.Products;
            Represents = _db.Represents;
        }
    }
}
