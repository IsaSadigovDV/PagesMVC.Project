using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Basket:BaseModel
    {
        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public List<BasketItem>? BasketItems { get; set; }
    }
}
