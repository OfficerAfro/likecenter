using Microsoft.EntityFrameworkCore;
using likecenter.Models;

namespace likecenter.Models {
    public class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Post> Posts {get;set;}
        public DbSet<Synergy> Synergies {get;set;}
    }
}