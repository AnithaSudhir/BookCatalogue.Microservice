using BookCatalogue.Microservice.DBContexts;
using BookCatalogue.Microservice.Entities;
using BookCatalogue.Microservice.Messages;
using BookCatalogue.Microservice.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BookCatalogue.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCatalogueController : Controller
    {
        public IBookCatalogueContext BookCatalogueContext;

        public BookCatalogueController(IBookCatalogueContext context)
        {
            BookCatalogueContext = context;
        }


        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            try
            {
                if (book != null && book.ISBN != null)
                {
                    var valid = BookCatalogueValidations.IsValid(book.ISBN);
                    if (!valid) return BadRequest("Invalid ISBN format");

                    var bookitem = BookCatalogueContext.Books
                                    .Where(c => c.ISBN == book.ISBN)
                                    .Include(u => u.Authors)
                                    .FirstOrDefault();

                    if (bookitem != null) return BadRequest("Record already exists");


                    BookCatalogueContext.Books.Add(book);
                    await BookCatalogueContext.SaveChanges();
                    MessageSender sender = new MessageSender();
                    sender.SendMQ("Book Added");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok(book.Id);

        }

        [HttpGet(Name = "GetAllBooks")]
        public IEnumerable<Book> Get()
        {
            return BookCatalogueContext.Books
            .Include(u => u.Authors).AsQueryable<Book>();
        }

        [HttpGet("ISBN", Name = "GetByISBN")]
        public ActionResult<Book> Get(string isbn)
        {
            Book book = new Book();
            try
            {
                book = BookCatalogueContext.Books
                           .Where(c => c.ISBN == isbn)
                           .Include(u => u.Authors)
                           .FirstOrDefault();
                
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(book);
        }

        [HttpDelete("ISBN")]
        public async Task<IActionResult> Delete(string isbn)
        {
            Book book = new Book();
            try
            {
                book = BookCatalogueContext.Books.ToList()
                        .Where(c => c.ISBN == isbn)
                        .FirstOrDefault();

                if (book != null)
                {
                    var authors = BookCatalogueContext.Authors.ToList()
                   .Where(c => c.BookId == book.Id);

                    BookCatalogueContext.Authors.RemoveRange(authors);
                    BookCatalogueContext.Books.Remove(book);

                    await BookCatalogueContext.SaveChanges();
                    MessageSender sender = new MessageSender();
                    sender.SendMQ("Book Deleted");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPut("ISBN")]
        public async Task<IActionResult> Update(string isbn, Entities.Book book)
        {
            try
            {
                var bookItem = BookCatalogueContext.Books.Where(a => a.ISBN.Replace("-", "") == isbn.Replace("-", "")).FirstOrDefault();

                if (bookItem != null)
                {
                    var bookitem = BookCatalogueContext.Books
                                          .Where(c => c.ISBN == book.ISBN)
                                          .Include(u => u.Authors)
                                          .FirstOrDefault();

                    if (bookitem != null) return BadRequest("Record already exists");

                    bookItem.Title = book.Title;
                    bookItem.ISBN = book.ISBN;
                    bookItem.Authors = new List<Author>();
                    bookItem.Authors.AddRange(book.Authors);

                    await BookCatalogueContext.SaveChanges();
                    MessageSender sender = new MessageSender();
                    sender.SendMQ("Book Updated");

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();

        }

    }
}
