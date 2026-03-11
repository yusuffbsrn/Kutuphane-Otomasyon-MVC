namespace Kutuphane_Otomasyonu.Models;

public class Member
{
    public int MemberId { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool IsActive { get; set; }=true;
    public ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
}

