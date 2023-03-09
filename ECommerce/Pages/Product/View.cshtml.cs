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
        //public string ProductName { get; set; }
        public ViewModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int pid)
        {
            //Products = _db.Products;
            try
            {
                Products = _db.Products.Find(pid);
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                Response.Redirect("/../Error");
            }
        }
    }
}
