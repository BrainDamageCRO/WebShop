using Microsoft.AspNetCore.Mvc;
using WebShop.DataAccess.Data;
using WebShop.DataAccess.Repository;
using WebShop.DataAccess.Repository.IRepository;
using WebShop.Models.Models;

namespace WebShop.Controllers;

public class CategoryController : Controller
{
    // private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    //public CategoryController(ICategoryRepository categoryRepository)
    //{
    //    _categoryRepository = categoryRepository;
    //}

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //public IActionResult Index()
    //{
    //    List<Category> categoryList = _categoryRepository.GetAll().ToList();
    //    return View(categoryList);
    //}

    public IActionResult Index()
    {
        List<Category> categoryList = _unitOfWork.CategoryRepository.GetAll().ToList();
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
            _unitOfWork.CategoryRepository.Add(category);
            _unitOfWork.Save();
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

        Category? category = _unitOfWork.CategoryRepository.Get(c => c.Id == categoryId);
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
            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.Save();
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

        Category? category = _unitOfWork.CategoryRepository.Get(c => c.Id == categoryId);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? categoryId)
    {
        Category? category = _unitOfWork.CategoryRepository.Get(c => c.Id == categoryId);

        if (category == null)
        {
            return NotFound();
        }

        _unitOfWork.CategoryRepository.Delete(category);
        _unitOfWork.Save();
        TempData["success"] = "Category deleted successfully";

        return RedirectToAction("Index", "Category");
    }
}
