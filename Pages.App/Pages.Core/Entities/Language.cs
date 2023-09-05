using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Language:BaseModel
    {
        public string Name { get; set; }
        public  List<BookLanguage>? BookLanguages { get; set; }
        public  List<AuthorLanguage>? AuthorLanguages { get; set; }
    }
}
