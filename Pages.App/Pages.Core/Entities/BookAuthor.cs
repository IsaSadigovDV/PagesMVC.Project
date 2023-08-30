using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class BookAuthor:BaseModel
    {
        public Book? Book { get; set; }
        public int BookId { get; set; }
        public Author? Author { get; set; }
        public int AuthorId { get; set; }
    }
}
