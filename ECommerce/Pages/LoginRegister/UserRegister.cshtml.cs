using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using ECommerce.Data;

namespace ECommerce.Pages.LoginRegister
{
    [BindProperties]
    public class UserRegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public Model.User User { get; set; }

        public UserRegisterModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                //Sends the Data of the Customer being registered to the database
                await _db.User.AddAsync(User);
                await _db.SaveChangesAsync();
                TempData["success"] = "Registration was Successful";
                return RedirectToPage("/LoginRegister/Login");
            }

            return Page();
        }
    }
}
