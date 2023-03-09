using System.ComponentModel.DataAnnotations;

namespace ECommerce.Model
{
	public class Products
	{
		[Key]
		public int ProductID { get; set; }

		public int ShopID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public decimal Price { get; set; }

		public int Quantity { get; set; }
	}
}
