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
                // Generate unique identifier using base64 encoding to find product image
                string uniqueIdentifier = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Concat(Products.ProductID.ToString(), Products.Name)));
                string imgPath = string.Concat("/ProductImages/", uniqueIdentifier, ".png");
                
                // Store product image path to ViewData
                ViewData["imgsrc"] = imgPath;
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                Response.Redirect("/../Error");
            }
        }

        
    }

}
