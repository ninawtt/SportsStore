﻿ using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class FakeProductRepository /*: IProductRepository*/ // hard coding the product data
    { // repository is the collection of data
        public IQueryable<Product> Products => new List<Product>
        {
            new Product {Name = "Football", Price = 25},
            new Product {Name = "Surf board", Price = 179},
            new Product {Name = "Running shoes", Price = 95}
        }.AsQueryable<Product>(); // convert List to IQueryable
    }
}
