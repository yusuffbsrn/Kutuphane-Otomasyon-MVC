using Microsoft.AspNetCore.Mvc;
using Kutuphane_Otomasyonu.data;
using Microsoft.EntityFrameworkCore;
using Kutuphane_Otomasyonu.Models;

namespace Kutuphane_Otomasyonu.Controllers;

public class MemberController : Controller
{
    private readonly LibraryContext _context;
    public MemberController(LibraryContext context)
    {
        _context = context;
    }

    //READ: üyeleri listele
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var members = await _context.Members.ToListAsync();
        return View(members);
    }

    //CREATE: üye ekleme-get
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    //CREATE: üye ekleme -post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Member member)
    {
        if (ModelState.IsValid)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(member);
    }

    //DELETE: üye silme confirm (GET)
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member == null)
        {
            return NotFound();
        }
        // show confirmation view
        return View(member);
    }

    //DELETE: üye silme (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Member member)
    {
        if (member == null || member.MemberId == 0)
        {
            return BadRequest();
        }

        var entity = await _context.Members.FindAsync(member.MemberId);
        if (entity != null)
        {
            _context.Members.Remove(entity);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    //EDIT: üye güncelle-get
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member == null) return NotFound();
        return View(member);
    }

    //EDIT: üye güncelle-post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Member member)
    {
        if (ModelState.IsValid)
        {
            _context.Update(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(member);
    }
}
