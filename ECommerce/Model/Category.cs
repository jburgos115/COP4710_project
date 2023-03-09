using System.ComponentModel.DataAnnotations;

namespace ECommerce.Model
{
	public class Category
	{

		[Key]
		public int CategoryID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
	}
}
