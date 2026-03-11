namespace Kutuphane_Otomasyonu.Models;

public class Author
{
    public int AuthorId { get; set; }
    public string? FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();

}

