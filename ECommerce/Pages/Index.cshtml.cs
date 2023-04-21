using ECommerce.Data;
using ECommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public Model.Products Products { get; set; }

        public IEnumerable<Model.Category> Category { get; set; }
        public IEnumerable<Model.Products> Products_i { get; set; }

        public IEnumerable<Model.Represents> Represents { get; set; }

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Category = _db.Category; //Automatically opens db connection and executes SQL queries
            Products_i = _db.Products;
            Represents = _db.Represents;
        }

        public void OnGetFetchImg(int pid)
		{
			// Retrieve product info by product id (pid)
			Products = _db.Products.Find(pid);
			// Generate unique identifier using base64 encoding to find product image
			string uniqueIdentifier = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Concat(Products.ProductID.ToString(), Products.Name)));
			string imgPath = string.Concat("/ProductImages/", uniqueIdentifier, ".png");

            // Store product image path to ViewData
            ViewData["imgsrc"] = imgPath;
        }
    }
}