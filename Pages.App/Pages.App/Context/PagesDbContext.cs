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
        public DbSet<Tag> Tags { get; set; }  
        public DbSet<Author> Authors { get; set; }  
        public DbSet<ALanguage> ALanguages { get; set; }
        public DbSet<AuthoreGenre> AuthoreGenres { get; set; }
        public DbSet<AuthorLanguage> AuthorLanguages { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors{ get; set; }
        public DbSet<ChapterCommit> ChapterCommits { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Social> Socials { get; set; }
        public DbSet<WhatLearn> WhatLearns { get; set; }
        public DbSet<AuthorSocial> AuthorSocials { get; set; }


        public PagesDbContext(DbContextOptions<PagesDbContext> options) : base(options)
        {

        }
    }
}
