namespace Kutuphane_Otomasyonu.Models;

public class Borrow
{
    public int BorrowId { get; set; }
    public Book? Book { get; set; }
    public int BookId { get; set; }
    public Member? Member { get; set; }
    public int MemberId { get; set; }
    public DateTime BorrowDate { get; set; }=DateTime.Now;
    public DateTime ReturnDate { get; set; }
    public bool IsReturned { get; set; }=false;
}

