using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;
using ECommerce.Data;
using ECommerce.Model;

/*
 * Profile Settings Page allowing for a user to update their information
 */

namespace ECommerce.Pages.LoginRegister
{
    public class ProfileSettingsModel : PageModel
    {
        /*private readonly ApplicationDbContext _db;
        public User general { get; set; }
        public ProfileSettingsModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            if (User.HasClaim("User", "General"))
            {
                general = _db.User.Find(Convert.ToInt32(User.FindFirst("UserId").Value));
            }
            else if (User.HasClaim("UserCrewmember", "Crewmember"))
            {
                crewmember = _db.Crewmember.Find(Convert.ToInt32(User.FindFirst("UserId").Value));
            }
            else if (User.HasClaim("manager", "Manager"))
            {
                manager = _db.Manager.Find(Convert.ToInt32(User.FindFirst("UserId").Value));
            }
            else
            {
                return new RedirectToPageResult("/LoginRegister/Login");
            }

            return Page();
        }*/
    }
}
