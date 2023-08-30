
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class ALanguage:BaseModel
    {
        public string Name { get; set; }
        public List<AuthorLanguage>? AuthorLanguage { get; set; }
    }
}
