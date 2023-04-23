using ECommerce.Data;
using ECommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerce.Pages.LoginRegister
{
    public class CreateShopModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public Model.User User { get; set; }

        public Model.Shop Shop { get; set; }

        public CreateShopModel(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnPost(Model.Shop Shop, int UserID)
        {
            try
            {
                string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String myCommand = "INSERT INTO [Shop] (OwnerID, Name, Description) VALUES(@OwnerID, @Name, @Description)";
                    SqlCommand cmd = new SqlCommand(myCommand, connection);

                    cmd.Parameters.Add("@OwnerID", SqlDbType.Int).Value = UserID;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = Shop.Name;
                    cmd.Parameters.Add("@Description", SqlDbType.Text).Value = Shop.Description;
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
            return RedirectToPage("../Index");

        }
    }
}
