namespace ECommerce.Model
{
	public class CategoryAndProducts
	{
		public IEnumerable<Category> Category { get; set; }
		public IEnumerable<Products> Products { get; set; }
	}
}
