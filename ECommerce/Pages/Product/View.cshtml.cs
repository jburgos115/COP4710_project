using ECommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Pages.Product
{
    [BindProperties]
    public class ViewModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public Model.Product Product { get; set; }
        public ViewModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int pid)
        {
            if (_db.Product.Find(pid) != null)
                Product = _db.Product.Find(pid);
            else
                Console.WriteLine("is null");
            /*try
            {
                Product = _db.Product.Find(pid);
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                Response.Redirect("/../Error");
            }*/
        }
    }
}
