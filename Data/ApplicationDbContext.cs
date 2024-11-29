namespace amazonBooks.Data;
using amazonBooks.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<BooksEntity> BooksEntity { get; set; }
}