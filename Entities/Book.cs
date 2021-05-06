using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookCatalogue.Microservice.Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate{ get; set; }

        [ForeignKey("BookId")]
        public virtual List<Author> Authors { get; set; }
        
        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string ISBN { get; set; }

     
    }
}
