using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Social:BaseModel
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }
        public int? SettingId { get; set; }
        public Setting Setting { get; set; }

    }
}
