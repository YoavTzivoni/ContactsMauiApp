using Microsoft.EntityFrameworkCore;

namespace ContactsWebAPI.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) 
		{ 

		}

		public DbSet<Model.Contact> Contacts { get; set; }
	}
}
