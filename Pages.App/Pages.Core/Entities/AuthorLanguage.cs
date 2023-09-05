using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class AuthorLanguage:BaseModel
    {
        public int AuthorId { get; set; }
        public virtual Author? Author { get; set; }
        public int LanguageId { get; set; }
        public virtual Language? Language { get; set; }
    }
}
