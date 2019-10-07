 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

        // Constructor
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        // default action
        public ViewResult List() => View(repository.Products);
    }
}
