using System.ComponentModel.DataAnnotations;

namespace ECommerce.Model
{
	public class User
	{
		[Key]
		public int UserID { get; set; }

		public string Email { get; set; }

		[Required(ErrorMessage = "Please provide a password", AllowEmptyStrings = false)]
		[RegularExpression(@"^.{8,}$", ErrorMessage = "Minimum 8 characters required")]
		public byte[] PasswordHash { get; set; }

		[Required(ErrorMessage = "Please provide a name", AllowEmptyStrings = false)]
		public string Name { get; set; }
	}
}