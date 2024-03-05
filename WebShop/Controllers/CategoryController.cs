using Microsoft.AspNetCore.Mvc;
using WebShop.DataAccess.Data;
using WebShop.Models.Models;

namespace WebShop.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        List<Category> categoryList = _context.Categories.ToList();
        return View(categoryList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category category)
    {
        //Custom validation
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
        }

        if (ModelState.IsValid) 
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index", "Category");
        }

        return View();
    }

    public IActionResult Edit(int? categoryId)
    {
        if (categoryId is null or 0)
        {
            return NotFound();
        }

        Category? category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
        //Category? category1 = _context.Categories.Find(categoryId);
        //Category? category2 = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            TempData["success"] = "Category edited successfully";
            return RedirectToAction("Index", "Category");
        }

        return View();
    }

    public IActionResult Delete(int? categoryId)
    {
        if (categoryId is null or 0)
        {
            return NotFound();
        }

        Category? category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? categoryId)
    {
        Category? category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);

        if (category == null)
        {
            return NotFound();
        }
        
        _context.Categories.Remove(category);
        _context.SaveChanges();
        TempData["success"] = "Category deleted successfully";

        return RedirectToAction("Index", "Category");
    }
}
