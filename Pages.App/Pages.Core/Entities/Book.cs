using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Book:BaseModel
    {

        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public DateTime PubslishDate { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int PaperCount { get; set; }
        public string Dimensions { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile? file { get; set; } 
        public List<BookAuthor>? BookAuthors { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
