using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Tag : BaseModel
    {
        public string Name { get; set; }
        public List<Blog>? Blogs { get; set; }
    }
}
