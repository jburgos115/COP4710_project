using ECommerce.Model;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Data;
public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{

	}

	//List of Model objects
	public DbSet<Products> Products { get; set; }
}
