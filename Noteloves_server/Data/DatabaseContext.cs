using Microsoft.EntityFrameworkCore;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Avatar> avatars { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(user => new { user.Email })
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .Property(user => user.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<User>()
                .Property(user => user.UpdateAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Avatar>()
                .Property(avatars => avatars.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Avatar>()
                .Property(avatars => avatars.UpdateAt)
                .HasDefaultValueSql("getdate()");
        }
    }
}
