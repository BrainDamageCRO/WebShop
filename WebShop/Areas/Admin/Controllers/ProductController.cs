using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.DataAccess.Data;
using WebShop.DataAccess.Repository;
using WebShop.DataAccess.Repository.IRepository;
using WebShop.Models.Models;
using WebShop.Models.ViewModels;

namespace WebShop.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        List<Product> productList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();

        return View(productList);
    }

    public IActionResult Upsert(int? id)
    {
        IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepository
            .GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

        // ViewBag.CategoryList = categoryList;
        // ViewData["CategoryList"] = categoryList;

        ProductViewModel productViewModel = new ProductViewModel()
        {
            CategoryList = categoryList,
            Product = new Product(),
        };

        if (id == null || id == 0)
        {
            // Create
            return View(productViewModel);
        }
        else
        {
            // Update
            productViewModel.Product = _unitOfWork.ProductRepository.Get(p => p.Id == id);
            return View(productViewModel);
        }
    }

    [HttpPost]
    public IActionResult Upsert(ProductViewModel productViewModel, IFormFile? formFile)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (formFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                // Check if we need to delete the image first
                if (!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, productViewModel.Product.ImageUrl.Trim('\\'));

                    // Check if exists first always
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }

                productViewModel.Product.ImageUrl = @"\images\product\" + fileName;
            }

            if (productViewModel.Product.Id == 0)
            {
                _unitOfWork.ProductRepository.Add(productViewModel.Product);
                TempData["success"] = "Product created successfully";
            }
            else
            {
                _unitOfWork.ProductRepository.Update(productViewModel.Product);
                TempData["success"] = "Product updated successfully";
            }
            
            _unitOfWork.Save();

            return RedirectToAction("Index", "Product");
        }
        else
        {
            productViewModel.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View(productViewModel);
        }
    }

    //public IActionResult Delete(int? productId)
    //{
    //    if (productId is null or 0)
    //    {
    //        return NotFound();
    //    }

    //    Product? product = _unitOfWork.ProductRepository.Get(c => c.Id == productId);

    //    if (product == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(product);
    //}

    //[HttpPost, ActionName("Delete")]
    //public IActionResult DeletePOST(int? productId)
    //{
    //    Product? product = _unitOfWork.ProductRepository.Get(c => c.Id == productId);

    //    if (product == null)
    //    {
    //        return NotFound();
    //    }

    //    _unitOfWork.ProductRepository.Delete(product);
    //    _unitOfWork.Save();
    //    TempData["success"] = "Product deleted successfully";

    //    return RedirectToAction("Index", "Product");
    //}

    #region API Calls
    [HttpGet]
    public IActionResult GetAll()
    {
        List<Product> productList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
        return Json(new { data = productList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var product = _unitOfWork.ProductRepository.Get(p => p.Id == id);

        if (product == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.Trim('\\'));

        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.ProductRepository.Delete(product);
        _unitOfWork.Save();

        return Json(new { success = true, message = "Delete Successful" });
    }
    #endregion
}
