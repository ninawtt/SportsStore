 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public interface IProductRepository // interface, should be public so that controller can use it in the constructor
    {
        IQueryable<Product> Products { get; } // IQueryable is more optimal for query a list
        void SaveProduct(Product product); // abstract method

        Product DeleteProduct(int productID);
    }
}
