using Pages.Core.Entities;

namespace Pages.App.ViewModels
{
    public class BlogVM
    {
        public Blog Blog { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
    }
}
