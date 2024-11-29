using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using amazonBooks.Models;
using amazonBooks.Data;
namespace amazonBooks.Controllers;

public class BookController : Controller
{
    private ApplicationDbContext _db;

    public BookController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var books = _db.BooksEntity.ToList();
        return View(books);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateBook(BooksEntity book)
    {
        _db.BooksEntity.Add(book);
        _db.SaveChanges();
        return RedirectToAction("Index");

    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var book = _db.BooksEntity.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    [HttpPost]
    public IActionResult Edit(BooksEntity book)
    {
        if (ModelState.IsValid)
        {
            _db.BooksEntity.Update(book);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(book);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var book = _db.BooksEntity.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var book = _db.BooksEntity.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            _db.BooksEntity.Remove(book);
            _db.SaveChanges();
        }
        return RedirectToAction("Index");
    }


}