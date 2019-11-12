using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    // store the entire collection of CartLine
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity) // set it virtual so that it can be overriden
        {
            // if the cartcollection already have the product, only need to add quantity
            CartLine line = lineCollection
                .Where(p => p.Product.ProductID == product.ProductID)
                .FirstOrDefault(); // return if the product is already inside the lineCollection

            // if there's no same product in the cart, add it into the cart
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else // if there's already the same product in the cart, add the quantity
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) => 
            lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);

        public decimal ComputeTotalValue() =>
            lineCollection.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => lineCollection.Clear(); // clear all items inside the shopping cart

        // Return the whole cartline inside the linecollection
        public IEnumerable<CartLine> Lines => lineCollection;
    }
}
