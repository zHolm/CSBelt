using Microsoft.EntityFrameworkCore;

namespace Belt.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}
        public DbSet<User> Users {get;set;}
        public DbSet<Product> products {get;set;}
        public DbSet<Bid> bids {get;set;}

    }
}