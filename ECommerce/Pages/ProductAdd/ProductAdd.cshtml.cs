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

        public ProductAddModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int id)
        {
            Shop = _db.Shop.Find(id); //Automatically opens db connection and executes SQL queries
        }

        public async Task<IActionResult> OnPost(Model.Products products)
        {
            //products.ShopID = shop.ShopID;

            try
            {
                await _db.Products.AddAsync(products);
                await _db.SaveChangesAsync();
                TempData["success"] = "New Paper Created Successfully";
                return RedirectToPage("Upload", "PaperID", new { ProductID = products.ProductID });
            }
            catch (Exception ex)
            {
                TempData["error"] = "Sorry, we are unable to process your request at this time. Please try again later.";
            }
            return RedirectToPage("Index");
        }
    }
}
