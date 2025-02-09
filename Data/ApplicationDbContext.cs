namespace ASP.NETCore;

using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<Thread> Threads { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>().HasKey(b => b.Serial); // Set Serial as the primary key

        modelBuilder.Entity<Board>().Property(b => b.Serial).ValueGeneratedOnAdd(); // Configure the Serial property to be auto-generated
    }
}
