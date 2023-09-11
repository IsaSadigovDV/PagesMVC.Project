using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Comment : BaseModel
    {
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public int? BookId { get; set; } 

        public Book? Book { get; set; }
        public int? BlogId { get; set; }
        public Blog? Blog { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }

    }
}
