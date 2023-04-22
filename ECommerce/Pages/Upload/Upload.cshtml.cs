using ECommerce.Data;
using ECommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Pages.Upload
{
    public class UploadModel : PageModel
    {
        public Model.Products Products { get; set; }

        private readonly IWebHostEnvironment _environment;

        private readonly ApplicationDbContext _db;
        public UploadModel(IWebHostEnvironment environment, ApplicationDbContext db)
        {
            _environment = environment;
            _db = db;
        }

        //Retrieves PaperID to attach to file upload
        public void OnGetProductID(int ProductID)
        {
            TempData["id"] = ProductID;
        }

        //Controller for uploading a pdf file to wwwroot/Uploads folder as a custom named file
        public async Task<IActionResult> OnPostUpload(List<IFormFile> postedFiles)
        {
            Products = _db.Products.Find(TempData["id"]);

            string myPath = this._environment.WebRootPath;
            string contentPath = this._environment.ContentRootPath;

            string path = Path.Combine(this._environment.WebRootPath, "ProductImages");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                //Generate unique identifier using base64 encoding to find product image
                string uniqueIdentifier = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Concat(TempData["id"].ToString(), Products.Name)));
                string imgPath = string.Concat(uniqueIdentifier, ".png");
                using (FileStream stream = new FileStream(Path.Combine(path, imgPath), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(imgPath);
                }
                TempData["success"] = "Uploaded Successfully";
			}
			return RedirectToPage("../ShopDisplay/ShopDisplay", "ShopID", new { ShopID = Products.ShopID });
        }
    }
}
