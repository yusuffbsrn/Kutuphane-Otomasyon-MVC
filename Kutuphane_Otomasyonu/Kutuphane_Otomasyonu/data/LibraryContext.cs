using Microsoft.EntityFrameworkCore;
using Kutuphane_Otomasyonu.Models;

namespace Kutuphane_Otomasyonu.data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Borrow> Borrows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Üye silinirse, ödünç kütüğü de silinsin (Cascade Delete)
        modelBuilder.Entity<Borrow>()
            .HasOne(b => b.Member)
            .WithMany(m => m.Borrows)
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        // Kitap silinirse, ödünç kütüğü de silinsin
        modelBuilder.Entity<Borrow>()
            .HasOne(b => b.Book)
            .WithMany()
            .HasForeignKey(b => b.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}