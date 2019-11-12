using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IProductRepository repository;

        public NavigationMenuViewComponent(IProductRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke() // be called in _Layout by using @await Component.InvokeAsync("NavigationMenu");
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];  // URL might not have category, to avoid the null then give us the error, put ? after RouteData
            return View(repository.Products // filter the product category 
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x));
        }
    } 
}
