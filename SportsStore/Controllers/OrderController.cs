using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        // only return orders that haven't shipped yet
        public ViewResult List() => View(repository.Orders.Where(o => !o.Shipped)); 

        // The reason why we use IActionResult instead of ViewResult is that
        // if the user reflash the view, it won't execute this HttpPost again

        [HttpPost]
        public IActionResult MarkShipped(int orderID)
        {
            // filter and return the single record >> use FirstOrDefault
            Order order = repository.Orders.FirstOrDefault(o => o.OrderID == orderID);

            if(order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }

            //return RedirectToAction("List");
            return RedirectToAction(nameof(List)); // same as the above one
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid) // if all required fields in the form are all filled
            {
                order.Lines = cart.Lines.ToArray(); // assigne the Lines inside the cart into order
                repository.SaveOrder(order);
                //return View("Completed");
                //return RedirectToAction("Completed");
                return RedirectToAction(nameof(Completed)); // instead of hard coding the action's name
            }
            else {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}
