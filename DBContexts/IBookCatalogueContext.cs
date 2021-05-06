using BookCatalogue.Microservice.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
