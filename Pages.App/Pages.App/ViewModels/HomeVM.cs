using Pages.Core.Entities;

namespace Pages.App.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Blog> Blogs { get; set;}
        public IEnumerable<Sponsor> Sponsors { get; set;}
        public IEnumerable<Book> Books { get; set;}
        public IEnumerable<Author> Authors { get; set;}
        public IEnumerable<Comment> Comments { get; set; }

    }
}
