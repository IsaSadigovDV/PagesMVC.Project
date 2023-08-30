using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class ChapterCommit:BaseModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }

    }
}
