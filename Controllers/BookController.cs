using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using amazonBooks.Models;
using amazonBooks.Data;
using Microsoft.EntityFrameworkCore;

namespace amazonBooks.Controllers;

public class BookController : Controller
{
    private ApplicationDbContext _db;

    public BookController(ApplicationDbContext db)
    {
        _db = db;
    }


    [HttpGet]
    public IActionResult Index() => View(_db.BooksEntity.ToList());

    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Create(BooksEntity book, IFormFile PdfFile)
    {
        if (PdfFile != null && PdfFile.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                PdfFile.CopyTo(memoryStream);
                book.PdfContent = memoryStream.ToArray(); // Converting the file to respective binary format**
            }
        }

        _db.BooksEntity.Add(book);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }


    [HttpGet("Edit/{id}")]
    public IActionResult Edit(int id)
    {
        var book = _db.BooksEntity.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }


    // POST: Book/Edit/5
    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        IFormFile? PdfContent,
        [Bind("Id,Title,Author,ISBN")] BooksEntity book)
    {
        if (id != book.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingBook = _db.BooksEntity.FirstOrDefault(b => b.Id == id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;

                if (PdfContent != null && PdfContent.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await PdfContent.CopyToAsync(memoryStream);
                        existingBook.PdfContent = memoryStream.ToArray();
                    }
                }

                _db.Update(existingBook);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksEntityExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        return View(book);
    }


    private bool BooksEntityExists(int id)
    {
        return _db.BooksEntity.Any(e => e.Id == id);
    }


    [HttpPost("DeletePdf/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePdf(int id)
    {
        var book = _db.BooksEntity.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        book.PdfContent = null;

        _db.Update(book);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _db.BooksEntity.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        _db.BooksEntity.Remove(book);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _db.BooksEntity.FindAsync(id);
        if (book != null)
        {
            _db.BooksEntity.Remove(book);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult ViewPdf(int id)
    {
        var book = _db.BooksEntity.FirstOrDefault(b => b.Id == id);

        if (book == null || book.PdfContent == null || book.PdfContent.Length == 0)
        {
            return NotFound();
        }

        return File(book.PdfContent, "application/pdf");
    }




}