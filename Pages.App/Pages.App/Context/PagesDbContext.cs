    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pages.Core.Entities;

namespace Pages.App.Context
{
    public class PagesDbContext: IdentityDbContext<AppUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs  { get; set; }
        public DbSet<Sponsor> Sponsors  { get; set; }
        public DbSet<Subscribe> Subscribes  { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Language> Languages { get; set; }  
        public DbSet<Author> Authors { get; set; }  
        public DbSet<Book> Books { get; set; }  
        public DbSet<BookAuthor> BookAuthors { get; set; }  
        public PagesDbContext(DbContextOptions<PagesDbContext> options) : base(options)
        {

        }
    }
}
