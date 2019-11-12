using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private Cart cart;

        // Dependency injection: sessioncart will be created 
        public CartSummaryViewComponent(Cart cartService)
        {
            cart = cartService;
        }

        public IViewComponentResult Invoke() // be called in _Layout by using @await Component.InvokeAsync("CartSummary");
        {
            return View(cart);
        }
    }
}
