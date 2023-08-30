using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Author:BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<BookAuthor>? BookAuthors { get; set; }
        public int CountryId { get; set; }
        public Country? Country { get; set; }
        public List<AuthorLanguage>? AuthorLanguage { get; set; }
        public int PublicationDate { get; set; }
        public List<Social>?  Socials { get; set; }

        [NotMapped]
        public IFormFile? file { get; set; }
   
    }
}
