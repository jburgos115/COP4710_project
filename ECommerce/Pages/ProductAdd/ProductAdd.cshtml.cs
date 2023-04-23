using ECommerce.Data;
using ECommerce.Pages.LoginRegister;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Pages.ProductAdd
{
    public class ProductAddModel : PageModel
    {

        private readonly ApplicationDbContext _db;

        public Model.Shop Shop { get; set; }

        public Model.Products Products { get; set; }

        public Model.User User { get; set; }

        public ProductAddModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int shopID)
        {
            Shop = _db.Shop.Find(shopID); //Automatically opens db connection and executes SQL queries
        }

        public async Task<IActionResult> OnPost(Model.Products products)
        {
            int x = 0;
            string shopTemp = (string)Request.Query["shopID"];
            Int32.TryParse(shopTemp, out x);

            products.ShopID = x;

            try
            {
                await _db.Products.AddAsync(products);
                await _db.SaveChangesAsync();
                TempData["success"] = "New Product Created Successfully";
                return RedirectToPage("../Upload/Upload", "ProductID", new { ProductID = products.ProductID });
            }
            catch (Exception ex)
            {
                TempData["error"] = "Sorry, we are unable to process your request at this time. Please try again later.";
            }
            return RedirectToPage("../Index");

        }
    }
}
