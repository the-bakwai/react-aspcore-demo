using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace react_demo.Models
{
    public class TodoDataContext : DbContext
    {
        public TodoDataContext(DbContextOptions<TodoDataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .HasOne<User>(t => t.User)
                .WithMany(u => u.Todos)
                .HasForeignKey(t => t.UserId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }


}