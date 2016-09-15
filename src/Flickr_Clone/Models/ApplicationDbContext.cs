using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Flickr_Clone.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Flickr;integrated security=True");

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ImageTag>()
                .HasOne(it => it.Tag)
                .WithMany(t => t.ImageTags);
            builder.Entity<ImageTag>()
                .HasOne(it => it.Image)
                .WithMany(i => i.ImageTags);
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ImageTag> ImageTags { get; set; }


    }
}
