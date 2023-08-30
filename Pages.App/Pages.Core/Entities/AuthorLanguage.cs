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
        public Author? Author { get; set; }
        public int ALanguageId { get; set; }
        public ALanguage? ALanguage { get; set; }
    }
}
