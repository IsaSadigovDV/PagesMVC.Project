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
        public string? Image { get; set; }
        public List<BookAuthor>? BookAuthors { get; set; }
        public List<AuthorLanguage>? AuthorLanguages { get; set; }
        public List<AuthoreGenre>? AuthoreGenres { get; set; }
        public List<AuthorSocial>? AuthorSocials { get; set; }
        public int CountryId { get; set; }
        public Country? Country { get; set; }

        [NotMapped]
        public IFormFile? FormFile { get; set; }
        [NotMapped]
        public int[] language { get; set; }

        [NotMapped]
        public int[] social { get; set; }

        [NotMapped]
        public int[] genre { get; set; }

    }
}
