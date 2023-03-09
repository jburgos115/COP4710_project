using System.ComponentModel.DataAnnotations;

namespace ECommerce.Model
{
    //declares the attributes of author table
    //these variables are used to register keep records of authors in the database

    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        public int ShopID { get; set; }

        [Required(ErrorMessage = "Please provide a Product Name", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide a Product Description", AllowEmptyStrings = false)]
        public string Description { get; set; }

        //[Required(ErrorMessage = "Please provide a Product Price", AllowEmptyStrings = false)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please provide a Product Stock Quantity", AllowEmptyStrings = false)]
        public int Quantity { get; set; }

    }
}

