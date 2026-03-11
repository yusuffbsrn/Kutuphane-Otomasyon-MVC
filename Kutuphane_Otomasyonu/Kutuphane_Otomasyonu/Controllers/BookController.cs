using Microsoft.AspNetCore.Mvc;
using Kutuphane_Otomasyonu.data;
// using Kutuphane_Otomasyonu.Models;
using Microsoft.EntityFrameworkCore;
using Kutuphane_Otomasyonu.Models;


namespace Kutuphane_Otomasyonu.Controllers;




public class BookController : Controller
{
    private readonly LibraryContext _context;
    public BookController(LibraryContext context)
    {
        _context = context;
    }
    //READ:kitapları listele
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var books = await _context.Books.Include(b => b.Author).ToListAsync();
        return View(books);
    }
    //CREATE:kitap ekleme-get
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Authors = _context.Authors.ToList();
        return View();
    }
    //CREATE:kitap ekleme -post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book)
    {
        if (ModelState.IsValid)
        {
            book.IsAvailable = true;
            _context.Books.Add(book);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        ViewBag.Authors = _context.Authors.ToList();
        return View(book);
    }
    //DELETE: kitap silme confirm (GET)
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.BookId == id);
        if (book == null)
        {
            return NotFound();
        }
        // show confirmation view
        return View(book);
    }

    //DELETE: kitap silme (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Book book)
    {
        if (book == null || book.BookId == 0)
        {
            return BadRequest();
        }

        var entity = await _context.Books.FindAsync(book.BookId);
        if (entity != null)
        {
            _context.Books.Remove(entity);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
    //edit:kitap güncelle-get
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();
        ViewBag.Authors = _context.Authors.ToList();
        return View(book);
    }
    //edit:kitap güncelle-post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Update(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }// Eğer form valid değilse tekrar dropdown ver
        ViewBag.Authors = _context.Authors.ToList();
        return View(book);
    }

}

