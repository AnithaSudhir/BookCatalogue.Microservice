using BookCatalogue.Microservice.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookCatalogue.Microservice.DBContexts
{ 
    public interface IBookCatalogueContext
    {
        DbSet<Book> Books { get; set; }
        DbSet<Author> Authors { get; set; }
        Task<int> SaveChanges();

    }
}
