using System.ComponentModel.DataAnnotations;

namespace ECommerce.Model
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public int UserID { get; set; }

        public decimal TotalCost { get; set; }

        public int BuyQuantity { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string CardNum { get; set; }

        public int CardExpMonth { get; set; }

        public int CardExpYear { get; set; }

        public string NameOnCard { get; set; }

    }
}
