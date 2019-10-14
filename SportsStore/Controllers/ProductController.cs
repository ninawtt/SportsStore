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
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        // default action
        public ViewResult List(string category, int productPage = 1) => 
            // the argument is passed from 
            
            View( new ProductsListViewModel
            {
                Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
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
