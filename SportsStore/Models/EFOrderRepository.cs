using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        // Constructor 
        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines) // Lines is stored in different table, we need to explicitly specify it should include Lines
                .ThenInclude(l => l.Product); // Product is stored in different table, we need to explicitly specify it should include Product

        public void SaveOrder(Order order)
        {
            // Avoid the application to insert products into Product table again
            // Avoid duplicate
            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0) // check if the order is saved yet or not, if it's 0, it means it's not saved yet
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
