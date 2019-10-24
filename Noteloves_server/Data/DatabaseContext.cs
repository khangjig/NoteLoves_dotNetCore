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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(user => new { user.Email })
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .Property(user => user.CreatedAt)
                .HasDefaultValue(DateTime.Now)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(user => user.UpdateAt)
                .HasDefaultValue(DateTime.Now)
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
