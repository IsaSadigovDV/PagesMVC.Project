using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Genre:BaseModel
    {
        public string Name { get; set; }
        public virtual List<AuthoreGenre> AuthoreGenres { get; set; }

    }
}
