using System.ComponentModel.DataAnnotations;

namespace ECommerce.Model
{
	public class User
	{
		[Key]
		public int UserID { get; set; }

		[Required(ErrorMessage = "Please provide a password", AllowEmptyStrings = false)]
		[RegularExpression(@"^.{8,}$", ErrorMessage = "Minimum 8 characters required")]
		public byte[] PasswordHash { get; set; }

		[Required(ErrorMessage = "Please provide a name", AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Required(ErrorMessage = "Please provide a phone number", AllowEmptyStrings = false)]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Please provide a shipping address", AllowEmptyStrings = false)]
		public string Address { get; set; }

		[Required(ErrorMessage = "Please provide billing info", AllowEmptyStrings = false)]
		public string BillingInfo { get; set; }
	}
}