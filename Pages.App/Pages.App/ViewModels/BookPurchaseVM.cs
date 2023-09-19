using Pages.Core.Entities;

namespace Pages.App.ViewModels
{
    public class BookPurchaseVM
    {
        public string Nonce { get; set; }
        public double TotalPrice { get; set; }
        public int Id { get; set; }
    }
}
