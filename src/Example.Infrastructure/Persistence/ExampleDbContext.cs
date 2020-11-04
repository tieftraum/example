using System;
using Microsoft.EntityFrameworkCore;
using Example.Domain.Models;

namespace Example.Infrastructure.Persistence
{
    public class ExampleDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(
                u => u.UserName).IsUnique();

            modelBuilder.Entity<Contact>()
                .HasMany(c => c.PhoneNumbers)
                .WithOne(n => n.Contact)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhoneNumber>().HasIndex(pn =>
                new { pn.Phone, pn.ContactId }).IsUnique();

            modelBuilder.Entity<RefreshToken>().HasIndex(rt => rt.Token);
        }
    }
}
