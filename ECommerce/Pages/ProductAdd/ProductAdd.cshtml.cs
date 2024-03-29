using ECommerce.Data;
using ECommerce.Model;
using ECommerce.Pages.LoginRegister;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ECommerce.Pages.ProductAdd
{
    [Authorize(Policy = "Basic")]
    public class ProductAddModel : PageModel
    {

        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public Model.Shop Shop { get; set; }

        public Model.Products Products { get; set; }

        public Model.User User { get; set; }

		public IEnumerable<Model.Category> Category { get; set; }

        public List<Boolean> catList = new List<Boolean>();

        public int Size;

        public ProductAddModel(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public void OnGet(int shopID)
        {
            Size = _db.Category.Count();
            for(int i = 0; i < Size; i++)
            {
                catList.Add(false);
            }
            Category = _db.Category;
            Shop = _db.Shop.Find(shopID); //Automatically opens db connection and executes SQL queries
        }

        public async Task<IActionResult> OnPost(Model.Products products, List<Boolean> catList)
        {
            int x = 0;
            string shopTemp = (string)Request.Query["shopID"];
            Int32.TryParse(shopTemp, out x);

            products.ShopID = x;

            int prodID = 0;

            try
            {
                string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Inserts Product into Table
                    String myCommand = "INSERT INTO [Products] (ShopID, Name, Description, Price, Quantity) output INSERTED.ProductID VALUES(@ShopID, @Name, @Description, @Price, @Quantity)";
                    SqlCommand cmd = new SqlCommand(myCommand, connection);

                    cmd.Parameters.Add("@ShopID", SqlDbType.Int).Value = x;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = products.Name;
                    cmd.Parameters.Add("@Description", SqlDbType.Text).Value = products.Description;
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = products.Price;
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = products.Quantity;
                    prodID = (int)cmd.ExecuteScalar();

                    cmd.Dispose();
                    connection.Close();

                    TempData["success"] = "Profile Updated Successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Sorry, we are unable to process your request at this time. Please try again later.";
            }

            for(int i = 1; i <= catList.Count(); i++)
            {
                if (catList[i-1] == true)
                {
                    try
                    {
                        string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            //Inserts Represents into Table
                            String myCommand = "INSERT INTO [Represents] (CategoryID, ProductID) VALUES(@CategoryID, @ProductID)";
                            SqlCommand cmd = new SqlCommand(myCommand, connection);

                            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = i;
                            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = prodID;
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
                    catch (SqlException)
                    {
                        TempData["error"] = "Sorry, we are experiencing connection issues. Please try again later.";
                        return Page();
                    }
                }
            }

            return RedirectToPage("../Upload/Upload", "ProductID", new { ProductID = prodID });

        }
    }
}
