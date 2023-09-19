using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pages.Core.Entities
{
    public class Order:BaseModel
    {
        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public bool isPaid { get; set; }
    }
}
