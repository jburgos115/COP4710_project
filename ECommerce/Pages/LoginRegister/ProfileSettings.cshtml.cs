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
    [BindProperties]
    public class ProfileSettingsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private string connectionString;
        public User general { get; set; }
        public ProfileSettingsModel(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            general = new User();
        }
        public void OnGet()
        {
            try
            {
                //Read appsettings for connection string
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    int id = Int32.Parse(User.FindFirst("UserID").Value);

                    String myCommand = "SELECT Name, Email FROM [User] WHERE UserID = @UserID";

                    SqlCommand cmd = new SqlCommand(myCommand, connection);
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = id;

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        dataReader.Read();
                        general.Name = dataReader["Name"].ToString();
                        general.Email = dataReader["Email"].ToString();
                    }

                    cmd.Dispose();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Sorry, we are unable to process your request at this time. Please try again later.";
                Response.Redirect("/../Index");
            }

            //return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                //Read appsettings for connection string
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    int id = Int32.Parse(User.FindFirst("UserID").Value);

                    //Update User details in User table
                    String myCommand = "UPDATE [User] SET Name = @Name, Email = @Email WHERE UserID = @UserID";

                    SqlCommand cmd = new SqlCommand(myCommand, connection);
                    cmd = new SqlCommand(myCommand, connection);

                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = general.Name;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = general.Email;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = id;

                    int success = Convert.ToInt32(cmd.ExecuteNonQuery());

                    //Check if query was unsuccessful
                    if (success < 1)
                    {
                        TempData["error"] = "There was an error updating your profile, please try again later.";
                    }

                    cmd.Dispose();
                    connection.Close();

                    TempData["success"] = "Profile Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Sorry, we are unable to process your request at this time. Please try again later.";
            }

            return Redirect("/../Index");
        }
    }
}
