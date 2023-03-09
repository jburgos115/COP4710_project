using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace ECommerce.Pages.Categories
{
    public class CategoriesDisplayModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public Model.Category Category { get; set; }
		public IEnumerable<Model.Products> Products { get; set; }

		public IEnumerable<Model.Represents> Represents { get; set; }

		public CategoriesDisplayModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int id)
        {
            Category = _db.Category.Find(id); //Automatically opens db connection and executes SQL queries
            Products = _db.Products;
            Represents = _db.Represents;
        }
    }
}
