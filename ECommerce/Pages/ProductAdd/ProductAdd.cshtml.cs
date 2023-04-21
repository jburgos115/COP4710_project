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
        public ProductAddInfo ProductAddInfo { get; set; }

        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _db;

        public Model.Shop Shop { get; set; }
        public IEnumerable<Model.Products> Products { get; set; }

        public ProductAddModel(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public void OnGet(int id)
        {
            Shop = _db.Shop.Find(id); //Automatically opens db connection and executes SQL queries
            Products = _db.Products;
        }
        public async Task<IActionResult> OnPostAddProduct()
        {
            //Read appsettings for connection string
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Searches for User in Customer Table
                    String myCommand = "INSERT INTO Products (ShopID, Name, Description, Price, Quanity) VALUES (@shop.ShopID, @Name, @Description, @Price, @Quanity) ";
                    SqlCommand cmd = new SqlCommand(myCommand, connection);
                }
                catch (SqlException)
                {
                    TempData["error"] = "Sorry, we are experiencing connection issues. Please try again later.";
                }
            }
            return Page();
        }
    }

    public class ProductAddInfo
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
    }
}
