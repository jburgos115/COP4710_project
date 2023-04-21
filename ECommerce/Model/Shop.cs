using System.ComponentModel.DataAnnotations;

namespace ECommerce.Model
{
	public class Shop
	{
		[Key]
		public int ShopID { get; set; }

		public int OwnerID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
	}
}
