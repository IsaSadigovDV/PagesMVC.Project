using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class BasketItem : BaseModel
    {
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int BasketId { get; set; }
        public Basket? Basket { get; set; }
        public int BookCount { get; set; }
    }
}
