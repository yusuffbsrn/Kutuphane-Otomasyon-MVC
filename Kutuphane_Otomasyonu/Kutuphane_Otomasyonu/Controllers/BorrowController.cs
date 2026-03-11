using Microsoft.AspNetCore.Mvc;
using Kutuphane_Otomasyonu.data;
using Kutuphane_Otomasyonu.Models;
using Microsoft.EntityFrameworkCore;

namespace Kutuphane_Otomasyonu.Controllers;

public class BorrowController : Controller
{
    private readonly LibraryContext _context;
    public BorrowController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var borrows = await _context.Borrows
            .Include(b => b.Book)
            .Include(b => b.Member)
            .ToListAsync();
        return View(borrows);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Books = _context.Books.Where(b => b.IsAvailable).ToList();
        // only active members may borrow
        ViewBag.Members = _context.Members.Where(m => m.IsActive).ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Borrow borrow)
    {
        // verify that selected member is active
        var member = await _context.Members.FindAsync(borrow.MemberId);
        if (member == null || !member.IsActive)
        {
            ModelState.AddModelError(nameof(borrow.MemberId), "Seçilen üye pasif; ödünç alma işlemi için önce üyeyi aktif hale getirin.");
        }

        if (ModelState.IsValid)
        {
            borrow.BorrowDate = DateTime.Now;
            borrow.IsReturned = false;

            var book = await _context.Books.FindAsync(borrow.BookId);
            if (book != null)
            {
                book.IsAvailable = false;
            }

            _context.Borrows.Add(borrow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Books = _context.Books.Where(b => b.IsAvailable).ToList();
        ViewBag.Members = _context.Members.Where(m => m.IsActive).ToList();
        return View(borrow);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var borrow = await _context.Borrows.FindAsync(id);
        if (borrow == null) return NotFound();
        ViewBag.Books = _context.Books.ToList();
        // only active members or current member (in case it's been deactivated since borrow was taken)
        ViewBag.Members = _context.Members
            .Where(m => m.IsActive || m.MemberId == borrow.MemberId)
            .ToList();
        return View(borrow);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Borrow borrow)
    {
        var member = await _context.Members.FindAsync(borrow.MemberId);
        if (member == null || !member.IsActive)
        {
            ModelState.AddModelError(nameof(borrow.MemberId), "Seçilen üye pasif olduğundan ödünç kaydı güncellenemez.");
        }

        if (ModelState.IsValid)
        {
            var existingBorrow = await _context.Borrows.FindAsync(borrow.BorrowId);
            if (existingBorrow != null)
            {
                // Eğer kitap teslim edilirse, kitabı tekrar ödünç alınabilir yapın
                if (!existingBorrow.IsReturned && borrow.IsReturned)
                {
                    existingBorrow.IsReturned = true;
                    existingBorrow.ReturnDate = DateTime.Now;
                    
                    var book = await _context.Books.FindAsync(existingBorrow.BookId);
                    if (book != null)
                    {
                        book.IsAvailable = true;
                    }
                }
                else
                {
                    existingBorrow.MemberId = borrow.MemberId;
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Books = _context.Books.ToList();
        ViewBag.Members = _context.Members
            .Where(m => m.IsActive || m.MemberId == borrow.MemberId)
            .ToList();
        return View(borrow);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var borrow = await _context.Borrows
            .Include(b => b.Book)
            .Include(b => b.Member)
            .FirstOrDefaultAsync(b => b.BorrowId == id);
        if (borrow == null) return NotFound();
        return View(borrow);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Borrow borrow)
    {
        if (borrow == null || borrow.BorrowId == 0)
        {
            return BadRequest();
        }

        var entity = await _context.Borrows.FindAsync(borrow.BorrowId);
        if (entity != null)
        {
            if (!entity.IsReturned)
            {
                var book = await _context.Books.FindAsync(entity.BookId);
                if (book != null) book.IsAvailable = true;
            }
            _context.Borrows.Remove(entity);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}