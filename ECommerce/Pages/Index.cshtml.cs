using ECommerce.Data;
using ECommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web;

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
            if (Products != null)
            {
                // Generate unique identifier using base64 encoding to find product image
                string uniqueIdentifier = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Concat(Products.ProductID.ToString(), Products.Name)));
                string filename = string.Concat("\\ProductImages\\", uniqueIdentifier, ".png");

                string imgDir = String.Concat(Directory.GetCurrentDirectory(), "\\wwwroot");
                string imgPath = string.Concat(imgDir, filename);

                // Check if image exists for product id
                bool found = System.IO.File.Exists(imgPath);
                if (found)
                    ViewData["imgsrc"] = filename; // Pass image path if exists
                else
                    ViewData["imgsrc"] = "\\ProductImages\\default-image.png"; // Pass default image
            }
            else
            {
                ViewData["imgsrc"] = "\\ProductImages\\default-image.png";
                Products = new Model.Products();
            }
        }
    }
}