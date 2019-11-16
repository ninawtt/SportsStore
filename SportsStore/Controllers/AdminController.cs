using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        // Constructor
        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index() => View(repository.Products);

        // FirstOrDefault means it will return the first object that matchs the condition
        // if the productID is not found, the default "NULL" will return 
        public ViewResult Edit(int ProductID) => 
            View(repository.Products
                .FirstOrDefault(p => p.ProductID == ProductID)); 

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if(ModelState.IsValid)
            {
                repository.SaveProduct(product);
                // ViewBag cannot carry the data to the different view
                // so we use TempData
                TempData["message"] = $"{product.Name} has been saved!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create() => View("Edit", new Product());

        [HttpPost] // when dealing with HttpPost, use IActionResult
        public IActionResult Delete(int productID)
        {
            Product deletedProduct = repository.DeleteProduct(productID);

            if (deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Name} was deleted!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
