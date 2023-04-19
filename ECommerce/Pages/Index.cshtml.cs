using ECommerce.Data;
using ECommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IEnumerable<Model.Category> Category { get; set; } //Category objects
                                                                  //Database context
        public IEnumerable<Model.Products> Products { get; set; }

        public IEnumerable<Model.Represents> Represents { get; set; }

        public IndexModel(ApplicationDbContext db)
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