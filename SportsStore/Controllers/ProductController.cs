 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        // Constructor
        public ProductController(IProductRepository repo) // magic of Dependency Injection
        {
            repository = repo;
        }

        // default action
        public ViewResult List(string category, int productPage = 1) => 
            // the argument is passed from the NavigationMenuViewComponent's default view when user clicks on a category
            
            View( new ProductsListViewModel
            {
                Products = repository.Products
                    .Where(p => category == null || p.Category == category) // filter the category, if it's null, pick all items, it there's a category selected, only display the products belong to that category.
                    .OrderBy(p => p.ProductID) // order by the productID
                    .Skip((productPage - 1) * PageSize) // and then if we are on the page 2, it should skip the previous "PageSize" items
                    .Take(PageSize), // after skip some items, we take(retrieve) "PageSize" items and display them
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products
                        .Where(p => category == null || p.Category == category) // filter the data, if the category is null, return all products, if category is eqaul to the selected category, return only that category
                        .Count()
                },
                CurrentCategory = category
            });
    }
}
