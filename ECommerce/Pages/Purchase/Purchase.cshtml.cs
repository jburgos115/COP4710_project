using ECommerce.Data;
using ECommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

namespace ECommerce.Pages.Purchase
{
    public class PurchaseModel : PageModel
    {

        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public Model.Order Order { get; set; }
        public Model.Products Products { get; set; }
        public PurchaseModel(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public void OnGet(int pid)
        {
            Products = _db.Products.Find(pid);
        }

        public async Task<IActionResult> OnPost(Model.Order order, int pid)
        {
            Products = _db.Products.Find(pid);
            var uid = User.FindFirst("UserID").Value;
            decimal totalCost = order.BuyQuantity * Products.Price;
            int orderID;
            try
            {
                string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Inserts Product into Table
                    String myCommand = "INSERT INTO [Orders] (ProductID, UserID, TotalCost, BuyQuantity, Street, City, State, Zip, CardNum, CardExpMonth, CardExpYear, NameOnCard) output INSERTED.OrderID VALUES(@ProductID, @UserID, @TotalCost, @BuyQuantity, @Street, @City, @State, @Zip, @CardNum, @CardExpMonth, @CardExpYear, @NameOnCard)";
                    SqlCommand cmd = new SqlCommand(myCommand, connection);

                    cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = Products.ProductID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = uid;
                    cmd.Parameters.Add("@TotalCost", SqlDbType.Decimal).Value = totalCost;
                    cmd.Parameters.Add("@BuyQuantity", SqlDbType.Int).Value = order.BuyQuantity;
                    cmd.Parameters.Add("@Street", SqlDbType.NVarChar, 100).Value = order.Street;
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar, 100).Value = order.City;
                    cmd.Parameters.Add("@State", SqlDbType.NVarChar, 2).Value = order.State;
                    cmd.Parameters.Add("@Zip", SqlDbType.NChar, 5).Value = order.Zip;
                    cmd.Parameters.Add("@CardNum", SqlDbType.NVarChar, 16).Value = order.CardNum;
                    cmd.Parameters.Add("@CardExpMonth", SqlDbType.Int).Value = order.CardExpMonth;
                    cmd.Parameters.Add("@CardExpYear", SqlDbType.Int).Value = order.CardExpYear;
                    cmd.Parameters.Add("@NameOnCard", SqlDbType.NVarChar, 100).Value = order.NameOnCard;
                    orderID = (int)cmd.ExecuteScalar();

                    cmd.Dispose();
                    connection.Close();

                    TempData["success"] = "Order processed successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Sorry, we are unable to process your request at this time. Please try again later.";
            }

            int newQuantity = Products.Quantity - order.BuyQuantity;

            try
            {
                string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Update Quantity details in Products table
                    String myCommand = "UPDATE [Products] SET Quantity = @Quantity WHERE ProductID = @ProductID";

                    SqlCommand cmd = new SqlCommand(myCommand, connection);
                    cmd = new SqlCommand(myCommand, connection);

                    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = newQuantity;
                    cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = Products.ProductID;

                    int success = Convert.ToInt32(cmd.ExecuteNonQuery());

                    //Check if query was unsuccessful
                    if (success < 1)
                    {
                        TempData["error"] = "There was an error updating product quantity";
                    }

                    cmd.Dispose();
                    connection.Close();

                    TempData["success"] = "Order processed successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Sorry, we are unable to process your request at this time. Please try again later.";
            }

            return RedirectToPage("../ShopDisplay/ShopDisplay", "ShopID", new { ShopID = Products.ShopID });
        }
    }
}
