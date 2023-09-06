using Pages.Core.Entities;

namespace Pages.App.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Blog> Blogs { get; set;}
        public IEnumerable<Sponsor> Sponsors { get; set;}
    }
}
