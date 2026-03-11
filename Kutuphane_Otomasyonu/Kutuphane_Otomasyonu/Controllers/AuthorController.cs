using Microsoft.AspNetCore.Mvc;
using Kutuphane_Otomasyonu.data;
using Microsoft.EntityFrameworkCore;
using Kutuphane_Otomasyonu.Models;


namespace Kutuphane_Otomasyonu.Controllers;


public class AuthorController : Controller
{
    private readonly LibraryContext _context;
    public AuthorController(LibraryContext context)
    {
        _context = context;
    }

    //READ: yazarları listele
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var authors = await _context.Authors.ToListAsync();
        return View(authors);
    }

    //CREATE: yazar ekleme-get
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    //CREATE: yazar ekleme -post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Author author)
    {
        if (ModelState.IsValid)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(author);
    }

    //DELETE: yazar silme confirm (GET)
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }
        // show confirmation view
        return View(author);
    }

    //DELETE: yazar silme (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Author author)
    {
        if (author == null || author.AuthorId == 0)
        {
            return BadRequest();
        }

        var entity = await _context.Authors.FindAsync(author.AuthorId);
        if (entity != null)
        {
            _context.Authors.Remove(entity);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    //EDIT: yazar güncelle-get
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null) return NotFound();
        return View(author);
    }

    //EDIT: yazar güncelle-post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Author author)
    {
        if (ModelState.IsValid)
        {
            _context.Update(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(author);
    }
}
