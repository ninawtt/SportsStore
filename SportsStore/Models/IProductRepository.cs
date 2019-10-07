 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public interface IProductRepository // interface, should be public so that controller can use it in the constructor
    {
        IQueryable<Product> Products { get; }
    }
}
