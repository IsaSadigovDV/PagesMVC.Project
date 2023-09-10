using Pages.Core.Entities;

namespace Pages.App.ViewModels
{
    public class BookVM
    {
        public Book Book { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
