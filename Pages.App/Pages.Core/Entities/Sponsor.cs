using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Sponsor:BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

    }
}
