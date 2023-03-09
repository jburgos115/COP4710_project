using System.ComponentModel.DataAnnotations;

namespace ECommerce.Model
{
	public class Represents
	{
		[Key]
		public int RepresentsID { get; set; }

		public int CategoryID { get; set; }

		public int ProductID { get; set; }
	}
}
