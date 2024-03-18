using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.DataAccess.Data;
using WebShop.DataAccess.Repository;
using WebShop.DataAccess.Repository.IRepository;
using WebShop.Models.Models;

namespace WebShop.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        List<Product> productList = _unitOfWork.ProductRepository.GetAll().ToList();

        return View(productList);
    }

    public IActionResult Create()
    {
        IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepository
            .GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

        ViewBag.CategoryList = categoryList;

        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.ProductRepository.Add(product);
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";
            return RedirectToAction("Index", "Product");
        }

        return View();
    }


    public IActionResult Edit(int? productId)
    {
        if (productId is null or 0)
        {
            return NotFound();
        }

        Product? product = _unitOfWork.ProductRepository.Get(c => c.Id == productId);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Save();
            TempData["success"] = "Product edited successfully";
            return RedirectToAction("Index", "Product");
        }

        return View();
    }

    public IActionResult Delete(int? productId)
    {
        if (productId is null or 0)
        {
            return NotFound();
        }

        Product? product = _unitOfWork.ProductRepository.Get(c => c.Id == productId);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? productId)
    {
        Product? product = _unitOfWork.ProductRepository.Get(c => c.Id == productId);

        if (product == null)
        {
            return NotFound();
        }

        _unitOfWork.ProductRepository.Delete(product);
        _unitOfWork.Save();
        TempData["success"] = "Product deleted successfully";

        return RedirectToAction("Index", "Product");
    }
}
