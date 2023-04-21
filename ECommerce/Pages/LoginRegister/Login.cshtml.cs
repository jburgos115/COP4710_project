using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Pages.LoginRegister
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInfo LoginInfo { get; set; }

        private readonly IConfiguration _configuration;

        //Initialize configuration to read appsettings
        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> OnPostAsync()
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
                    String myCommand = "SELECT * FROM [User] WHERE Email = @EmailAddress AND PasswordHash = @PasswordHash";
                    SqlCommand cmd = new SqlCommand(myCommand, connection);

                    SHA256 sha256 = SHA256.Create();
                    byte[] bytes = Encoding.Unicode.GetBytes(LoginInfo.Password);
                    byte[] hashValue = sha256.ComputeHash(bytes);

                    cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 50).Value = LoginInfo.Email;
                    cmd.Parameters.Add("@PasswordHash", SqlDbType.Binary, 32).Value = hashValue;
                    int id = Convert.ToInt32(cmd.ExecuteScalar());


                    //Check if query returned a primary key
                    if (id > 0)
                    {
                        string userName = "";
                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {
                            dataReader.Read();
                            userName = dataReader["Name"].ToString();
                        }

                        //Builds the Users Id card
                        var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(ClaimTypes.Email, LoginInfo.Email),
                            new Claim("User", "General"),
                            new Claim("UserId", id.ToString())
                        };
                        //Creates Cookie for user session
                        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                        return RedirectToPage("/Index");
                    }
                    /*
                    //Searches for User in Crewmember Table if not found in Customer Table
                    myCommand = "SELECT* FROM Crewmember WHERE Email = @EmailAddress AND Password = @Password ";
                    cmd = new SqlCommand(myCommand, connection);

                    cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 50).Value = LoginInfo.Email;
                    cmd.Parameters.Add("@Password", SqlDbType.Char, 8).Value = LoginInfo.PasswordHash;
                    idNumber = Convert.ToInt32(cmd.ExecuteScalar());

                    if (idNumber > 0)
                    {
                        //Builds the Users Id card
                        var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, LoginInfo.Email),
                            new Claim(ClaimTypes.Email, LoginInfo.Email),
                            new Claim("UserCrewmember", "Crewmember"),
                            new Claim("UserId", idNumber.ToString())
                        };
                        //Creates Cookie for user session
                        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                        return RedirectToPage("/Index");
                    }
                    //Searches for User in Manager Table if not found in Crewmember Table
                    myCommand = "SELECT EmpID, CinemaID FROM Manager WHERE Email = @EmailAddress AND Password = @Password ";
                    cmd = new SqlCommand(myCommand, connection);

                    cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 50).Value = LoginInfo.Email;
                    cmd.Parameters.Add("@Password", SqlDbType.Char, 8).Value = LoginInfo.PasswordHash;
                    SqlDataReader reader = cmd.ExecuteReader();

                    int ordEmpID = Convert.ToInt32(reader.GetOrdinal("EmpID"));
                    int ordCinemaID = reader.GetOrdinal("CinemaID");

                    reader.Read();
                    int temp = reader.GetInt32(ordEmpID);
                    if (temp > 0)
                    {
                        //Builds the Users Id card
                        var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, LoginInfo.Email),
                        new Claim(ClaimTypes.Email, LoginInfo.Email),
                        new Claim("manager", "Manager"),
                        new Claim("UserId", reader.GetInt32(ordEmpID).ToString()),
                        new Claim("CinemaId", reader.GetInt32(ordCinemaID).ToString())
                        };
                        //Creates Cookie for user session
                        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                        return RedirectToPage("/Index");
                    }*/
                }
            }
            catch (SqlException)
            {
                TempData["error"] = "Sorry, we are experiencing connection issues. Please try again later.";
                return Page();
            }

            ModelState.AddModelError("", "Incorrect Email Address or Password");
            return Page();
        }
    }

    public class LoginInfo
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^.{8,}$", ErrorMessage = "Minimum 8 characters required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
