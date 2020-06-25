using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BookStoreProject.Models
{
    public class BookStoreDbContext : IdentityDbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
        {

        }
        public BookStoreDbContext()
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Subcriber> Subcribers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);
          
           
            modelBuilder.Entity<WishList>().HasKey(x => new { x.ApplicationUserId, x.BookID });
            modelBuilder.Entity<CartItems>().HasKey(x => new { x.ApplicationUserId, x.BookID });
            modelBuilder.Entity<Categories>().HasKey(x => new { x.CategoryID });
            modelBuilder.Entity<OrderItems>().HasKey(x => new { x.OrderID, x.BookID });
            

        }
    }
}
