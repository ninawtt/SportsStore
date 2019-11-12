using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;

        // Constructor 
        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products;

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0) // this means the productID has a default value "0" so this product must be a new one
            {
                context.Products.Add(product);
            }
            else
            {
                Product productEntry = context.Products
                    .FirstOrDefault(p => p.ProductID == product.ProductID);

                if(productEntry != null)
                {
                    productEntry.Name = product.Name;
                    productEntry.Description = product.Description;
                    productEntry.Category = product.Category;
                    productEntry.Price = product.Price;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int productID)
        {
            Product productEntry = context.Products
                    .FirstOrDefault(p => p.ProductID == productID);

            if (productEntry != null)
            {
                context.Products.Remove(productEntry);
                context.SaveChanges();
            }

            return productEntry;
        }
    }
}
