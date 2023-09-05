using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class AuthorSocial:BaseModel
    {
        public int AuthorId { get; set; }
        public Author? Author { get; set; }  

        public int SocialId { get; set; }
        public Social? Social { get; set; }
    }
}
