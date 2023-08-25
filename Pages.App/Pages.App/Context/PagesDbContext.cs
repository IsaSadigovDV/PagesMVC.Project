using Microsoft.EntityFrameworkCore;
using Pages.Core.Entities;

namespace Pages.App.Context
{
    public class PagesDbContext:DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs  { get; set; }
        public DbSet<Sponsor> Sponsors  { get; set; }
        public DbSet<Subscribe> Subscribes  { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Language> Languages { get; set; }  
        public PagesDbContext(DbContextOptions<PagesDbContext> options) : base(options)
        {

        }
    }
}
