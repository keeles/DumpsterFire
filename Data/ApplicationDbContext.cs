using System.ComponentModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCore;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    //public override DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Thread> Threads { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Board>().HasKey(b => b.Serial); // Set Serial as the primary key

        modelBuilder.Entity<Board>().Property(b => b.Serial).ValueGeneratedOnAdd(); // Configure the Serial property to be auto-generated
    }
}
