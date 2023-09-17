using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class OrderItem:BaseModel
    {
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductCount { get; set; }
    }
}
