using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookCatalogue.Microservice.Controllers;
using BookCatalogue.Microservice.DBContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using BookCatalogue.Microservice.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalogue.UnitTest
{
    [TestClass]
    public class BookCatalogueControllerTest
    {
        public IBookCatalogueContext BookCatalogueContext;
        private IConfigurationRoot _configuration;
        private DbContextOptions<BookCatalogueContext> _options;
        public BookCatalogueControllerTest()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            _configuration = builder.Build();
            _options = new DbContextOptionsBuilder<BookCatalogueContext>().UseSqlServer(_configuration.GetConnectionString("DefaultConnection")).Options;
            BookCatalogueContext = new BookCatalogueContext(_options);
        }

        [TestMethod]
        public void GetBookByISBN()
        {
            var controller = new BookCatalogueController(BookCatalogueContext);
            var actionResult = controller.Get("978-1734314502");
            var contentResult = (ObjectResult)actionResult.Result;
            Assert.AreEqual(200, contentResult.StatusCode);
            Assert.AreEqual("978-1734314502", ((Book)((ObjectResult)actionResult.Result).Value).ISBN);
        }

        [TestMethod]
        public void CreateBook()
        {
            var controller = new BookCatalogueController(BookCatalogueContext);
            var author = new Author();
            author.Name = "Shive Khera";

            var book = new Book();
            book.Authors = new System.Collections.Generic.List<Author>();
            book.Authors.Add(author);
            book.ISBN = "9781581130225";
            book.PublicationDate = System.DateTime.Now;
            book.Title = "You Can Win";

            var actionResult = controller.Create(book);
            var contentResult = (ObjectResult)actionResult.Result;

            Assert.IsNotNull(actionResult);
            Assert.AreEqual(200, contentResult.StatusCode);
        }


        [TestMethod]
        public void UpdateBook()
        {
            var controller = new BookCatalogueController(BookCatalogueContext);
            var book = new Book();
            book.PublicationDate = System.DateTime.Now;
            book.Title = "You Can Win";
            var actionResult = controller.Update("9781581130225", book);
            var contentResult = (ObjectResult)actionResult.Result;
            Assert.IsNotNull(actionResult);
            Assert.AreNotEqual(200, contentResult.StatusCode);
        }

        [TestMethod]
        public void DeleteBook()
        {
            var controller = new BookCatalogueController(BookCatalogueContext);
            var actionResult = controller.Delete("string");
            var contentResult = (Microsoft.AspNetCore.Mvc.OkResult)actionResult.Result;
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(200, contentResult.StatusCode);
        }
    }
}
