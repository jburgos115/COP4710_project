using ECommerce.Model;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		//List of Model objects
		public DbSet<Category> Category { get; set; }
		public DbSet<Products> Products { get; set; }
		public DbSet<Represents> Represents { get; set; }
		public DbSet<Shop> Shop { get; set; }
		public DbSet<User> User { get; set; }
	}
}
