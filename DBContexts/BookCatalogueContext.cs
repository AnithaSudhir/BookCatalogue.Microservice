using BookCatalogue.Microservice.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookCatalogue.Microservice.DBContexts
{
    public class BookCatalogueContext : DbContext, IBookCatalogueContext
	{
		public DbSet<Book> Books { get; set; }
		public DbSet<Author> Authors { get; set; }
		public BookCatalogueContext(DbContextOptions<BookCatalogueContext> options)
			: base(options)
		{
		}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($"Server=DESKTOP-HRDMQNN\\SQLEXPRESS;" +
        $"Database=BookCatalogueDatabase;" +
        $"Trusted_Connection=True;" +
        $"MultipleActiveResultSets=true;" +
        $"ConnectRetryCount=0");
        }
        
        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
      
    }
}
    
