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
            .Include(o => o.Lines)
                .ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            // Avoid the application to insert products into Product table again
            // Avoid duplicate
            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
