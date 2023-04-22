using System.Text;
using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Pages.Product
{
    [BindProperties]
    public class ViewModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public Model.Products Products { get; set; }
        //public string ProductImg { get; set; }
        public ViewModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int pid)
        {
            try
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
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                Response.Redirect("/../Error");
            }
        }
    }

}
