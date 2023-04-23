using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using ECommerce.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Pages.LoginRegister
{
    [BindProperties]
    public class UserRegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public Model.User User { get; set; }

        public Boolean ownerAccount;

        public UserRegisterModel(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnPost(bool ownerAccount)
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                //Read appsettings for connection string
                string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Searches for User in Customer Table
                    String myCommand = "INSERT INTO [User] (Email, PasswordHash, Name) output INSERTED.UserID VALUES(@Email, @PasswordHash, @Name)";
                    SqlCommand cmd = new SqlCommand(myCommand, connection);

                    SHA256 sha256 = SHA256.Create();
                    byte[] bytes = Encoding.Unicode.GetBytes(User.Password);
                    byte[] hashValue = sha256.ComputeHash(bytes);

                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = User.Email;
                    cmd.Parameters.Add("@PasswordHash", SqlDbType.Binary, 32).Value = hashValue;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = User.Name;
                    int modified = (int)cmd.ExecuteScalar();

					TempData["success"] = "Registration was Successful";
					if (ownerAccount == false)
                    {
                        return RedirectToPage("/LoginRegister/Login");
                    }
                    else
                    {
						return RedirectToPage("/LoginRegister/CreateShop", "UserID", new { UserID = modified });
					}
                }
            }
            catch (SqlException)
            {
                TempData["error"] = "Sorry, we are experiencing connection issues. Please try again later.";
                return Page();
            }

            ModelState.AddModelError("", "There was an error processing your request");
            return Page();



            //SHA256 sha256 = SHA256.Create();
            //byte[] bytes = Encoding.Unicode.GetBytes(User.PasswordHash);
            //byte[] hashValue = sha256.ComputeHash(User.PasswordHash);

            //Sends the Data of the Customer being registered to the database
            //await _db.User.AddAsync(User);
            //await _db.SaveChangesAsync();
            //TempData["success"] = "Registration was Successful";
            //return RedirectToPage("/LoginRegister/Login");
            

            
        }
    }
}
