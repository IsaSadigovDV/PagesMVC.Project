using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class BookLanguage:BaseModel
    {
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
    }
}
